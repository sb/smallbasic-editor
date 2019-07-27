// <copyright file="TriangleShape.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Shapes
{
    using System;
    using SmallBasic.Editor.Libraries.Graphics;
    using SmallBasic.Editor.Libraries.Utilities;

    internal sealed class TriangleShape : BaseShape<TriangleGraphicsObject>
    {
        public TriangleShape(decimal x1, decimal y1, decimal x2, decimal y2, decimal x3, decimal y3, GraphicsWindowStyles styles)
            : base(new TriangleGraphicsObject(x1, y1, x2, y2, x3, y3, styles))
        {
        }

        public override decimal Height => Math.Max(this.Graphics.Y1, Math.Max(this.Graphics.Y2, this.Graphics.Y3)) - Math.Min(this.Graphics.Y1, Math.Min(this.Graphics.Y2, this.Graphics.Y3));

        public override decimal Width => Math.Max(this.Graphics.X1, Math.Max(this.Graphics.X2, this.Graphics.X3)) - Math.Min(this.Graphics.X1, Math.Min(this.Graphics.X2, this.Graphics.X3));
    }
}
