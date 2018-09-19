// <copyright file="GenerateLibraries.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Scanning
{
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateLibraries : BaseGeneratorTask<LibraryCollection>
    {
        protected override void Generate(LibraryCollection model)
        {
            this.Line("namespace SuperBasic.Compiler.Runtime");
            this.Brace();

            this.Line("using System;");
            this.Line("using System.Collections.Generic;");
            this.Line("using SuperBasic.Utilities.Resources;");
            this.Blank();

            this.GenerateModelTypes();
            this.GenerateLibrariesType(model);

            this.Unbrace();
        }

        private void GenerateModelTypes()
        {
            this.Line("internal delegate void DExecuteLibraryMember(SuperBasicEngine engine);");
            this.Blank();

            generateType(
                "Library",
                ("string", "Name"),
                ("string", "Description"),
                ("bool", "IsDeprecated"),
                ("ProgramKind?", "ProgramKind"),
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
                ("bool", "IsDeprecated"),
                ("DExecuteLibraryMember", "Execute"),
                ("IReadOnlyDictionary<string, Parameter>", "Parameters"));

            generateType(
                "Property",
                ("string", "Name"),
                ("string", "Description"),
                ("bool", "IsDeprecated"),
                ("DExecuteLibraryMember", "Getter"),
                ("DExecuteLibraryMember", "Setter"));

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
                    var (Type, Name) = members[i];
                    this.Line($"{Type} {Name.ToLowerFirstChar()}{(i + 1 < members.Length ? "," : ")")}");
                }

                this.Unindent();

                this.Brace();

                foreach (var (Type, Name) in members)
                {
                    this.Line($"this.{Name} = {Name.ToLowerFirstChar()};");
                }

                this.Unbrace();

                foreach (var (Type, Name) in members)
                {
                    this.Blank();
                    this.Line($"public {Type} {Name} {{ get; private set; }}");
                }

                this.Unbrace();
            }
        }

        private void GenerateLibrariesType(LibraryCollection model)
        {
            this.Line("internal static class Libraries");
            this.Brace();

            this.Line("public static readonly IReadOnlyDictionary<string, Library> Types;");
            this.Blank();

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

            string[] arguments = new string[]
            {
                $@"""{library.Name}""",
                $"LibrariesResources.{library.Name}",
                $"isDeprecated: {(library.IsDeprecated ? "true" : "false")}",
                $"programKind: {(library.ProgramKind.IsDefault() ? "default" : $"ProgramKind.{library.ProgramKind}")}",
                "methods",
                "properties",
                "events"
            };

            this.Line($@"types.Add(""{library.Name}"", new Library({arguments.Join(", ")}));");
            this.Unbrace();
        }

        private void GenerateMethodsInitialization(Library library)
        {
            this.Line("var methods = new Dictionary<string, Method>();");

            foreach (var method in library.Methods)
            {
                this.Blank();

                this.Line($"// Initialization code for method {library.Name}.{method.Name}:");
                this.Brace();

                this.Line("void execute(SuperBasicEngine engine)");
                this.Brace();

                if (library.IsDeprecated)
                {
                    this.Line($@"throw new InvalidOperationException(""Library '{library.Name}' is deprecated."");");
                }
                else if (method.IsDeprecated)
                {
                    this.Line($@"throw new InvalidOperationException(""Library method '{library.Name}.{method.Name}' is deprecated."");");
                }
                else
                {
                    foreach (var parameter in Enumerable.Reverse(method.Parameters))
                    {
                        this.Line($"{parameter.Type.ToNativeType()} {parameter.Name.ToLowerFirstChar()} = engine.EvaluationStack.Pop(){parameter.Type.ToNativeTypeConverter()};");
                    }

                    string arguments = method.Parameters.Select(p => p.Name.ToLowerFirstChar()).Select(p => $"{p}: {p}").Join(", ");

                    if (method.ReturnType.IsDefault())
                    {
                        this.Line($"engine.Libraries.{library.Name}.{method.Name}({arguments});");
                    }
                    else
                    {
                        this.Line($"{method.ReturnType.ToNativeType()} returnValue = engine.Libraries.{library.Name}.{method.Name}({arguments});");
                        this.Line($@"engine.EvaluationStack.Push({"returnValue".ToValueConstructor(method.ReturnType)});");
                    }
                }

                this.Unbrace();

                this.Line($@"methods.Add(""{method.Name}"", new Method(");
                this.Indent();

                this.Line($@"""{method.Name}"",");
                this.Line($"LibrariesResources.{library.Name}_{method.Name},");

                if (method.ReturnType.IsDefault())
                {
                    this.Line("returnsValue: false,");
                    this.Line("returnValueDescription: null,");
                }
                else
                {
                    this.Line("returnsValue: true,");
                    this.Line($"LibrariesResources.{library.Name}_{method.Name}_ReturnValue,");
                }

                this.Line($"isDeprecated: {(method.IsDeprecated ? "true" : "false")},");
                this.Line("execute: execute,");

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
                this.Unbrace();
            }
        }

        private void GeneratePropertiesInitialization(Library library)
        {
            this.Line("var properties = new Dictionary<string, Property>();");

            foreach (var property in library.Properties)
            {
                this.Blank();

                this.Line($"// Initialization code for property {library.Name}.{property.Name}:");
                this.Brace();

                if (property.HasGetter)
                {
                    this.Line("void getter(SuperBasicEngine engine)");
                    this.Brace();

                    if (library.IsDeprecated)
                    {
                        this.Line($@"throw new InvalidOperationException(""Library '{library.Name}' is deprecated."");");
                    }
                    else if (property.IsDeprecated)
                    {
                        this.Line($@"throw new InvalidOperationException(""Library property '{library.Name}.{property.Name}' is deprecated."");");
                    }
                    else
                    {
                        this.Line($"{property.Type.ToNativeType()} value = engine.Libraries.{library.Name}.{property.Name};");
                        this.Line($"engine.EvaluationStack.Push({"value".ToValueConstructor(property.Type)});");
                    }

                    this.Unbrace();
                }

                if (property.HasSetter)
                {
                    this.Line("void setter(SuperBasicEngine engine)");
                    this.Brace();

                    if (library.IsDeprecated)
                    {
                        this.Line($@"throw new InvalidOperationException(""Library '{library.Name}' is deprecated."");");
                    }
                    else if (property.IsDeprecated)
                    {
                        this.Line($@"throw new InvalidOperationException(""Library property '{library.Name}.{property.Name}' is deprecated."");");
                    }
                    else
                    {
                        this.Line($"{property.Type.ToNativeType()} value = engine.EvaluationStack.Pop(){property.Type.ToNativeTypeConverter()};");
                        this.Line($"engine.Libraries.{library.Name}.{property.Name} = value;");
                    }

                    this.Unbrace();
                }

                string[] arguments = new string[]
                {
                    $@"""{property.Name}""",
                    $"LibrariesResources.{library.Name}_{property.Name}",
                    $"isDeprecated: {(property.IsDeprecated ? "true" : "false")}",
                    $"getter: {(property.HasGetter ? "getter" : "null")}",
                    $"setter: {(property.HasSetter ? "setter" : "null")}",
                };

                this.Line($@"properties.Add(""{property.Name}"", new Property({arguments.Join(", ")}));");
                this.Unbrace();
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
