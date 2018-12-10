// <copyright file="BaseControl.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries.Controls
{
    using System.Collections.Generic;
    using SuperBasic.Editor.Components;

    internal abstract class BaseControl
    {
        protected BaseControl(string name, decimal left, decimal top, decimal width, decimal height)
        {
            this.Name = name;
            this.Left = left;
            this.Top = top;
            this.Width = width;
            this.Height = height;
            this.Visible = true;
        }

        public string Name { get; set; }

        public decimal Left { get; set; }

        public decimal Top { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }

        public bool Visible { get; set; }

        protected IReadOnlyDictionary<string, string> Styles => new Dictionary<string, string>
        {
            { "left", $"{this.Left}px" },
            { "top", $"{this.Top}px" },
            { "width", $"{this.Width}px" },
            { "height", $"{this.Height}px" },
            { "visibility", this.Visible ? "visible" : "hidden" }
        };

        public abstract void ComposeTree(ControlsLibrary library, TreeComposer composer);
    }
}
