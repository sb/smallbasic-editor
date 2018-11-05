// <copyright file="TestExtensions.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using FluentAssertions;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Diagnostics;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Utilities;

    internal static class TestExtensions
    {
        public static SuperBasicCompilation VerifyDiagnostics(this SuperBasicCompilation compilation, params Diagnostic[] diagnostics)
        {
            string[] textLines = Regex.Split(compilation.Text, @"\r?\n");
            string expected = SerializeDiagnostics(textLines, diagnostics);
            string actual = SerializeDiagnostics(textLines, compilation.Diagnostics);

            expected.Should().Be(actual);
            return compilation;
        }

        public static void VerifyRuntime(this SuperBasicCompilation compilation, string expectedLog = default, string memoryContents = default)
        {
            compilation.VerifyDiagnostics();

            StringBuilder log = new StringBuilder();
            SuperBasicEngine engine = new SuperBasicEngine(compilation, new LoggingEngineLibraries(log));

            while (engine.State != ExecutionState.Terminated)
            {
                engine.State.Should().Be(ExecutionState.Running, "loggers cannot move to another state");
                engine.Execute(pauseAtNextStatement: false);
            }

            DebuggerSnapshot snapshot = engine.GetSnapshot();
            snapshot.ExecutionStack.Should().BeEmpty();

            if (!expectedLog.IsDefault())
            {
                (Environment.NewLine + log.ToString()).Should().Be(expectedLog);
            }

            if (!memoryContents.IsDefault())
            {
                string values = Environment.NewLine
                    + snapshot.Memory.Select(pair => $"{pair.Key} = {pair.Value.ToDisplayString()}").Join(Environment.NewLine);

                values.Should().Be(memoryContents);
            }
        }

        private static string SerializeDiagnostics(string[] textLines, IEnumerable<Diagnostic> diagnostics)
        {
            return diagnostics.Select(diagnostic =>
            {
                diagnostic.Range.Start.Line.Should().Be(diagnostic.Range.End.Line, "multiline diagnostics are not supported yet");

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
                // {(textLines.Length > line ? textLines[line] : string.Empty)}
                // {new string(' ', start)}{new string('^', end - start + 1)}
                // {diagnostic.ToDisplayString()}
                new Diagnostic({constructorArgs.Join(", ")})";
            }).Join(",") + Environment.NewLine;
        }
    }
}
