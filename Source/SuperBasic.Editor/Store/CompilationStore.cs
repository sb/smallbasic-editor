// <copyright file="CompilationStore.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Store
{
    using System;
    using SuperBasic.Compiler;
    using SuperBasic.Utilities;

    internal static class CompilationStore
    {
        static CompilationStore()
        {
            Compilation = new SuperBasicCompilation(
@"' A new Program!
TextWindow.WriteLine(""What is your name?"")
name = TextWindow.Read()
TextWindow.WriteLine(""Hello "" + name + ""!"")");
        }

        public static event Action CodeChanged;

        public static SuperBasicCompilation Compilation { get; private set; }

        public static void NotifyCodeChanged(string code)
        {
            Compilation = new SuperBasicCompilation(code);

            if (!CodeChanged.IsDefault())
            {
                CodeChanged();
            }
        }
    }
}
