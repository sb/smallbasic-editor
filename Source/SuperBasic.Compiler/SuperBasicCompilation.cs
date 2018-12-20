// <copyright file="SuperBasicCompilation.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Compiler.Binding;
    using SuperBasic.Compiler.Diagnostics;
    using SuperBasic.Compiler.Parsing;
    using SuperBasic.Compiler.Scanning;
    using SuperBasic.Compiler.Services;
    using SuperBasic.Utilities;

    public sealed class SuperBasicCompilation
    {
        private readonly DiagnosticBag diagnostics;
        private readonly bool isRunningOnDesktop;

        private readonly Scanner scanner;
        private readonly Parser parser;
        private readonly Binder binder;

        private readonly Lazy<RuntimeAnalysis> lazyAnalysis;

        public SuperBasicCompilation(string text)
           : this(text, SuperBasicEnv.IsBuildingForDesktop)
        {
        }

        public SuperBasicCompilation(string text, bool isRunningOnDesktop)
        {
            this.diagnostics = new DiagnosticBag();
            this.isRunningOnDesktop = isRunningOnDesktop;

            this.Text = text;

            this.scanner = new Scanner(this.Text, this.diagnostics);
            this.parser = new Parser(this.scanner.Tokens, this.diagnostics);
            this.binder = new Binder(this.parser.SyntaxTree, this.diagnostics, isRunningOnDesktop);

            this.lazyAnalysis = new Lazy<RuntimeAnalysis>(() => new RuntimeAnalysis(this));
        }

        public string Text { get; private set; }

        public RuntimeAnalysis Analysis => this.lazyAnalysis.Value;

        public IReadOnlyList<Diagnostic> Diagnostics => this.diagnostics.Contents;

        internal BoundStatementBlock MainModule => this.binder.MainModule;

        internal IReadOnlyDictionary<string, BoundSubModule> SubModules => this.binder.SubModules;

        public MonacoCompletionItem[] ProvideCompletionItems(TextPosition position) => CompletionItemProvider.Provide(this.parser, position);

        public string[] ProvideHover(TextPosition position) => HoverProvider.Provide(this.diagnostics, this.parser, position);
    }
}
