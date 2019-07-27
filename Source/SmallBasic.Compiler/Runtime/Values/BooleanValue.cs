// <copyright file="BooleanValue.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Runtime
{
    public sealed class BooleanValue : BaseValue
    {
        public BooleanValue(bool value)
        {
            this.Value = value;
        }

        public bool Value { get; private set; }

        public override string ToDisplayString() => this.Value ? "True" : "False";

        internal override bool ToBoolean() => this.Value;

        internal override decimal ToNumber() => 0;

        internal override ArrayValue ToArray() => new ArrayValue();
    }
}
