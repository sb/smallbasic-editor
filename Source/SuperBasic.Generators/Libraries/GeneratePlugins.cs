// <copyright file="GeneratePlugins.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Scanning
{
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GeneratePlugins : BaseGeneratorTask<LibraryCollection>
    {
        protected override void Generate(LibraryCollection model)
        {
            this.Line("namespace SuperBasic.Compiler.Runtime");
            this.Brace();

            this.Line("using System;");
            this.Line("using SuperBasic.Utilities;");
            this.Blank();

            List<Library> libraries = model.Where(library => library.NeedsPlugin).ToList();
            if (!libraries.Any())
            {
                this.Log.LogError("No libraries that need plugins were found.");
            }

            foreach (Library library in libraries)
            {
                this.GeneratePluginInterface(library);
            }

            this.GeneratePluginsCollection(libraries);

            this.Unbrace();
        }

        private void GeneratePluginsCollection(List<Library> libraries)
        {
            this.Line("public sealed class PluginsCollection");
            this.Brace();

            foreach (var library in libraries)
            {
                this.Line($"private I{library.Name}Plugin {library.Name.ToLowerFirstChar()};");
            }

            this.Blank();

            this.Line("public PluginsCollection(");
            this.Indent();

            for (int i = 0; i < libraries.Count; i++)
            {
                Library library = libraries[i];
                this.Line($"I{library.Name}Plugin {library.Name.ToLowerFirstChar()}{(i + 1 < libraries.Count ? "," : ")")}");
            }

            this.Unindent();
            this.Brace();

            foreach (var library in libraries)
            {
                this.Line($"this.{library.Name.ToLowerFirstChar()} = {library.Name.ToLowerFirstChar()};");
            }

            this.Unbrace();

            foreach (var library in libraries)
            {
                this.Blank();
                this.Line($"public I{library.Name}Plugin {library.Name}");
                this.Brace();

                this.Line("get");
                this.Brace();

                this.Line($"if (this.{library.Name.ToLowerFirstChar()}.IsDefault())");
                this.Brace();
                this.Line(@"throw new InvalidOperationException(""This plugin was not provided to the engine."");");
                this.Unbrace();

                this.Line($"return this.{library.Name.ToLowerFirstChar()};");
                this.Unbrace();
                this.Unbrace();
            }

            this.Line("internal void SetEventsCallback(SuperBasicEngine engine)");
            this.Brace();

            foreach (Library library in libraries.Where(library => library.Events.Any()))
            {
                this.Line($"if (!this.{library.Name.ToLowerFirstChar()}.IsDefault())");
                this.Brace();

                foreach (Event @event in library.Events)
                {
                    this.Line($@"this.{library.Name.ToLowerFirstChar()}.{@event.Name} += () => engine.RaiseEvent(""{library.Name}"", ""{@event.Name}"");");
                }

                this.Unbrace();
            }

            this.Unbrace();
            this.Unbrace();
        }

        private void GeneratePluginInterface(Library library)
        {
            this.Line($"public interface I{library.Name}Plugin");
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
    }
}
