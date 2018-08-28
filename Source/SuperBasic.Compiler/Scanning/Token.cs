// <copyright file="Token.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Scanning
{
    internal sealed class Token
    {
        public Token(TokenKind kind, string text, TextRange range)
        {
            this.Kind = kind;
            this.Text = text;
            this.Range = range;
        }

        public TokenKind Kind { get; private set; }

        public string Text { get; private set; }

        public TextRange Range { get; private set; }
    }
}
