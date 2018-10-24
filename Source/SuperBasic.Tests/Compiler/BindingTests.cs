// <copyright file="BindingTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Tests.Compiler
{
    using FluentAssertions;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Diagnostics;
    using Xunit;

    public sealed class BindingTests
    {
        [Fact]
        public void ItReportsInForLoopFromExpression()
        {
            SuperBasicCompilation.CreateTextProgram(@"
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
            SuperBasicCompilation.CreateTextProgram(@"
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
            SuperBasicCompilation.CreateTextProgram(@"
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
            SuperBasicCompilation.CreateTextProgram(@"
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
            SuperBasicCompilation.CreateTextProgram(@"
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
            SuperBasicCompilation.CreateTextProgram(@"
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
            SuperBasicCompilation.CreateTextProgram(@"
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
            SuperBasicCompilation.CreateGraphicsProgram(@"
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
            SuperBasicCompilation.CreateTextProgram(@"
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
            SuperBasicCompilation.CreateTextProgram(@"
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
            SuperBasicCompilation.CreateTextProgram(@"
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
            SuperBasicCompilation.CreateTextProgram(@"
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
            SuperBasicCompilation.CreateTextProgram(@"
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
            SuperBasicCompilation.CreateTextProgram(@"
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
            SuperBasicCompilation.CreateTextProgram(@"
TextWindow.WriteLine() = 5").VerifyDiagnostics(
                // TextWindow.WriteLine() = 5
                // ^^^^^^^^^^^^^^^^^^^^^^
                // You are passing '0' arguments to this method, while it expects '1' arguments.
                new Diagnostic(DiagnosticCode.UnexpectedArgumentsCount, ((1, 0), (1, 21)), "0", "1"));

            SuperBasicCompilation.CreateTextProgram(@"
TextWindow.WriteLine(4) = 5").VerifyDiagnostics(
                // TextWindow.WriteLine(4) = 5
                // ^^^^^^^^^^^^^^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 0), (1, 22))));
        }

        [Fact]
        public void ItReportsStandAloneVariables()
        {
            SuperBasicCompilation.CreateTextProgram(@"
x").VerifyDiagnostics(
                // x
                // ^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 0))));
        }

        [Fact]
        public void ItReportsStandAloneExpressions()
        {
            SuperBasicCompilation.CreateTextProgram(@"
x and y").VerifyDiagnostics(
                // x and y
                // ^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 6))));
        }

        [Fact]
        public void ItReportsStandAloneLibrary()
        {
            SuperBasicCompilation.CreateTextProgram(@"
TextWindow").VerifyDiagnostics(
                // TextWindow
                // ^^^^^^^^^^
                // This expression is not a valid statement.
                new Diagnostic(DiagnosticCode.InvalidExpressionStatement, ((1, 0), (1, 9))));
        }

        [Fact]
        public void ItReportsStandAloneLibraryMethod()
        {
            SuperBasicCompilation.CreateTextProgram(@"
TextWindow.WriteLine").VerifyDiagnostics(
                // TextWindow.WriteLine
                // ^^^^^^^^^^^^^^^^^^^^
                // This expression is not a valid statement.
                new Diagnostic(DiagnosticCode.InvalidExpressionStatement, ((1, 0), (1, 19))));
        }

        [Fact]
        public void ItReportsStandAloneSubModule()
        {
            SuperBasicCompilation.CreateTextProgram(@"
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
            SuperBasicCompilation.CreateTextProgram(@"
x = TextWindow[0]").VerifyDiagnostics(
                // x = TextWindow[0]
                //     ^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 4), (1, 13))));
        }

        [Fact]
        public void ItReportsArrayAccessIntoLibraryMethod()
        {
            SuperBasicCompilation.CreateTextProgram(@"
x = TextWindow.WriteLine[0]").VerifyDiagnostics(
                // x = TextWindow.WriteLine[0]
                //     ^^^^^^^^^^^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 4), (1, 23))));
        }

        [Fact]
        public void ItReportsArrayAccessIntoLibraryProperty()
        {
            SuperBasicCompilation.CreateTextProgram(@"
x = Clock.Time[0]").VerifyDiagnostics(
                // x = Clock.Time[0]
                //     ^^^^^^^^^^
                // This expression is not a valid array.
                new Diagnostic(DiagnosticCode.UnsupportedArrayBaseExpression, ((1, 4), (1, 13))));
        }

        [Fact]
        public void ItReportsArrayAccessIntoLibraryEvent()
        {
            SuperBasicCompilation.CreateGraphicsProgram(@"
x = Controls.ButtonClicked[0]").VerifyDiagnostics(
                // x = Controls.ButtonClicked[0]
                //     ^^^^^^^^^^^^^^^^^^^^^^
                // This expression is not a valid array.
                new Diagnostic(DiagnosticCode.UnsupportedArrayBaseExpression, ((1, 4), (1, 25))));
        }

        [Fact]
        public void ItReportsIndexerExpressionWithoutValue()
        {
            SuperBasicCompilation.CreateTextProgram(@"
x = y[TextWindow]").VerifyDiagnostics(
                // x = y[TextWindow]
                //       ^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 6), (1, 15))));
        }

        [Fact]
        public void ItReportsArgumentExpressionWithoutValue()
        {
            SuperBasicCompilation.CreateTextProgram(@"
TextWindow.WriteLine(TextWindow)").VerifyDiagnostics(
                // TextWindow.WriteLine(TextWindow)
                //                      ^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 21), (1, 30))));
        }

        [Fact]
        public void ItReportsInvalidArgumentCountForMethodInvocations()
        {
            SuperBasicCompilation.CreateTextProgram(@"
TextWindow.WriteLine(1, 2)").VerifyDiagnostics(
                // TextWindow.WriteLine(1, 2)
                // ^^^^^^^^^^^^^^^^^^^^^^^^^^
                // You are passing '2' arguments to this method, while it expects '1' arguments.
                new Diagnostic(DiagnosticCode.UnexpectedArgumentsCount, ((1, 0), (1, 25)), "2", "1"));
        }

        [Fact]
        public void ItReportsInvokingNonMethods()
        {
            SuperBasicCompilation.CreateTextProgram(@"
TextWindow.WriteLine(1)()").VerifyDiagnostics(
                // TextWindow.WriteLine(1)()
                // ^^^^^^^^^^^^^^^^^^^^^^^^^
                // This expression is not a valid submodule or method to be called.
                new Diagnostic(DiagnosticCode.UnsupportedInvocationBaseExpression, ((1, 0), (1, 24))));
        }

        [Fact]
        public void ItReportsMemberAccessToNonLibraries()
        {
            SuperBasicCompilation.CreateTextProgram(@"
x = y.z").VerifyDiagnostics(
                // x = y.z
                //     ^
                // You can only use dot access with a library. Did you mean to use an existing library instead?
                new Diagnostic(DiagnosticCode.UnsupportedDotBaseExpression, ((1, 4), (1, 4))));
        }

        [Fact]
        public void ItReportsNonExistentLibraryMembers()
        {
            SuperBasicCompilation.CreateTextProgram(@"
x = TextWindow.Anything").VerifyDiagnostics(
                // x = TextWindow.Anything
                //     ^^^^^^^^^^^^^^^^^^^
                // The library 'TextWindow' has no member named 'Anything'.
                new Diagnostic(DiagnosticCode.LibraryMemberNotFound, ((1, 4), (1, 22)), "TextWindow", "Anything"));
        }

        [Fact]
        public void ItReportsAssigningToLibraryMethodInvocation()
        {
            SuperBasicCompilation.CreateTextProgram(@"
TextWindow.WriteLine(0) = 5").VerifyDiagnostics(
                // TextWindow.WriteLine(0) = 5
                // ^^^^^^^^^^^^^^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 0), (1, 22))));
        }

        [Fact]
        public void ItReportsAssigningToPropertyWithoutASetter()
        {
            SuperBasicCompilation.CreateTextProgram(@"
Clock.Time = 5").VerifyDiagnostics(
                // Clock.Time = 5
                // ^^^^^^^^^^
                // Property 'Clock.Time' cannot be assigned to. It is ready only.
                new Diagnostic(DiagnosticCode.PropertyHasNoSetter, ((1, 0), (1, 9)), "Clock", "Time"));
        }

        [Fact]
        public void ItReportsAssigningToBinaryExpression()
        {
            SuperBasicCompilation.CreateTextProgram(@"
x and y = 5").VerifyDiagnostics(
                // x and y = 5
                // ^^^^^^^^^^^
                // This expression returns a result. Did you mean to assign it to a variable?
                new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, ((1, 0), (1, 10))));
        }

        [Fact]
        public void ItReportsAssigningToLibraryType()
        {
            SuperBasicCompilation.CreateTextProgram(@"
TextWindow = 5").VerifyDiagnostics(
                // TextWindow = 5
                // ^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 0), (1, 9))));
        }

        [Fact]
        public void ItReportsAssigningToLibraryMethod()
        {
            SuperBasicCompilation.CreateTextProgram(@"
TextWindow.WriteLine = 5").VerifyDiagnostics(
                // TextWindow.WriteLine = 5
                // ^^^^^^^^^^^^^^^^^^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((1, 0), (1, 19))));
        }

        [Fact]
        public void ItReportsAssigningToSubModule()
        {
            SuperBasicCompilation.CreateTextProgram(@"
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
            SuperBasicCompilation.CreateTextProgram(@"
Sub x
EndSub
x() = 5").VerifyDiagnostics(
                // x() = 5
                // ^^^
                // This expression must have a value to be used here.
                new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, ((3, 0), (3, 2))));
        }

        [Fact]
        public void ItReportsConflictingLibrariesUsed()
        {
            SuperBasicCompilation.CreateTextProgram(@"
TextWindow.WriteLine(5)
Turtle.Show()
").VerifyDiagnostics(
                // Turtle.Show()
                // ^^^^^^
                // You cannot use the library 'Turtle' here since this is a 'Text' program. Did you want to change it?
                new Diagnostic(DiagnosticCode.LibraryAndCompilationKindMismatch, ((2, 0), (2, 5)), "Turtle", "Text"));
        }

        [Fact]
        public void ItReportsDeprecatedMethods()
        {
            SuperBasicCompilation.CreateTextProgram(@"
x = Program.GetArgument(0)").VerifyDiagnostics(
                // x = Program.GetArgument(0)
                //     ^^^^^^^^^^^^^^^^^^^
                // The library member 'Program.GetArgument' was available in older versions only, and has not been made available in this version yet.
                new Diagnostic(DiagnosticCode.LibraryMemberDeprecatedFromOlderVersion, ((1, 4), (1, 22)), "Program", "GetArgument"));
        }

        [Fact]
        public void ItReportsDesktopFunctionsInEditor()
        {
            SuperBasicCompilation.CreateTextProgram(@"
File.DeleteFile(""a.txt"")", isRunningOnDesktop: true).VerifyDiagnostics();

            SuperBasicCompilation.CreateTextProgram(@"
File.DeleteFile(""a.txt"")", isRunningOnDesktop: false).VerifyDiagnostics(
                // File.DeleteFile("a.txt")
                // ^^^^^^^^^^^^^^^
                // The library member 'File.DeleteFile' cannot be used in the online editor. Please download the desktop editor to use it.
                new Diagnostic(DiagnosticCode.LibraryMemberNeedsDesktop, ((1, 0), (1, 14)), "File", "DeleteFile"));
        }
    }
}
