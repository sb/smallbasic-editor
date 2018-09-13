// <copyright file="OtherInstructions.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System;
    using SuperBasic.Compiler.Scanning;

    internal sealed class InvokeSubModuleInstruction : BaseNonJumpInstruction
    {
        private readonly string subModuleName;

        public InvokeSubModuleInstruction(string subModuleName, TextRange range)
            : base(range)
        {
            this.subModuleName = subModuleName;
        }

        protected override void Execute(SuperBasicEngine engine)
        {
            Frame frame = new Frame(engine.Modules[this.subModuleName]);
            engine.ExecutionStack.Push(frame);
        }
    }

    internal sealed class MethodInvocationInstruction : BaseNonJumpInstruction
    {
        private readonly string library;
        private readonly string method;

        public MethodInvocationInstruction(string library, string method, TextRange range)
            : base(range)
        {
            this.library = library;
            this.method = method;
        }

        protected override void Execute(SuperBasicEngine engine)
        {
            // TODO call method
        }
    }

    internal sealed class StorePropertyInstruction : BaseNonJumpInstruction
    {
        private readonly string library;
        private readonly string property;

        public StorePropertyInstruction(string library, string property, TextRange range)
            : base(range)
        {
            this.library = library;
            this.property = property;
        }

        protected override void Execute(SuperBasicEngine engine)
        {
            BaseValue value = engine.EvaluationStack.Pop();
            // TODO call setter
        }
    }

    internal sealed class LoadPropertyInstruction : BaseNonJumpInstruction
    {
        private readonly string library;
        private readonly string property;

        public LoadPropertyInstruction(string library, string property, TextRange range)
            : base(range)
        {
            this.library = library;
            this.property = property;
        }

        protected override void Execute(SuperBasicEngine engine)
        {
            BaseValue value = null;
            // TODO call getter
            engine.EvaluationStack.Push(value);
        }
    }
}
