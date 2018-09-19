// <copyright file="ProgramLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using System.Threading;
    using SuperBasic.Compiler.Runtime;

    public sealed class ProgramLibrary : IProgramLibrary
    {
        public void Delay(decimal milliSeconds) => Thread.Sleep((int)milliSeconds);

        public void End() => throw new InvalidOperationException("This should have been removed in binding.");

        public void Pause() => throw new InvalidOperationException("This should have been removed in binding.");
    }
}
