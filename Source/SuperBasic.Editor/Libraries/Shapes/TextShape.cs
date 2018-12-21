// <copyright file="TextShape.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries.Shapes
{
    using SuperBasic.Editor.Libraries.Graphics;
    using SuperBasic.Editor.Libraries.Utilities;

    // TODO-later: now to calculate height and width from pen size?
    internal sealed class TextShape : BaseShape<TextGraphicsObject>
    {
        public TextShape(string text, GraphicsWindowStyles styles)
            : base(new TextGraphicsObject(x: 0, y: 0, text, width: default, styles))
        {
        }

        public override decimal Height => 20;

        public override decimal Width => this.Graphics.Width ?? 200;
    }
}
