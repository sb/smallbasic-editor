// <copyright file="MemoryInstructions.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System;
    using System.Diagnostics;
    using SuperBasic.Compiler.Scanning;

    internal sealed class StoreVariableInstruction : BaseNonJumpInstruction
    {
        private readonly string variable;

        public StoreVariableInstruction(string variable, TextRange range)
            : base(range)
        {
            this.variable = variable;
        }

        protected override void Execute(SuperBasicEngine engine)
        {
            engine.Memory.Contents[this.variable] = engine.EvaluationStack.Pop();
        }
    }

    internal sealed class LoadVariableInstruction : BaseNonJumpInstruction
    {
        private readonly string variable;

        public LoadVariableInstruction(string variable, TextRange range)
            : base(range)
        {
            this.variable = variable;
        }

        protected override void Execute(SuperBasicEngine engine)
        {
            if (engine.Memory.Contents.TryGetValue(this.variable, out BaseValue value))
            {
                engine.EvaluationStack.Push(value);
            }
            else
            {
                engine.EvaluationStack.Push(StringValue.Empty);
            }
        }
    }

    internal sealed class StoreArrayElementInstruction : BaseNonJumpInstruction
    {
        private readonly string array;
        private readonly int indicesCount;

        public StoreArrayElementInstruction(string array, int indicesCount, TextRange range)
            : base(range)
        {
            this.array = array;
            this.indicesCount = indicesCount;
        }

        protected override void Execute(SuperBasicEngine engine)
        {
            BaseValue value = engine.EvaluationStack.Pop();

            string index = this.array;
            ArrayValue memory = engine.Memory;
            int remainingIndices = this.indicesCount;

            while (remainingIndices-- > 0)
            {
                if (!memory.Contents.ContainsKey(index) || !(memory.Contents[index] is ArrayValue))
                {
                    memory.Contents[index] = new ArrayValue();
                }

                memory = (ArrayValue)memory.Contents[index];
                index = engine.EvaluationStack.Pop().ToString();
            }

            memory.Contents[index] = value;
        }
    }

    internal sealed class LoadArrayElementInstruction : BaseNonJumpInstruction
    {
        private readonly string array;
        private readonly int indicesCount;

        public LoadArrayElementInstruction(string array, int indicesCount, TextRange range)
            : base(range)
        {
            this.array = array;
            this.indicesCount = indicesCount;
        }

        protected override void Execute(SuperBasicEngine engine)
        {
            string index = this.array;
            ArrayValue memory = engine.Memory;
            int remainingIndices = this.indicesCount;

            while (remainingIndices-- > 0)
            {
                if (!memory.Contents.ContainsKey(index) || !(memory.Contents[index] is ArrayValue))
                {
                    memory.Contents[index] = new ArrayValue();
                }

                memory = (ArrayValue)memory.Contents[index];
                index = engine.EvaluationStack.Pop().ToString();
            }

            if (memory.Contents.TryGetValue(index, out BaseValue value))
            {
                engine.EvaluationStack.Push(value);
            }
            else
            {
                engine.EvaluationStack.Push(StringValue.Empty);
            }
        }
    }

    internal sealed class PushValueInstruction : BaseNonJumpInstruction
    {
        private readonly BaseValue value;

        public PushValueInstruction(BaseValue value, TextRange range)
            : base(range)
        {
            this.value = value;
        }

        protected override void Execute(SuperBasicEngine engine)
        {
            engine.EvaluationStack.Push(this.value);
        }
    }
}
