// <copyright file="Diagnostic.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Diagnostics
{
    using System.Globalization;
    using SuperBasic.Compiler.Syntax;

    public sealed class Diagnostic
    {
        private string[] args;

        internal Diagnostic(DiagnosticCode kind, TextRange range, params string[] args)
        {
            this.Kind = kind;
            this.Range = range;
            this.args = args;
        }

        public DiagnosticCode Kind { get; private set; }

        public TextRange Range { get; private set; }

        public string ToDisplayString()
        {
            return string.Format(CultureInfo.CurrentCulture, this.Kind.ToDisplayString(), this.args);
        }
    }
}
