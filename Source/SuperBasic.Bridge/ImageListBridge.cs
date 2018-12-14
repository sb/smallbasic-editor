//<copyright file = "ImageListBridge.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Bridge
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using SuperBasic.Utilities.Bridge;

    internal class ImageListBridge : IImageListBridge
    {
        public ImageListBridgeModels.ImageData LoadImage(string fileNameOrUrl)
        {
            if (new Uri(fileNameOrUrl).IsFile)
            {
                return CreateImage(() => File.ReadAllBytes(fileNameOrUrl));
            }
            else
            {
                using (var client = new WebClient())
                {
                    return CreateImage(() => client.DownloadData(fileNameOrUrl));
                }
            }
        }

        private static ImageListBridgeModels.ImageData CreateImage(Func<byte[]> factory)
        {
            try
            {
                byte[] bytes = factory();
                using (var stream = new MemoryStream(bytes))
                {
                    using (var image = new Bitmap(Image.FromStream(stream)))
                    {
                        string base64Contents = Convert.ToBase64String(bytes, Base64FormattingOptions.None);
                        return new ImageListBridgeModels.ImageData(image.Width, image.Height, base64Contents);
                    }
                }
            }
            catch
            {
                return new ImageListBridgeModels.ImageData(width: 0, height: 0, base64Contents: string.Empty);
            }
        }
    }
}
