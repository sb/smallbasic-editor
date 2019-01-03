// <copyright file = "Frame.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System.Diagnostics;

    public sealed class Frame
    {
        private int index = 0;

        internal Frame(RuntimeModule module)
        {
            this.Module = module;
        }

        public RuntimeModule Module { get; private set; }

        public int InstructionIndex
        {
            get
            {
                return this.index;
            }

            internal set
            {
                Debug.Assert(value >= 0 && value <= this.Module.Instructions.Count, "Value should be within the module length");
                this.index = value;
            }
        }

        public int CurrentSourceLine
        {
            get
            {
                if (this.index < this.Module.Instructions.Count)
                {
                    return this.Module.Instructions[this.index].Range.Start.Line;
                }
                else
                {
                    return this.Module.Syntax.Range.End.Line;
                }
            }
        }
    }
}
