// <copyright file="MonacoInterop.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Interop
{
    using System.Linq;
    using System.Threading.Tasks;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Scanning;
    using SuperBasic.Compiler.Services;
    using SuperBasic.Editor.Components.Toolbox;

    public class MonacoInterop : IMonacoInterop
    {
        public async Task<MonacoRange[]> OnChange(string id, string code)
        {
            var ranges = await MonacoEditor.TriggerOnChange(id, code).ConfigureAwait(false);
            return ranges.Select(range => range.ToMonacoRange()).ToArray();
        }

        public Task<MonacoCompletionItem[]> ProvideCompletionItems(string code, MonacoPosition position)
        {
            return Task.FromResult(new SuperBasicCompilation(code).ProvideCompletionItems(position.ToCompilerPosition()));
        }

        public Task<string[]> ProvideHover(string code, MonacoPosition position)
        {
            return Task.FromResult(new SuperBasicCompilation(code).ProvideHover(position.ToCompilerPosition()));
        }
    }
}
