// <copyright file="DebuggerSnapshot.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Runtime
{
    using System.Collections.Generic;

    public sealed class DebuggerSnapshot
    {
        internal DebuggerSnapshot(int currentSourceLine, IReadOnlyCollection<Frame> executionStack, IReadOnlyDictionary<string, BaseValue> memory)
        {
            this.CurrentSourceLine = currentSourceLine;
            this.ExecutionStack = executionStack;
            this.Memory = memory;
        }

        public int CurrentSourceLine { get; set; }

        public IReadOnlyCollection<Frame> ExecutionStack { get; private set; }

        public IReadOnlyDictionary<string, BaseValue> Memory { get; private set; }
    }
}
