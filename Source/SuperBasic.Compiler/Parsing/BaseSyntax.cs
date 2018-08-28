// <copyright file="BaseSyntax.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Parsing
{
    using System.Collections.Generic;
    using SuperBasic.Compiler.Scanning;

    internal abstract class BaseSyntax
    {
        public abstract IEnumerable<BaseSyntax> Children { get; }
    }
}
