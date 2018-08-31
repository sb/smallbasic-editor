// <copyright file="GenerateDiagnosticBag.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Diagnostics
{
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateDiagnosticBag : BaseGeneratorTask<DiagnosticsModels.DiagnosticsCollection>
    {
        protected override void Generate(DiagnosticsModels.DiagnosticsCollection model)
        {
            this.Line("namespace SuperBasic.Compiler.Diagnostics");
            this.Brace();

            this.Line("using System.Collections.Generic;");
            this.Line("using SuperBasic.Compiler.Scanning;");
            this.Line("using SuperBasic.Utilities;");
            this.Blank();

            this.Line("internal sealed class DiagnosticBag");
            this.Brace();

            this.Line("private readonly List<Diagnostic> builder = new List<Diagnostic>();");
            this.Blank();

            this.Line("public IReadOnlyList<Diagnostic> Contents => this.builder;");
            this.Blank();

            foreach (var diagnostic in model)
            {
                string arguments = diagnostic.Parameters.Select(parameter => $", {parameter.Type} {parameter.Name}").Join();
                this.Line($"public void Report{diagnostic.Name}(TextRange range{arguments})");
                this.Brace();

                string displayStrings = diagnostic.Parameters.Select(parameter => $", {parameter.Name}.ToDisplayString()").Join();
                this.Line($"this.builder.Add(new Diagnostic(DiagnosticCode.{diagnostic.Name}, range{displayStrings}));");
                this.Unbrace();
            }

            this.Unbrace();
            this.Unbrace();
        }
    }
}
