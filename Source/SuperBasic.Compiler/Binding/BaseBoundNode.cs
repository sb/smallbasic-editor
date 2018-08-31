// <copyright file="BaseBoundNode.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Binding
{
    using System.Collections.Generic;
    using SuperBasic.Compiler.Parsing;

    internal abstract class BaseBoundNode
    {
        public abstract BaseSyntaxNode Syntax { get; }

        public abstract IEnumerable<BaseBoundNode> Children { get; }
    }
}
