// <copyright file="GraphicsWindowLibrary.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using SmallBasic.Compiler.Runtime;
    using SmallBasic.Editor.Components;
    using SmallBasic.Editor.Interop;
    using SmallBasic.Editor.Libraries.Graphics;
    using SmallBasic.Editor.Libraries.Utilities;
    using SmallBasic.Editor.Store;
    using SmallBasic.Utilities;

    internal sealed class GraphicsWindowLibrary : IGraphicsWindowLibrary, IDisposable
    {
        private readonly LibrariesCollection libraries;
        private readonly List<BaseGraphicsObject> graphics = new List<BaseGraphicsObject>();

        private static readonly Random RandomInstance = new Random((int)DateTime.Now.Ticks);
        private static readonly Regex HexColorRegex = new Regex("^#[0-9A-Fa-f]{6}([0-9A-Fa-f]{2})?$");
        private static readonly IReadOnlyList<string> PredefinedFonts = new List<string>
        {
            "Roboto",
            "Arial",
            "Helvetica",
            "Times New Roman",
            "Times",
            "Courier New",
            "Courier",
            "Consolas"
        };

        private string backgroundColor = PredefinedColors.GetHexColor("White");

        private string lastKey = string.Empty;
        private string lastText = string.Empty;

        private decimal mouseX = 0;
        private decimal mouseY = 0;

        public GraphicsWindowLibrary(LibrariesCollection libraries)
        {
            this.libraries = libraries;

            GraphicsDisplayStore.KeyDown += this.KeyDownCallback;
            GraphicsDisplayStore.KeyUp += this.KeyUpCallback;
            GraphicsDisplayStore.MouseUp += this.MouseUpCallback;
            GraphicsDisplayStore.MouseDown += this.MouseDownCallback;
            GraphicsDisplayStore.MouseMove += this.MouseMoveCallback;
        }

        public event Action KeyDown;

        public event Action KeyUp;

        public event Action MouseDown;

        public event Action MouseMove;

        public event Action MouseUp;

        public event Action TextInput;

        public void Dispose()
        {
            GraphicsDisplayStore.KeyDown -= this.KeyDownCallback;
            GraphicsDisplayStore.KeyUp -= this.KeyUpCallback;
            GraphicsDisplayStore.MouseUp -= this.MouseUpCallback;
            GraphicsDisplayStore.MouseDown -= this.MouseDownCallback;
            GraphicsDisplayStore.MouseMove -= this.MouseMoveCallback;
        }

        public void Clear()
        {
            this.graphics.Clear();
            this.libraries.Controls.Clear();
            this.libraries.Shapes.Clear();
            this.libraries.Turtle.Clear();
        }

        public void DrawBoundText(decimal x, decimal y, decimal width, string text)
            => this.graphics.Add(new TextGraphicsObject(x, y, text, width, this.libraries.Styles));

        public void DrawEllipse(decimal x, decimal y, decimal width, decimal height)
            => this.graphics.Add(new EllipseGraphicsObject(x, y, width, height, this.libraries.Styles.With(brushColor: PredefinedColors.Transparent)));

        public void DrawImage(string imageName, decimal x, decimal y)
            => this.graphics.Add(new ImageGraphicsObject(x, y, scaleX: 1, scaleY: 1, imageName, this.libraries.Styles));

        public void DrawLine(decimal x1, decimal y1, decimal x2, decimal y2)
            => this.graphics.Add(new LineGraphicsObject(x1, y1, x2, y2, this.libraries.Styles));

        public void DrawRectangle(decimal x, decimal y, decimal width, decimal height)
            => this.graphics.Add(new RectangleGraphicsObject(x, y, width, height, this.libraries.Styles.With(brushColor: PredefinedColors.Transparent)));

        public void DrawResizedImage(string imageName, decimal x, decimal y, decimal width, decimal height)
        {
            if (width == 0 || height == 0)
            {
                return;
            }

            decimal scaleX = this.libraries.ImageList.GetWidthOfImage(imageName) / width;
            decimal scaleY = this.libraries.ImageList.GetHeightOfImage(imageName) / height;
            this.graphics.Add(new ImageGraphicsObject(x, y, scaleX, scaleY, imageName, this.libraries.Styles));
        }

        public void DrawText(decimal x, decimal y, string text)
            => this.graphics.Add(new TextGraphicsObject(x, y, text, width: default, this.libraries.Styles));

        public void DrawTriangle(decimal x1, decimal y1, decimal x2, decimal y2, decimal x3, decimal y3)
            => this.graphics.Add(new TriangleGraphicsObject(x1, y1, x2, y2, x3, y3, this.libraries.Styles.With(brushColor: PredefinedColors.Transparent)));

        public void FillEllipse(decimal x, decimal y, decimal width, decimal height)
            => this.graphics.Add(new EllipseGraphicsObject(x, y, width, height, this.libraries.Styles.With(penColor: PredefinedColors.Transparent)));

        public void FillRectangle(decimal x, decimal y, decimal width, decimal height)
            => this.graphics.Add(new RectangleGraphicsObject(x, y, width, height, this.libraries.Styles.With(penColor: PredefinedColors.Transparent)));

        public void FillTriangle(decimal x1, decimal y1, decimal x2, decimal y2, decimal x3, decimal y3)
            => this.graphics.Add(new TriangleGraphicsObject(x1, y1, x2, y2, x3, y3, this.libraries.Styles.With(penColor: PredefinedColors.Transparent)));

        public void SetPixel(decimal x, decimal y, string color)
        {
            color = color.Trim();
            if (PredefinedColors.TryGetHexColor(color, out string hexColor))
            {
                color = hexColor;
            }
            else if (!HexColorRegex.IsMatch(color))
            {
                return;
            }

            this.graphics.Add(new LineGraphicsObject(x, y, x + 1, y, new GraphicsWindowStyles(
                penWidth: 1,
                penColor: color,
                brushColor: PredefinedColors.Transparent,
                fontBold: false,
                fontItalic: false,
                fontName: string.Empty,
                fontSize: 0)));
        }

        public string GetColorFromRGB(decimal red, decimal green, decimal blue) => string.Format(
            CultureInfo.CurrentCulture,
            "#{0:X2}{1:X2}{2:X2}",
            (int)Math.Min(byte.MaxValue, Math.Max(byte.MinValue, red)),
            (int)Math.Min(byte.MaxValue, Math.Max(byte.MinValue, green)),
            (int)Math.Min(byte.MaxValue, Math.Max(byte.MinValue, blue)));

        public string GetRandomColor() => string.Format(
            CultureInfo.CurrentCulture,
            "#{0:X2}{1:X2}{2:X2}",
            (int)RandomInstance.Next(byte.MinValue, byte.MaxValue + 1),
            (int)RandomInstance.Next(byte.MinValue, byte.MaxValue + 1),
            (int)RandomInstance.Next(byte.MinValue, byte.MaxValue + 1));

        public string Get_BackgroundColor() => this.backgroundColor;

        public string Get_BrushColor() => this.libraries.Styles.BrushColor;

        public bool Get_FontBold() => this.libraries.Styles.FontBold;

        public bool Get_FontItalic() => this.libraries.Styles.FontItalic;

        public string Get_FontName() => this.libraries.Styles.FontName;

        public decimal Get_FontSize() => this.libraries.Styles.FontSize;

        public Task<decimal> Get_Height() => JSInterop.Layout.GetElementHeight(GraphicsDisplayStore.RenderArea);

        public string Get_LastKey() => this.lastKey;

        public string Get_LastText() => this.lastText;

        public decimal Get_MouseX() => this.mouseX - GraphicsDisplayStore.LocationX;

        public decimal Get_MouseY() => this.mouseY - GraphicsDisplayStore.LocationY;

        public string Get_PenColor() => this.libraries.Styles.PenColor;

        public decimal Get_PenWidth() => this.libraries.Styles.PenWidth;

        public Task<decimal> Get_Width() => JSInterop.Layout.GetElementWidth(GraphicsDisplayStore.RenderArea);

        public string Get_Title() => GraphicsDisplayStore.Title;

        public void Set_BackgroundColor(string value)
        {
            value = value.Trim();
            if (PredefinedColors.TryGetHexColor(value, out string hexColor))
            {
                this.backgroundColor = hexColor;
            }
            else if (HexColorRegex.IsMatch(value))
            {
                this.backgroundColor = value;
            }
        }

        public void Set_BrushColor(string value)
        {
            value = value.Trim();
            if (PredefinedColors.TryGetHexColor(value, out string hexColor))
            {
                this.libraries.Styles = this.libraries.Styles.With(brushColor: hexColor);
            }
            else if (HexColorRegex.IsMatch(value))
            {
                this.libraries.Styles = this.libraries.Styles.With(brushColor: value);
            }
        }

        public void Set_FontBold(bool value) => this.libraries.Styles = this.libraries.Styles.With(fontBold: value);

        public void Set_FontItalic(bool value) => this.libraries.Styles = this.libraries.Styles.With(fontItalic: value);

        public void Set_FontName(string value)
        {
            string fontName = PredefinedFonts.SingleOrDefault(supported => string.Compare(supported, value.Trim(), StringComparison.CurrentCultureIgnoreCase) == 0);
            if (!fontName.IsDefault())
            {
                this.libraries.Styles = this.libraries.Styles.With(fontName: fontName);
            }
        }

        public void Set_FontSize(decimal value) => this.libraries.Styles = this.libraries.Styles.With(fontSize: value);

        public Task Set_Height(decimal value) => JSInterop.Layout.ShowMessage("Setting GraphicsWindow.Height is not supported in this version of SmallBasic", "");

        public void Set_PenColor(string value)
        {
            value = value.Trim();
            if (PredefinedColors.TryGetHexColor(value, out string hexColor))
            {
                this.libraries.Styles = this.libraries.Styles.With(penColor: hexColor);
            }
            else if (HexColorRegex.IsMatch(value))
            {
                this.libraries.Styles = this.libraries.Styles.With(penColor: value);
            }
        }

        public void Set_PenWidth(decimal value) => this.libraries.Styles = this.libraries.Styles.With(penWidth: value);

        public void Set_Title(string value) => GraphicsDisplayStore.Title = value;

        public Task Set_Width(decimal value) => JSInterop.Layout.ShowMessage("Setting GraphicsWindow.Width is not supported in this version of SmallBasic", "");

        public Task ShowMessage(string text, string title) => JSInterop.Layout.ShowMessage(text, title);

        public void Hide() => GraphicsDisplayStore.SetVisibility(false);

        public void Show() => GraphicsDisplayStore.SetVisibility(true);

        internal void ComposeTree(TreeComposer composer)
        {
            composer.Element(name: "rect", attributes: new Dictionary<string, string>
            {
                { "width", "100%" },
                { "height", "100%" },
                { "fill", this.backgroundColor }
            });

            foreach (var graphics in this.graphics)
            {
                graphics.ComposeTree(composer);
            }
        }

        private void KeyDownCallback(string key)
        {
            this.lastKey = key;
            this.KeyDown();

            if (key.Length == 1)
            {
                this.lastText = key;
                this.TextInput();
            }
        }

        private void KeyUpCallback(string key)
        {
            this.lastKey = key;
            this.KeyUp();
        }

        private void MouseDownCallback(decimal x, decimal y, MouseButton button)
        {
            this.mouseX = x;
            this.mouseY = y;
            this.MouseDown();
        }

        private void MouseUpCallback(decimal x, decimal y, MouseButton button)
        {
            this.mouseX = x;
            this.mouseY = y;
            this.MouseUp();
        }

        private void MouseMoveCallback(decimal x, decimal y)
        {
            this.mouseX = x;
            this.mouseY = y;
            this.MouseMove();
        }
    }
}
