// <copyright file="BindingTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Tests.Compiler
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Diagnostics;
    using Xunit;

    // TODO: how to set timeout for all tests globally? they currently run indefinetely.
    public sealed class BindingTests
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

        [Fact]
        public void ItReportsOnlyOneErrorOnExpressionsThatHaveMultipleErrors()
        {
            new SuperBasicCompilation(@"
TextWindow.WriteLine() = 5").VerifyDiagnostics(
                // TextWindow.WriteLine() = 5
                // ^^^^^^^^^^^^^^^^^^^^^^
                // You are passing '0' arguments to this method, while it expects '1' arguments.
                new Diagnostic(DiagnosticCode.UnexpectedArgumentsCount, ((1, 0), (1, 21)), "0", "1"));

            new SuperBasicCompilation(@"
TextWindow.WriteLine(4) = 5").VerifyDiagnostics(
                // TextWindow.WriteLine(4) = 5
                // ^^^^^^^^^^^^^^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 0), (1, 22))));
        }

        [Fact]
        public void ItReportsStandAloneVariables()
        {
            new SuperBasicCompilation(@"
x").VerifyDiagnostics(
                // x
                // ^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 0))));
        }

        [Fact]
        public void ItReportsStandAloneExpressions()
        {
            new SuperBasicCompilation(@"
x and y").VerifyDiagnostics(
                // x and y
                // ^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 6))));
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
        public void ItReportsMemberAccessToNonLibraries()
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
        public void ItReportsAssigningToBinaryExpression()
        {
            new SuperBasicCompilation(@"
x and y = 5").VerifyDiagnostics(
                // x and y = 5
                // ^^^^^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 10))));
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

        [Fact]
        public void ItReportsDeprecatedMethods()
        {
            new SuperBasicCompilation(@"
x = Program.GetArgument(0)").VerifyDiagnostics(
                // x = Program.GetArgument(0)
                //     ^^^^^^^^^^^^^^^^^^^
                // The library member 'Program.GetArgument' was available in older versions only, and has not been made available in this version yet.
                new Diagnostic(DiagnosticCode.LibraryMemberDeprecatedFromOlderVersion, ((1, 4), (1, 22)), "Program", "GetArgument"));
        }

        [Fact]
        public void ItReportsDesktopFunctionsInEditor()
        {
            new SuperBasicCompilation(@"
File.DeleteFile(""a.txt"")", isRunningOnDesktop: true).VerifyDiagnostics();

            new SuperBasicCompilation(@"
File.DeleteFile(""a.txt"")", isRunningOnDesktop: false).VerifyDiagnostics(
                // File.DeleteFile("a.txt")
                // ^^^^^^^^^^^^^^^
                // The library member 'File.DeleteFile' cannot be used in the online editor. Please download the desktop editor to use it.
                new Diagnostic(DiagnosticCode.LibraryMemberNeedsDesktop, ((1, 0), (1, 14)), "File", "DeleteFile"));
        }

        [Fact]
        public async Task ItDoesNotUseGraphicsWindowWhenNotNeeded()
        {
            var engine = await new SuperBasicCompilation("TextWindow.WriteLine(5)").VerifyRuntime().ConfigureAwait(false);
            engine.Analysis.UsesTextWindow.Should().Be(true);
            engine.Analysis.UsesGraphicsWindow.Should().Be(false);
        }

        [Fact]
        public async Task ItUsesGraphicsWindowWhenNeeded()
        {
            var engine = await new SuperBasicCompilation("Controls.ShowControl(something)").VerifyRuntime().ConfigureAwait(false);
            engine.Analysis.UsesTextWindow.Should().Be(false);
            engine.Analysis.UsesGraphicsWindow.Should().Be(true);
        }

        [Fact]
        public async Task ItUsesTextWindowWhenNothingIsNeeded()
        {
            var engine = await new SuperBasicCompilation("x = Math.Sin(5)").VerifyRuntime().ConfigureAwait(false);
            engine.Analysis.UsesTextWindow.Should().Be(true);
            engine.Analysis.UsesGraphicsWindow.Should().Be(false);
        }
    }
}
