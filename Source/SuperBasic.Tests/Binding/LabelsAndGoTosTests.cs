// <copyright file="LabelsAndGoTosTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Tests.Binding
{
    using FluentAssertions;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Diagnostics;
    using Xunit;

    public sealed class LabelsAndGoTosTests
    {
        [Fact]
        public void ItReportsGoToNonExistentLabel()
        {
            new SuperBasicCompilation(@"
label1:
GoTo label1
GoTo label2").VerifyDiagnostics(
                // GoTo label2
                //      ^^^^^^
                // The label 'label2' is not defined in the same module.
                new Diagnostic(DiagnosticCode.GoToUndefinedLabel, ((3, 5), (3, 10)), "label2"));
        }

        [Fact]
        public void ItReportsGoToLabelInMainModuleToLabelInSubModule()
        {
            new SuperBasicCompilation(@"
Sub x
    label1:
EndSub
GoTo label1").VerifyDiagnostics(
                // GoTo label1
                //      ^^^^^^
                // The label 'label1' is not defined in the same module.
                new Diagnostic(DiagnosticCode.GoToUndefinedLabel, ((4, 5), (4, 10)), "label1"));
        }

        [Fact]
        public void ItReportsGoToLabelInSubModuleToLabelInMainModule()
        {
            new SuperBasicCompilation(@"
Sub x
    GoTo label1
EndSub
label1:").VerifyDiagnostics(
                //     GoTo label1
                //          ^^^^^^
                // The label 'label1' is not defined in the same module.
                new Diagnostic(DiagnosticCode.GoToUndefinedLabel, ((2, 9), (2, 14)), "label1"));
        }

        [Fact]
        public void ItReportsDuplicateLabelsInMainModule()
        {
            new SuperBasicCompilation(@"
label1:
label1:
Sub x
    label1:
EndSub").VerifyDiagnostics(
                // label1:
                // ^^^^^^
                // A label with the same name 'label1' is already defined.
                new Diagnostic(DiagnosticCode.TwoLabelsWithTheSameName, ((2, 0), (2, 5)), "label1"));
        }

        [Fact]
        public void ItReportsDuplicateLabelsInSubModule()
        {
            new SuperBasicCompilation(@"
label1:
Sub x
    label1:
    label1:
EndSub").VerifyDiagnostics(
                //     label1:
                //     ^^^^^^
                // A label with the same name 'label1' is already defined.
                new Diagnostic(DiagnosticCode.TwoLabelsWithTheSameName, ((4, 4), (4, 9)), "label1"));
        }
    }
}
