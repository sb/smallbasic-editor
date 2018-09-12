// <copyright file="ExpressionsTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Tests.Binding
{
    using FluentAssertions;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Diagnostics;
    using Xunit;

    public sealed class ExpressionsTests
    {
        [Fact]
        public void ItReportsOnlyOneErrorOnExpressionsThatHaveMultipleErrors()
        {
            new SuperBasicCompilation(@"
TextWindow.WriteLine() = 5").VerifyDiagnostics(
                // TextWindow.WriteLine() = 5
                // ^^^^^^^^^^^^^^^^^^^^^^
                // You are passing '0' arguments to this method, while it expects '1' arguments.
                new Diagnostic(DiagnosticCode.UnexpectedArgumentsCount, ((1, 0), (1, 21)), "0", "1"));
        }

        [Fact]
        public void ItReportsStatementsStartingWithNegation()
        {
            new SuperBasicCompilation(@"
-x").VerifyDiagnostics(
                // -x
                // ^
                // I didn't expect to see '-' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((1, 0), (1, 0)), "-"));
        }

        [Fact]
        public void ItReportsStatementsStartingWithParenthesis()
        {
            new SuperBasicCompilation(@"
(x + y) = 5").VerifyDiagnostics(
                // (x + y) = 5
                // ^
                // I didn't expect to see '(' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((1, 0), (1, 0)), "("));
        }

        [Fact]
        public void ItReportsStatementsStartingWithStringLiteral()
        {
            new SuperBasicCompilation(@"
""text"" = 5").VerifyDiagnostics(
                // "text" = 5
                // ^^^^^^
                // I didn't expect to see 'string' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((1, 0), (1, 5)), "string"));
        }

        [Fact]
        public void ItReportsStatementsStartingWithNumberLiteral()
        {
            new SuperBasicCompilation(@"
1 = 5").VerifyDiagnostics(
                // 1 = 5
                // ^
                // I didn't expect to see 'number' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((1, 0), (1, 0)), "number"));
        }


        [Fact]
        public void ItReportsStandAloneVariable()
        {
            new SuperBasicCompilation(@"
x").VerifyDiagnostics(
                // x
                // ^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 0))));
        }

        [Fact]
        public void ItReportsStandAloneArrayAccess()
        {
            new SuperBasicCompilation(@"
x[0]").VerifyDiagnostics(
                // x[0]
                // ^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 3))));
        }

        [Fact]
        public void ItReportsStandAloneLibraryProperty()
        {
            new SuperBasicCompilation(@"
Clock.Time").VerifyDiagnostics(
                // Clock.Time
                // ^^^^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 9))));
        }

        [Fact]
        public void ItReportsStandAloneParenthesis()
        {
            new SuperBasicCompilation(@"
(x)").VerifyDiagnostics(
                // (x)
                // ^
                // I didn't expect to see '(' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((1, 0), (1, 0)), "("));
        }

        [Fact]
        public void ItReportsStandAloneAnd()
        {
            new SuperBasicCompilation(@"
x and y").VerifyDiagnostics(
                // x and y
                // ^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 6))));
        }

        [Fact]
        public void ItReportsStandAloneOr()
        {
            new SuperBasicCompilation(@"
x or y").VerifyDiagnostics(
                // x or y
                // ^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 5))));
        }

        [Fact]
        public void ItReportsStandAloneNotEqual()
        {
            new SuperBasicCompilation(@"
x <> y").VerifyDiagnostics(
                // x <> y
                // ^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 5))));
        }

        [Fact]
        public void ItReportsStandAloneAddition()
        {
            new SuperBasicCompilation(@"
x + y").VerifyDiagnostics(
                // x + y
                // ^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 4))));
        }

        [Fact]
        public void ItReportsStandAloneSubtraction()
        {
            new SuperBasicCompilation(@"
x - y").VerifyDiagnostics(
                // x - y
                // ^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 4))));
        }

        [Fact]
        public void ItReportsStandAloneMultiplication()
        {
            new SuperBasicCompilation(@"
x * y").VerifyDiagnostics(
                // x * y
                // ^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 4))));
        }

        [Fact]
        public void ItReportsStandAloneDivision()
        {
            new SuperBasicCompilation(@"
x / y").VerifyDiagnostics(
                // x / y
                // ^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 4))));
        }

        [Fact]
        public void ItReportsStandAloneLessThan()
        {
            new SuperBasicCompilation(@"
x < y").VerifyDiagnostics(
                // x < y
                // ^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 4))));
        }

        [Fact]
        public void ItReportsStandAloneGreaterThan()
        {
            new SuperBasicCompilation(@"
x > y").VerifyDiagnostics(
                // x > y
                // ^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 4))));
        }

        [Fact]
        public void ItReportsStandAloneLessThanOrEqual()
        {
            new SuperBasicCompilation(@"
x <= y").VerifyDiagnostics(
                // x <= y
                // ^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 5))));
        }

        [Fact]
        public void ItReportsStandAloneGreaterThanOrEqual()
        {
            new SuperBasicCompilation(@"
x >= y").VerifyDiagnostics(
                // x >= y
                // ^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 5))));
        }

        [Fact]
        public void ItReportsStandAloneLibrary()
        {
            new SuperBasicCompilation(@"
TextWindow").VerifyDiagnostics(
                // TextWindow
                // ^^^^^^^^^^
                // This expression is not a valid statement.
                new Diagnostic(DiagnosticCode.InvalidExpressionStatement, ((1, 0), (1, 9))));
        }

        [Fact]
        public void ItReportsStandAloneLibraryMethod()
        {
            new SuperBasicCompilation(@"
TextWindow.WriteLine").VerifyDiagnostics(
                // TextWindow.WriteLine
                // ^^^^^^^^^^^^^^^^^^^^
                // This expression is not a valid statement.
                new Diagnostic(DiagnosticCode.InvalidExpressionStatement, ((1, 0), (1, 19))));
        }

        [Fact]
        public void ItReportsStandAloneStringLiteral()
        {
            new SuperBasicCompilation(@"
""text""").VerifyDiagnostics(
                // "text"
                // ^^^^^^
                // I didn't expect to see 'string' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((1, 0), (1, 5)), "string"));
        }

        [Fact]
        public void ItReportsStandAloneNumberLiteral()
        {
            new SuperBasicCompilation(@"
5").VerifyDiagnostics(
                // 5
                // ^
                // I didn't expect to see 'number' here. I was expecting the start of a new statement.
                new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, ((1, 0), (1, 0)), "number"));
        }

        [Fact]
        public void ItReportsStandAloneSubModule()
        {
            new SuperBasicCompilation(@"
Sub x
EndSub
x").VerifyDiagnostics(
                // x
                // ^
                // This expression is not a valid statement.
                new Diagnostic(DiagnosticCode.InvalidExpressionStatement, ((3, 0), (3, 0))));
        }

        [Fact]
        public void ItReportsArrayAccessIntoLibraryType()
        {
            new SuperBasicCompilation(@"
x = TextWindow[0]").VerifyDiagnostics(
                // x = TextWindow[0]
                //     ^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 4), (1, 13))));
        }

        [Fact]
        public void ItReportsArrayAccessIntoLibraryMethod()
        {
            new SuperBasicCompilation(@"
x = TextWindow.WriteLine[0]").VerifyDiagnostics(
                // x = TextWindow.WriteLine[0]
                //     ^^^^^^^^^^^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 4), (1, 23))));
        }

        [Fact]
        public void ItReportsArrayAccessIntoLibraryProperty()
        {
            new SuperBasicCompilation(@"
x = Clock.Time[0]").VerifyDiagnostics(
                // x = Clock.Time[0]
                //     ^^^^^^^^^^
                // This expression is not a valid array.
                new Diagnostic(DiagnosticCode.UnsupportedArrayBaseExpression, ((1, 4), (1, 13))));
        }

        [Fact]
        public void ItReportsArrayAccessIntoLibraryEvent()
        {
            new SuperBasicCompilation(@"
x = Controls.ButtonClicked[0]").VerifyDiagnostics(
                // x = Controls.ButtonClicked[0]
                //     ^^^^^^^^^^^^^^^^^^^^^^
                // This expression is not a valid array.
                new Diagnostic(DiagnosticCode.UnsupportedArrayBaseExpression, ((1, 4), (1, 25))));
        }

        [Fact]
        public void ItReportsIndexerExpressionWithoutValue()
        {
            new SuperBasicCompilation(@"
x = y[TextWindow]").VerifyDiagnostics(
                // x = y[TextWindow]
                //       ^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 6), (1, 15))));
        }

        [Fact]
        public void ItReportsArgumentExpressionWithoutValue()
        {
            new SuperBasicCompilation(@"
TextWindow.WriteLine(TextWindow)").VerifyDiagnostics(
                // TextWindow.WriteLine(TextWindow)
                //                      ^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 21), (1, 30))));
        }

        [Fact]
        public void ItReportsInvalidArgumentCountForMethodInvocations()
        {
            new SuperBasicCompilation(@"
TextWindow.WriteLine(1, 2)").VerifyDiagnostics(
                // TextWindow.WriteLine(1, 2)
                // ^^^^^^^^^^^^^^^^^^^^^^^^^^
                // You are passing '2' arguments to this method, while it expects '1' arguments.
                new Diagnostic(DiagnosticCode.UnexpectedArgumentsCount, ((1, 0), (1, 25)), "2", "1"));
        }

        [Fact]
        public void ItReportsInvokingNonMethods()
        {
            new SuperBasicCompilation(@"
TextWindow.WriteLine(1)()").VerifyDiagnostics(
                // TextWindow.WriteLine(1)()
                // ^^^^^^^^^^^^^^^^^^^^^^^^^
                // This expression is not a valid submodule or method to be called.
                new Diagnostic(DiagnosticCode.UnsupportedInvocationBaseExpression, ((1, 0), (1, 24))));
        }

        [Fact]
        public void ItReportsMemberAccessToNonTypes()
        {
            new SuperBasicCompilation(@"
x = y.z").VerifyDiagnostics(
                // x = y.z
                //     ^
                // You can only use dot access with a library. Did you mean to use an existing library instead?
                new Diagnostic(DiagnosticCode.UnsupportedDotBaseExpression, ((1, 4), (1, 4))));
        }

        [Fact]
        public void ItReportsNonExistentLibraryMembers()
        {
            new SuperBasicCompilation(@"
x = TextWindow.Anything").VerifyDiagnostics(
                // x = TextWindow.Anything
                //     ^^^^^^^^^^^^^^^^^^^
                // The library 'TextWindow' has no member named 'Anything'.
                new Diagnostic(DiagnosticCode.LibraryMemberNotFound, ((1, 4), (1, 22)), "TextWindow", "Anything"));
        }
    }
}
