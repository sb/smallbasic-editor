// <copyright file="LineGraphicsObject.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries.Graphics
{
    using System.Collections.Generic;
    using System.Globalization;
    using SuperBasic.Editor.Components;
    using SuperBasic.Editor.Libraries.Utilities;

    internal sealed class LineGraphicsObject : BaseGraphicsObject
    {
        public LineGraphicsObject(decimal x1, decimal y1, decimal x2, decimal y2, GraphicsWindowStyles styles)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.X2 = x2;
            this.Y2 = y2;
            this.Styles = styles;
        }

        public decimal X1 { get; set; }

        public decimal Y1 { get; set; }

        public decimal X2 { get; set; }

        public decimal Y2 { get; set; }

        public GraphicsWindowStyles Styles { get; set; }

        public override void ComposeTree(TreeComposer composer)
        {
            composer.Element(
                name: "line",
                styles: new Dictionary<string, string>
                {
                    { "stroke", this.Styles.PenColor },
                    { "stroke-width", $"{this.Styles.PenWidth}px" },
                },
                attributes: new Dictionary<string, string>
                {
                    { "x1", this.X1.ToString(CultureInfo.CurrentCulture) },
                    { "y1", this.Y1.ToString(CultureInfo.CurrentCulture) },
                    { "x2", this.X2.ToString(CultureInfo.CurrentCulture) },
                    { "y2", this.Y2.ToString(CultureInfo.CurrentCulture) },
                });
        }
    }
}
