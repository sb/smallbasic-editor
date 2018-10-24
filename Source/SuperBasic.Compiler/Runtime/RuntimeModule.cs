// <copyright file = "RuntimeModule.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System.Collections.Generic;

    public sealed class RuntimeModule
    {
        internal RuntimeModule(string name, IReadOnlyList<BaseInstruction> instructions)
        {
            this.Name = name;
            this.Instructions = instructions;
        }

        public string Name { get; private set; }

        internal IReadOnlyList<BaseInstruction> Instructions { get; private set; }
    }
}
