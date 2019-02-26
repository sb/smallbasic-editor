// <copyright file="TextShape.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Shapes
{
    using SmallBasic.Editor.Libraries.Graphics;
    using SmallBasic.Editor.Libraries.Utilities;

    internal sealed class TextShape : BaseShape<TextGraphicsObject>
    {
        public TextShape(string text, GraphicsWindowStyles styles)
            : base(new TextGraphicsObject(x: 0, y: 0, text, width: default, styles))
        {
        }

        public override decimal Height => this.Graphics.Styles.FontSize;

        public override decimal Width => this.Graphics.Width ?? 0;
    }
}
