﻿// <copyright file="GenerateLoggingTestLibraries.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Generators.Scanning
{
    using System.Linq;
    using SmallBasic.Utilities;

    public sealed class GenerateLoggingTestLibraries : BaseConverterTask<LibraryCollection>
    {
        protected override void Generate(LibraryCollection model)
        {
            this.Line("namespace SmallBasic.Tests");
            this.Brace();
            this.Line("#pragma warning disable CS0067 // The event '{0}' is never used");

            this.Line("using System;");
            this.Line("using System.Text;");
            this.Line("using System.Threading.Tasks;");
            this.Line("using SmallBasic.Compiler.Runtime;");
            this.Blank();

            foreach (Library library in model)
            {
                this.GenerateLibraryImplementation(library);
            }

            this.GenerateLibrariesCollection(model);

            this.Line("#pragma warning restore CS0067 // The event '{0}' is never used");
            this.Unbrace();
        }

        private void GenerateLibraryImplementation(Library library)
        {
            this.Line($"internal sealed class Logging{library.Name}Library : I{library.Name}Library");
            this.Brace();

            this.Line("private readonly StringBuilder log;");
            this.Blank();

            this.Line($"public Logging{library.Name}Library(StringBuilder log)");
            this.Brace();
            this.Line("this.log = log;");
            this.Unbrace();

            foreach (Event @event in library.Events)
            {
                this.Line($"public event Action {@event.Name};");
                this.Blank();
            }

            foreach (Property property in library.Properties.Where(p => !p.IsDeprecated))
            {
                if (property.HasGetter)
                {
                    string type = property.IsAsync
                        ? $"Task<{property.Type.ToNativeType()}>"
                        : property.Type.ToNativeType();

                    this.Line($"public {type} Get_{property.Name}()");
                    this.Brace();

                    this.Line($@"this.log.AppendLine($""{library.Name}.Get_{property.Name}()"");");

                    if (property.IsAsync)
                    {
                        this.Line($"return Task.FromResult({GetDefaultForType(property.Type)});");
                    }
                    else
                    {
                        this.Line($"return {GetDefaultForType(property.Type)};");
                    }

                    this.Unbrace();
                }

                if (property.HasSetter)
                {
                    this.Line($"public {(property.IsAsync ? "Task" : "void")} Set_{property.Name}({property.Type.ToNativeType()} value)");
                    this.Brace();

                    this.Line($@"this.log.AppendLine($""{library.Name}.Set_{property.Name}('{{{PrintValueOfType("value", property.Type)}}}')"");");

                    if (property.IsAsync)
                    {
                        this.Line($"return Task.CompletedTask;");
                    }

                    this.Unbrace();
                }
            }

            foreach (Method method in library.Methods.Where(m => !m.IsDeprecated))
            {
                string type = method.ReturnType.IsDefault()
                    ? (method.IsAsync ? "Task" : "void")
                    : (method.IsAsync ? $"Task<{method.ReturnType.ToNativeType()}>" : method.ReturnType.ToNativeType());

                this.Line($"public {type} {method.Name}({method.Parameters.Select(p => $"{p.Type.ToNativeType()} {p.Name.ToLowerFirstChar()}").Join(", ")})");
                this.Brace();

                this.Line($@"this.log.AppendLine($""{library.Name}.{method.Name}({method.Parameters.Select(p => $"{p.Name.ToLowerFirstChar()}: '{{{PrintValueOfType(p.Name, p.Type)}}}'").Join(", ")})"");");

                if (method.ReturnType.IsDefault())
                {
                    if (method.IsAsync)
                    {
                        this.Line("return Task.CompletedTask;");
                    }
                }
                else
                {
                    if (method.IsAsync)
                    {
                        this.Line($"return Task.FromResult({GetDefaultForType(method.ReturnType)});");
                    }
                    else
                    {
                        this.Line($"return {GetDefaultForType(method.ReturnType)};");
                    }
                }

                this.Unbrace();
            }

            this.Unbrace();
        }

        private void GenerateLibrariesCollection(LibraryCollection model)
        {
            this.Line("internal sealed class LoggingEngineLibraries : IEngineLibraries");
            this.Brace();

            this.Line("public LoggingEngineLibraries(StringBuilder log)");
            this.Brace();

            foreach (var library in model)
            {
                this.Line($"this.{library.Name} = new Logging{library.Name}Library(log);");
            }

            this.Unbrace();

            foreach (var library in model)
            {
                this.Blank();
                this.Line($"public I{library.Name}Library {library.Name} {{ get; private set; }}");
            }

            this.Unbrace();
        }

        private static string GetDefaultForType(string type)
        {
            switch (type)
            {
                case "StringValue": return "string.Empty";
                case "NumberValue": return "0m";
                case "BooleanValue": return "false";
                case "ArrayValue": return "new ArrayValue()";
                case "BaseValue": return "StringValue.Create(string.Empty)";
                default: throw ExceptionUtilities.UnexpectedValue(type);
            }
        }

        private static string PrintValueOfType(string name, string type)
        {
            switch (type)
            {
                case "StringValue":
                case "NumberValue":
                case "BooleanValue":
                    return name;
                case "BaseValue":
                case "ArrayValue":
                    return $"{name}.ToDisplayString()";
                default:
                    throw ExceptionUtilities.UnexpectedValue(type);
            }
        }
    }
}