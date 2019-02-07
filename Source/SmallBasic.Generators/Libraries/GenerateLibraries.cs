// <copyright file="GenerateLibraries.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Generators.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SmallBasic.Utilities;

    public sealed class GenerateLibraries : BaseConverterTask<LibraryCollection>
    {
        protected override void Generate(LibraryCollection model)
        {
            this.Line("namespace SmallBasic.Compiler.Runtime");
            this.Brace();

            this.Line("using System;");
            this.Line("using System.Collections.Generic;");
            this.Line("using System.Threading.Tasks;");
            this.Line("using SmallBasic.Utilities.Resources;");
            this.Blank();

            this.Line("internal delegate Task DExecuteLibraryMember(SmallBasicEngine engine);");
            this.Blank();

            this.GenerateModelTypes();
            this.GenerateLibrariesType(model);

            this.Unbrace();
        }

        private void GenerateModelTypes()
        {
            generateType(
                "Library",
                ("public", "string", "Name"),
                ("public", "string", "Description"),
                ("public", "string", "ExplorerIcon"),
                ("public", "bool", "UsesGraphicsWindow"),
                ("public", "bool", "UsesTextWindow"),
                ("public", "IReadOnlyDictionary<string, Method>", "Methods"),
                ("public", "IReadOnlyDictionary<string, Property>", "Properties"),
                ("public", "IReadOnlyDictionary<string, Event>", "Events"));

            generateType(
                "Parameter",
                ("public", "string", "Name"),
                ("public", "string", "Description"));

            generateType(
                "Method",
                ("public", "string", "Name"),
                ("public", "string", "Description"),
                ("public", "bool", "ReturnsValue"),
                ("public", "string", "ReturnValueDescription"),
                ("public", "IReadOnlyDictionary<string, Parameter>", "Parameters"),
                ("internal", "bool", "IsDeprecated"),
                ("internal", "bool", "NeedsDesktop"),
                ("internal", "DExecuteLibraryMember", "Execute"));

            generateType(
                "Property",
                ("public", "string", "Name"),
                ("public", "string", "Description"),
                ("internal", "bool", "IsDeprecated"),
                ("internal", "bool", "NeedsDesktop"),
                ("internal", "DExecuteLibraryMember", "Getter"),
                ("internal", "DExecuteLibraryMember", "Setter"));

            generateType(
                "Event",
                ("public", "string", "Name"),
                ("public", "string", "Description"));

            void generateType(string name, params (string Visibility, string Type, string Name)[] members)
            {
                this.Line($"public sealed class {name}");
                this.Brace();

                this.Line($"internal {name}(");
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
                    this.Line($"{member.Visibility} {member.Type} {member.Name} {{ get; private set; }}");
                }

                this.Unbrace();
            }
        }

        private void GenerateLibrariesType(LibraryCollection model)
        {
            this.Line("public static class Libraries");
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
                $@"""{library.ExplorerIcon}""",
                $"usesGraphicsWindow: {(library.UsesGraphicsWindow ? "true" : "false")}",
                $"usesTextWindow: {(library.UsesTextWindow ? "true" : "false")}",
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

                this.Line($"{(method.IsAsync && !method.ReturnType.IsDefault() ? "async " : string.Empty)}Task execute(SmallBasicEngine engine)");
                this.Brace();

                if (method.IsDeprecated)
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
                        if (method.IsAsync)
                        {
                            this.Line($"return engine.Libraries.{library.Name}.{method.Name}({arguments});");
                        }
                        else
                        {
                            this.Line($"engine.Libraries.{library.Name}.{method.Name}({arguments});");
                            this.Line("return Task.CompletedTask;");
                        }
                    }
                    else
                    {
                        if (method.IsAsync)
                        {
                            this.Line($"{method.ReturnType.ToNativeType()} returnValue = await engine.Libraries.{library.Name}.{method.Name}({arguments}).ConfigureAwait(false);");
                            this.Line($@"engine.EvaluationStack.Push({"returnValue".ToValueConstructor(method.ReturnType)});");
                        }
                        else
                        {
                            this.Line($"{method.ReturnType.ToNativeType()} returnValue = engine.Libraries.{library.Name}.{method.Name}({arguments});");
                            this.Line($@"engine.EvaluationStack.Push({"returnValue".ToValueConstructor(method.ReturnType)});");
                            this.Line("return Task.CompletedTask;");
                        }
                    }
                }

                this.Unbrace();

                this.Line($@"methods.Add(""{method.Name}"", new Method(");
                this.Indent();

                this.Line($@"name: ""{method.Name}"",");
                this.Line($"description: LibrariesResources.{library.Name}_{method.Name},");

                if (method.ReturnType.IsDefault())
                {
                    this.Line("returnsValue: false,");
                    this.Line("returnValueDescription: null,");
                }
                else
                {
                    this.Line("returnsValue: true,");
                    this.Line($"returnValueDescription: LibrariesResources.{library.Name}_{method.Name}_ReturnValue,");
                }

                if (method.Parameters.Any())
                {
                    this.Line("parameters: new Dictionary<string, Parameter>");
                    this.Line("{");
                    this.Indent();

                    foreach (var parameter in method.Parameters)
                    {
                        this.Line($@"{{ ""{parameter.Name}"", new Parameter(""{parameter.Name}"", LibrariesResources.{library.Name}_{method.Name}_{parameter.Name}) }},");
                    }

                    this.Unindent();
                    this.Line("},");
                }
                else
                {
                    this.Line("parameters: new Dictionary<string, Parameter>(),");
                }

                this.Line($"isDeprecated: {(method.IsDeprecated ? "true" : "false")},");
                this.Line($"needsDesktop: {(method.NeedsDesktop ? "true" : "false")},");
                this.Line("execute: execute));");

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
                    this.Line($"{(property.IsAsync ? "async " : string.Empty)}Task getter(SmallBasicEngine engine)");
                    this.Brace();

                    if (property.IsDeprecated)
                    {
                        this.Line($@"throw new InvalidOperationException(""Library property '{library.Name}.{property.Name}' is deprecated."");");
                    }
                    else
                    {
                        if (property.IsAsync)
                        {
                            this.Line($"{property.Type.ToNativeType()} value = await engine.Libraries.{library.Name}.Get_{property.Name}().ConfigureAwait(false);");
                            this.Line($"engine.EvaluationStack.Push({"value".ToValueConstructor(property.Type)});");
                        }
                        else
                        {
                            this.Line($"{property.Type.ToNativeType()} value = engine.Libraries.{library.Name}.Get_{property.Name}();");
                            this.Line($"engine.EvaluationStack.Push({"value".ToValueConstructor(property.Type)});");
                            this.Line("return Task.CompletedTask;");
                        }
                    }

                    this.Unbrace();
                }

                if (property.HasSetter)
                {
                    this.Line("Task setter(SmallBasicEngine engine)");
                    this.Brace();

                    if (property.IsDeprecated)
                    {
                        this.Line($@"throw new InvalidOperationException(""Library property '{library.Name}.{property.Name}' is deprecated."");");
                    }
                    else
                    {
                        if (property.IsAsync)
                        {
                            this.Line($"{property.Type.ToNativeType()} value = engine.EvaluationStack.Pop(){property.Type.ToNativeTypeConverter()};");
                            this.Line($"return engine.Libraries.{library.Name}.Set_{property.Name}(value);");
                        }
                        else
                        {
                            this.Line($"{property.Type.ToNativeType()} value = engine.EvaluationStack.Pop(){property.Type.ToNativeTypeConverter()};");
                            this.Line($"engine.Libraries.{library.Name}.Set_{property.Name}(value);");
                            this.Line("return Task.CompletedTask;");
                        }
                    }

                    this.Unbrace();
                }

                string[] arguments = new string[]
                {
                    $@"""{property.Name}""",
                    $"LibrariesResources.{library.Name}_{property.Name}",
                    $"isDeprecated: {(property.IsDeprecated ? "true" : "false")}",
                    $"needsDesktop: {(property.NeedsDesktop ? "true" : "false")}",
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
