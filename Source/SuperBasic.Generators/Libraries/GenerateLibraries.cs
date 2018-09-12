// <copyright file="GenerateLibraries.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateLibraries : BaseGeneratorTask<LibraryCollection>
    {
        protected override void Generate(LibraryCollection model)
        {
            this.Line("namespace SuperBasic.Compiler.Binding");
            this.Brace();

            this.Line("using System.Collections.Generic;");
            this.Line("using SuperBasic.Utilities;");
            this.Line("using SuperBasic.Utilities.Resources;");
            this.Blank();

            this.GenerateModelTypes();
            this.GenerateLibrariesType(model);

            this.Unbrace();
        }

        private void GenerateModelTypes()
        {
            generateType(
                "Library",
                ("string", "Name"),
                ("string", "Description"),
                ("IReadOnlyDictionary<string, Method>", "Methods"),
                ("IReadOnlyDictionary<string, Property>", "Properties"),
                ("IReadOnlyDictionary<string, Event>", "Events"));

            generateType(
                "Parameter",
                ("string", "Name"),
                ("string", "Description"));

            generateType(
                "Method",
                ("string", "Name"),
                ("string", "Description"),
                ("bool", "ReturnsValue"),
                ("string", "ReturnValueDescription"),
                ("IReadOnlyDictionary<string, Parameter>", "Parameters"));

            generateType(
                "Property",
                ("string", "Name"),
                ("string", "Description"),
                ("bool", "HasGetter"),
                ("bool", "HasSetter"));

            generateType(
                "Event",
                ("string", "Name"),
                ("string", "Description"));

            void generateType(string name, params (string Type, string Name)[] members)
            {
                this.Line($"internal sealed class {name}");
                this.Brace();

                this.Line($"public {name}(");
                this.Indent();

                for (var i = 0; i < members.Length; i++)
                {
                    var member = members[i];
                    this.Line($"{member.Type} {member.Name.ToLowerFirstChar()}{(i + 1 < members.Length ? "," : ")")}");
                }

                this.Unindent();

                this.Brace();

                foreach (var member in members)
                {
                    this.Line($"this.{member.Name} = {member.Name.ToLowerFirstChar()};");
                }

                this.Unbrace();

                foreach (var member in members)
                {
                    this.Blank();
                    this.Line($"public {member.Type} {member.Name} {{ get; private set; }}");
                }

                this.Unbrace();
            }
        }

        private void GenerateLibrariesType(LibraryCollection model)
        {
            this.Line("internal static class Libraries");
            this.Brace();

            this.Line("static Libraries()");
            this.Brace();

            this.Line("var types = new Dictionary<string, Library>();");
            this.Blank();

            foreach (var library in model)
            {
                this.GenerateLibraryInitialization(library);
                this.Blank();
            }

            this.Line("Types = types;");
            this.Unbrace();

            this.Blank();
            this.Line("public static IReadOnlyDictionary<string, Library> Types { get; private set; }");

            this.Unbrace();
        }

        private void GenerateLibraryInitialization(Library library)
        {
            this.Line($"// Initialization code for library '{library.Name}'");
            this.Brace();

            this.GenerateMethodsInitialization(library);
            this.Blank();

            this.GeneratePropertiesInitialization(library);
            this.Blank();

            this.GenerateEventsInitialization(library);
            this.Blank();

            this.Line($@"types.Add(""{library.Name}"", new Library(""{library.Name}"", LibrariesResources.{library.Name}, methods, properties, events));");
            this.Unbrace();
        }

        private void GenerateMethodsInitialization(Library library)
        {
            this.Line("var methods = new Dictionary<string, Method>();");

            foreach (var method in library.Methods)
            {
                this.Blank();

                this.Line($@"methods.Add(""{method.Name}"", new Method(");
                this.Indent();

                this.Line($@"""{method.Name}"",");
                this.Line($"LibrariesResources.{library.Name}_{method.Name},");

                if (method.ReturnsValue)
                {
                    this.Line("returnsValue: true,");
                    this.Line($"LibrariesResources.{library.Name}_{method.Name}_ReturnValue,");
                }
                else
                {
                    this.Line("returnsValue: false,");
                    this.Line("returnValueDescription: null,");
                }

                if (method.Parameters.Any())
                {
                    this.Line("new Dictionary<string, Parameter>");
                    this.Line("{");
                    this.Indent();

                    foreach (var parameter in method.Parameters)
                    {
                        this.Line($@"{{ ""{parameter.Name}"", new Parameter(""{parameter.Name}"", LibrariesResources.{library.Name}_{method.Name}_{parameter.Name}) }},");
                    }

                    this.Unindent();
                    this.Line("}));");
                }
                else
                {
                    this.Line("new Dictionary<string, Parameter>()));");
                }

                this.Unindent();
            }
        }

        private void GeneratePropertiesInitialization(Library library)
        {
            if (library.Properties.Any())
            {
                this.Line("var properties = new Dictionary<string, Property>");
                this.Line("{");
                this.Indent();

                foreach (var property in library.Properties)
                {
                    this.Line(
                        $@"{{ ""{property.Name}"", new Property(" +
                        $@"""{property.Name}"", " +
                        $"LibrariesResources.{library.Name}_{property.Name}, " +
                        $"hasGetter: {(property.HasGetter ? "true" : "false")}, " +
                        $"hasSetter: {(property.HasSetter ? "true" : "false")}) }},");
                }

                this.Unindent();
                this.Line("};");
            }
            else
            {
                this.Line("var properties = new Dictionary<string, Property>();");
            }
        }

        private void GenerateEventsInitialization(Library library)
        {
            if (library.Events.Any())
            {
                this.Line("var events = new Dictionary<string, Event>");
                this.Line("{");
                this.Indent();

                foreach (var @event in library.Events)
                {
                    this.Line($@"{{ ""{@event.Name}"", new Event(""{@event.Name}"", LibrariesResources.{library.Name}_{@event.Name}) }},");
                }

                this.Unindent();
                this.Line("};");
            }
            else
            {
                this.Line("var events = new Dictionary<string, Event>();");
            }
        }
    }
}
