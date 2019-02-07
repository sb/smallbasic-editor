// <copyright file="NetworkLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries
{
    using System.Threading.Tasks;
    using SmallBasic.Compiler.Runtime;

    internal sealed class NetworkLibrary : INetworkLibrary
    {
        public Task<string> DownloadFile(string url)
            => Bridge.Network.DownloadFile(url);

        public Task<string> GetWebPageContents(string url)
            => Bridge.Network.GetWebPageContents(url);
    }
}
