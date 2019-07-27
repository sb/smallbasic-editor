// <copyright file="RuntimeModule.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Runtime
{
    using System.Collections.Generic;
    using SmallBasic.Compiler.Parsing;

    public sealed class RuntimeModule
    {
        internal RuntimeModule(string name, IReadOnlyList<BaseInstruction> instructions, StatementBlockSyntax syntax)
        {
            this.Name = name;
            this.Instructions = instructions;
            this.Syntax = syntax;
        }

        public string Name { get; private set; }

        internal IReadOnlyList<BaseInstruction> Instructions { get; private set; }

        internal StatementBlockSyntax Syntax { get; private set; }
    }
}
