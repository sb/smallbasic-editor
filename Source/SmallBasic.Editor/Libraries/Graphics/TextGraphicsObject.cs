// <copyright file="TextGraphicsObject.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Graphics
{
    using System.Collections.Generic;
    using System.Globalization;
    using SmallBasic.Editor.Components;
    using SmallBasic.Editor.Libraries.Utilities;

    internal sealed class TextGraphicsObject : BaseGraphicsObject
    {
        public TextGraphicsObject(decimal x, decimal y, string text, decimal? width, GraphicsWindowStyles styles)
            : base(styles)
        {
            this.X = x;
            this.Y = y;
            this.Text = text;
            this.Width = width;
        }

        public decimal X { get; set; }

        public decimal Y { get; set; }

        public string Text { get; set; }

        public decimal? Width { get; set; }

        public override void ComposeTree(TreeComposer composer)
        {
            var attributes = new Dictionary<string, string>
            {
                { "x", this.X.ToString(CultureInfo.CurrentCulture) },
                { "y", this.Y.ToString(CultureInfo.CurrentCulture) },
                { "dy", this.Styles.FontSize.ToString(CultureInfo.CurrentCulture) }
            };

            if (this.Width.HasValue)
            {
                attributes.Add("textLength", this.Width.Value.ToString(CultureInfo.CurrentCulture));
            }

            composer.Element(
                name: "text",
                styles: new Dictionary<string, string>
                {
                    { "fill", this.Styles.BrushColor },
                    { "font-weight", this.Styles.FontBold ? "bold" : "normal" },
                    { "font-style", this.Styles.FontItalic ? "italic" : "normal" },
                    { "font-family", $@"""{this.Styles.FontName}""" },
                    { "font-size", $"{this.Styles.FontSize}px" },
                    { "pointer-events", "none" }
                },
                attributes: attributes,
                body: () => composer.Text(this.Text));
        }
    }
}
