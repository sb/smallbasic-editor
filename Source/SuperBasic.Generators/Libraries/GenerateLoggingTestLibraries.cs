﻿// <copyright file="GenerateLoggingTestLibraries.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Scanning
{
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateLoggingTestLibraries : BaseGeneratorTask<LibraryCollection>
    {
        protected override void Generate(LibraryCollection model)
        {
            this.Line("namespace SuperBasic.Tests");
            this.Brace();
            this.Line("#pragma warning disable CS0067 // The event '{0}' is never used");

            this.Line("using System;");
            this.Line("using System.Text;");
            this.Line("using SuperBasic.Compiler.Runtime;");
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
                this.Blank();
                this.Line($"public event Action {@event.Name};");
            }

            foreach (Property property in library.Properties.Where(p => !p.IsDeprecated))
            {
                this.Blank();
                this.Line($"public {property.Type.ToNativeType()} {property.Name}");
                this.Brace();

                if (property.HasGetter)
                {
                    this.Line("get");
                    this.Brace();
                    this.Line($@"this.log.AppendLine($""{library.Name}.{property.Name}.Get()"");");
                    this.Line($"return {GetDefaultForType(property.Type)};");
                    this.Unbrace();
                }

                if (property.HasSetter)
                {
                    this.Line("set");
                    this.Brace();
                    this.Line($@"this.log.AppendLine($""{library.Name}.{property.Name}.Set()"");");
                    this.Unbrace();
                }

                this.Unbrace();
            }

            foreach (Method method in library.Methods.Where(m => !m.IsDeprecated))
            {
                this.Blank();
                this.Line($"public {method.ReturnType?.ToNativeType() ?? "void"} {method.Name}({method.Parameters.Select(p => $"{p.Type.ToNativeType()} {p.Name.ToLowerFirstChar()}").Join(", ")})");
                this.Brace();
                this.Line($@"this.log.AppendLine($""{library.Name}.{method.Name}({method.Parameters.Select(p => $"{p.Name.ToLowerFirstChar()}: '{{{GetValueForParameter(p)}}}'").Join(", ")})"");");

                if (!method.ReturnType.IsDefault())
                {
                    this.Line($"return {GetDefaultForType(method.ReturnType)};");
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
                case "NumberValue": return "0";
                case "BooleanValue": return "false";
                case "ArrayValue": return "new ArrayValue()";
                default: throw ExceptionUtilities.UnexpectedValue(type);
            }
        }

        private static string GetValueForParameter(Parameter parameter)
        {
            switch (parameter.Type)
            {
                case "StringValue":
                case "NumberValue":
                case "BooleanValue":
                    return parameter.Name.ToLowerFirstChar();
                case "BaseValue":
                case "ArrayValue":
                    return $"{parameter.Name.ToLowerFirstChar()}.ToDisplayString()";
                default:
                    throw ExceptionUtilities.UnexpectedValue(parameter.Type);
            }
        }
    }
}