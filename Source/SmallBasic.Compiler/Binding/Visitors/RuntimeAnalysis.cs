// <copyright file="RuntimeAnalysis.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Binding
{
    using System.Diagnostics;
    using System.Linq;
    using SmallBasic.Compiler.Runtime;

    public sealed class RuntimeAnalysis : BaseBoundNodeVisitor
    {
        internal RuntimeAnalysis(SmallBasicCompilation compilation)
        {
            Debug.Assert(!compilation.Diagnostics.Any(), "Cannot analyze a compilation with errors.");
            this.Compilation = compilation;

            this.Visit(this.Compilation.MainModule);
            foreach (var subModule in this.Compilation.SubModules.Values)
            {
                this.Visit(subModule);
            }

            if (!this.UsesGraphicsWindow)
            {
                this.UsesTextWindow = true;
            }
        }

        public SmallBasicCompilation Compilation { get; private set; }

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
