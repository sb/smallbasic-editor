// <copyright file="StaticStore.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components
{
    using System;
    using SuperBasic.Compiler;
    using SuperBasic.Utilities;

    internal static class StaticStore
    {
        static StaticStore()
        {
            Compilation = new SuperBasicCompilation(
 @"' Below is a sample code to print 'Hello, World!' on the screen.
' Press Run for output.
TextWindow.WriteLine(""Hello, World!"")");
        }

        public static event Action CodeChanged;

        public static SuperBasicCompilation Compilation { get; private set; }

        public static void Update(string code)
        {
            Compilation = new SuperBasicCompilation(code);

            if (!CodeChanged.IsDefault())
            {
                CodeChanged();
            }
        }
    }
}
