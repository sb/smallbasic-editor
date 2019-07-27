// <copyright file="EllipseGraphicsObject.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Graphics
{
    using System.Collections.Generic;
    using System.Globalization;
    using SmallBasic.Editor.Components;
    using SmallBasic.Editor.Libraries.Utilities;

    internal sealed class EllipseGraphicsObject : BaseGraphicsObject
    {
        public EllipseGraphicsObject(decimal x, decimal y, decimal width, decimal height, GraphicsWindowStyles styles)
            : base(styles)
        {
            this.X = x + width;
            this.Y = y + height;
            this.Width = width;
            this.Height = height;
        }

        public decimal X { get; set; }

        public decimal Y { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }

        public override void ComposeTree(TreeComposer composer)
        {
            composer.Element(
                name: "ellipse",
                styles: new Dictionary<string, string>
                {
                    { "fill", this.Styles.BrushColor },
                    { "stroke", this.Styles.PenColor },
                    { "stroke-width", $"{this.Styles.PenWidth}px" },
                },
                attributes: new Dictionary<string, string>
                {
                    { "cx", (this.X - (this.Width / 2)).ToString(CultureInfo.CurrentCulture) },
                    { "cy", (this.Y - (this.Height / 2)).ToString(CultureInfo.CurrentCulture) },
                    { "rx", (this.Width / 2).ToString(CultureInfo.CurrentCulture) },
                    { "ry", (this.Height / 2).ToString(CultureInfo.CurrentCulture) },
                });
        }
    }
}
