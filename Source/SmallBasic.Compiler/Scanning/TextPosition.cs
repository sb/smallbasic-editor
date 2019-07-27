// <copyright file="TextPosition.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Scanning
{
    using System;
    using System.Diagnostics;
    using SmallBasic.Compiler.Services;

    [DebuggerDisplay("{ToDisplayString()}")]
    public readonly struct TextPosition : IEquatable<TextPosition>
    {
        internal TextPosition(int line, int column)
        {
            this.Line = line;
            this.Column = column;
        }

        public int Line { get; }

        public int Column { get; }

        public static implicit operator TextPosition(in (int Line, int Column) tuple)
        {
            return new TextPosition(tuple.Line, tuple.Column);
        }

        public static bool operator ==(TextPosition left, TextPosition right) => left.Line == right.Line && left.Column == right.Column;

        public static bool operator !=(TextPosition left, TextPosition right) => !(left == right);

        public static bool operator <(TextPosition left, TextPosition right) => left.Line < right.Line || (left.Line == right.Line && left.Column < right.Column);

        public static bool operator >(TextPosition left, TextPosition right) => left.Line > right.Line || (left.Line == right.Line && left.Column > right.Column);

        public static bool operator <=(TextPosition left, TextPosition right) => left < right || left == right;

        public static bool operator >=(TextPosition left, TextPosition right) => left > right || left == right;

        public override bool Equals(object obj) => obj is TextPosition other && this == other;

        public override int GetHashCode() => this.Line ^ this.Column;

        public bool Equals(TextPosition other) => this == other;

        public string ToDisplayString() => $"({this.Line}, {this.Column})";

        public MonacoPosition ToMonacoPosition() => new MonacoPosition
        {
            lineNumber = this.Line + 1,
            column = this.Column + 1
        };
    }
}
