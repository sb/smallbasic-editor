// <copyright file="GraphicsDisplayInterop.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
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

        public Task OnKeyUp(string key)
        {
            GraphicsDisplayStore.NotifyKeyUp(key);
            return Task.CompletedTask;
        }

        public Task OnKeyDown(string key)
        {
            GraphicsDisplayStore.NotifyKeyDown(key);
            return Task.CompletedTask;
        }
    }
}
