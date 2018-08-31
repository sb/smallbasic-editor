// <copyright file="SubModuleNamesCollector.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Binding
{
    using System.Collections.Generic;
    using SuperBasic.Compiler.Diagnostics;
    using SuperBasic.Compiler.Parsing;
    using SuperBasic.Utilities;

    internal class SubModuleNamesCollector : BaseSyntaxNodeVisitor
    {
        private readonly DiagnosticBag diagnostics;
        private readonly HashSet<string> names = new HashSet<string>();

        public SubModuleNamesCollector(DiagnosticBag diagnostics)
        {
            this.diagnostics = diagnostics;
        }

        public IReadOnlyHashSet<string> Names => this.names;

        public override void VisitSubModuleStatement(SubModuleStatementSyntax node)
        {
            string name = node.NameToken.Text;
            if (!this.names.Add(name))
            {
                this.diagnostics.ReportTwoSubModulesWithTheSameName(node.NameToken.Range, name);
            }
        }
    }
}
