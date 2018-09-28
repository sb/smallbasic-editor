// <copyright file="GraphicsInterop.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Interop
{
    using System;
    using System.Threading.Tasks;
    using SuperBasic.Editor.Libraries;

    internal class GraphicsInterop : IGraphicsInterop
    {
        private LibrariesCollection libraries;

        public GraphicsInterop(LibrariesCollection libraries)
        {
            this.libraries = libraries;
        }

        public Task NotifyButtonClicked(string buttonName)
        {
            throw new NotImplementedException();
        }

        public Task NotifyTextBoxControlEntry(string textBoxName)
        {
            throw new NotImplementedException();
        }

        public Task NotifyGraphicsWindowTextEntry(decimal keyCode)
        {
            throw new NotImplementedException();
        }

        public Task NotifyKeyDown(decimal keyCode)
        {
            throw new NotImplementedException();
        }

        public Task NotifyKeyUp(decimal keyCode)
        {
            throw new NotImplementedException();
        }

        public Task NotifyMouseDown(decimal x, decimal y)
        {
            throw new NotImplementedException();
        }

        public Task NotifyMouseMove(decimal x, decimal y)
        {
            throw new NotImplementedException();
        }

        public Task NotifyMouseUp(decimal x, decimal y)
        {
            throw new NotImplementedException();
        }
    }
}
