// <copyright file="BaseInstructions.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Runtime
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using SmallBasic.Compiler.Scanning;

    internal abstract class BaseInstruction
    {
        public BaseInstruction(TextRange range)
        {
            Debug.Assert(range.Start.Line == range.End.Line, "Multiline instructions are not supported.");
            this.Range = range;
        }

        public TextRange Range { get; private set; }

        public abstract Task Execute(SmallBasicEngine engine, Frame frame);
    }

    internal abstract class BaseJumpInstruction : BaseInstruction
    {
        public BaseJumpInstruction(TextRange range)
            : base(range)
        {
        }

        public override sealed Task Execute(SmallBasicEngine engine, Frame frame)
        {
            frame.InstructionIndex = this.GetNextInstructionIndex(engine);
            return Task.CompletedTask;
        }

        protected abstract int GetNextInstructionIndex(SmallBasicEngine engine);
    }

    internal abstract class BaseNonJumpInstruction : BaseInstruction
    {
        public BaseNonJumpInstruction(TextRange range)
            : base(range)
        {
        }

        public override sealed Task Execute(SmallBasicEngine engine, Frame frame)
        {
            frame.InstructionIndex++;
            this.Execute(engine);
            return Task.CompletedTask;
        }

        protected abstract void Execute(SmallBasicEngine engine);
    }

    internal abstract class BaseAsyncNonJumpInstruction : BaseInstruction
    {
        public BaseAsyncNonJumpInstruction(TextRange range)
            : base(range)
        {
        }

        public override sealed Task Execute(SmallBasicEngine engine, Frame frame)
        {
            frame.InstructionIndex++;
            return this.Execute(engine);
        }

        protected abstract Task Execute(SmallBasicEngine engine);
    }

    internal abstract class BaseUnaryInstruction : BaseNonJumpInstruction
    {
        public BaseUnaryInstruction(TextRange range)
            : base(range)
        {
        }

        protected override sealed void Execute(SmallBasicEngine engine)
        {
            BaseValue value = engine.EvaluationStack.Pop();
            engine.EvaluationStack.Push(this.Execute(value));
        }

        protected abstract BaseValue Execute(BaseValue value);
    }

    internal abstract class BaseBinaryInstruction : BaseNonJumpInstruction
    {
        public BaseBinaryInstruction(TextRange range)
            : base(range)
        {
        }

        protected override sealed void Execute(SmallBasicEngine engine)
        {
            BaseValue second = engine.EvaluationStack.Pop();
            BaseValue first = engine.EvaluationStack.Pop();
            engine.EvaluationStack.Push(this.Execute(first, second));
        }

        protected abstract BaseValue Execute(BaseValue first, BaseValue second);
    }
}
