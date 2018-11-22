// <copyright file="MonacoInterop.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Interop
{
    using System.Threading.Tasks;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Services;

    public class MonacoInterop : IMonacoInterop
    {
        public Task<MonacoCompletionItem[]> ProvideCompletionItems(string code, decimal line, decimal column)
        {
            return Task.FromResult(new SuperBasicCompilation(code).ProvideCompletionItems(((short)(line - 1), (short)(column - 1))));
        }

        public Task<string[]> ProvideHover(string code, decimal line, decimal column)
        {
            return Task.FromResult(new SuperBasicCompilation(code).ProvideHover(((short)(line - 1), (short)(column - 1))));
        }
    }
}
