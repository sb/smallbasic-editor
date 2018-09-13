// <copyright file="GoToUndefinedLabelChecker.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Binding
{
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Compiler.Diagnostics;

    internal class GoToUndefinedLabelChecker : BaseBoundNodeVisitor
    {
        private readonly DiagnosticBag diagnostics;
        private readonly IReadOnlyCollection<string> labels = new HashSet<string>();

        public GoToUndefinedLabelChecker(DiagnosticBag diagnostics, IReadOnlyCollection<string> labels)
        {
            this.diagnostics = diagnostics;
            this.labels = labels;
        }

        public override void VisitGoToStatement(BoundGoToStatement node)
        {
            if (!this.labels.Contains(node.Label))
            {
                this.diagnostics.ReportGoToUndefinedLabel(node.Syntax.LabelToken.Range, node.Label);
            }
        }
    }
}
