// <copyright file="ScanningTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Tests
{
    using SuperBasic.Compiler.Diagnostics;
    using Xunit;

    public sealed class ScanningTests
    {
        [Fact]
        public void ItReportsUnterminatedStringLiterals()
        {
            new SuperBasicCompilation(@"
x = ""name").VerifyDiagnostics(
                // x = "name
                //     ^^^^^
                // This string is missing its right double quotes.
                new Diagnostic(DiagnosticCode.UnterminatedStringLiteral, ((1, 4), (1, 8))));
        }

        [Fact]
        public void ItReportsMultipleUnterminatedStringLiterals()
        {
            new SuperBasicCompilation(@"
x = ""name
y = ""another").VerifyDiagnostics(
                // x = "name
                //     ^^^^^
                // This string is missing its right double quotes.
                new Diagnostic(DiagnosticCode.UnterminatedStringLiteral, ((1, 4), (1, 8))),
                // y = "another
                //     ^^^^^^^^
                // This string is missing its right double quotes.
                new Diagnostic(DiagnosticCode.UnterminatedStringLiteral, ((2, 4), (2, 11))));
        }

        [Fact]
        public void ItReportsUnrecognizedCharacters()
        {
            new SuperBasicCompilation(@"
$").VerifyDiagnostics(
                // $
                // ^
                // I don't understand this character '$'.
                new Diagnostic(DiagnosticCode.UnrecognizedCharacter, ((1, 0), (1, 0)), "$"));
        }

        [Fact]
        public void ItReportsMultipleUnrecognizedCharacters()
        {
            new SuperBasicCompilation(@"
x = ____^
ok = ""value $ value""
not_ok = 6 $
' $ still ok").VerifyDiagnostics(
                // x = ____^
                //         ^
                // I don't understand this character '^'.
                new Diagnostic(DiagnosticCode.UnrecognizedCharacter, ((1, 8), (1, 8)), "^"),
                // not_ok = 6 $
                //            ^
                // I don't understand this character '$'.
                new Diagnostic(DiagnosticCode.UnrecognizedCharacter, ((3, 11), (3, 11)), "$"));
        }
    }
}
