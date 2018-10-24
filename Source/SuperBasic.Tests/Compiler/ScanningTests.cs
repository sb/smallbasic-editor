// <copyright file="ScanningTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Tests.Compiler
{
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Diagnostics;
    using Xunit;

    public sealed class ScanningTests
    {
        [Fact]
        public void ItReportsUnterminatedStringLiterals()
        {
            SuperBasicCompilation.CreateTextProgram(@"
x = ""name").VerifyDiagnostics(
                // x = "name
                //     ^^^^^
                // This string is missing its right double quotes.
                new Diagnostic(DiagnosticCode.UnterminatedStringLiteral, ((1, 4), (1, 8))));
        }

        [Fact]
        public void ItReportsMultipleUnterminatedStringLiterals()
        {
            SuperBasicCompilation.CreateTextProgram(@"
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
        public void ItReportsUnrecognizedCharactersOnStartOfLine()
        {
            SuperBasicCompilation.CreateTextProgram(@"
$").VerifyDiagnostics(
                // $
                // ^
                // I don't understand this character '$'.
                new Diagnostic(DiagnosticCode.UnrecognizedCharacter, ((1, 0), (1, 0)), "$"));
        }

        [Fact]
        public void ItReportsMultipleUnrecognizedCharacters()
        {
            SuperBasicCompilation.CreateTextProgram(@"
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
