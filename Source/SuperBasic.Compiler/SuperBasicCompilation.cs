// <copyright file="SuperBasicCompilation.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler
{
    using System.Collections.Generic;
    using SuperBasic.Compiler.Diagnostics;
    using SuperBasic.Compiler.Syntax;

    public sealed class SuperBasicCompilation
    {
        private readonly DiagnosticBag diagnostics = new DiagnosticBag();

        public SuperBasicCompilation(string text)
        {
            this.Text = text;
            var scanner = new Scanner(this.diagnostics, this.Text);
        }

        public string Text { get; private set; }

        public IReadOnlyList<Diagnostic> Diagnostics => this.diagnostics.Contents;
    }
}
