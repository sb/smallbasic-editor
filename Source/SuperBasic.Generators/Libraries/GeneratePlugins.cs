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

            this.Unbrace();
        }

        private void GeneratePluginInterface(Library library)
        {
            List<Property> properties = library.Properties.Where(p => !p.IsDeprecated).ToList();
            List<Method> methods = library.Methods.Where(m => !m.IsDeprecated).ToList();

            this.Line($"public interface I{library.Name}Plugin");
            this.Brace();

            for (int i = 0; i < properties.Count; i++)
            {
                if (i > 0)
                {
                    this.Blank();
                }

                Property property = properties[i];
                this.Line($"{property.Type.ToNativeType()} {property.Name} {{{(property.HasGetter ? " get;" : string.Empty)}{(property.HasSetter ? " set;" : string.Empty)} }}");
            }

            if (properties.Any() && methods.Any())
            {
                this.Blank();
            }

            for (int i = 0; i < methods.Count; i++)
            {
                if (i > 0)
                {
                    this.Blank();
                }

                Method method = methods[i];

                this.Line($"{method.ReturnType?.ToNativeType() ?? "void"} {method.Name}({method.Parameters.Select(p => $"{p.Type.ToNativeType()} {p.Name.ToLowerFirstChar()}").Join(", ")});");
            }

            this.Unbrace();
        }
    }
}
