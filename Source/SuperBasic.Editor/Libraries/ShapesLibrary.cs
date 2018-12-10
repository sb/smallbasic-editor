// <copyright file="ShapesLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Editor.Interop;
    using SuperBasic.Utilities;

    internal sealed class ShapesLibrary : IShapesLibrary
    {
        public string AddEllipse(decimal width, decimal height)
        {
            throw new NotImplementedException();
        }

        public string AddImage(string imageName)
        {
            throw new NotImplementedException();
        }

        public string AddLine(decimal x1, decimal y1, decimal x2, decimal y2)
        {
            throw new NotImplementedException();
        }

        public string AddRectangle(decimal width, decimal height)
        {
            throw new NotImplementedException();
        }

        public string AddText(string text)
        {
            throw new NotImplementedException();
        }

        public string AddTriangle(decimal x1, decimal y1, decimal x2, decimal y2, decimal x3, decimal y3)
        {
            throw new NotImplementedException();
        }

        public void Animate(string shapeName, decimal x, decimal y, decimal duration)
        {
            throw new NotImplementedException();
        }

        public decimal GetLeft(string shapeName)
        {
            throw new NotImplementedException();
        }

        public decimal GetOpacity(string shapeName)
        {
            throw new NotImplementedException();
        }

        public decimal GetTop(string shapeName)
        {
            throw new NotImplementedException();
        }

        public void HideShape(string shapeName)
        {
            throw new NotImplementedException();
        }

        public void Move(string shapeName, decimal x, decimal y)
        {
            throw new NotImplementedException();
        }

        public void Remove(string shapeName)
        {
            throw new NotImplementedException();
        }

        public void Rotate(string shapeName, decimal angle)
        {
            throw new NotImplementedException();
        }

        public void SetOpacity(string shapeName, decimal level)
        {
            throw new NotImplementedException();
        }

        public void SetText(string shapeName, string text)
        {
            throw new NotImplementedException();
        }

        public void ShowShape(string shapeName)
        {
            throw new NotImplementedException();
        }

        public void Zoom(string shapeName, decimal scaleX, decimal scaleY)
        {
            throw new NotImplementedException();
        }

        internal void Clear()
        {
            // TODO-now clear
            throw new NotImplementedException();
        }
    }
}
