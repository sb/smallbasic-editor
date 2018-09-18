// <copyright file="ConflictingLibrariesChecker.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Binding
{
    using SuperBasic.Compiler.Diagnostics;
    using SuperBasic.Compiler.Runtime;

    internal class ConflictingLibrariesChecker : BaseBoundNodeVisitor
    {
        private readonly DiagnosticBag diagnostics;

        private bool reportedError = false;
        private string firstLibrary;
        private ProgramKind? firstKind;

        public ConflictingLibrariesChecker(DiagnosticBag diagnostics)
        {
            this.diagnostics = diagnostics;
        }

        public ProgramKind ProgramKind => this.firstKind.HasValue ? this.firstKind.Value : ProgramKind.Text;

        public override void VisitLibraryTypeExpression(BoundLibraryTypeExpression node)
        {
            if (!this.reportedError)
            {
                if (Libraries.Types.TryGetValue(node.Name, out Library library) && library.ProgramKind.HasValue)
                {
                    if (this.firstKind.HasValue && this.firstKind.Value != library.ProgramKind.Value)
                    {
                        this.reportedError = true;
                        this.diagnostics.ReportMultipleProgramKindsUsed(node.Syntax.Range, library.Name, this.firstLibrary);
                    }
                    else
                    {
                        this.firstKind = library.ProgramKind;
                        this.firstLibrary = library.Name;
                    }
                }
            }
        }
    }
}
