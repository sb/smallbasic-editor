// <copyright file="ExpressionsTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Tests.Parsing
{
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Diagnostics;
    using Xunit;

    public sealed class ExpressionsTests
    {
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
