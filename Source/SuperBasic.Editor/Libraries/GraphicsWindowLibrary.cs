// <copyright file="GraphicsWindowLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Utilities;

    // TODO: all colors should be a color object that holds RBG values, and can be converted to #XXXXXX or to a name to return from properties
    public interface IGraphicsWindowPlugin
    {
        int Height { get; }

        int Width { get; }

        void SetTextInputCallback(Action<decimal> callback);

        void SetKeyDownCallback(Action<decimal> callback);

        void SetKeyUpCallback(Action<decimal> callback);

        void SetMouseDownCallback(Action<decimal, decimal> callback);

        void SetMouseMoveCallback(Action<decimal, decimal> callback);

        void SetMouseUpCallback(Action<decimal, decimal> callback);

        void Clear();

        void DrawBoundText(decimal x, decimal y, decimal width, string text);

        void DrawEllipse(decimal x, decimal y, decimal width, decimal height);

        void DrawImage(string imageName, decimal x, decimal y);

        void DrawLine(decimal x1, decimal y1, decimal x2, decimal y2);

        void DrawRectangle(decimal x, decimal y, decimal width, decimal height);

        void DrawResizedImage(string imageName, decimal x, decimal y, decimal width, decimal height);

        void DrawText(decimal x, decimal y, string text);

        void DrawTriangle(decimal x1, decimal y1, decimal x2, decimal y2, decimal x3, decimal y3);

        void FillEllipse(decimal x, decimal y, decimal width, decimal height);

        void FillRectangle(decimal x, decimal y, decimal width, decimal height);

        void FillTriangle(decimal x1, decimal y1, decimal x2, decimal y2, decimal x3, decimal y3);

        string GetPixel(decimal x, decimal y);

        void SetPixel(decimal x, decimal y, string color);

        void ShowMessage(string text, string title);
    }

    internal sealed class GraphicsWindowLibrary
    {
        private static readonly Regex HexColorRegex = new Regex("^#[0-9a-fA-F]{6}$");
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        private readonly IGraphicsWindowPlugin graphicsWindowPlugin;
        private readonly StylesSettings settings;

        public GraphicsWindowLibrary(IGraphicsWindowPlugin graphicsWindowPlugin, StylesSettings settings)
        {
            this.graphicsWindowPlugin = graphicsWindowPlugin;
            this.settings = settings;

            this.LastText = string.Empty;
            this.LastKey = string.Empty;

            this.MouseX = 0;
            this.MouseY = 0;

            this.graphicsWindowPlugin.SetTextInputCallback(keyCode =>
            {
                this.LastKey = ((char)keyCode).ToString(CultureInfo.CurrentCulture);
                this.TextInput();
            });

            this.graphicsWindowPlugin.SetKeyDownCallback(keyCode =>
            {
                this.LastKey = ((char)keyCode).ToString(CultureInfo.CurrentCulture);
                this.KeyDown();
            });

            this.graphicsWindowPlugin.SetKeyUpCallback(keyCode =>
            {
                this.LastKey = ((char)keyCode).ToString(CultureInfo.CurrentCulture);
                this.KeyUp();
            });

            this.graphicsWindowPlugin.SetMouseDownCallback((mouseX, mouseY) =>
            {
                this.MouseX = mouseX;
                this.MouseY = mouseY;
                this.MouseDown();
            });

            this.graphicsWindowPlugin.SetMouseMoveCallback((mouseX, mouseY) =>
            {
                this.MouseX = mouseX;
                this.MouseY = mouseY;
                this.MouseMove();
            });

            this.graphicsWindowPlugin.SetMouseUpCallback((mouseX, mouseY) =>
            {
                this.MouseX = mouseX;
                this.MouseY = mouseY;
                this.MouseUp();
            });
        }

        public event Action KeyDown;

        public event Action KeyUp;

        public event Action MouseDown;

        public event Action MouseMove;

        public event Action MouseUp;

        public event Action TextInput;

        public string LastKey { get; private set; }

        public string LastText { get; private set; }

        public decimal MouseX { get; private set; }

        public decimal MouseY { get; private set; }

        public string BackgroundColor
        {
            get => this.settings.BackgroundColor;
            set => this.settings.BackgroundColor = GetColorHexOpt(value) ?? this.settings.BackgroundColor;
        }

        public string BrushColor
        {
            get => this.settings.BrushColor;
            set => this.settings.BrushColor = GetColorHexOpt(value) ?? this.settings.BrushColor;
        }

        public string PenColor
        {
            get => this.settings.PenColor;
            set => this.settings.PenColor = GetColorHexOpt(value) ?? this.settings.PenColor;
        }

        public decimal PenWidth
        {
            get => this.settings.PenWidth;
            set => this.settings.PenWidth = value;
        }

        public bool FontBold
        {
            get => this.settings.FontBold;
            set => this.settings.FontBold = value;
        }

        public bool FontItalic
        {
            get => this.settings.FontItalic;
            set => this.settings.FontItalic = value;
        }

        public string FontName
        {
            get => this.settings.FontName;
            set => this.settings.FontName = value;
        }

        public decimal FontSize
        {
            get => this.settings.FontSize;
            set => this.settings.FontSize = value;
        }

        public decimal Height => this.graphicsWindowPlugin.Height;

        public decimal Width => this.graphicsWindowPlugin.Width;

        public void Clear() => this.graphicsWindowPlugin.Clear();

        public void DrawBoundText(decimal x, decimal y, decimal width, string text) => this.graphicsWindowPlugin.DrawBoundText(x, y, width, text);

        public void DrawEllipse(decimal x, decimal y, decimal width, decimal height) => this.graphicsWindowPlugin.DrawEllipse(x, y, width, height);

        public void DrawImage(string imageName, decimal x, decimal y) => this.graphicsWindowPlugin.DrawImage(imageName, x, y);

        public void DrawLine(decimal x1, decimal y1, decimal x2, decimal y2) => this.graphicsWindowPlugin.DrawLine(x1, y1, x2, y2);

        public void DrawRectangle(decimal x, decimal y, decimal width, decimal height) => this.graphicsWindowPlugin.DrawRectangle(x, y, width, height);

        public void DrawResizedImage(string imageName, decimal x, decimal y, decimal width, decimal height) => this.graphicsWindowPlugin.DrawResizedImage(imageName, x, y, width, height);

        public void DrawText(decimal x, decimal y, string text) => this.graphicsWindowPlugin.DrawText(x, y, text);

        public void DrawTriangle(decimal x1, decimal y1, decimal x2, decimal y2, decimal x3, decimal y3) => this.graphicsWindowPlugin.DrawTriangle(x1, y1, x2, y2, x3, y3);

        public void FillEllipse(decimal x, decimal y, decimal width, decimal height) => this.graphicsWindowPlugin.FillEllipse(x, y, width, height);

        public void FillRectangle(decimal x, decimal y, decimal width, decimal height) => this.graphicsWindowPlugin.FillRectangle(x, y, width, height);

        public void FillTriangle(decimal x1, decimal y1, decimal x2, decimal y2, decimal x3, decimal y3) => this.graphicsWindowPlugin.FillTriangle(x1, y1, x2, y2, x3, y3);

        public string GetColorFromRGB(decimal red, decimal green, decimal blue) => string.Format(
                CultureInfo.CurrentCulture,
                "#{0:X2}{1:X2}{2:X2}",
                Math.Min(byte.MaxValue, Math.Max(byte.MinValue, red)),
                Math.Min(byte.MaxValue, Math.Max(byte.MinValue, green)),
                Math.Min(byte.MaxValue, Math.Max(byte.MinValue, blue)));

        public string GetPixel(decimal x, decimal y) => this.graphicsWindowPlugin.GetPixel(x, y);

        public string GetRandomColor() => string.Format(
                CultureInfo.CurrentCulture,
                "#{0:X2}{1:X2}{2:X2}",
                Random.Next(byte.MinValue, byte.MaxValue + 1),
                Random.Next(byte.MinValue, byte.MaxValue + 1),
                Random.Next(byte.MinValue, byte.MaxValue + 1));

        public void SetPixel(decimal x, decimal y, string color)
        {
            color = GetColorHexOpt(color);
            if (!color.IsDefault())
            {
                this.graphicsWindowPlugin.SetPixel(x, y, color);
            }
        }

        public void ShowMessage(string text, string title) => this.graphicsWindowPlugin.ShowMessage(text, title);

        private static string GetColorHexOpt(string userInput)
        {
            switch (userInput.Trim().ToUpper(CultureInfo.CurrentCulture))
            {
                case string hex when HexColorRegex.IsMatch(hex):
                    return hex;
                case string name when ColorParser.TryParseColorName(name, out string result):
                    return result;
                default:
                    return default;
            }
        }
    }
}
