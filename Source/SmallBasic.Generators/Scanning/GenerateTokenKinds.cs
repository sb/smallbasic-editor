// <copyright file="GenerateTokenKinds.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Generators.Scanning
{
    using System;
    using System.Linq;
    using SmallBasic.Utilities;

    public sealed class GenerateTokenKinds : BaseConverterTask<TokenKindCollection>
    {
        protected override void Generate(TokenKindCollection model)
        {
            this.Line("namespace SmallBasic.Compiler.Scanning");
            this.Brace();

            this.Line("using SmallBasic.Utilities;");
            this.Line("using SmallBasic.Utilities.Resources;");
            this.Blank();

            this.Line("internal enum TokenKind");
            this.Brace();

            foreach (var tokenKind in model)
            {
                this.Line($"{tokenKind.Name},");
            }

            this.Unbrace();
            this.Blank();

            this.Line("internal static partial class TokenKindExtensions");
            this.Brace();

            this.Line("public static string ToDisplayString(this TokenKind kind)");
            this.Brace();

            this.Line("switch (kind)");
            this.Brace();

            foreach (var tokenKind in model)
            {
                this.Line($"case TokenKind.{tokenKind.Name}: return {tokenKind.Display};");
            }

            this.Line("default: throw ExceptionUtilities.UnexpectedValue(kind);");

            this.Unbrace();
            this.Unbrace();
            this.Unbrace();
            this.Unbrace();
        }
    }
}
