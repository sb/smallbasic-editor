// <copyright file="SubModuleNamesCollector.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Parsing
{
    using System.Collections.Generic;
    using SuperBasic.Compiler.Diagnostics;

    internal sealed class SubModuleNamesCollector : BaseSyntaxNodeVisitor
    {
        private readonly HashSet<string> names = new HashSet<string>();

        public SubModuleNamesCollector(StatementBlockSyntax syntaxTree)
        {
            this.Visit(syntaxTree);
        }

        public IReadOnlyCollection<string> Names => this.names;

        private protected override void VisitSubModuleStatement(SubModuleStatementSyntax node)
        {
            this.names.Add(node.NameToken.Text);
        }
    }
}
