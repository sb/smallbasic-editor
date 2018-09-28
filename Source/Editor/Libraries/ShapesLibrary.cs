// <copyright file="ShapesLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using System.Collections.Generic;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Utilities;

    public interface IShapesPlugin
    {
        void AddEllipse(string shapeName, decimal width, decimal height);

        void AddImage(string shapeName, string imageName);

        void AddLine(string shapeName, decimal x1, decimal y1, decimal x2, decimal y2);

        void AddRectangle(string shapeName, decimal width, decimal height);

        void AddText(string shapeName, string text);

        void AddTriangle(string shapeName, decimal x1, decimal y1, decimal x2, decimal y2, decimal x3, decimal y3);

        void Animate(string shapeName, decimal x, decimal y, decimal duration);

        decimal GetTop(string shapeName);

        decimal GetLeft(string shapeName);

        decimal GetOpacity(string shapeName);

        void SetOpacity(string shapeName, decimal level);

        void Hide(string shapeName);

        void Show(string shapeName);

        void Remove(string shapeName);

        void Move(string shapeName, decimal x, decimal y);

        void Rotate(string shapeName, decimal angle);

        void SetText(string shapeName, string text);

        void Zoom(string shapeName, decimal scaleX, decimal scaleY);
    }

    internal sealed class ShapesLibrary : IShapesLibrary
    {
        private readonly NamedCounter counters;
        private readonly HashSet<string> shapes;
        private readonly IShapesPlugin plugin;
        private readonly StylesSettings settings;

        public ShapesLibrary(IShapesPlugin plugin, StylesSettings settings)
        {
            this.counters = new NamedCounter();
            this.shapes = new HashSet<string>();

            this.plugin = plugin;
            this.settings = settings;
        }

        public string AddEllipse(decimal width, decimal height)
        {
            string shapeName = this.counters.GetNext("Ellipse");
            this.plugin.AddEllipse(shapeName, width, height);

            this.shapes.Add(shapeName);
            return shapeName;
        }

        public string AddImage(string imageName)
        {
            string shapeName = this.counters.GetNext("Image");
            this.plugin.AddImage(shapeName, imageName);

            this.shapes.Add(shapeName);
            return shapeName;
        }

        public string AddLine(decimal x1, decimal y1, decimal x2, decimal y2)
        {
            string shapeName = this.counters.GetNext("Line");
            this.plugin.AddLine(shapeName, x1, y1, x2, y2);

            this.shapes.Add(shapeName);
            return shapeName;
        }

        public string AddRectangle(decimal width, decimal height)
        {
            string shapeName = this.counters.GetNext("Rectangle");
            this.plugin.AddRectangle(shapeName, width, height);

            this.shapes.Add(shapeName);
            return shapeName;
        }

        public string AddText(string text)
        {
            string shapeName = this.counters.GetNext("Text");
            this.plugin.AddText(shapeName, text);

            this.shapes.Add(shapeName);
            return shapeName;
        }

        public string AddTriangle(decimal x1, decimal y1, decimal x2, decimal y2, decimal x3, decimal y3)
        {
            string shapeName = this.counters.GetNext("Triangle");
            this.plugin.AddTriangle(shapeName, x1, y1, x2, y2, x3, y3);

            this.shapes.Add(shapeName);
            return shapeName;
        }

        public void Animate(string shapeName, decimal x, decimal y, decimal duration)
        {
            if (this.shapes.Contains(shapeName))
            {
                this.plugin.Animate(shapeName, x, y, duration);
            }
        }

        public decimal GetLeft(string shapeName)
        {
            if (this.shapes.Contains(shapeName))
            {
                return this.plugin.GetLeft(shapeName);
            }

            return 0;
        }

        public decimal GetOpacity(string shapeName)
        {
            if (this.shapes.Contains(shapeName))
            {
                return this.plugin.GetOpacity(shapeName);
            }

            return 0;
        }

        public decimal GetTop(string shapeName)
        {
            if (this.shapes.Contains(shapeName))
            {
                return this.plugin.GetTop(shapeName);
            }

            return 0;
        }

        public void HideShape(string shapeName)
        {
            if (this.shapes.Contains(shapeName))
            {
                this.plugin.Hide(shapeName);
            }
        }

        public void Move(string shapeName, decimal x, decimal y)
        {
            if (this.shapes.Contains(shapeName))
            {
                this.plugin.Move(shapeName, x, y);
            }
        }

        public void Remove(string shapeName)
        {
            if (this.shapes.Contains(shapeName))
            {
                this.plugin.Remove(shapeName);
                this.shapes.Remove(shapeName);
            }
        }

        public void Rotate(string shapeName, decimal angle)
        {
            if (this.shapes.Contains(shapeName))
            {
                this.plugin.Rotate(shapeName, angle);
            }
        }

        public void SetOpacity(string shapeName, decimal level)
        {
            if (this.shapes.Contains(shapeName))
            {
                this.plugin.SetOpacity(shapeName, level);
            }
        }

        public void SetText(string shapeName, string text)
        {
            if (this.shapes.Contains(shapeName) && shapeName.StartsWith("Text", StringComparison.CurrentCulture))
            {
                this.plugin.SetText(shapeName, text);
            }
        }

        public void ShowShape(string shapeName)
        {
            if (this.shapes.Contains(shapeName))
            {
                this.plugin.Show(shapeName);
            }
        }

        public void Zoom(string shapeName, decimal scaleX, decimal scaleY)
        {
            if (this.shapes.Contains(shapeName))
            {
                this.plugin.Zoom(shapeName, scaleX, scaleY);
            }
        }
    }
}
