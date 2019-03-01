// <copyright file="TurtleGraphicsObject.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Graphics
{
    using System.Collections.Generic;
    using System.Globalization;
    using SmallBasic.Editor.Components;
    using SmallBasic.Editor.Libraries.Utilities;

    internal sealed class TurtleGraphicsObject : BaseGraphicsObject
    {
        public TurtleGraphicsObject(GraphicsWindowStyles styles)
            : base(styles)
        {
        }

        public static decimal Width => 48;

        public static decimal Height => 61;

        public override void ComposeTree(TreeComposer composer)
        {
            composer.Element(
                name: "image",
                attributes: new Dictionary<string, string>
                {
                    { "href", $"Turtle.svg" },
                    /* width and height attributes required on Firefox */
                    { "width", Width.ToString(CultureInfo.CurrentCulture) },
                    { "height", Height.ToString(CultureInfo.CurrentCulture) }
                });
        }
    }
}
