//<copyright file = "TestBridge.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Bridge
{
    using System;
    using SuperBasic.Utilities.Bridge;

    internal class TestBridge : ITestBridge
    {
        public void TestMethod1(string value)
        {
            Console.WriteLine("method1: " + value);
        }

        public Bean1 TestMethod2()
        {
            return new Bean1
            {
                X = "method2"
            };
        }

        public string TestMethod3()
        {
            return "method3";
        }

        public string TestMethod4(Bean1 val4)
        {
            return "method4: " + val4.X;
        }
    }
}
