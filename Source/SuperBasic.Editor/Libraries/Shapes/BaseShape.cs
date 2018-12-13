// <copyright file="BaseShape.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries.Shapes
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using SuperBasic.Editor.Components;
    using SuperBasic.Editor.Libraries.Graphics;

    internal abstract class BaseShape
    {
        public BaseShape()
        {
            this.Opacity = 100;
            this.IsVisible = true;
            this.Angle = 0;
            this.ScaleX = 1;
            this.ScaleY = 1;
            this.TranslateX = 0;
            this.TranslateY = 0;
        }

        public decimal Opacity { get; set; }

        public bool IsVisible { get; set; }

        public decimal Angle { get; set; }

        public decimal ScaleX { get; set; }

        public decimal ScaleY { get; set; }

        public decimal TranslateX { get; set; }

        public decimal TranslateY { get; set; }

        public abstract decimal Left { get; }

        public abstract decimal Top { get; }

        public abstract void ComposeTree(TreeComposer composer);
    }

    internal abstract class BaseShape<TGraphicsObject> : BaseShape
        where TGraphicsObject : BaseGraphicsObject
    {
        protected BaseShape(TGraphicsObject graphics)
        {
            this.Graphics = graphics;
        }

        public TGraphicsObject Graphics { get; private set; }

        public override void ComposeTree(TreeComposer composer)
        {
            if (!this.IsVisible)
            {
                return;
            }

            this.Angle %= 360;
            string opacityValue = (this.Opacity / 100).ToString(CultureInfo.CurrentCulture);

            composer.Element(
                name: "g",
                attributes: new Dictionary<string, string>
                {
                    { "transform", $"translate({this.TranslateX}, {this.TranslateY}) scale({this.ScaleX}, {this.ScaleY}) rotate({this.Angle})" },
                    { "stroke-opacity", opacityValue },
                    { "fill-opacity", opacityValue },
                },
                styles: new Dictionary<string, string>
                {
                    { "transform-origin", "center center" },
                    { "transform-box", "fill-box" },
                },
                body: () => this.Graphics.ComposeTree(composer));
        }
    }
}
