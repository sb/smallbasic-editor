// <copyright file="Token.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Scanning
{
    using System.Diagnostics;

    [DebuggerDisplay("{ToDisplayString()}")]
    internal sealed class Token
    {
        public Token(TokenKind kind, string text, TextRange range)
        {
            Debug.Assert(range.Start.Line == range.End.Line, "Tokens should never span multiple lines");

            this.Kind = kind;
            this.Text = text;
            this.Range = range;
        }

        public TokenKind Kind { get; private set; }

        public string Text { get; private set; }

        public TextRange Range { get; private set; }

        public string ToDisplayString() => $"{nameof(TokenKind)}.{this.Kind}: '{this.Text}' at {this.Range.ToDisplayString()}";
    }
}
