// <copyright file="SuperBasicCompilation.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using SuperBasic.Compiler.Binding;
    using SuperBasic.Compiler.Diagnostics;
    using SuperBasic.Compiler.Parsing;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Compiler.Scanning;

    public sealed class SuperBasicCompilation
    {
        private readonly DiagnosticBag diagnostics = new DiagnosticBag();

        public SuperBasicCompilation(string text, bool isRunningOnDesktop = false)
        {
            this.Text = text;
            var scanner = new Scanner(this.Text, this.diagnostics);
            var parser = new Parser(scanner.Tokens, this.diagnostics);
            var binder = new Binder(parser.SyntaxTree, this.diagnostics, isRunningOnDesktop);

            this.UsesGraphicsWindow |= binder.UsesGraphicsWindow;

            this.MainModule = binder.MainModule;
            this.SubModules = binder.SubModules;
        }

        public string Text { get; private set; }

        // TODO: this will eventually move to an engine analyzer, that computes that, along with things like, does it track mouse? should we terminate or does it listen to events? etc....
        public bool UsesGraphicsWindow { get; private set; }

        public IReadOnlyList<Diagnostic> Diagnostics => this.diagnostics.Contents;

        internal IReadOnlyDictionary<string, BoundSubModule> SubModules { get; private set; }

        internal BoundStatementBlock MainModule { get; private set; }
    }
}
