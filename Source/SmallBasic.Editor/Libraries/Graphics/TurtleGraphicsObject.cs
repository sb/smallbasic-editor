// <copyright file="TurtleGraphicsObject.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Graphics
{
    using System.Collections.Generic;
    using System.Globalization;
    using SmallBasic.Editor.Components;
    using SmallBasic.Editor.Libraries.Utilities;

    internal sealed class TurtleGraphicsObject : BaseGraphicsObject
    {
        public TurtleGraphicsObject(GraphicsWindowStyles styles)
            : base(styles)
        {
            this.Width = 48;
            this.Height = 61;
        }

        public decimal Width { get; set; }

        public decimal Height { get; set; }

        public override void ComposeTree(TreeComposer composer)
        {
            composer.Element(
                name: "image",
                attributes: new Dictionary<string, string>
                {
                    { "href", $"Turtle.svg" },
                    /* width and height attributes required on Firefox */
                    { "width", this.Width.ToString(CultureInfo.CurrentCulture) },
                    { "height", this.Height.ToString(CultureInfo.CurrentCulture) }
                });
        }
    }
}
