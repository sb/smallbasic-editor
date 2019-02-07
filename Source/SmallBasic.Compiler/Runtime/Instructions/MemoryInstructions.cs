// <copyright file="MemoryInstructions.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using SmallBasic.Compiler.Scanning;

    internal sealed class StoreVariableInstruction : BaseNonJumpInstruction
    {
        private readonly string variable;

        public StoreVariableInstruction(string variable, TextRange range)
            : base(range)
        {
            this.variable = variable;
        }

        protected override void Execute(SmallBasicEngine engine)
        {
            engine.Memory[this.variable] = engine.EvaluationStack.Pop();
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

        protected override void Execute(SmallBasicEngine engine)
        {
            if (engine.Memory.TryGetValue(this.variable, out BaseValue value))
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

        protected override void Execute(SmallBasicEngine engine)
        {
            BaseValue value = engine.EvaluationStack.Pop();
            executeAux(engine.Memory, this.array, this.indicesCount);

            BaseValue executeAux(Dictionary<string, BaseValue> memory, string index, int remainingIndices)
            {
                if (remainingIndices == 0)
                {
                    memory[index] = value;
                }
                else
                {
                    Dictionary<string, BaseValue> nextMemory =
                        memory.TryGetValue(index, out BaseValue elementValue) && elementValue is ArrayValue elementArrayValue
                        ? elementArrayValue.ToDictionary()
                        : new Dictionary<string, BaseValue>();

                    string nextIndex = engine.EvaluationStack.Pop().ToString();

                    memory[index] = executeAux(nextMemory, nextIndex, remainingIndices - 1);
                }

                return new ArrayValue(memory);
            }
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

        protected override void Execute(SmallBasicEngine engine)
        {
            IReadOnlyDictionary<string, BaseValue> memory = engine.Memory;
            int remainingIndices = this.indicesCount;
            string index = this.array;

            while (remainingIndices > 0 && memory.TryGetValue(index, out BaseValue elementValue) && elementValue is ArrayValue elementArrayValue)
            {
                index = engine.EvaluationStack.Pop().ToString();
                memory = elementArrayValue;
                remainingIndices--;
            }

            if (remainingIndices > 0 || !memory.TryGetValue(index, out BaseValue value))
            {
                engine.EvaluationStack.Push(StringValue.Empty);
            }
            else
            {
                engine.EvaluationStack.Push(value);
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

        protected override void Execute(SmallBasicEngine engine)
        {
            engine.EvaluationStack.Push(this.value);
        }
    }
}
