// <copyright file="TriangleGraphicsObject.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Graphics
{
    using System.Collections.Generic;
    using SmallBasic.Editor.Components;
    using SmallBasic.Editor.Libraries.Utilities;

    internal sealed class TriangleGraphicsObject : BaseGraphicsObject
    {
        public TriangleGraphicsObject(decimal x1, decimal y1, decimal x2, decimal y2, decimal x3, decimal y3, GraphicsWindowStyles styles)
            : base(styles)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.X2 = x2;
            this.Y2 = y2;
            this.X3 = x3;
            this.Y3 = y3;
        }

        public decimal X1 { get; set; }

        public decimal Y1 { get; set; }

        public decimal X2 { get; set; }

        public decimal Y2 { get; set; }

        public decimal X3 { get; set; }

        public decimal Y3 { get; set; }

        public override void ComposeTree(TreeComposer composer)
        {
            composer.Element(
                name: "polygon",
                styles: new Dictionary<string, string>
                {
                    { "fill", this.Styles.BrushColor },
                    { "stroke", this.Styles.PenColor },
                    { "stroke-width", $"{this.Styles.PenWidth}px" },
                },
                attributes: new Dictionary<string, string>
                {
                    { "points", string.Join(",", this.X1, this.Y1, this.X2, this.Y2, this.X3, this.Y3) }
                });
        }
    }
}
