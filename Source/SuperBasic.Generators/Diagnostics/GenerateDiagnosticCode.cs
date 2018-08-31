// <copyright file="GenerateDiagnosticCode.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Diagnostics
{
    public sealed class GenerateDiagnosticCode : BaseGeneratorTask<DiagnosticsModels.DiagnosticsCollection>
    {
        protected override void Generate(DiagnosticsModels.DiagnosticsCollection model)
        {
            this.Line("namespace SuperBasic.Compiler.Diagnostics");
            this.Brace();

            this.Line("using SuperBasic.Utilities;");
            this.Line("using SuperBasic.Utilities.Resources;");
            this.Blank();

            this.Line("public enum DiagnosticCode");
            this.Brace();

            foreach (var diagnostic in model)
            {
                this.Line($"{diagnostic.Name},");
            }

            this.Unbrace();

            this.Line("internal static partial class DiagnosticCodeExtensions");
            this.Brace();

            this.Line("public static string ToDisplayString(this DiagnosticCode kind)");
            this.Brace();

            this.Line("switch (kind)");
            this.Brace();

            foreach (var diagnostic in model)
            {
                this.Line($"case DiagnosticCode.{diagnostic.Name}: return DiagnosticsResources.{diagnostic.Name};");
            }

            this.Line("default: throw ExceptionUtilities.UnexpectedValue(kind);");

            this.Unbrace();
            this.Unbrace();
            this.Unbrace();
            this.Unbrace();
        }
    }
}
