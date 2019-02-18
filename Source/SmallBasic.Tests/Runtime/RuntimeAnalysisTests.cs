// <copyright file="RuntimeAnalysisTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Tests.Runtime
{
    using FluentAssertions;
    using SmallBasic.Compiler;
    using SmallBasic.Compiler.Binding;
    using Xunit;

    public sealed class RuntimeAnalysisTests : IClassFixture<CultureFixture>
    {
        [Fact]
        public void ItDoesNotUseGraphicsWindowWhenNotNeeded()
        {
            var compilation = new SmallBasicCompilation("TextWindow.WriteLine(5)");
            compilation.Analysis.UsesTextWindow.Should().Be(true);
            compilation.Analysis.UsesGraphicsWindow.Should().Be(false);
        }

        [Fact]
        public void ItUsesGraphicsWindowWhenNeeded()
        {
            var compilation = new SmallBasicCompilation("GraphicsWindow.Clear()");
            compilation.Analysis.UsesTextWindow.Should().Be(false);
            compilation.Analysis.UsesGraphicsWindow.Should().Be(true);
        }

        [Fact]
        public void ItUsesTextWindowWhenNothingIsNeeded()
        {
            var compilation = new SmallBasicCompilation("x = 1");
            compilation.Analysis.UsesTextWindow.Should().Be(true);
            compilation.Analysis.UsesGraphicsWindow.Should().Be(false);
        }
    }
}
