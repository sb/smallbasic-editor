// <copyright file="GenerateTokenKinds.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Scanning
{
    using System;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateTokenKinds : BaseGeneratorTask<ScanningModels.TokenKindCollection>
    {
        protected override string Convert(ScanningModels.TokenKindCollection root) => $@"
namespace SuperBasic.Compiler.Scanning
{{
    using SuperBasic.Utilities;
    using SuperBasic.Utilities.Resources;

    internal enum TokenKind
    {{
{root.Select(tokenKind => $"        {tokenKind.Name},").Join(Environment.NewLine)}
    }}

    internal static partial class TokenKindExtensions
    {{
        public static string ToDisplayString(this TokenKind kind)
        {{
            switch (kind)
            {{
{root.Select(tokenKind => $"                case TokenKind.{tokenKind.Name}: return {tokenKind.Display};").Join(Environment.NewLine)}
                default: throw ExceptionUtilities.UnexpectedValue(kind);
            }}
        }}
    }}
}}
";
    }
}
