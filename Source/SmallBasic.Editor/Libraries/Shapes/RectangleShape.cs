// <copyright file="RectangleShape.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Shapes
{
    using SmallBasic.Editor.Libraries.Graphics;
    using SmallBasic.Editor.Libraries.Utilities;

    internal sealed class RectangleShape : BaseShape<RectangleGraphicsObject>
    {
        public RectangleShape(decimal width, decimal height, GraphicsWindowStyles styles)
            : base(new RectangleGraphicsObject(x: 0, y: 0, width, height, styles))
        {
        }

        public override decimal Height => this.Graphics.Height;

        public override decimal Width => this.Graphics.Width;
    }
}
