// <copyright file="StaticStore.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components
{
    using System;
    using SuperBasic.Compiler;
    using SuperBasic.Editor.Components.Display;
    using SuperBasic.Utilities;

    internal static class StaticStore
    {
        static StaticStore()
        {
            Compilation = new SuperBasicCompilation(
 @"
' A new Program!
TextWindow.WriteLine(""What is your name?"")
name = TextWindow.Read()
TextWindow.WriteLine(""Hello "" + name + ""!"")");
        }

        public static event Action CodeChanged;

        public static SuperBasicCompilation Compilation { get; private set; }

        public static TextDisplay TextDisplay { get; private set; }

        public static GraphicsDisplay GraphicsDisplay { get; private set; }

        public static void UpdateText(string code)
        {
            Compilation = new SuperBasicCompilation(code);

            if (!CodeChanged.IsDefault())
            {
                CodeChanged();
            }
        }

        public static void SetTextDisplay(TextDisplay display)
        {
            TextDisplay = display;
        }

        public static void SetGraphicsDisplay(GraphicsDisplay display)
        {
            GraphicsDisplay = display;
        }
    }
}
