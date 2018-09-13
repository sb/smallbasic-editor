// <copyright file = "DebuggerSnapshot.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public sealed class DebuggerSnapshot
    {
        internal DebuggerSnapshot(int currentSourceLine, Stack<Frame> executionStack, ArrayValue memory)
        {
            Debug.Assert(executionStack.Peek().Module.Instructions[executionStack.Peek().InstructionIndex].Range.Start.Line == currentSourceLine, $"Invalid '{nameof(currentSourceLine)}'.");

            this.CurrentSourceLine = currentSourceLine;
            this.ExecutionStack = executionStack;
            this.Memory = memory.Contents.ToDictionary(pair => pair.Key, pair => pair.Value.ToString());
        }

        public int CurrentSourceLine { get; set; }

        public IReadOnlyCollection<Frame> ExecutionStack { get; private set; }

        public IReadOnlyDictionary<string, string> Memory { get; private set; }
    }
}
