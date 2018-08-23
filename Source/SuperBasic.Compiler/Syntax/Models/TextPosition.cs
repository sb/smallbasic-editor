// <copyright file="TextPosition.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Syntax
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{ToDisplayString()}")]
    public readonly struct TextPosition : IEquatable<TextPosition>
    {
        internal TextPosition(short line, short column)
        {
            this.Line = line;
            this.Column = column;
        }

        public short Line { get; }

        public short Column { get; }

        public static implicit operator TextPosition(in (short Line, short Column) tuple)
        {
            return new TextPosition(tuple.Line, tuple.Column);
        }

        public static bool operator ==(TextPosition left, TextPosition right) => left.Line == right.Line && left.Column == right.Column;

        public static bool operator !=(TextPosition left, TextPosition right) => !(left == right);

        public override bool Equals(object obj) => obj is TextPosition other && this == other;

        public override int GetHashCode() => this.Line ^ this.Column;

        public bool Equals(TextPosition other) => this == other;

        public string ToDisplayString() => $"({this.Line}, {this.Column})";
    }
}
