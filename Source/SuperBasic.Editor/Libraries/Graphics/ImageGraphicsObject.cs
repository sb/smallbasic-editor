// <copyright file="ImageGraphicsObject.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries.Graphics
{
    using System.Collections.Generic;
    using System.Globalization;
    using SuperBasic.Editor.Components;
    using SuperBasic.Editor.Libraries.Utilities;

    internal sealed class ImageGraphicsObject : BaseGraphicsObject
    {
        public ImageGraphicsObject(decimal x, decimal y, decimal scaleX, decimal scaleY, string name, GraphicsWindowStyles styles)
            : base(styles)
        {
            this.X = x;
            this.Y = y;
            this.ScaleX = scaleX;
            this.ScaleY = scaleY;
            this.Name = name;
        }

        public decimal X { get; set; }

        public decimal Y { get; set; }

        public decimal ScaleX { get; set; }

        public decimal ScaleY { get; set; }

        public string Name { get; set; }

        public override void ComposeTree(TreeComposer composer)
        {
            composer.Element(
                name: "use",
                attributes: new Dictionary<string, string>
                {
                    { "href", $"#{this.Name}" },
                    { "x", this.X.ToString(CultureInfo.CurrentCulture) },
                    { "y", this.Y.ToString(CultureInfo.CurrentCulture) },
                    { "transform", $"scale({this.ScaleX}, {this.ScaleY})" }
                });
        }
    }
}
