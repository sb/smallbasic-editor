// <copyright file="ProgramLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System;
    using System.Threading;

    internal static partial class Libraries
    {
        private static void Execute_Program_Delay(decimal milliSeconds) => Thread.Sleep((int)milliSeconds);

        private static void Execute_Program_End() => throw new InvalidOperationException("This should have been removed in binding.");

        private static void Execute_Program_Pause() => throw new InvalidOperationException("This should have been removed in binding.");
    }
}
