// <copyright file="ProgramLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using SmallBasic.Compiler.Runtime;

    internal sealed class ProgramLibrary : IProgramLibrary
    {
        public Task Delay(decimal milliSeconds) => Task.Delay((int)milliSeconds);

        public void End() => throw new InvalidOperationException("This should have been removed in binding.");

        public void Pause() => throw new InvalidOperationException("This should have been removed in binding.");
    }
}
