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

        private SuperBasicCompilation(string text, CompilationKind kind)
        {
            this.Text = text;
            this.Kind = kind;
            var scanner = new Scanner(this.Text, this.diagnostics);
            var parser = new Parser(scanner.Tokens, this.diagnostics);
            var binder = new Binder(parser.SyntaxTree, this.diagnostics, this.Kind);

            this.MainModule = binder.MainModule;
            this.SubModules = binder.SubModules;
        }

        public string Text { get; private set; }

        public IReadOnlyList<Diagnostic> Diagnostics => this.diagnostics.Contents;

        internal CompilationKind Kind { get; private set; }

        internal BoundStatementBlock MainModule { get; private set; }

        internal IReadOnlyDictionary<string, BoundSubModule> SubModules { get; private set; }

        public static SuperBasicCompilation CreateTextProgram(string text)
            => new SuperBasicCompilation(text, CompilationKind.Text);

        public static SuperBasicCompilation CreateGraphicsProgram(string text)
            => new SuperBasicCompilation(text, CompilationKind.Graphics);
    }
}
