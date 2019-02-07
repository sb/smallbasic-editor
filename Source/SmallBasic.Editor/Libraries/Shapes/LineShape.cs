// <copyright file="LineShape.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Shapes
{
    using System;
    using SmallBasic.Editor.Libraries.Graphics;
    using SmallBasic.Editor.Libraries.Utilities;

    internal sealed class LineShape : BaseShape<LineGraphicsObject>
    {
        public LineShape(decimal x1, decimal y1, decimal x2, decimal y2, GraphicsWindowStyles styles)
            : base(new LineGraphicsObject(x1, y1, x2, y2, styles))
        {
        }

        public override decimal Height => Math.Abs(this.Graphics.Y1 - this.Graphics.Y2);

        public override decimal Width => Math.Abs(this.Graphics.X1 - this.Graphics.X2);
    }
}
