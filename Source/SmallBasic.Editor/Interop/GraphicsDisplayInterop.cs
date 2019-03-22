// <copyright file="GraphicsDisplayInterop.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Interop
{
    using System.Threading.Tasks;
    using SmallBasic.Editor.Store;

    public class GraphicsDisplayInterop : IGraphicsDisplayInterop
    {
        public Task UpdateDisplayLocation(decimal x, decimal y)
        {
            GraphicsDisplayStore.UpdateDisplayLocation(x, y);
            return Task.CompletedTask;
        }
    }
}
