// <copyright file="TurtleShape.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Shapes
{
    using SmallBasic.Editor.Libraries.Graphics;
    using SmallBasic.Editor.Libraries.Utilities;

    internal sealed class TurtleShape : BaseShape<TurtleGraphicsObject>
    {
        public TurtleShape(GraphicsWindowStyles styles)
            : base(new TurtleGraphicsObject(styles))
        {
        }

        public override decimal Height => 61;

        public override decimal Width => 48;
    }
}
