// <copyright file="LibrariesTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Tests.Runtime
{
    using System.Threading.Tasks;
    using SmallBasic.Compiler;
    using Xunit;

    public sealed class LibrariesTests
    {
        [Theory]
        [InlineData(0, 0, "")]
        [InlineData(-5, 0, "")]
        [InlineData(0, -2, "")]
        [InlineData(1, -2, "")]
        [InlineData(0, 200, "")]
        [InlineData(1, 200, "1234567890")]
        [InlineData(4, 200, "4567890")]
        [InlineData(4, 3, "456")]
        public Task ItGetsSubTextOfAString(int start, int length, string result)
        {
            return new SmallBasicCompilation($@"
x = ""1234567890""
y = Text.GetSubText(x, {start}, {length})
").VerifyRealRuntime($@"
x = 1234567890
y = {result}");
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(-5, "")]
        [InlineData(50, "")]
        [InlineData(1, "1234567890")]
        [InlineData(4, "4567890")]
        public Task ItGetsSubTextToEndOfAString(int start, string result)
        {
            return new SmallBasicCompilation($@"
x = ""1234567890""
y = Text.GetSubTextToEnd(x, {start})
").VerifyRealRuntime($@"
x = 1234567890
y = {result}");
        }
    }
}
