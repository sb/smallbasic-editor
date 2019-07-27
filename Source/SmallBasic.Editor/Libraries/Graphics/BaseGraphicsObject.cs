// <copyright file="BaseGraphicsObject.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Graphics
{
    using SmallBasic.Editor.Components;
    using SmallBasic.Editor.Libraries.Utilities;

    internal abstract class BaseGraphicsObject
    {
        protected BaseGraphicsObject(GraphicsWindowStyles styles)
        {
            this.Styles = styles;
        }

        public GraphicsWindowStyles Styles { get; set; }

        public abstract void ComposeTree(TreeComposer composer);
    }
}
