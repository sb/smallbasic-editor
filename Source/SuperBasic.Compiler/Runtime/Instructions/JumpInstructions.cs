// <copyright file="JumpInstructions.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System;
    using System.Linq;
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

    internal sealed class TransientUnconditionalGoToInstruction : BaseJumpInstruction
    {
        public TransientUnconditionalGoToInstruction(string label, TextRange range)
            : base(range)
        {
            this.Label = label;
        }

        public string Label { get; private set; }

        protected override int GetNextInstructionIndex(SuperBasicEngine engine) => throw new InvalidOperationException("This should have been removed during rewriting.");
    }

    internal sealed class TransientConditionalGoToInstruction : BaseJumpInstruction
    {
        public TransientConditionalGoToInstruction(string trueLabelOpt, string falseLabelOpt, TextRange range)
            : base(range)
        {
            this.TrueLabelOpt = trueLabelOpt;
            this.FalseLabelOpt = falseLabelOpt;
        }

        public string TrueLabelOpt { get; private set; }

        public string FalseLabelOpt { get; private set; }

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
        private readonly int? trueTargetOpt;
        private readonly int? falseTargetOpt;

        public ConditionalJumpInstruction(int? trueTargetOpt, int? falseTargetOpt, TextRange range)
            : base(range)
        {
            this.trueTargetOpt = trueTargetOpt;
            this.falseTargetOpt = falseTargetOpt;
        }

        protected override int GetNextInstructionIndex(SuperBasicEngine engine)
        {
            if (engine.EvaluationStack.Pop().ToBoolean())
            {
                return this.trueTargetOpt ?? engine.ExecutionStack.Last().InstructionIndex + 1;
            }
            else
            {
                return this.falseTargetOpt ?? engine.ExecutionStack.Last().InstructionIndex + 1;
            }
        }
    }
}
