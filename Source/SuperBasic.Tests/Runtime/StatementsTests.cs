// <copyright file="StatementsTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Tests.Runtime
{
    using System.Threading.Tasks;
    using SuperBasic.Compiler;
    using Xunit;

    public sealed class StatementsTests
    {
        [Fact]
        public async Task ItEvaluatesSingleIfTrueExpression()
        {
            await new SuperBasicCompilation(@"
If ""True"" Then
    TextWindow.WriteLine(""first"")
EndIf").VerifyRuntime(expectedLog: @"
TextWindow.WriteLine(data: 'first')
").ConfigureAwait(false);
        }

        [Fact]
        public async Task ItEvaluatesSingleIfFalseExpression()
        {
            await new SuperBasicCompilation(@"
If ""False"" Then
    TextWindow.WriteLine(""first"")
EndIf").VerifyRuntime(expectedLog: @"
").ConfigureAwait(false);
        }

        [Fact]
        public async Task ItEvaluatesIfElseTrueExpression()
        {
            await new SuperBasicCompilation(@"
If ""True"" Then
    TextWindow.WriteLine(""first"")
Else
    TextWindow.WriteLine(""second"")
EndIf").VerifyRuntime(expectedLog: @"
TextWindow.WriteLine(data: 'first')
").ConfigureAwait(false);
        }

        [Fact]
        public async Task ItEvaluatesIfElseFalseExpression()
        {
            await new SuperBasicCompilation(@"
If ""False"" Then
    TextWindow.WriteLine(""first"")
Else
    TextWindow.WriteLine(""second"")
EndIf").VerifyRuntime(expectedLog: @"
TextWindow.WriteLine(data: 'second')
").ConfigureAwait(false);
        }

        [Fact]
        public async Task ItEvaluatesDifferentElseIfBranches()
        {
            await new SuperBasicCompilation(@"
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
EndFor").VerifyRuntime(expectedLog: @"
TextWindow.WriteLine(data: 'first')
TextWindow.WriteLine(data: 'second')
TextWindow.WriteLine(data: 'third')
TextWindow.WriteLine(data: 'fourth')
").ConfigureAwait(false);
        }

        [Fact]
        public async Task ItEvalutesForLoopWithNoStep()
        {
            await new SuperBasicCompilation(@"
For x = 1 To 4
    TextWindow.WriteLine(x)
EndFor").VerifyRuntime(expectedLog: @"
TextWindow.WriteLine(data: '1')
TextWindow.WriteLine(data: '2')
TextWindow.WriteLine(data: '3')
TextWindow.WriteLine(data: '4')
").ConfigureAwait(false);
        }

        [Fact]
        public async Task ItEvalutesForLoopWithStepOne()
        {
            await new SuperBasicCompilation(@"
For x = 1 To 4 Step 1
    TextWindow.WriteLine(x)
EndFor").VerifyRuntime(expectedLog: @"
TextWindow.WriteLine(data: '1')
TextWindow.WriteLine(data: '2')
TextWindow.WriteLine(data: '3')
TextWindow.WriteLine(data: '4')
").ConfigureAwait(false);
        }

        [Fact]
        public async Task ItEvalutesForLoopWithStepTwo()
        {
            await new SuperBasicCompilation(@"
For x = 1 To 8 Step 2
    TextWindow.WriteLine(x)
EndFor").VerifyRuntime(expectedLog: @"
TextWindow.WriteLine(data: '1')
TextWindow.WriteLine(data: '3')
TextWindow.WriteLine(data: '5')
TextWindow.WriteLine(data: '7')
").ConfigureAwait(false);
        }

        [Fact]
        public async Task ItEvalutesForLoopWithInverseStepOne()
        {
            await new SuperBasicCompilation(@"
For x = 4 To 1 Step -1
    TextWindow.WriteLine(x)
EndFor").VerifyRuntime(expectedLog: @"
TextWindow.WriteLine(data: '4')
TextWindow.WriteLine(data: '3')
TextWindow.WriteLine(data: '2')
TextWindow.WriteLine(data: '1')
").ConfigureAwait(false);
        }

        [Fact]
        public async Task ItEvalutesForLoopWithInverseStepTwo()
        {
            await new SuperBasicCompilation(@"
For x = 8 To 1 Step -2
    TextWindow.WriteLine(x)
EndFor").VerifyRuntime(expectedLog: @"
TextWindow.WriteLine(data: '8')
TextWindow.WriteLine(data: '6')
TextWindow.WriteLine(data: '4')
TextWindow.WriteLine(data: '2')
").ConfigureAwait(false);
        }

        [Fact]
        public async Task ItEvalutesForLoopWithEqualRangeAndNoStep()
        {
            await new SuperBasicCompilation(@"
For x = 1 To 1
    TextWindow.WriteLine(x)
EndFor").VerifyRuntime(expectedLog: @"
TextWindow.WriteLine(data: '1')
").ConfigureAwait(false);
        }

        [Fact]
        public async Task ItEvalutesForLoopWithEqualRangeAndPositiveStep()
        {
            await new SuperBasicCompilation(@"
For x = 1 To 1 Step 1
    TextWindow.WriteLine(x)
EndFor").VerifyRuntime(expectedLog: @"
TextWindow.WriteLine(data: '1')
").ConfigureAwait(false);
        }

        [Fact]
        public async Task ItEvalutesForLoopWithEqualRangeAndNegativeStep()
        {
            await new SuperBasicCompilation(@"
For x = 1 To 1 Step -1
    TextWindow.WriteLine(x)
EndFor").VerifyRuntime(expectedLog: @"
TextWindow.WriteLine(data: '1')
").ConfigureAwait(false);
        }

        [Fact]
        public async Task ItEvalutesGoToLabels()
        {
            await new SuperBasicCompilation(@"
GoTo two
one:
TextWindow.WriteLine(1)
GoTo three
two:
TextWindow.WriteLine(2)
GoTo one
three:
TextWindow.WriteLine(3)").VerifyRuntime(expectedLog: @"
TextWindow.WriteLine(data: '2')
TextWindow.WriteLine(data: '1')
TextWindow.WriteLine(data: '3')
").ConfigureAwait(false);
        }

        [Fact]
        public async Task ItEvalutesWhileLoop()
        {
            await new SuperBasicCompilation(@"
x = 5
result = 1
While x > 0
    result = result + result
    x = x - 1
EndWhile
TextWindow.WriteLine(result)
").VerifyRuntime(expectedLog: @"
TextWindow.WriteLine(data: '32')
").ConfigureAwait(false);
        }

        [Fact]
        public async Task ItDoesNotEvaluteWhileLoopWithNegativeCondition()
        {
            await new SuperBasicCompilation(@"
result = 1
While ""False""
    result = 2
EndWhile
TextWindow.WriteLine(result)
").VerifyRuntime(expectedLog: @"
TextWindow.WriteLine(data: '1')
").ConfigureAwait(false);
        }

        [Fact]
        public async Task ItExitsFromAnInfitinteWhileLoop()
        {
            await new SuperBasicCompilation(@"
result = 1
While ""True""
    TextWindow.WriteLine(result)
    result = result + 1
    If result > 4 Then
        Program.End()
    EndIf
EndWhile
").VerifyRuntime(expectedLog: @"
TextWindow.WriteLine(data: '1')
TextWindow.WriteLine(data: '2')
TextWindow.WriteLine(data: '3')
TextWindow.WriteLine(data: '4')
").ConfigureAwait(false);
        }
    }
}
