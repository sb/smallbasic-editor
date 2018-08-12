// <copyright file="Diagnostic.Generated.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

/// <summary>
/// This file is auto-generated by a build task. It shouldn't be edited by hand.
/// </summary>
namespace SuperBasic.Compiler.Diagnostics
{
    using SuperBasic.Compiler.Syntax;
    using SuperBasic.Utilities;

    internal sealed class Diagnostic
    {
        private string[] args;

        private Diagnostic(DiagnosticCode kind, TextRange range, params string[] args)
        {
            this.Kind = kind;
            this.Range = range;
            this.args = args;
        }

        public DiagnosticCode Kind { get; private set; }

        public TextRange Range { get; private set; }

        public static Diagnostic UnrecognizedCharacter(TextRange range, char character)
        {
            return new Diagnostic(DiagnosticCode.UnrecognizedCharacter, range, character.ToDisplayString());
        }

        public static Diagnostic UnterminatedStringLiteral(TextRange range)
        {
            return new Diagnostic(DiagnosticCode.UnterminatedStringLiteral, range);
        }

        public string ToDisplayString()
        {
            return string.Format(this.Kind.ToDisplayString(), this.args);
        }
    }
}
