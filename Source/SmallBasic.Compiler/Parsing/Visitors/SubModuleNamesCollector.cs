// <copyright file="SubModuleNamesCollector.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Parsing
{
    using System.Collections.Generic;
    using SmallBasic.Compiler.Diagnostics;

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
