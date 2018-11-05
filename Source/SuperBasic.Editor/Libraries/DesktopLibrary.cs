// <copyright file="DesktopLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Utilities.Resources;

    public interface IDesktopPlugin
    {
        decimal Height { get; }

        decimal Width { get; }

        void SetWallPaper(string fileOrUrl);
    }

    internal sealed class DesktopLibrary : IDesktopLibrary
    {
        private readonly IDesktopPlugin plugin;

        public DesktopLibrary(IDesktopPlugin plugin)
        {
            this.plugin = plugin;
        }

        public decimal Get_Height() => this.plugin.Height;

        public decimal Get_Width() => this.plugin.Width;

        public void SetWallPaper(string fileOrUrl) => this.plugin.SetWallPaper(fileOrUrl);
    }
}
