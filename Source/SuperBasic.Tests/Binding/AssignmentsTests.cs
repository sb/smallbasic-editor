// <copyright file="AssignmentsTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Tests.Binding
{
    using FluentAssertions;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Diagnostics;
    using Xunit;

    public sealed class AssignmentsTests
    {
        [Fact]
        public void ItReportsAssigningToLibraryMethodInvocation()
        {
            new SuperBasicCompilation(@"
TextWindow.WriteLine(0) = 5").VerifyDiagnostics(
                // TextWindow.WriteLine(0) = 5
                // ^^^^^^^^^^^^^^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 0), (1, 22))));
        }

        [Fact]
        public void ItReportsAssigningToPropertyWithoutASetter()
        {
            new SuperBasicCompilation(@"
Clock.Time = 5").VerifyDiagnostics(
                // Clock.Time = 5
                // ^^^^^^^^^^
                // Property 'Clock.Time' cannot be assigned to. It is ready only.
                new Diagnostic(DiagnosticCode.PropertyHasNoSetter, ((1, 0), (1, 9)), "Clock", "Time"));
        }

        [Fact]
        public void ItReportsAssigningToAnd()
        {
            new SuperBasicCompilation(@"
x and y = 5").VerifyDiagnostics(
                // x and y = 5
                // ^^^^^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 10))));
        }

        [Fact]
        public void ItReportsAssigningToOr()
        {
            new SuperBasicCompilation(@"
x or y = 5").VerifyDiagnostics(
                // x or y = 5
                // ^^^^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 9))));
        }

        [Fact]
        public void ItReportsAssigningToEqual()
        {
            new SuperBasicCompilation(@"
x = y = 5").VerifyDiagnostics(
                // x = y = 5
                // ^^^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 8))));
        }

        [Fact]
        public void ItReportsAssigningToNotEqual()
        {
            new SuperBasicCompilation(@"
x <> y = 5").VerifyDiagnostics(
                // x <> y = 5
                // ^^^^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 9))));
        }

        [Fact]
        public void ItReportsAssigningToAddition()
        {
            new SuperBasicCompilation(@"
x + y = 5").VerifyDiagnostics(
                // x + y = 5
                // ^^^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 8))));
        }

        [Fact]
        public void ItReportsAssigningToSubtraction()
        {
            new SuperBasicCompilation(@"
x - y = 5").VerifyDiagnostics(
                // x - y = 5
                // ^^^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 8))));
        }

        [Fact]
        public void ItReportsAssigningToMultiplication()
        {
            new SuperBasicCompilation(@"
x * y = 5").VerifyDiagnostics(
                // x * y = 5
                // ^^^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 8))));
        }

        [Fact]
        public void ItReportsAssigningToDivision()
        {
            new SuperBasicCompilation(@"
x / y = 5").VerifyDiagnostics(
                // x / y = 5
                // ^^^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 8))));
        }

        [Fact]
        public void ItReportsAssigningToLessThan()
        {
            new SuperBasicCompilation(@"
x < y = 5").VerifyDiagnostics(
                // x < y = 5
                // ^^^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 8))));
        }

        [Fact]
        public void ItReportsAssigningToGreaterThan()
        {
            new SuperBasicCompilation(@"
x > y = 5").VerifyDiagnostics(
                // x > y = 5
                // ^^^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 8))));
        }

        [Fact]
        public void ItReportsAssigningToLessThanOrEqual()
        {
            new SuperBasicCompilation(@"
x <= y = 5").VerifyDiagnostics(
                // x <= y = 5
                // ^^^^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 9))));
        }

        [Fact]
        public void ItReportsAssigningToGreaterThanOrEqual()
        {
            new SuperBasicCompilation(@"
x >= y = 5").VerifyDiagnostics(
                // x >= y = 5
                // ^^^^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 9))));
        }

        [Fact]
        public void ItReportsAssigningToLibraryType()
        {
            new SuperBasicCompilation(@"
TextWindow = 5").VerifyDiagnostics(
                // TextWindow = 5
                // ^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 0), (1, 9))));
        }

        [Fact]
        public void ItReportsAssigningToLibraryMethod()
        {
            new SuperBasicCompilation(@"
TextWindow.WriteLine = 5").VerifyDiagnostics(
                // TextWindow.WriteLine = 5
                // ^^^^^^^^^^^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 0), (1, 19))));
        }

        [Fact]
        public void ItReportsAssigningToSubModule()
        {
            new SuperBasicCompilation(@"
Sub x
EndSub
x = 5").VerifyDiagnostics(
                // x = 5
                // ^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((3, 0), (3, 0))));
        }

        [Fact]
        public void ItReportsAssigningToSubModuleInvocation()
        {
            new SuperBasicCompilation(@"
Sub x
EndSub
x() = 5").VerifyDiagnostics(
                // x() = 5
                // ^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((3, 0), (3, 2))));
        }
    }
}
