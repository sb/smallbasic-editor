// <copyright file="CSInterop.Generated.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

/// <summary>
/// This file is auto-generated by a build task. It shouldn't be edited by hand.
/// </summary>
namespace SmallBasic.Editor.Interop
{
    using System.Threading.Tasks;
    using Microsoft.JSInterop;
    using SmallBasic.Compiler.Services;

    internal interface IMonacoInterop
    {
        Task<MonacoRange[]> UpdateDiagnostics(string code);

        Task<MonacoCompletionItem[]> ProvideCompletionItems(string code, MonacoPosition position);

        Task<string[]> ProvideHover(string code, MonacoPosition position);
    }

    internal interface IGraphicsDisplayInterop
    {
        Task UpdateDisplayLocation(decimal x, decimal y);

        Task OnKeyUp(string key);

        Task OnKeyDown(string key);
    }

    public static class CSInterop
    {
        private static readonly IMonacoInterop Monaco = new MonacoInterop();

        private static readonly IGraphicsDisplayInterop GraphicsDisplay = new GraphicsDisplayInterop();

        [JSInvokable("CSIntrop.Monaco.UpdateDiagnostics")]
        public static Task<MonacoRange[]> Monaco_UpdateDiagnostics(string code)
        {
            return Monaco.UpdateDiagnostics(code);
        }

        [JSInvokable("CSIntrop.Monaco.ProvideCompletionItems")]
        public static Task<MonacoCompletionItem[]> Monaco_ProvideCompletionItems(string code, MonacoPosition position)
        {
            return Monaco.ProvideCompletionItems(code, position);
        }

        [JSInvokable("CSIntrop.Monaco.ProvideHover")]
        public static Task<string[]> Monaco_ProvideHover(string code, MonacoPosition position)
        {
            return Monaco.ProvideHover(code, position);
        }

        [JSInvokable("CSIntrop.GraphicsDisplay.UpdateDisplayLocation")]
        public static async Task<bool> GraphicsDisplay_UpdateDisplayLocation(decimal x, decimal y)
        {
            await GraphicsDisplay.UpdateDisplayLocation(x, y).ConfigureAwait(false);
            return true;
        }

        [JSInvokable("CSIntrop.GraphicsDisplay.OnKeyUp")]
        public static async Task<bool> GraphicsDisplay_OnKeyUp(string key)
        {
            await GraphicsDisplay.OnKeyUp(key).ConfigureAwait(false);
            return true;
        }

        [JSInvokable("CSIntrop.GraphicsDisplay.OnKeyDown")]
        public static async Task<bool> GraphicsDisplay_OnKeyDown(string key)
        {
            await GraphicsDisplay.OnKeyDown(key).ConfigureAwait(false);
            return true;
        }
    }
}
