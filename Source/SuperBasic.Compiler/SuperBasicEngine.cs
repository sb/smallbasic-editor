// <copyright file="SuperBasicEngine.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Utilities;

    public enum ExecutionMode
    {
        RunToEnd,
        Debug,
        NextStatement,
    }

    public enum ExecutionState
    {
        Running,
        Paused,
        BlockedOnStringInput,
        BlockedOnNumberInput,
        Terminated,
    }

    public sealed class SuperBasicEngine
    {
        private ExecutionMode mode = ExecutionMode.RunToEnd;
        private int currentSourceLine = 0;

        public SuperBasicEngine(SuperBasicCompilation compilation)
        {
            Debug.Assert(!compilation.Diagnostics.Any(), "Cannot execute a compilation with errors.");

            this.State = ExecutionState.Running;

            this.ExecutionStack = new Stack<Frame>();
            this.EvaluationStack = new Stack<BaseValue>();
            this.Memory = new ArrayValue();
            this.Modules = new Dictionary<string, RuntimeModule>();

            // TODO: emit compilation into this.modules
            // add main to exeuction stack
        }

        public ExecutionState State { get; private set; }

        internal Stack<Frame> ExecutionStack { get; private set; }

        internal Stack<BaseValue> EvaluationStack { get; private set; }

        internal ArrayValue Memory { get; private set; }

        internal Dictionary<string, RuntimeModule> Modules { get; private set; }

        public DebuggerSnapshot GetSnapshot()
        {
            return new DebuggerSnapshot(this.currentSourceLine, this.ExecutionStack, this.Memory);
        }

        public void SetExecutionMode(ExecutionMode mode)
        {
            Debug.Assert(this.State != ExecutionState.Terminated, "Engine is already terminated.");
            this.mode = mode;
        }

        public void Execute()
        {
            Debug.Assert(this.State == ExecutionState.Running || this.State == ExecutionState.Paused, "Engine is not in a executable state.");

            if (this.State == ExecutionState.Paused)
            {
                this.State = ExecutionState.Running;
            }

            while (this.State == ExecutionState.Running)
            {
                if (this.ExecutionStack.Count == 0)
                {
                    this.State = ExecutionState.Terminated;
                    return;
                }

                Frame frame = this.ExecutionStack.Peek();
                if (frame.InstructionIndex == frame.Module.Instructions.Count)
                {
                    this.ExecutionStack.Pop();
                    continue;
                }

                BaseInstruction instruction = frame.Module.Instructions[frame.InstructionIndex];
                int instructionLine = instruction.Range.Start.Line;
                if (instructionLine != this.currentSourceLine && this.mode == ExecutionMode.NextStatement)
                {
                    this.currentSourceLine = instructionLine;
                    this.State = ExecutionState.Paused;
                    return;
                }

                instruction.Execute(this, frame);
            }
        }

        internal void Pause()
        {
            Debug.Assert(this.State == ExecutionState.Running, "Engine is not running to be paused.");
            if (this.mode != ExecutionMode.RunToEnd)
            {
                this.State = ExecutionState.Paused;
            }
        }

        internal void BlockOnStringInput()
        {
            Debug.Assert(this.State == ExecutionState.Running, "Engine is not running to be blocked.");
            this.State = ExecutionState.BlockedOnStringInput;
        }

        internal void BlockOnNumberInput()
        {
            Debug.Assert(this.State == ExecutionState.Running, "Engine is not running to be blocked.");
            this.State = ExecutionState.BlockedOnNumberInput;
        }
    }
}
