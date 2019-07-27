// <copyright file="BaseValue.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Runtime
{
    using System.Diagnostics;

    [DebuggerDisplay("{ToDisplayString()}")]
    public abstract class BaseValue
    {
        public abstract string ToDisplayString();

        public sealed override string ToString() => this.ToDisplayString();

        internal abstract bool ToBoolean();

        internal abstract decimal ToNumber();

        internal abstract ArrayValue ToArray();
    }
}
