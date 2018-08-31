// <copyright file="GenerateLibraries.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Scanning
{
    using System;
    using System.Globalization;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateLibraries : BaseGeneratorTask<LibrariesModels.LibraryCollection>
    {
        protected override void Generate(LibrariesModels.LibraryCollection model)
        {
            this.Line("namespace SuperBasic.Compiler.Parsing");
            this.Brace();

            this.Line("using SuperBasic.Utilities;");
            this.Blank();

            this.Line("internal static class LibrariesMetadata");
            this.Brace();
            this.GenerateLibraryExists(model);
            this.GenerateMethodExists(model);
            this.GenerateGetMethodParameterCount(model);
            this.GenerateDoesMethodReturnValue(model);
            this.GeneratePropertyExists(model);
            this.GeneratePropertyAccess(model, "HasGetter", property => property.HasGetter);
            this.GeneratePropertyAccess(model, "HasSetter", property => property.HasSetter);
            this.Unbrace();

            this.Unbrace();
        }

        private void GenerateLibraryExists(LibrariesModels.LibraryCollection model)
        {
            this.Line("public static bool LibraryExists(string library)");
            this.Brace();

            this.Line("switch (library)");
            this.Brace();

            foreach (var library in model)
            {
                this.Line($@"case ""{library.Name}"":");
            }

            this.Indent();
            this.Line("return true;");
            this.Unindent();

            this.Unbrace();

            this.Line("return false;");
            this.Unbrace();
        }

        private void GenerateMethodExists(LibrariesModels.LibraryCollection model)
        {
            this.Line("public static bool MethodExists(string library, string method)");
            this.Brace();

            this.Line("switch (library)");
            this.Brace();

            foreach (var library in model.Where(library => library.Methods.Any()))
            {
                this.Line($@"case ""{library.Name}"":");
                this.Brace();

                this.Line("switch (method)");
                this.Brace();

                foreach (var method in library.Methods)
                {
                    this.Line($@"case ""{method.Name}"":");
                }

                this.Indent();
                this.Line("return true;");
                this.Unindent();

                this.Unbrace();

                this.Line("break;");
                this.Unbrace();
            }

            this.Unbrace();

            this.Line("return false;");
            this.Unbrace();
        }

        private void GenerateGetMethodParameterCount(LibrariesModels.LibraryCollection model)
        {
            this.Line($"public static int GetMethodParameterCount(string library, string method)");
            this.Brace();

            this.Line("switch (library)");
            this.Brace();

            foreach (var library in model.Where(library => library.Methods.Any()))
            {
                this.Line($@"case ""{library.Name}"":");
                this.Brace();

                this.Line("switch (method)");
                this.Brace();

                foreach (var method in library.Methods)
                {
                    this.Line($@"case ""{method.Name}"": return {method.Parameters.Count};");
                }

                this.Unbrace();

                this.Line("break;");
                this.Unbrace();
            }

            this.Unbrace();

            this.Line($@"throw ExceptionUtilities.UnexpectedValue($""{{library}}.{{method}}"");");
            this.Unbrace();
        }

        private void GenerateDoesMethodReturnValue(LibrariesModels.LibraryCollection model)
        {
            this.Line($"public static bool DoesMethodReturnValue(string library, string method)");
            this.Brace();

            this.Line("switch (library)");
            this.Brace();

            foreach (var library in model.Where(library => library.Methods.Any()))
            {
                this.Line($@"case ""{library.Name}"":");
                this.Brace();

                this.Line("switch (method)");
                this.Brace();

                foreach (var method in library.Methods)
                {
                    string returnsValue = method.ReturnsValue.ToString(CultureInfo.CurrentCulture).ToLowerFirstChar();
                    this.Line($@"case ""{method.Name}"": return {returnsValue};");
                }

                this.Unbrace();

                this.Line("break;");
                this.Unbrace();
            }

            this.Unbrace();

            this.Line($@"throw ExceptionUtilities.UnexpectedValue($""{{library}}.{{method}}"");");
            this.Unbrace();
        }

        private void GeneratePropertyExists(LibrariesModels.LibraryCollection model)
        {
            this.Line("public static bool PropertyExists(string library, string property)");
            this.Brace();

            this.Line("switch (library)");
            this.Brace();

            foreach (var library in model.Where(library => library.Properties.Any()))
            {
                this.Line($@"case ""{library.Name}"":");
                this.Brace();

                this.Line("switch (property)");
                this.Brace();

                foreach (var property in library.Properties)
                {
                    this.Line($@"case ""{property.Name}"":");
                }

                this.Indent();
                this.Line("return true;");
                this.Unindent();

                this.Unbrace();

                this.Line("break;");
                this.Unbrace();
            }

            this.Unbrace();

            this.Line("return false;");
            this.Unbrace();
        }

        private void GeneratePropertyAccess(LibrariesModels.LibraryCollection model, string methodName, Func<LibrariesModels.Property, bool> accessExists)
        {
            this.Line($"public static bool {methodName}(string library, string property)");
            this.Brace();

            this.Line("switch (library)");
            this.Brace();

            foreach (var library in model.Where(library => library.Properties.Any()))
            {
                this.Line($@"case ""{library.Name}"":");
                this.Brace();

                this.Line("switch (property)");
                this.Brace();

                void writeSwitch(bool returnValue)
                {
                    var properties = library.Properties.Where(property => accessExists(property) == returnValue);
                    if (properties.Any())
                    {
                        foreach (var property in properties)
                        {
                            this.Line($@"case ""{property.Name}"":");
                        }

                        this.Indent();
                        this.Line($"return {returnValue.ToString(CultureInfo.CurrentCulture).ToLowerFirstChar()};");
                        this.Unindent();
                    }
                }

                writeSwitch(true);
                writeSwitch(false);

                this.Unbrace();

                this.Line("break;");
                this.Unbrace();
            }

            this.Unbrace();

            this.Line($@"throw ExceptionUtilities.UnexpectedValue($""{{library}}.{{property}}"");");
            this.Unbrace();
        }
    }
}
