// <copyright file="ImageListLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using SuperBasic.Compiler.Runtime;

    public interface IImageListPlugin
    {
        decimal GetHeightOfImage(string imageName);

        decimal GetWidthOfImage(string imageName);

        string LoadImageFromFile(string filePath);

        string LoadImageFromUrl(string url);
    }

    internal sealed class ImageListLibrary : IImageListLibrary
    {
        private readonly IImageListPlugin plugin;

        public ImageListLibrary(IImageListPlugin plugin)
        {
            this.plugin = plugin;
        }

        public decimal GetHeightOfImage(string imageName) => this.plugin.GetHeightOfImage(imageName);

        public decimal GetWidthOfImage(string imageName) => this.plugin.GetWidthOfImage(imageName);

        public string LoadImage(string fileNameOrUrl)
        {
            if (new Uri(fileNameOrUrl).IsFile)
            {
                return this.plugin.LoadImageFromFile(fileNameOrUrl);
            }
            else
            {
                return this.plugin.LoadImageFromUrl(fileNameOrUrl);
            }
        }
    }
}
