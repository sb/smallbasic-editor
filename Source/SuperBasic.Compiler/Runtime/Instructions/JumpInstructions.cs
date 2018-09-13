// <copyright file="JumpInstructions.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System;
    using SuperBasic.Compiler.Scanning;

    internal sealed class TransientLabelInstruction : BaseJumpInstruction
    {
        public TransientLabelInstruction(string label, TextRange range)
            : base(range)
        {
            this.Label = label;
        }

        public string Label { get; private set; }

        protected override int GetNextInstructionIndex(SuperBasicEngine engine) => throw new InvalidOperationException("This should have been removed during rewriting.");
    }

    internal sealed class TransientGoToInstruction : BaseJumpInstruction
    {
        public TransientGoToInstruction(string label, TextRange range)
            : base(range)
        {
            this.Label = label;
        }

        public string Label { get; private set; }

        protected override int GetNextInstructionIndex(SuperBasicEngine engine) => throw new InvalidOperationException("This should have been removed during rewriting.");
    }

    internal sealed class TransientConditionalGoToInstruction : BaseJumpInstruction
    {
        public TransientConditionalGoToInstruction(string trueLabel, string falseLabel, TextRange range)
            : base(range)
        {
            this.TrueLabel = trueLabel;
            this.FalseLabel = falseLabel;
        }

        public string TrueLabel { get; private set; }

        public string FalseLabel { get; private set; }

        protected override int GetNextInstructionIndex(SuperBasicEngine engine) => throw new InvalidOperationException("This should have been removed during rewriting.");
    }

    internal sealed class UnconditionalJumpInstruction : BaseJumpInstruction
    {
        private readonly int target;

        public UnconditionalJumpInstruction(int target, TextRange range)
            : base(range)
        {
            this.target = target;
        }

        protected override int GetNextInstructionIndex(SuperBasicEngine engine) => this.target;
    }

    internal sealed class ConditionalJumpInstruction : BaseJumpInstruction
    {
        private readonly int trueTarget;
        private readonly int falseTarget;

        public ConditionalJumpInstruction(int trueTarget, int falseTarget, TextRange range)
            : base(range)
        {
            this.trueTarget = trueTarget;
            this.falseTarget = falseTarget;
        }

        protected override int GetNextInstructionIndex(SuperBasicEngine engine) => engine.EvaluationStack.Pop().ToBoolean() ? this.trueTarget : this.falseTarget;
    }
}
