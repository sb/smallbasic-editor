// <copyright file="ImageListLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using SmallBasic.Compiler.Runtime;
    using SmallBasic.Editor.Components;
    using SmallBasic.Editor.Libraries.Utilities;
    using SmallBasic.Utilities.Bridge;

    internal sealed class ImageListLibrary : IImageListLibrary
    {
        private readonly LibrariesCollection libraries;
        private readonly NamedCounter counter = new NamedCounter();
        private readonly Dictionary<string, ImageListBridgeModels.ImageData> images = new Dictionary<string, ImageListBridgeModels.ImageData>();

        public ImageListLibrary(LibrariesCollection libraries)
        {
            this.libraries = libraries;
        }

        public decimal GetHeightOfImage(string imageName)
        {
            if (this.images.TryGetValue(imageName, out ImageListBridgeModels.ImageData image))
            {
                return image.Height;
            }

            return 0;
        }

        public decimal GetWidthOfImage(string imageName)
        {
            if (this.images.TryGetValue(imageName, out ImageListBridgeModels.ImageData image))
            {
                return image.Width;
            }

            return 0;
        }

        public async Task<string> LoadImage(string fileNameOrUrl)
        {
            var name = this.counter.GetNext("ImageList");
            var data = await Bridge.Network.LoadImage(fileNameOrUrl).ConfigureAwait(false);
            this.images.Add(name, data);
            return name;
        }

        internal void EmbedImages(TreeComposer composer)
        {
            composer.Element(name: "defs", body: () =>
            {
                foreach (var pair in this.images)
                {
                    composer.Element("image", attributes: new Dictionary<string, string>
                    {
                        { "id", pair.Key },
                        { "width", pair.Value.Width.ToString(CultureInfo.CurrentCulture) },
                        { "height", pair.Value.Height.ToString(CultureInfo.CurrentCulture) },
                        { "href", $"data:image/image;base64,{pair.Value.Base64Contents}" }
                    });
                }
            });
        }
    }
}
