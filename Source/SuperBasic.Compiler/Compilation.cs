// <copyright file="Compilation.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler
{
    using System.Collections.Generic;
    using SuperBasic.Compiler.Diagnostics;
    using SuperBasic.Compiler.Syntax;

    public sealed class Compilation
    {
        private readonly DiagnosticBag diagnostics = new DiagnosticBag();

        public Compilation(string text)
        {
            var scanner = new Scanner(this.diagnostics, text);
        }

        public IReadOnlyList<Diagnostic> Diagnostics => this.diagnostics.Contents;
    }
}
