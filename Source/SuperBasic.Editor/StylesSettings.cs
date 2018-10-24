// <copyright file="StylesSettings.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor
{
    public class StylesSettings
    {
        public StylesSettings()
        {
            this.BackgroundColor = "#FFFFFF";
            this.BrushColor = "#000000";

            this.PenColor = "#6A5ACD";
            this.PenWidth = 2;

            this.FontBold = false;
            this.FontItalic = false;
            this.FontName = "Calibri";
            this.FontSize = 16;
        }

        public string BackgroundColor { get; internal set; }

        public string BrushColor { get; internal set; }

        public string PenColor { get; internal set; }

        public decimal PenWidth { get; internal set; }

        public bool FontBold { get; internal set; }

        public bool FontItalic { get; internal set; }

        public string FontName { get; internal set; }

        public decimal FontSize { get; internal set; }
    }
}
