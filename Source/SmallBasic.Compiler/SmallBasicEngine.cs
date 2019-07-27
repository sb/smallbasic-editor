// <copyright file="SmallBasicEngine.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using SmallBasic.Compiler.Binding;
    using SmallBasic.Compiler.Runtime;
    using SmallBasic.Utilities;

    public enum ExecutionMode
    {
        RunToEnd,
        Debug,
        NextLine
    }

    public enum ExecutionState
    {
        Running,
        Paused,
        BlockedOnStringInput,
        BlockedOnNumberInput,
        Terminated,
    }

    public sealed class SmallBasicEngine
    {
        private readonly SmallBasicCompilation compilation;
        private readonly Dictionary<string, string> eventCallbacks;

        public SmallBasicEngine(SmallBasicCompilation compilation, IEngineLibraries libraries)
        {
            Debug.Assert(!compilation.Diagnostics.Any(), "Cannot execute a compilation with errors.");

            this.compilation = compilation;
            this.eventCallbacks = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

            this.CurrentSourceLine = 0;

            this.Mode = ExecutionMode.RunToEnd;
            this.State = ExecutionState.Running;
            this.ExecutionStack = new LinkedList<Frame>();
            this.EvaluationStack = new Stack<BaseValue>();
            this.Memory = new Dictionary<string, BaseValue>();
            this.Modules = new Dictionary<string, RuntimeModule>();
            this.Libraries = libraries;

            this.Libraries.SetEventCallbacks(this);

            RuntimeModule mainModule = this.EmitAndSaveModule("Program", compilation.MainModule);
            foreach (BoundSubModule subModule in compilation.SubModules.Values)
            {
                this.EmitAndSaveModule(subModule.Name, subModule.Body);
            }

            this.ExecutionStack.AddLast(new Frame(mainModule));
        }

        public ExecutionMode Mode { get; set; }

        public ExecutionState State { get; private set; }

        public int CurrentSourceLine { get; private set; }

        internal LinkedList<Frame> ExecutionStack { get; private set; }

        internal Stack<BaseValue> EvaluationStack { get; private set; }

        internal Dictionary<string, BaseValue> Memory { get; private set; }

        internal Dictionary<string, RuntimeModule> Modules { get; private set; }

        internal IEngineLibraries Libraries { get; private set; }

        public DebuggerSnapshot GetSnapshot()
        {
            return new DebuggerSnapshot(this.CurrentSourceLine, this.ExecutionStack, this.Memory);
        }

        public async Task Execute()
        {
            Debug.Assert(this.State == ExecutionState.Running || this.State == ExecutionState.Paused, "Engine is not in a executable state.");

            while (this.State == ExecutionState.Running)
            {
                if (this.ExecutionStack.Count == 0)
                {
                    if (!this.compilation.Analysis.ListensToEvents)
                    {
                        this.Terminate();
                    }

                    break;
                }

                Frame frame = this.ExecutionStack.Last();
                if (frame.InstructionIndex == frame.Module.Instructions.Count)
                {
                    this.ExecutionStack.RemoveLast();
                    continue;
                }

                BaseInstruction instruction = frame.Module.Instructions[frame.InstructionIndex];
                int instructionLine = instruction.Range.Start.Line;

                bool shouldPause = this.Mode == ExecutionMode.NextLine && this.CurrentSourceLine != instructionLine;
                this.CurrentSourceLine = instructionLine;

                if (shouldPause)
                {
                    this.Pause();
                    return;
                }
                else
                {
                    await instruction.Execute(this, frame).ConfigureAwait(false);
                }
            }
        }

        public void InputReceived()
        {
            switch (this.State)
            {
                case ExecutionState.BlockedOnNumberInput:
                case ExecutionState.BlockedOnStringInput:
                    this.State = ExecutionState.Running;
                    break;
                default:
                    throw ExceptionUtilities.UnexpectedValue(this.State);
            }
        }

        public void Pause()
        {
            if (this.Mode != ExecutionMode.RunToEnd)
            {
                this.State = ExecutionState.Paused;
            }
        }

        public void Continue()
        {
            if (this.Mode != ExecutionMode.RunToEnd)
            {
                this.State = ExecutionState.Running;
            }
        }

        public void Terminate()
        {
            this.State = ExecutionState.Terminated;
            this.ExecutionStack.Clear();
        }

        internal void SetEventCallback(string library, string eventName, string subModule)
        {
            this.eventCallbacks[$"{library}.${eventName}"] = subModule;
        }

        internal void RaiseEvent(string library, string eventName)
        {
            if (this.eventCallbacks.TryGetValue($"{library}.${eventName}", out string subModule))
            {
                var existing = this.ExecutionStack.Take(this.ExecutionStack.Count - 1).FirstOrDefault(frame => frame.Module.Name == subModule);
                if (!existing.IsDefault())
                {
                    this.ExecutionStack.Remove(existing);
                }

                this.ExecutionStack.AddFirst(new Frame(this.Modules[subModule]));
            }
        }

        internal void BlockOnStringInput()
        {
            this.State = ExecutionState.BlockedOnStringInput;
        }

        internal void BlockOnNumberInput()
        {
            this.State = ExecutionState.BlockedOnNumberInput;
        }

        private RuntimeModule EmitAndSaveModule(string name, BoundStatementBlock body)
        {
            ModuleEmitter emitter = new ModuleEmitter(body);
            RuntimeModule module = new RuntimeModule(name, emitter.Instructions, body.Syntax);

            this.Modules.Add(module.Name, module);
            return module;
        }
    }
}
