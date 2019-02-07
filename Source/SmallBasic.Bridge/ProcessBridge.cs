//<copyright file = "ProcessBridge.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Bridge
{
    using System.Diagnostics;

    internal class ProcessBridge : IProcessBridge
    {
        public void OpenExternalLink(string url)
        {
            Process.Start(url);
        }
    }
}
