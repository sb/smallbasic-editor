// <copyright file="ImageShape.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries.Shapes
{
    using System;
    using SuperBasic.Editor.Libraries.Graphics;
    using SuperBasic.Editor.Libraries.Utilities;

    internal sealed class ImageShape : BaseShape<ImageGraphicsObject>
    {
        public ImageShape(string imageName, GraphicsWindowStyles styles)
            : base(new ImageGraphicsObject(x: 0, y: 0, scaleX: 1, scaleY: 1, imageName, styles))
        {
        }

        public override decimal Left => this.Graphics.X;

        public override decimal Top => this.Graphics.Y;
    }
}
