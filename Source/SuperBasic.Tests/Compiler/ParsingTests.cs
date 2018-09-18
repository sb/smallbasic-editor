// <copyright file="ParsingTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Tests.Compiler
{
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Diagnostics;
    using Xunit;

    public sealed class ParsingTests
    {
        [Fact]
        public void ItReportsUnterminatedSubModules()
        {
            new SuperBasicCompilation(@"
Sub a").VerifyDiagnostics(
                // Sub a
                //     ^
                // I was expecting to see 'EndSub' after this.
                new Diagnostic(DiagnosticCode.UnexpectedEndOfStream, ((1, 4), (1, 4)), "EndSub"));
        }

        [Fact]
        public void ItReportsNestedSubModules()
        {
            new SuperBasicCompilation(@"
If x < 1 Then
Sub b
EndSub
EndIf").VerifyDiagnostics(
                // Sub b
                // ^^^
                // I didn't expect to see 'Sub' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((2, 0), (2, 2)), "Sub"),
                // EndSub
                // ^^^^^^
                // I didn't expect to see 'EndSub' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((3, 0), (3, 5)), "EndSub"));
        }

        [Fact]
        public void ItReportsIncompleteNestedSubModules()
        {
            new SuperBasicCompilation(@"
Sub a
Sub b
EndSub").VerifyDiagnostics(
                // Sub b
                // ^^^
                // I didn't expect to see 'Sub' here. I was expecting 'EndSub' instead.
                new Diagnostic(DiagnosticCode.UnexpectedTokenFound, ((2, 0), (2, 2)), "Sub", "EndSub"));
        }

        [Fact]
        public void ItReportsEndSubWithoutSub()
        {
            new SuperBasicCompilation(@"
x = 1
EndSub").VerifyDiagnostics(
                // EndSub
                // ^^^^^^
                // I didn't expect to see 'EndSub' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((2, 0), (2, 5)), "EndSub"));
        }

        [Fact]
        public void ItReportsElseIfWithoutIf()
        {
            new SuperBasicCompilation(@"
x = 1
ElseIf x < 1 Then
EndIf").VerifyDiagnostics(
                // ElseIf x < 1 Then
                // ^^^^^^
                // I didn't expect to see 'ElseIf' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((2, 0), (2, 5)), "ElseIf"),
                // EndIf
                // ^^^^^
                // I didn't expect to see 'EndIf' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((3, 0), (3, 4)), "EndIf"));
        }

        [Fact]
        public void ItReportsElseWithoutIf()
        {
            new SuperBasicCompilation(@"
x = 1
Else
EndIf").VerifyDiagnostics(
                // Else
                // ^^^^
                // I didn't expect to see 'Else' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((2, 0), (2, 3)), "Else"),
                // EndIf
                // ^^^^^
                // I didn't expect to see 'EndIf' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((3, 0), (3, 4)), "EndIf"));
        }

        [Fact]
        public void ItReportsEndIfWithoutIf()
        {
            new SuperBasicCompilation(@"
x = 1
EndIf").VerifyDiagnostics(
                // EndIf
                // ^^^^^
                // I didn't expect to see 'EndIf' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((2, 0), (2, 4)), "EndIf"));
        }

        [Fact]
        public void ItReportsElseIfAfterElse()
        {
            new SuperBasicCompilation(@"
x = 1
If x > 1 Then
Else
ElseIf x < 1
EndIf").VerifyDiagnostics(
                // ElseIf x < 1
                // ^^^^^^
                // I didn't expect to see 'ElseIf' here. I was expecting 'EndIf' instead.
                new Diagnostic(DiagnosticCode.UnexpectedTokenFound, ((4, 0), (4, 5)), "ElseIf", "EndIf"),
                // ElseIf x < 1
                // ^^^^^^
                // I didn't expect to see 'ElseIf' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((4, 0), (4, 5)), "ElseIf"),
                // EndIf
                // ^^^^^
                // I didn't expect to see 'EndIf' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((5, 0), (5, 4)), "EndIf"));
        }

        [Fact]
        public void ItReportsEndWhileWithoutWhile()
        {
            new SuperBasicCompilation(@"
x = 1
EndWhile").VerifyDiagnostics(
                // EndWhile
                // ^^^^^^^^
                // I didn't expect to see 'EndWhile' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((2, 0), (2, 7)), "EndWhile"));
        }

        [Fact]
        public void ItReportsInvalidStartOfStatements()
        {
            new SuperBasicCompilation(@"
x = 1
Then").VerifyDiagnostics(
                // Then
                // ^^^^
                // I didn't expect to see 'Then' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((2, 0), (2, 3)), "Then"));
        }

        [Fact]
        public void ItReportsTokensAfterACompleteStatement()
        {
            new SuperBasicCompilation(@"
If x = 1 Then y
EndIf").VerifyDiagnostics(
                // If x = 1 Then y
                //               ^
                // This statement should go on a new line.
                new Diagnostic(DiagnosticCode.UnexpectedStatementInsteadOfNewLine, ((1, 14), (1, 14))));
        }

        [Fact]
        public void ItReportsAssignmentWithNonExpressions()
        {
            new SuperBasicCompilation(@"
x = .").VerifyDiagnostics(
                // x = .
                //     ^
                // I didn't expect to see '.' here. I was expecting 'identifier' instead.
                new Diagnostic(DiagnosticCode.UnexpectedTokenFound, ((1, 4), (1, 4)), ".", "identifier"));
        }

        [Fact]
        public void ItReportsAssignmentWithNothing()
        {
            new SuperBasicCompilation(@"
x =").VerifyDiagnostics(
                // x =
                //   ^
                // I was expecting to see 'identifier' after this.
                new Diagnostic(DiagnosticCode.UnexpectedEndOfStream, ((1, 2), (1, 2)), "identifier"));
        }

        [Fact]
        public void ItReportsNonExpressionsInWhileLoop()
        {
            new SuperBasicCompilation(@"
While .
EndWhile").VerifyDiagnostics(
                // While .
                //       ^
                // I didn't expect to see '.' here. I was expecting 'identifier' instead.
                new Diagnostic(DiagnosticCode.UnexpectedTokenFound, ((1, 6), (1, 6)), ".", "identifier"));
        }

        [Fact]
        public void ItReportsMissingThenAfterIf()
        {
            new SuperBasicCompilation(@"
If x < 1
EndIf").VerifyDiagnostics(
                // EndIf
                // ^^^^^
                // I didn't expect to see 'EndIf' here. I was expecting 'Then' instead.
                new Diagnostic(DiagnosticCode.UnexpectedTokenFound, ((2, 0), (2, 4)), "EndIf", "Then"));
        }

        [Fact]
        public void ItReportsMissingThenAfterElseIf()
        {
            new SuperBasicCompilation(@"
If x < 1 Then
ElseIf x > 1
EndIf").VerifyDiagnostics(
                // EndIf
                // ^^^^^
                // I didn't expect to see 'EndIf' here. I was expecting 'Then' instead.
                new Diagnostic(DiagnosticCode.UnexpectedTokenFound, ((3, 0), (3, 4)), "EndIf", "Then"));
        }

        [Fact]
        public void ItReportsStepAfterIf()
        {
            new SuperBasicCompilation(@"
If x < 1 Step
EndIf").VerifyDiagnostics(
                // If x < 1 Step
                //          ^^^^
                // I didn't expect to see 'Step' here. I was expecting 'Then' instead.
                new Diagnostic(DiagnosticCode.UnexpectedTokenFound, ((1, 9), (1, 12)), "Step", "Then"),
                // If x < 1 Step
                //          ^^^^
                // This statement should go on a new line.
                new Diagnostic(DiagnosticCode.UnexpectedStatementInsteadOfNewLine, ((1, 9), (1, 12))));
        }

        [Fact]
        public void ItReportsStepAfterElseIf()
        {
            new SuperBasicCompilation(@"
If x > 1 Then
ElseIf x < 1 Step
EndIf").VerifyDiagnostics(
                // ElseIf x < 1 Step
                //              ^^^^
                // I didn't expect to see 'Step' here. I was expecting 'Then' instead.
                new Diagnostic(DiagnosticCode.UnexpectedTokenFound, ((2, 13), (2, 16)), "Step", "Then"),
                // ElseIf x < 1 Step
                //              ^^^^
                // This statement should go on a new line.
                new Diagnostic(DiagnosticCode.UnexpectedStatementInsteadOfNewLine, ((2, 13), (2, 16))));
        }

        [Fact]
        public void ItReportsExtraTokensAfterIfStatement()
        {
            new SuperBasicCompilation(@"
For x  = 1 To 2 Step 1 :
EndFor").VerifyDiagnostics(
                // For x  = 1 To 2 Step 1 :
                //                        ^
                // This statement should go on a new line.
                new Diagnostic(DiagnosticCode.UnexpectedStatementInsteadOfNewLine, ((1, 23), (1, 23))));
        }

        [Fact]
        public void ItReportsUnevenParenthesis()
        {
            new SuperBasicCompilation(@"
TextWindow.WriteLine(5").VerifyDiagnostics(
                // TextWindow.WriteLine(5
                //                      ^
                // I was expecting to see ')' after this.
                new Diagnostic(DiagnosticCode.UnexpectedEndOfStream, ((1, 21), (1, 21)), ")"));
        }

        [Fact]
        public void ItReportsUnevenBrackets()
        {
            new SuperBasicCompilation(@"
x = a[1").VerifyDiagnostics(
                // x = a[1
                //       ^
                // I was expecting to see ']' after this.
                new Diagnostic(DiagnosticCode.UnexpectedEndOfStream, ((1, 6), (1, 6)), "]"));
        }

        [Fact]
        public void ItReportsArgumentsWithoutCommasInInvocation()
        {
            new SuperBasicCompilation(@"
x = Math.Power(1 4)").VerifyDiagnostics(
                // x = Math.Power(1 4)
                //                  ^
                // I didn't expect to see 'number' here. I was expecting ',' instead.
                new Diagnostic(DiagnosticCode.UnexpectedTokenFound, ((1, 17), (1, 17)), "number", ","));
        }

        [Fact]
        public void ItReportsCommasWithoutArgumentsInInvocation()
        {
            new SuperBasicCompilation(@"
x = Math.Sin(, )").VerifyDiagnostics(
                // x = Math.Sin(, )
                //              ^
                // I didn't expect to see ',' here. I was expecting 'identifier' instead.
                new Diagnostic(DiagnosticCode.UnexpectedTokenFound, ((1, 13), (1, 13)), ",", "identifier"));
        }
    }
}
