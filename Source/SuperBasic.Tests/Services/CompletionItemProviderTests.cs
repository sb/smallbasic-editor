// <copyright file="CompletionItemProviderTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Tests.Services
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Diagnostics;
    using Xunit;

    public sealed class CompletionItemProviderTests
    {
        [Fact]
        public void NoItemsOnEmptyString()
        {
            TestForCompletionItems("$");
        }

        [Fact]
        public void CompletesLibrariesStartingWithT()
        {
            TestForCompletionItems("T$", "Text", "TextWindow", "Timer", "Turtle");
        }

        [Fact]
        public void CompletesLibrariesStartingWithS()
        {
            TestForCompletionItems("S$", "Shapes", "Sound", "Stack");
        }

        [Fact]
        public void NoCompletionAfterNonExistingLibraries()
        {
            TestForCompletionItems("Wrong$");
        }

        [Fact]
        public void CompletesAllMembersAfterDot()
        {
            TestForCompletionItems("Program.$", "Delay", "End", "GetArgument", "Pause", "ArgumentCount", "Directory");
        }

        [Fact]
        public void CompletesInACaseInsensitiveManner()
        {
            TestForCompletionItems("Program.d$", "Delay", "Directory");
        }

        [Fact]
        public void CompletesMembersStartingWithPrefix()
        {
            TestForCompletionItems("TextWindow.Wri$", "Write", "WriteLine");
        }

        private static void TestForCompletionItems(string text, params string[] expectedItems)
        {
            var markerCompilation = new SuperBasicCompilation(text);
            var marker = markerCompilation.Diagnostics.Single(d => d.Code == DiagnosticCode.UnrecognizedCharacter && d.Args.Single() == "$");

            var start = marker.Range.Start;
            var end = marker.Range.End;
            start.Line.Should().Be(end.Line);
            start.Column.Should().Be(end.Column);

            var compilation = new SuperBasicCompilation(text.Replace("$", string.Empty, StringComparison.CurrentCulture));
            var actualItems = compilation.ProvideCompletionItems((start.Line, start.Column));

            actualItems.Select(item => item.label).Should().Equal(expectedItems);
        }
    }
}
