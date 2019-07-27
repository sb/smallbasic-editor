// <copyright file="EllipseShape.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Shapes
{
    using SmallBasic.Editor.Libraries.Graphics;
    using SmallBasic.Editor.Libraries.Utilities;

    internal sealed class EllipseShape : BaseShape<EllipseGraphicsObject>
    {
        public EllipseShape(decimal width, decimal height, GraphicsWindowStyles styles)
            : base(new EllipseGraphicsObject(x: 0, y: 0, width, height, styles))
        {
        }

        public override decimal Height => this.Graphics.Height;

        public override decimal Width => this.Graphics.Width;
    }
}
