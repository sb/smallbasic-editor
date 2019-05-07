// <copyright file="StatementsTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Tests.Runtime
{
    using System.Threading.Tasks;
    using SmallBasic.Compiler;
    using Xunit;

    public sealed class StatementsTests : IClassFixture<CultureFixture>
    {
        // https://github.com/sb/smallbasic-editor/issues/71
        [Fact]
        public Task ItReportsAssigningToSubmodule()
        {
            return new SmallBasicCompilation(
            @"
Sub x
EndSub
x = 5
TextWindow.WriteLine(x)").VerifyLoggingRuntime(@"
TextWindow.WriteLine(data: '5')
");
        }

        // https://github.com/sb/smallbasic-editor/issues/71
        [Fact]
        public Task SubroutineAndLabelSameName()
        {
            return new SmallBasicCompilation(
            @"for i = 1 to 5
If (i > 4) Then
GoTo test
EndIf       
test()
EndFor

test:

Sub test
TextWindow.WriteLine(""Hello World!"")
EndSub").VerifyLoggingRuntime(@"
TextWindow.WriteLine(data: 'Hello World!')
TextWindow.WriteLine(data: 'Hello World!')
TextWindow.WriteLine(data: 'Hello World!')
TextWindow.WriteLine(data: 'Hello World!')
");
        }

        [Fact]
        public Task SubroutineAndForLoopVariableSameName()
        {
            return new SmallBasicCompilation(
            @"test()
for test = 1 to 3
TextWindow.WriteLine(test)
EndFor

test()
Sub test
TextWindow.WriteLine(""Hello World!"")
EndSub
test()
 ").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: 'Hello World!')
TextWindow.WriteLine(data: '1')
TextWindow.WriteLine(data: '2')
TextWindow.WriteLine(data: '3')
TextWindow.WriteLine(data: 'Hello World!')
TextWindow.WriteLine(data: 'Hello World!')
");
        }

        [Fact]
        public Task ItEvaluatesSingleIfTrueExpression()
        {
            return new SmallBasicCompilation(@"
If ""True"" Then
    TextWindow.WriteLine(""first"")
EndIf").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: 'first')
");
        }

        [Fact]
        public Task ItEvaluatesSingleIfFalseExpression()
        {
            return new SmallBasicCompilation(@"
If ""False"" Then
    TextWindow.WriteLine(""first"")
EndIf").VerifyLoggingRuntime(expectedLog: @"
");
        }

        [Fact]
        public Task ItEvaluatesIfElseTrueExpression()
        {
            return new SmallBasicCompilation(@"
If ""True"" Then
    TextWindow.WriteLine(""first"")
Else
    TextWindow.WriteLine(""second"")
EndIf").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: 'first')
");
        }

        [Fact]
        public Task ItEvaluatesIfElseFalseExpression()
        {
            return new SmallBasicCompilation(@"
If ""False"" Then
    TextWindow.WriteLine(""first"")
Else
    TextWindow.WriteLine(""second"")
EndIf").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: 'second')
");
        }

        [Fact]
        public Task ItEvaluatesDifferentElseIfBranches()
        {
            return new SmallBasicCompilation(@"
For x = 1 To 4
    If x = 1 Then
        TextWindow.WriteLine(""first"")
    ElseIf x = 2 Then
        TextWindow.WriteLine(""second"")
    ElseIf x = 3 Then
        TextWindow.WriteLine(""third"")
    Else
        TextWindow.WriteLine(""fourth"")
    EndIf
EndFor").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: 'first')
TextWindow.WriteLine(data: 'second')
TextWindow.WriteLine(data: 'third')
TextWindow.WriteLine(data: 'fourth')
");
        }

        [Fact]
        public Task ItEvalutesForLoopWithNoStep()
        {
            return new SmallBasicCompilation(@"
For x = 1 To 4
    TextWindow.WriteLine(x)
EndFor").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: '1')
TextWindow.WriteLine(data: '2')
TextWindow.WriteLine(data: '3')
TextWindow.WriteLine(data: '4')
");
        }

        [Fact]
        public Task ItEvalutesForLoopWithStepOne()
        {
            return new SmallBasicCompilation(@"
For x = 1 To 4 Step 1
    TextWindow.WriteLine(x)
EndFor").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: '1')
TextWindow.WriteLine(data: '2')
TextWindow.WriteLine(data: '3')
TextWindow.WriteLine(data: '4')
");
        }

        [Fact]
        public Task ItEvalutesForLoopWithStepTwo()
        {
            return new SmallBasicCompilation(@"
For x = 1 To 8 Step 2
    TextWindow.WriteLine(x)
EndFor").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: '1')
TextWindow.WriteLine(data: '3')
TextWindow.WriteLine(data: '5')
TextWindow.WriteLine(data: '7')
");
        }

        [Fact]
        public Task ItEvalutesForLoopWithInverseStepOne()
        {
            return new SmallBasicCompilation(@"
For x = 4 To 1 Step -1
    TextWindow.WriteLine(x)
EndFor").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: '4')
TextWindow.WriteLine(data: '3')
TextWindow.WriteLine(data: '2')
TextWindow.WriteLine(data: '1')
");
        }

        [Fact]
        public Task ItEvalutesForLoopWithInverseStepTwo()
        {
            return new SmallBasicCompilation(@"
For x = 8 To 1 Step -2
    TextWindow.WriteLine(x)
EndFor").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: '8')
TextWindow.WriteLine(data: '6')
TextWindow.WriteLine(data: '4')
TextWindow.WriteLine(data: '2')
");
        }

        [Fact]
        public Task ItEvalutesForLoopWithEqualRangeAndNoStep()
        {
            return new SmallBasicCompilation(@"
For x = 1 To 1
    TextWindow.WriteLine(x)
EndFor").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: '1')
");
        }

        [Fact]
        public Task ItEvalutesForLoopWithEqualRangeAndPositiveStep()
        {
            return new SmallBasicCompilation(@"
For x = 1 To 1 Step 1
    TextWindow.WriteLine(x)
EndFor").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: '1')
");
        }

        [Fact]
        public Task ItEvalutesForLoopWithEqualRangeAndNegativeStep()
        {
            return new SmallBasicCompilation(@"
For x = 1 To 1 Step -1
    TextWindow.WriteLine(x)
EndFor").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: '1')
");
        }

        [Fact]
        public Task ItEvalutesGoToLabels()
        {
            return new SmallBasicCompilation(@"
GoTo two
one:
TextWindow.WriteLine(1)
GoTo three
two:
TextWindow.WriteLine(2)
GoTo one
three:
TextWindow.WriteLine(3)").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: '2')
TextWindow.WriteLine(data: '1')
TextWindow.WriteLine(data: '3')
");
        }

        [Fact]
        public Task ItEvalutesWhileLoop()
        {
            return new SmallBasicCompilation(@"
x = 5
result = 1
While x > 0
    result = result + result
    x = x - 1
EndWhile
TextWindow.WriteLine(result)
").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: '32')
");
        }

        [Fact]
        public Task ItDoesNotEvaluteWhileLoopWithNegativeCondition()
        {
            return new SmallBasicCompilation(@"
result = 1
While ""False""
    result = 2
EndWhile
TextWindow.WriteLine(result)
").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: '1')
");
        }

        [Fact]
        public Task ItExitsFromAnInfitinteWhileLoop()
        {
            return new SmallBasicCompilation(@"
result = 1
While ""True""
    TextWindow.WriteLine(result)
    result = result + 1
    If result > 4 Then
        Program.End()
    EndIf
EndWhile
").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: '1')
TextWindow.WriteLine(data: '2')
TextWindow.WriteLine(data: '3')
TextWindow.WriteLine(data: '4')
");
        }
    }
}
