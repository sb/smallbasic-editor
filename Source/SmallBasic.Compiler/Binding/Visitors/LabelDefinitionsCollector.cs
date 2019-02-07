// <copyright file="LabelDefinitionsCollector.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Binding
{
    using System.Collections.Generic;
    using SmallBasic.Compiler.Diagnostics;
    using SmallBasic.Compiler.Parsing;
    using SmallBasic.Utilities;

    internal sealed class LabelDefinitionsCollector : BaseBoundNodeVisitor
    {
        private readonly DiagnosticBag diagnostics;
        private readonly HashSet<string> labels = new HashSet<string>();

        public LabelDefinitionsCollector(DiagnosticBag diagnostics, BoundStatementBlock module)
        {
            this.diagnostics = diagnostics;
            this.Visit(module);
        }

        public IReadOnlyCollection<string> Labels => this.labels;

        private protected override void VisitLabelStatement(BoundLabelStatement node)
        {
            if (!this.labels.Add(node.Label))
            {
                this.diagnostics.ReportTwoLabelsWithTheSameName(node.Syntax.LabelToken.Range, node.Label);
            }
        }
    }
}
