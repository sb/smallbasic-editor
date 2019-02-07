// <copyright file="CompilationStore.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Store
{
    using System;
    using SmallBasic.Compiler;
    using SmallBasic.Utilities;

    internal static class CompilationStore
    {
        static CompilationStore()
        {
            Compilation = new SmallBasicCompilation(
@"' A new Program!
TextWindow.WriteLine(""What is your name?"")
name = TextWindow.Read()
TextWindow.WriteLine(""Hello "" + name + ""!"")");
        }

        public static event Action CodeChanged;

        public static SmallBasicCompilation Compilation { get; private set; }

        public static void NotifyCodeChanged(string code)
        {
            Compilation = new SmallBasicCompilation(code);

            if (!CodeChanged.IsDefault())
            {
                CodeChanged();
            }
        }
    }
}
