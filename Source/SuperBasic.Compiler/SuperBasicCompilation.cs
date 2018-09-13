// <copyright file="SuperBasicCompilation.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler
{
    using System.Collections.Generic;
    using SuperBasic.Compiler.Binding;
    using SuperBasic.Compiler.Diagnostics;
    using SuperBasic.Compiler.Parsing;
    using SuperBasic.Compiler.Scanning;

    public sealed class SuperBasicCompilation
    {
        private readonly DiagnosticBag diagnostics = new DiagnosticBag();

        public SuperBasicCompilation(string text)
        {
            this.Text = text;
            var scanner = new Scanner(this.Text, this.diagnostics);
            var parser = new Parser(scanner.Tokens, this.diagnostics);
            var binder = new Binder(parser.SyntaxTree, this.diagnostics);

            this.MainModule = binder.MainModule;
            this.SubModules = binder.SubModules;
        }

        public string Text { get; private set; }

        public IReadOnlyList<Diagnostic> Diagnostics => this.diagnostics.Contents;

        internal BoundStatementBlock MainModule { get; private set; }

        internal IReadOnlyDictionary<string, BoundSubModule> SubModules { get; private set; }
    }
}
