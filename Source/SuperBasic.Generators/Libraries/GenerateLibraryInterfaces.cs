// <copyright file="GenerateLibraryInterfaces.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Scanning
{
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateLibraryInterfaces : BaseGeneratorTask<LibraryCollection>
    {
        protected override void Generate(LibraryCollection model)
        {
            this.Line("namespace SuperBasic.Compiler.Runtime");
            this.Brace();

            this.Line("using System;");
            this.Blank();

            foreach (Library library in model)
            {
                this.GenerateLibraryInterface(library);
            }

            this.GenerateLibrariesCollection(model);
            this.GenerateEventCallbacks(model);

            this.Unbrace();
        }

        private void GenerateLibraryInterface(Library library)
        {
            this.Line($"public interface I{library.Name}Library");
            this.Brace();

            List<string> members = new List<string>();

            foreach (Event @event in library.Events)
            {
                members.Add($"event Action {@event.Name};");
            }

            foreach (Property property in library.Properties.Where(p => !p.IsDeprecated))
            {
                members.Add($"{property.Type.ToNativeType()} {property.Name} {{{(property.HasGetter ? " get;" : string.Empty)}{(property.HasSetter ? " set;" : string.Empty)} }}");
            }

            foreach (Method method in library.Methods.Where(m => !m.IsDeprecated))
            {
                members.Add($"{method.ReturnType?.ToNativeType() ?? "void"} {method.Name}({method.Parameters.Select(p => $"{p.Type.ToNativeType()} {p.Name.ToLowerFirstChar()}").Join(", ")});");
            }

            for (int i = 0; i < members.Count; i++)
            {
                if (i > 0)
                {
                    this.Blank();
                }

                this.Line(members[i]);
            }

            this.Unbrace();
        }

        private void GenerateLibrariesCollection(LibraryCollection model)
        {
            this.Line("public interface IEngineLibraries");
            this.Brace();

            int i = 0;
            foreach (var library in model)
            {
                if (i++ > 0)
                {
                    this.Blank();
                }

                this.Line($"I{library.Name}Library {library.Name} {{ get; }}");
            }

            this.Unbrace();
        }

        private void GenerateEventCallbacks(LibraryCollection model)
        {
            this.Line("internal static class IEngineLibrariesExtensions");
            this.Brace();

            this.Line("public static void SetEventCallbacks(this IEngineLibraries libraries, SuperBasicEngine engine)");
            this.Brace();

            int i = 0;
            foreach (Library library in model.Where(library => library.Events.Any()))
            {
                if (i++ > 0)
                {
                    this.Blank();
                }

                foreach (Event @event in library.Events)
                {
                    this.Line($@"libraries.{library.Name}.{@event.Name} += () => engine.RaiseEvent(""{library.Name}"", ""{@event.Name}"");");
                }
            }

            this.Unbrace();
            this.Unbrace();
        }
    }
}
