// <copyright file="BaseBoundNode.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Binding
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using SmallBasic.Utilities;

    internal abstract class BaseBoundNode
    {
        private BaseBoundNode parent;

        public BaseBoundNode Parent
        {
            get => this.parent;
            set
            {
                Debug.Assert(this.parent.IsDefault(), "Parent node is already set.");
                this.parent = value;
            }
        }

        public abstract IEnumerable<BaseBoundNode> Children { get; }
    }
}
