// <copyright file="BaseSyntax.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Parsing
{
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Compiler.Scanning;

    internal abstract class BaseSyntax
    {
        public abstract IEnumerable<BaseSyntax> Children { get; }
    }

    internal static partial class BaseSyntaxExtensions
    {
        public static TextRange CalculateRange(this BaseSyntax node)
        {
            TextPosition calculateStart(BaseSyntax child)
            {
                switch (child)
                {
                    case TokenSyntax token: return token.Token.Range.Start;
                    default: return calculateStart(child.Children.First());
                }
            }

            TextPosition calculateEnd(BaseSyntax child)
            {
                switch (child)
                {
                    case TokenSyntax token: return token.Token.Range.End;
                    default: return calculateEnd(child.Children.Last());
                }
            }

            return new TextRange(calculateStart(node), calculateEnd(node));
        }
    }
}
