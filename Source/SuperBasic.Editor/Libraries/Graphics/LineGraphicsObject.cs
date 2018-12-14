// <copyright file="LineGraphicsObject.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries.Graphics
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using SuperBasic.Editor.Components;
    using SuperBasic.Editor.Libraries.Utilities;

    internal sealed class LineGraphicsObject : BaseGraphicsObject
    {
        public LineGraphicsObject(decimal x1, decimal y1, decimal x2, decimal y2, GraphicsWindowStyles styles)
            : base(styles)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.X2 = x2;
            this.Y2 = y2;
        }

        public decimal X1 { get; set; }

        public decimal Y1 { get; set; }

        public decimal X2 { get; set; }

        public decimal Y2 { get; set; }

        public override void ComposeTree(TreeComposer composer)
        {
            composer.Element(
                name: "polyline",
                styles: new Dictionary<string, string>
                {
                    { "stroke", this.Styles.PenColor },
                    { "stroke-width", $"{this.Styles.PenWidth}px" },
                },
                attributes: new Dictionary<string, string>
                {
                    { "points", $"{this.X1},{this.Y1} {this.X2},{this.Y2}" },
                });
        }
    }
}
