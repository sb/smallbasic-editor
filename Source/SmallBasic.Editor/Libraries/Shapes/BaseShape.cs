// <copyright file="BaseShape.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Shapes
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using SmallBasic.Editor.Components;
    using SmallBasic.Editor.Libraries.Graphics;
    using SmallBasic.Editor.Store;
    using SmallBasic.Utilities;

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

        public abstract decimal Height { get; }

        public abstract decimal Width { get; }

        protected (decimal x, decimal y, decimal duration, double start)? TranslationAnimation { get; private set; }

        protected (decimal angle, decimal duration, double start)? AngleAnimation { get; private set; }

        public abstract void ComposeTree(TreeComposer composer);

        public async Task AnimateTranslation(decimal x, decimal y, decimal duration)
        {
            this.TranslationAnimation = (x, y, duration, GraphicsDisplayStore.NextAnimationTime.TotalSeconds);
            GraphicsDisplayStore.UpdateDisplay();

            await Task.Delay((int)duration).ConfigureAwait(false);

            this.TranslateX = x;
            this.TranslateY = y;
            this.TranslationAnimation = default;
            GraphicsDisplayStore.UpdateDisplay();
        }

        public async Task AnimateAngle(decimal angle, decimal duration)
        {
            this.AngleAnimation = (angle, duration, GraphicsDisplayStore.NextAnimationTime.TotalSeconds);
            GraphicsDisplayStore.UpdateDisplay();

            await Task.Delay((int)duration).ConfigureAwait(false);

            this.Angle = angle;
            this.AngleAnimation = default;
            GraphicsDisplayStore.UpdateDisplay();
        }
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

            Action body = () => this.Graphics.ComposeTree(composer);

            body = this.AttachOpacity(body, composer);
            body = this.AttachRotation(body, composer);
            body = this.AttachScale(body, composer);
            body = this.AttachTranslation(body, composer);

            body();
        }

        private Action AttachOpacity(Action body, TreeComposer composer)
        {
            if (this.Opacity == 100)
            {
                return body;
            }

            return () =>
            {
                composer.Element(
                    name: "g",
                    attributes: new Dictionary<string, string>
                    {
                    { "opacity", (this.Opacity / 100).ToString(CultureInfo.CurrentCulture) },
                    },
                    body: body);
            };
        }

        private Action AttachRotation(Action body, TreeComposer composer)
        {
            return () =>
            {
                composer.Element(
                    name: "g",
                    attributes: new Dictionary<string, string>
                    {
                    { "transform", this.AngleAnimation.HasValue ? string.Empty : $"rotate({this.Angle})" },
                    },
                    styles: new Dictionary<string, string>
                    {
                        { "transform-origin", "center center" },
                        { "transform-box", "fill-box" },
                    },
                    body: () =>
                    {
                        if (this.AngleAnimation.HasValue)
                        {
                            (decimal angle, decimal duration, double start) = this.AngleAnimation.Value;

                            composer.Element(
                                name: "animateTransform",
                                attributes: new Dictionary<string, string>
                                {
                                { "attributeName", "transform" },
                                { "type", "rotate" },
                                { "from", this.Angle.ToString(CultureInfo.CurrentCulture) },
                                { "to", angle.ToString(CultureInfo.CurrentCulture) },
                                { "begin", $"{start}s" },
                                { "dur", $"{duration / 1000}s" },
                                { "fill", "freeze" },
                                { "additive", "sum" },
                                });
                        }

                        body();
                    });
            };
        }

        private Action AttachScale(Action body, TreeComposer composer)
        {
            if (this.ScaleX == 1 && this.ScaleY == 1)
            {
                return body;
            }

            return () =>
            {
                composer.Element(
                    name: "g",
                    attributes: new Dictionary<string, string>
                    {
                    { "transform", $"scale({this.ScaleX}, {this.ScaleY})" },
                    },
                    styles: new Dictionary<string, string>
                    {
                        { "transform-origin", "center center" },
                        { "transform-box", "fill-box" }
                    },
                    body: body);
            };
        }

        private Action AttachTranslation(Action body, TreeComposer composer)
        {
            if (this.TranslateX == 0 && this.TranslateY == 0 && !this.TranslationAnimation.HasValue)
            {
                return body;
            }

            return () =>
            {
                composer.Element(
                    name: "g",
                    attributes: new Dictionary<string, string>
                    {
                    { "transform", this.TranslationAnimation.HasValue ? string.Empty : $"translate({this.TranslateX}, {this.TranslateY})" },
                    },
                    styles: new Dictionary<string, string>
                    {
                    { "transform-origin", "0 0" },
                    { "transform-box", "view-box" },
                    },
                    body: () =>
                    {
                        if (this.TranslationAnimation.HasValue)
                        {
                            (decimal x, decimal y, decimal duration, double start) = this.TranslationAnimation.Value;

                            composer.Element(
                                name: "animateTransform",
                                attributes: new Dictionary<string, string>
                                {
                                { "attributeName", "transform" },
                                { "type", "translate" },
                                { "from", $"{this.TranslateX} {this.TranslateY}" },
                                { "to", $"{x} {y}" },
                                { "begin", $"{start}s" },
                                { "dur", $"{duration / 1000}s" },
                                { "fill", "freeze" },
                                { "additive", "sum" },
                                });
                        }

                        body();
                    });
            };
        }
    }
}
