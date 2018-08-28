// <copyright file="GenerateDiagnosticBag.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Diagnostics
{
    using System;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateDiagnosticBag : BaseGeneratorTask<DiagnosticsModels.DiagnosticsCollection>
    {
        protected override string Convert(DiagnosticsModels.DiagnosticsCollection root) => $@"
namespace SuperBasic.Compiler.Diagnostics
{{
    using System.Collections.Generic;
    using SuperBasic.Compiler.Scanning;
    using SuperBasic.Utilities;

    internal sealed class DiagnosticBag
    {{
        private readonly List<Diagnostic> builder = new List<Diagnostic>();

        public IReadOnlyList<Diagnostic> Contents => this.builder;
{root.Select(this.GenerateReportMethod).Join(Environment.NewLine)}
    }}
}}
";

        private string GenerateReportMethod(DiagnosticsModels.Diagnostic diagnostic) => $@"
        public void Report{diagnostic.Name}(TextRange range{diagnostic.Parameters.Select(parameter => $", {parameter.Type} {parameter.Name}").Join(string.Empty)})
        {{
            this.builder.Add(new Diagnostic(DiagnosticCode.{diagnostic.Name}, range{diagnostic.Parameters.Select(parameter => $", {parameter.Name}.ToDisplayString()").Join(string.Empty)}));
        }}";
    }
}
