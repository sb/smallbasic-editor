// <copyright file="BaseSyntaxNode.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Parsing
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using SmallBasic.Compiler.Scanning;
    using SmallBasic.Utilities;

    internal abstract class BaseSyntaxNode
    {
        private BaseSyntaxNode parent;

        public BaseSyntaxNode Parent
        {
            get => this.parent;
            set
            {
                Debug.Assert(this.parent.IsDefault(), "Parent node is already set.");
                this.parent = value;
            }
        }

        public abstract IEnumerable<BaseSyntaxNode> Children { get; }

        public abstract TextRange Range { get; }

        public BaseSyntaxNode FindNodeAt(TextPosition position)
        {
            if (!this.Range.Contains(position))
            {
                return null;
            }

            foreach (var child in this.Children)
            {
                var result = child.FindNodeAt(position);
                if (!result.IsDefault())
                {
                    return result;
                }
            }

            return this;
        }
    }
}
