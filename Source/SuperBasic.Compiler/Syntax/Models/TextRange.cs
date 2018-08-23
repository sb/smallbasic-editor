// <copyright file="TextRange.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Syntax
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{ToDisplayString()}")]
    public readonly struct TextRange : IEquatable<TextRange>
    {
        internal TextRange(TextPosition start, TextPosition end)
        {
            this.Start = start;
            this.End = end;
        }

        public TextPosition Start { get; }

        public TextPosition End { get; }

        public static implicit operator TextRange(in (TextPosition Start, TextPosition End) tuple)
        {
            return new TextRange(tuple.Start, tuple.End);
        }

        public static bool operator ==(TextRange left, TextRange right) => left.Start == right.Start && left.End == right.End;

        public static bool operator !=(TextRange left, TextRange right) => !(left == right);

        public override bool Equals(object obj) => obj is TextRange other && this == other;

        public override int GetHashCode() => this.Start.GetHashCode() ^ this.End.GetHashCode();

        public bool Equals(TextRange other) => this == other;

        public string ToDisplayString() => $"({this.Start.ToDisplayString()}, {this.End.ToDisplayString()})";
    }
}
