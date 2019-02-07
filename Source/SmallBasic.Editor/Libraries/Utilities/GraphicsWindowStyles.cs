// <copyright file="GraphicsWindowStyles.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries.Utilities
{
    internal sealed class GraphicsWindowStyles
    {
        public GraphicsWindowStyles(
            decimal penWidth,
            string penColor,
            string brushColor,
            bool fontBold,
            bool fontItalic,
            string fontName,
            decimal fontSize)
        {
            this.PenWidth = penWidth;
            this.PenColor = penColor;
            this.BrushColor = brushColor;
            this.FontBold = fontBold;
            this.FontItalic = fontItalic;
            this.FontName = fontName;
            this.FontSize = fontSize;
        }

        public decimal PenWidth { get; private set; }

        public string PenColor { get; private set; }

        public string BrushColor { get; private set; }

        public bool FontBold { get; private set; }

        public bool FontItalic { get; private set; }

        public string FontName { get; private set; }

        public decimal FontSize { get; private set; }

        public GraphicsWindowStyles With(
            decimal? penWidth = default,
            string penColor = default,
            string brushColor = default,
            bool? fontBold = default,
            bool? fontItalic = default,
            string fontName = default,
            decimal? fontSize = default)
            => new GraphicsWindowStyles(
                penWidth: penWidth ?? this.PenWidth,
                penColor: penColor ?? this.PenColor,
                brushColor: brushColor ?? this.BrushColor,
                fontBold: fontBold ?? this.FontBold,
                fontItalic: fontItalic ?? this.FontItalic,
                fontName: fontName ?? this.FontName,
                fontSize: fontSize ?? this.FontSize);
    }
}
