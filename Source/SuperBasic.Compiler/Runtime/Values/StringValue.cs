// <copyright file="StringValue.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System.Diagnostics;
    using System.Globalization;
    using SuperBasic.Utilities;

    public sealed class StringValue : BaseValue
    {
        private StringValue(string value)
        {
            Debug.Assert(!value.IsDefault(), "Value should never be null.");
            this.Value = value;
        }

        public string Value { get; private set; }

        internal static StringValue Empty => new StringValue(string.Empty);

        public static BaseValue Create(string value)
        {
            switch (value.Trim().ToLower(CultureInfo.CurrentCulture))
            {
                case "true":
                    return new BooleanValue(true);
                case "false":
                    return new BooleanValue(false);
                case string other when decimal.TryParse(other, out decimal decimalResult):
                    return new NumberValue(decimalResult);
                default:
                    return new StringValue(value);
            }
        }

        public override string ToDisplayString() => this.Value;

        internal override bool ToBoolean() => false;

        internal override decimal ToNumber() => 0;

        internal override ArrayValue ToArray() => new ArrayValue();
    }
}
