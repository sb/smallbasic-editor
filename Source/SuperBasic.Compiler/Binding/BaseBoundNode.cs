// <copyright file="BaseBoundNode.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Binding
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using SuperBasic.Utilities;

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
