// <copyright file="LineGraphicsObject.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Graphics
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using SmallBasic.Editor.Components;
    using SmallBasic.Editor.Libraries.Utilities;
    using SmallBasic.Editor.Store;

    internal sealed class LineGraphicsObject : BaseGraphicsObject
    {
        private (decimal x2, decimal y2, decimal duration, double start)? animation;

        public LineGraphicsObject(decimal x1, decimal y1, decimal x2, decimal y2, GraphicsWindowStyles styles)
            : base(styles)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.X2 = x2;
            this.Y2 = y2;
        }

        public decimal X1 { get; set; }

        public decimal Y1 { get; set; }

        public decimal X2 { get; set; }

        public decimal Y2 { get; set; }

        public async Task Animate(decimal x2, decimal y2, decimal duration)
        {
            this.animation = (x2, y2, duration, GraphicsDisplayStore.NextAnimationTime.TotalSeconds);
            await Task.Delay((int)duration).ConfigureAwait(false);

            this.X2 = x2;
            this.Y2 = y2;
            this.animation = default;
        }

        public override void ComposeTree(TreeComposer composer)
        {
            composer.Element(
                name: "polyline",
                styles: new Dictionary<string, string>
                {
                    { "stroke", this.Styles.PenColor },
                    { "stroke-width", $"{this.Styles.PenWidth}px" },
                },
                attributes: new Dictionary<string, string>
                {
                    { "points", this.animation.HasValue ? string.Empty : $"{this.X1},{this.Y1} {this.X2},{this.Y2}" },
                },
                body: () =>
                {
                    if (this.animation.HasValue)
                    {
                        (decimal x2, decimal y2, decimal duration, double start) = this.animation.Value;

                        composer.Element(
                            name: "animate",
                            attributes: new Dictionary<string, string>
                            {
                                { "attributeName", "points" },
                                { "from", $"{this.X1},{this.Y1} {this.X2},{this.Y2}" },
                                { "to",  $"{this.X1},{this.Y1} {x2},{y2}" },
                                { "begin", $"{start}s" },
                                { "dur", $"{duration / 1000}s" },
                                { "fill", "freeze" },
                                { "additive", "sum" },
                            });
                    }
                });
        }
    }
}
