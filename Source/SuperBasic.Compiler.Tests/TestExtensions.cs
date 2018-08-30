// <copyright file="TestExtensions.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using FluentAssertions;
    using SuperBasic.Compiler.Diagnostics;
    using SuperBasic.Utilities;

    internal static class TestExtensions
    {
        public static void VerifyDiagnostics(this SuperBasicCompilation compilation, params Diagnostic[] diagnostics)
        {
            string[] textLines = Regex.Split(compilation.Text, @"\r?\n");
            string expected = SerializeDiagnostics(textLines, diagnostics);
            string actual = SerializeDiagnostics(textLines, compilation.Diagnostics);

            expected.Should().Be(actual);
        }

        private static string SerializeDiagnostics(string[] textLines, IEnumerable<Diagnostic> diagnostics)
        {
            return diagnostics.Select(diagnostic =>
            {
                diagnostic.Range.Start.Line.Should().Be(diagnostic.Range.End.Line, "because multiline diagnostics are not supported yet");

                int line = diagnostic.Range.Start.Line;
                int start = diagnostic.Range.Start.Column;
                int end = diagnostic.Range.End.Column;

                List<string> constructorArgs = new List<string>()
                {
                    $"DiagnosticCode.{diagnostic.Code}",
                    diagnostic.Range.ToDisplayString()
                };

                constructorArgs.AddRange(diagnostic.Args.Select(arg => $@"""{arg}"""));

                return $@"
                // {textLines[line]}
                // {new string(' ', start)}{new string('^', end - start + 1)}
                // {diagnostic.ToDisplayString()}
                new Diagnostic({constructorArgs.Join(", ")})";
            }).Join(",") + Environment.NewLine;
        }
    }
}
