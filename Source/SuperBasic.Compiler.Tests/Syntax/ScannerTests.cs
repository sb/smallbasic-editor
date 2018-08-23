// <copyright file="ScannerTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Tests.Syntax
{
    using SuperBasic.Compiler.Diagnostics;
    using Xunit;

    public sealed class ScannerTests
    {
        [Fact]
        public void ItReportsUnterminatedStringLiterals()
        {
            new SuperBasicCompilation(@"
x = ""name").VerifyDiagnostics(
                // x = "name
                //     ^^^^^
                new Diagnostic(DiagnosticCode.UnterminatedStringLiteral, ((1, 4), (1, 9))));
        }

        [Fact]
        public void ItReportsMultipleUnterminatedStringLiterals()
        {
            new SuperBasicCompilation(@"
x = ""name
y = ""another").VerifyDiagnostics(
                // x = "name
                //     ^^^^^
                new Diagnostic(DiagnosticCode.UnterminatedStringLiteral, ((1, 4), (1, 9))),
                // y = "another
                //     ^^^^^^^^
                new Diagnostic(DiagnosticCode.UnterminatedStringLiteral, ((2, 4), (2, 12))));
        }

        [Fact]
        public void ItReportsUnrecognizedCharacters()
        {
            new SuperBasicCompilation(@"
$").VerifyDiagnostics(
                // $
                // ^
                new Diagnostic(DiagnosticCode.UnrecognizedCharacter, ((1, 0), (1, 1)), "$"));
        }

        [Fact]
        public void ItReportsMultipleUnrecognizedCharacters()
        {
            new SuperBasicCompilation(@"
x = ____^
ok = ""value $ value""
not_ok = ""value"" $
' $ still ok").VerifyDiagnostics(
                // x = ____^
                //         ^
                new Diagnostic(DiagnosticCode.UnrecognizedCharacter, ((1, 8), (1, 9)), "^"),
                // not_ok = "value" $
                //                  ^
                new Diagnostic(DiagnosticCode.UnrecognizedCharacter, ((3, 17), (3, 18)), "$"));
        }

        [Fact]
        public void ItReportsUnrecognizedCharactersInStringLiterals()
        {
            new SuperBasicCompilation($@"
x = ""test {char.ConvertFromUtf32(27)} string""").VerifyDiagnostics(
                // x = "test  string"
                //           ^
                new Diagnostic(DiagnosticCode.UnrecognizedCharacter, ((1, 10), (1, 11)), "\u001b"));
        }
    }
}
