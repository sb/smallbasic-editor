// <copyright file="GenerateDiagnosticCode.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Diagnostics
{
    using System;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateDiagnosticCode : BaseGeneratorTask<DiagnosticsModels.DiagnosticsCollection>
    {
        protected override string Convert(DiagnosticsModels.DiagnosticsCollection root) => $@"
namespace SuperBasic.Compiler.Diagnostics
{{
    using SuperBasic.Utilities;
    using SuperBasic.Utilities.Resources;

    public enum DiagnosticCode
    {{
{root.Select(diagnostic => $"        {diagnostic.Name},").Join(Environment.NewLine)}
    }}

    internal static partial class DiagnosticCodeExtensions
    {{
        public static string ToDisplayString(this DiagnosticCode kind)
        {{
            switch (kind)
            {{
{root.Select(diagnostic => $"                case DiagnosticCode.{diagnostic.Name}: return DiagnosticsResources.{diagnostic.Name};").Join(Environment.NewLine)}
                default: throw ExceptionUtilities.UnexpectedValue(kind);
            }}
        }}
    }}
}}
";
    }
}
