// <copyright file="RuntimeAnalysis.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Binding
{
    using System.Diagnostics;
    using System.Linq;
    using SuperBasic.Compiler.Runtime;

    public sealed class RuntimeAnalysis : BaseBoundNodeVisitor
    {
        public RuntimeAnalysis(SuperBasicCompilation compilation)
        {
            Debug.Assert(!compilation.Diagnostics.Any(), "Cannot analyze a compilation with errors.");

            this.Visit(compilation.MainModule);
            foreach (var subModule in compilation.SubModules.Values)
            {
                this.Visit(subModule);
            }

            if (!this.UsesGraphicsWindow)
            {
                this.UsesTextWindow = true;
            }
        }

        public bool UsesTextWindow { get; private set; }

        public bool UsesGraphicsWindow { get; private set; }

        public bool ListensToEvents { get; private set; }

        private protected override void VisitLibraryTypeExpression(BoundLibraryTypeExpression node)
        {
            base.VisitLibraryTypeExpression(node);

            var library = Libraries.Types[node.Name];
            this.UsesTextWindow |= library.UsesTextWindow;
            this.UsesGraphicsWindow |= library.UsesGraphicsWindow;
        }

        private protected override void VisitEventAssignmentStatement(BoundEventAssignmentStatement node)
        {
            base.VisitEventAssignmentStatement(node);

            this.ListensToEvents = true;
        }
    }
}
