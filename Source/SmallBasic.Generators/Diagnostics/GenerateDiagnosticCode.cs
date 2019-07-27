// <copyright file="GenerateDiagnosticCode.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Generators.Diagnostics
{
    public sealed class GenerateDiagnosticCode : BaseConverterTask<DiagnosticsCollection>
    {
        protected override void Generate(DiagnosticsCollection model)
        {
            this.Line("namespace SmallBasic.Compiler.Diagnostics");
            this.Brace();

            this.Line("using SmallBasic.Utilities;");
            this.Line("using SmallBasic.Utilities.Resources;");
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
