// <copyright file="MonacoInterop.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Interop
{
    using System.Linq;
    using System.Threading.Tasks;
    using SmallBasic.Compiler.Services;
    using SmallBasic.Editor.Components;
    using SmallBasic.Editor.Store;

    public class MonacoInterop : IMonacoInterop
    {
        public Task<MonacoRange[]> UpdateDiagnostics(string code)
        {
            CompilationStore.NotifyCodeChanged(code);
            return Task.FromResult(CompilationStore.Compilation.Diagnostics.Select(d => d.Range.ToMonacoRange()).ToArray());
        }

        public Task<MonacoCompletionItem[]> ProvideCompletionItems(string code, MonacoPosition position)
        {
            return Task.FromResult(CompilationStore.Compilation.ProvideCompletionItems(position.ToCompilerPosition()));
        }

        public Task<string[]> ProvideHover(string code, MonacoPosition position)
        {
            return Task.FromResult(CompilationStore.Compilation.ProvideHover(position.ToCompilerPosition()));
        }
    }
}
