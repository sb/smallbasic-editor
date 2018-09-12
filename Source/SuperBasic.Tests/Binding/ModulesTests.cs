// <copyright file="ModulesTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Tests.Binding
{
    using FluentAssertions;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Diagnostics;
    using Xunit;

    public sealed class ModulesTests
    {
        [Fact]
        public void ItReportsMultipleSubModulesWithTheSameName()
        {
            new SuperBasicCompilation(@"
Sub x
EndSub
Sub y
EndSub
Sub x
EndSub").VerifyDiagnostics(
                // Sub x
                //     ^
                // A submodule with the same name 'x' is already defined in the same program.
                new Diagnostic(DiagnosticCode.TwoSubModulesWithTheSameName, ((5, 4), (5, 4)), "x"));
        }

        [Fact]
        public void ItReportsAssigningNonSubModuleToEvent()
        {
            new SuperBasicCompilation(@"
Sub x
EndSub

Controls.ButtonClicked = x
Controls.ButtonClicked = y").VerifyDiagnostics(
                // Controls.ButtonClicked = y
                // ^^^^^^^^^^^^^^^^^^^^^^
                // You can only assign submodules to events.
                new Diagnostic(DiagnosticCode.AssigningNonSubModuleToEvent, ((5, 0), (5, 21))));
        }

        [Fact]
        public void ItReportsPassingArgumentsToModules()
        {
            new SuperBasicCompilation(@"
Sub x
EndSub

x(0)").VerifyDiagnostics(
                // x(0)
                // ^^^^
                // You are passing '1' arguments to this method, while it expects '0' arguments.
                new Diagnostic(DiagnosticCode.UnexpectedArgumentsCount, ((4, 0), (4, 3)), "1", "0"));
        }
    }
}
