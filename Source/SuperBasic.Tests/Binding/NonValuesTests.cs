// <copyright file="NonValuesTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Tests.Binding
{
    using FluentAssertions;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Diagnostics;
    using Xunit;

    public sealed class NonValuesTests
    {
        [Fact]
        public void ItReportsInForLoopFromExpression()
        {
            new SuperBasicCompilation(@"
For x = TextWindow.WriteLine("""") To 5
EndFor").VerifyDiagnostics(
                // For x = TextWindow.WriteLine("") To 5
                //         ^^^^^^^^^^^^^^^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 8), (1, 31))));
        }

        [Fact]
        public void ItReportsInForLoopToExpression()
        {
            new SuperBasicCompilation(@"
For x = 1 To TextWindow.WriteLine("""")
EndFor").VerifyDiagnostics(
                // For x = 1 To TextWindow.WriteLine("")
                //              ^^^^^^^^^^^^^^^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 13), (1, 36))));
        }

        [Fact]
        public void ItReportsInForLoopStepExpression()
        {
            new SuperBasicCompilation(@"
For x = 1 To 5 Step TextWindow.WriteLine("""")
EndFor").VerifyDiagnostics(
                // For x = 1 To 5 Step TextWindow.WriteLine("")
                //                     ^^^^^^^^^^^^^^^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 20), (1, 43))));
        }

        [Fact]
        public void ItReportsNonValueInIfStatementExpression()
        {
            new SuperBasicCompilation(@"
If TextWindow.WriteLine("""") Then
EndIf").VerifyDiagnostics(
                // If TextWindow.WriteLine("") Then
                //    ^^^^^^^^^^^^^^^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 3), (1, 26))));
        }

        [Fact]
        public void ItReportsNonValueInIfElseStatementExpression()
        {
            new SuperBasicCompilation(@"
If ""True"" Then
ElseIf TextWindow.WriteLine("""") Then
EndIf").VerifyDiagnostics(
                // ElseIf TextWindow.WriteLine("") Then
                //        ^^^^^^^^^^^^^^^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((2, 7), (2, 30))));
        }

        [Fact]
        public void ItReportsNonValueInWhileStatementExpression()
        {
            new SuperBasicCompilation(@"
While TextWindow.WriteLine("""")
EndWhile").VerifyDiagnostics(
                // While TextWindow.WriteLine("")
                //       ^^^^^^^^^^^^^^^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 6), (1, 29))));
        }
    }
}
