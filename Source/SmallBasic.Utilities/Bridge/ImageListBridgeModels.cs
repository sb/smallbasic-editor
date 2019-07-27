// <copyright file="ImageListBridgeModels.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Utilities.Bridge
{
    public static class ImageListBridgeModels
    {
        public sealed class ImageData
        {
            public ImageData()
            {
            }

            public ImageData(decimal width, decimal height, string base64Contents)
            {
                this.Width = width;
                this.Height = height;
                this.Base64Contents = base64Contents;
            }

            public decimal Width { get; set; }

            public decimal Height { get; set; }

            public string Base64Contents { get; set; }
        }
    }
}
