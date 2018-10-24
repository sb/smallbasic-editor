// <copyright file="BaseValue.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
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
