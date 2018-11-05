// <copyright file="SuperBasicEngine.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using SuperBasic.Compiler.Binding;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Utilities;

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
        private readonly bool isDebugging;
        private readonly SuperBasicCompilation compilation;
        private readonly Dictionary<(string library, string eventName), string> eventCallbacks;

        private int currentSourceLine;

        public SuperBasicEngine(SuperBasicCompilation compilation, IEngineLibraries libraries, bool isDebugging = false)
        {
            Debug.Assert(!compilation.Diagnostics.Any(), "Cannot execute a compilation with errors.");

            this.isDebugging = isDebugging;
            this.compilation = compilation;
            this.eventCallbacks = new Dictionary<(string library, string eventName), string>();

            this.currentSourceLine = 0;

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

        public ExecutionState State { get; private set; }

        internal LinkedList<Frame> ExecutionStack { get; private set; }

        internal Stack<BaseValue> EvaluationStack { get; private set; }

        internal Dictionary<string, BaseValue> Memory { get; private set; }

        internal Dictionary<string, RuntimeModule> Modules { get; private set; }

        internal IEngineLibraries Libraries { get; private set; }

        public DebuggerSnapshot GetSnapshot()
        {
            return new DebuggerSnapshot(this.currentSourceLine, this.ExecutionStack, this.Memory);
        }

        public async Task Execute(bool pauseAtNextStatement = false)
        {
            Debug.Assert(this.isDebugging || !pauseAtNextStatement, $"Cannot {nameof(pauseAtNextStatement)} if not debugging.");
            Debug.Assert(this.State == ExecutionState.Running || this.State == ExecutionState.Paused, "Engine is not in a executable state.");

            if (this.State == ExecutionState.Paused)
            {
                this.State = ExecutionState.Running;
            }

            while (this.State == ExecutionState.Running)
            {
                if (this.ExecutionStack.Count == 0)
                {
                    if (!this.compilation.UsesGraphicsWindow)
                    {
                        this.State = ExecutionState.Terminated;
                    }
                }

                Frame frame = this.ExecutionStack.Last();
                if (frame.InstructionIndex == frame.Module.Instructions.Count)
                {
                    this.ExecutionStack.RemoveLast();
                    continue;
                }

                BaseInstruction instruction = frame.Module.Instructions[frame.InstructionIndex];
                int instructionLine = instruction.Range.Start.Line;

                bool shouldPause = pauseAtNextStatement && this.State == ExecutionState.Running && this.currentSourceLine != instructionLine;
                this.currentSourceLine = instructionLine;

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

        internal void SetEventCallback(string library, string eventName, string subModule)
        {
            this.eventCallbacks[(library, eventName)] = subModule;
        }

        internal void RaiseEvent(string library, string eventName)
        {
            if (this.eventCallbacks.TryGetValue((library, eventName), out string subModule))
            {
                this.ExecutionStack.AddFirst(new Frame(this.Modules[subModule]));
            }
        }

        internal void Pause()
        {
            Debug.Assert(this.State == ExecutionState.Running, "Engine is not running to be paused.");
            if (this.isDebugging)
            {
                this.State = ExecutionState.Paused;
            }
        }

        internal void Terminate()
        {
            Debug.Assert(this.State == ExecutionState.Running, "Engine is not running to be terminated.");
            this.State = ExecutionState.Terminated;
            this.ExecutionStack.Clear();
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

        private RuntimeModule EmitAndSaveModule(string name, BoundStatementBlock body)
        {
            ModuleEmitter emitter = new ModuleEmitter(body);
            RuntimeModule module = new RuntimeModule(name, emitter.Instructions);

            this.Modules.Add(module.Name, module);
            return module;
        }
    }
}
