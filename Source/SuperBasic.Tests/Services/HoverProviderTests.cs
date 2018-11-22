// <copyright file="HoverProviderTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Tests.Services
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Diagnostics;
    using SuperBasic.Utilities;
    using SuperBasic.Utilities.Resources;
    using Xunit;

    public sealed class HoverProviderTests
    {
        [Fact]
        public void NoHoverOnEmptyString()
        {
            TestForHover("$");
        }

        [Fact]
        public void NoHoverOnVariables()
        {
            TestForHover("some$thing = 6");
        }

        [Fact]
        public void HoverOnLibraryNames()
        {
            TestForHover("Text$Window.WriteLine(5)", "TextWindow", LibrariesResources.TextWindow);
        }

        [Fact]
        public void HoverOnMethodName()
        {
            TestForHover("TextWindow.Write$Line(1)", "WriteLine", LibrariesResources.TextWindow_WriteLine);
        }

        [Fact]
        public void HoverOnPropertyName()
        {
            TestForHover("x = Clock.Ti$me", "Time", LibrariesResources.Clock_Time);
        }

        [Fact]
        public void HoverOnEventName()
        {
            TestForHover(@"
Sub b
EndSub
Controls.ButtonCl$icked = b",
                "ButtonClicked",
                LibrariesResources.Controls_ButtonClicked);
        }

        [Fact]
        public void HoverOnError()
        {
            TestForHover(
                "TextWindow.No$Method()",
                new Diagnostic(DiagnosticCode.LibraryMemberNotFound, ((1, 1), (2, 2)), "TextWindow", "NoMethod").ToDisplayString());
        }

        private static void TestForHover(string text, params string[] expectedHover)
        {
            var markerCompilation = new SuperBasicCompilation(text);
            var marker = markerCompilation.Diagnostics.Single(d => d.Code == DiagnosticCode.UnrecognizedCharacter && d.Args.Single() == "$");

            var start = marker.Range.Start;
            var end = marker.Range.End;
            start.Line.Should().Be(end.Line);
            start.Column.Should().Be(end.Column);

            var compilation = new SuperBasicCompilation(text.Replace("$", string.Empty, StringComparison.CurrentCulture));
            var hover = compilation.ProvideHover((start.Line, start.Column));
            hover.Should().Equal(expectedHover);
        }
    }
}
