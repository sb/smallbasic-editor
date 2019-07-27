// <copyright file="TurtleShape.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
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

        public override decimal Height => TurtleGraphicsObject.Height;

        public override decimal Width => TurtleGraphicsObject.Width;
    }
}
