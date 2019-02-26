// <copyright file="Scanner.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Scanning
{
    using System.Collections.Generic;
    using System.Globalization;
    using SmallBasic.Compiler.Diagnostics;
    using SmallBasic.Utilities;

    internal sealed class Scanner
    {
        private readonly string text;
        private readonly DiagnosticBag diagnostics;

        private readonly List<Token> tokens;

        private int index = 0;
        private int line = 0;
        private int column = 0;

        public Scanner(string text, DiagnosticBag diagnostics)
        {
            this.diagnostics = diagnostics;
            this.text = text;
            this.tokens = new List<Token>();

            while (this.index < this.text.Length)
            {
                this.ScanNextToken();
            }
        }

        public IReadOnlyList<Token> Tokens => this.tokens;

        private void ScanNextToken()
        {
            char current = this.text[this.index];
            char next = this.index + 1 < this.text.Length ? this.text[this.index + 1] : default;

            switch (current)
            {
                case '\r':
                    switch (next)
                    {
                        case '\n': this.index += 2; this.line++; this.column = 0; return;
                        default: this.index++; this.line++; this.column = 0; return;
                    }

                case '\n': this.index++; this.line++; this.column = 0; return;
                case ' ': this.index++; this.column++; return;
                case '\t': this.index++; this.column++; return;

                case '(': this.AddToken("(", TokenKind.LeftParen); return;
                case ')': this.AddToken(")", TokenKind.RightParen); return;
                case '[': this.AddToken("[", TokenKind.LeftBracket); return;
                case ']': this.AddToken("]", TokenKind.RightBracket); return;

                case '.': this.AddToken(".", TokenKind.Dot); return;
                case ',': this.AddToken(",", TokenKind.Comma); return;
                case '=': this.AddToken("=", TokenKind.Equal); return;
                case ':': this.AddToken(":", TokenKind.Colon); return;

                case '+': this.AddToken("+", TokenKind.Plus); return;
                case '-': this.AddToken("-", TokenKind.Minus); return;
                case '*': this.AddToken("*", TokenKind.Multiply); return;
                case '/': this.AddToken("/", TokenKind.Divide); return;

                case '<':
                    switch (next)
                    {
                        case '>': this.AddToken("<>", TokenKind.NotEqual); return;
                        case '=': this.AddToken("<=", TokenKind.LessThanOrEqual); return;
                        default: this.AddToken("<", TokenKind.LessThan); return;
                    }

                case '>':
                    switch (next)
                    {
                        case '=': this.AddToken(">=", TokenKind.GreaterThanOrEqual); return;
                        default: this.AddToken(">", TokenKind.GreaterThan); return;
                    }

                case '\'': this.ScanCommentToken(); return;

                case '\"': this.ScanStringToken(); return;

                case char ch when ch >= '0' && ch <= '9': this.ScanNumberToken(); return;

                case char ch when ch == '_' || (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'): this.ScanWordToken(); return;

                default:
                    {
                        var unrecognizedToken = this.AddToken(current.ToString(CultureInfo.CurrentCulture), TokenKind.Unrecognized);
                        this.diagnostics.ReportUnrecognizedCharacter(unrecognizedToken.Range, current);
                        return;
                    }
            }
        }

        private void ScanCommentToken()
        {
            int lookAhead = this.index;
            while (lookAhead < this.text.Length)
            {
                char current = this.text[lookAhead];
                if (current == '\r' || current == '\n')
                {
                    break;
                }

                lookAhead++;
            }

            this.AddToken(this.text.Substring(this.index, lookAhead - this.index), TokenKind.Comment);
        }

        private void ScanStringToken()
        {
            var lookAhead = this.index + 1;
            while (lookAhead < this.text.Length)
            {
                char current = this.text[lookAhead];
                switch (current)
                {
                    case '\"':
                        {
                            this.AddToken(this.text.Substring(this.index, lookAhead - this.index + 1), TokenKind.StringLiteral);
                            return;
                        }

                    case '\r':
                    case '\n':
                        {
                            Token token = this.AddToken(this.text.Substring(this.index, lookAhead - this.index), TokenKind.StringLiteral);
                            this.diagnostics.ReportUnterminatedStringLiteral(token.Range);
                            return;
                        }

                    default:
                        {
                            lookAhead++;
                            break;
                        }
                }
            }

            Token unrecognizedToken = this.AddToken(this.text.Substring(this.index, lookAhead - this.index), TokenKind.StringLiteral);
            this.diagnostics.ReportUnterminatedStringLiteral(unrecognizedToken.Range);
        }

        private void ScanNumberToken()
        {
            var lookAhead = this.index;
            while (lookAhead < this.text.Length && this.text[lookAhead] >= '0' && this.text[lookAhead] <= '9')
            {
                lookAhead++;
            }

            if (lookAhead < this.text.Length && this.text[lookAhead] == '.')
            {
                lookAhead++;

                while (lookAhead < this.text.Length && this.text[lookAhead] >= '0' && this.text[lookAhead] <= '9')
                {
                    lookAhead++;
                }
            }

            this.AddToken(this.text.Substring(this.index, lookAhead - this.index), TokenKind.NumberLiteral);
        }

        private void ScanWordToken()
        {
            var lookAhead = this.index;
            while (lookAhead < this.text.Length)
            {
                char current = this.text[lookAhead];
                if (current == '_' || (current >= 'a' && current <= 'z') || (current >= 'A' && current <= 'Z') || (current >= '0' && current <= '9'))
                {
                    lookAhead++;
                }
                else
                {
                    break;
                }
            }

            string word = this.text.Substring(this.index, lookAhead - this.index);

            switch (word.ToLower(CultureInfo.CurrentCulture))
            {
                case "if": this.AddToken(word, TokenKind.If); return;
                case "then": this.AddToken(word, TokenKind.Then); return;
                case "else": this.AddToken(word, TokenKind.Else); return;
                case "elseif": this.AddToken(word, TokenKind.ElseIf); return;
                case "endif": this.AddToken(word, TokenKind.EndIf); return;
                case "for": this.AddToken(word, TokenKind.For); return;
                case "to": this.AddToken(word, TokenKind.To); return;
                case "step": this.AddToken(word, TokenKind.Step); return;
                case "endfor": this.AddToken(word, TokenKind.EndFor); return;
                case "goto": this.AddToken(word, TokenKind.GoTo); return;
                case "while": this.AddToken(word, TokenKind.While); return;
                case "endwhile": this.AddToken(word, TokenKind.EndWhile); return;
                case "sub": this.AddToken(word, TokenKind.Sub); return;
                case "endsub": this.AddToken(word, TokenKind.EndSub); return;
                case "or": this.AddToken(word, TokenKind.Or); return;
                case "and": this.AddToken(word, TokenKind.And); return;
                default: this.AddToken(word, TokenKind.Identifier); return;
            }
        }

        private Token AddToken(string text, TokenKind kind)
        {
            Token token = new Token(kind, text, ((this.line, this.column), (this.line, this.column + text.Length - 1)));
            this.index += text.Length;
            this.column += text.Length;

            this.tokens.Add(token);
            return token;
        }
    }
}
