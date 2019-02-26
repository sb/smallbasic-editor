// <copyright file="ExpressionTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Tests.Runtime
{
    using System.Threading.Tasks;
    using SmallBasic.Compiler;
    using Xunit;

    public sealed class ExpressionTests : IClassFixture<CultureFixture>
    {
        [Fact]
        public Task NumericStringsAreTreatedAsNumbers()
        {
            return new SmallBasicCompilation(@"
x = ""1"" + 1
y = 4 + ""-1""").VerifyLoggingRuntime(memoryContents: @"
x = 2
y = 3");
        }

        [Fact]
        public Task ItEvaluatesArrayAccess()
        {
            return new SmallBasicCompilation(@"
ar[0] = ""first""
ar[1][0] = ""second""
ar[1][2] = ""third""

found_first = ar[0]
found_second = ar[1][2]

not_found_ar = none[0]
not_found_first = ar[4]
not_found_second = ar[1][6]
").VerifyLoggingRuntime(memoryContents: @"
ar = 0=first;1=0\=second\;2\=third\;;
found_first = first
found_second = third
not_found_ar = 
not_found_first = 
not_found_second = ");
        }

        [Fact]
        public Task ItSupportsDBCSInStrings()
        {
            return new SmallBasicCompilation(@"
TextWindow.WriteLine(""こんにちは!"")
").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: 'こんにちは!')
");
        }

        [Fact]
        public Task ItSupportsDBCSInIdentifiers()
        {
            return new SmallBasicCompilation(@"
サブ()
TextWindow.WriteLine(変数)
Goto ラベル

Sub サブ
  変数 = 5
EndSub

TextWindow.WriteLine(""should be skipped"")
ラベル:
").VerifyLoggingRuntime(expectedLog: @"
TextWindow.WriteLine(data: '5')
");
        }
    }
}
