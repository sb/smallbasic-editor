// <copyright file = "RuntimeModule.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System.Collections.Generic;
    using SuperBasic.Compiler.Parsing;

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
