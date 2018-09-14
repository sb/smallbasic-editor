// <copyright file="NativeStackPlugin.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed class NativeStackPlugin : IStackPlugin
    {
        private Dictionary<string, Stack<string>> stacks = new Dictionary<string, Stack<string>>();

        public decimal GetCount(string stackName)
        {
            if (this.stacks.TryGetValue(stackName, out Stack<string> stack))
            {
                return stack.Count;
            }

            return 0;
        }

        public string PopValue(string stackName)
        {
            if (this.stacks.TryGetValue(stackName, out Stack<string> stack) && stack.Any())
            {
                return stack.Pop();
            }

            return string.Empty;
        }

        public void PushValue(string stackName, string value)
        {
            if (!this.stacks.ContainsKey(stackName))
            {
                this.stacks.Add(stackName, new Stack<string>());
            }

            this.stacks[stackName].Push(value);
        }
    }
}
