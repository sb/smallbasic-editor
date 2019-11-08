// <copyright file="Diagnostic.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Diagnostics
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using SmallBasic.Compiler.Scanning;

    [DebuggerDisplay("{ToDisplayString()}")]
    public sealed class Diagnostic
    {
        private string[] args;

        public Diagnostic(DiagnosticCode code, TextRange range, params string[] args)
        {
            this.Code = code;
            this.Range = range;
            this.args = args;
        }

        public DiagnosticCode Code { get; private set; }

        public TextRange Range { get; private set; }

        public IReadOnlyList<string> Args => this.args;

        public string ToDisplayString() => string.Format(CultureInfo.CurrentCulture, this.Code.ToDisplayString(), this.args);

        public bool IsFatal() => this.Code != DiagnosticCode.LibraryMemberDeprecatedFromOlderVersion;
    }
}
