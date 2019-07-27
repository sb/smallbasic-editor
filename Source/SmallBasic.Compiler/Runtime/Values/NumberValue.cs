// <copyright file="NumberValue.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Runtime
{
    using System.Globalization;

    public sealed class NumberValue : BaseValue
    {
        public NumberValue(decimal value)
        {
            this.Value = value;
        }

        public decimal Value { get; private set; }

        public override string ToDisplayString() => this.Value.ToString(CultureInfo.CurrentCulture);

        internal override bool ToBoolean() => false;

        internal override decimal ToNumber() => this.Value;

        internal override ArrayValue ToArray() => new ArrayValue();
    }
}
