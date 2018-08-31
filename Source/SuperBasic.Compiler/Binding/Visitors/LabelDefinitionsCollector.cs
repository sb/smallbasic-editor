// <copyright file="LabelDefinitionsCollector.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Binding
{
    using System.Collections.Generic;
    using SuperBasic.Compiler.Diagnostics;
    using SuperBasic.Compiler.Parsing;
    using SuperBasic.Utilities;

    internal class LabelDefinitionsCollector : BaseBoundNodeVisitor
    {
        private readonly DiagnosticBag diagnostics;
        private readonly HashSet<string> labels = new HashSet<string>();

        public LabelDefinitionsCollector(DiagnosticBag diagnostics)
        {
            this.diagnostics = diagnostics;
        }

        public IReadOnlyHashSet<string> Labels => this.labels;

        public override void VisitLabelStatement(BoundLabelStatement node)
        {
            if (!this.labels.Add(node.Label))
            {
                this.diagnostics.ReportTwoLabelsWithTheSameName(node.Syntax.LabelToken.Range, node.Label);
            }
        }
    }
}
