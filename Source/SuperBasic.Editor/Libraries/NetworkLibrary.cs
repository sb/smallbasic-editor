// <copyright file="NetworkLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using SuperBasic.Compiler.Runtime;

    public interface INetworkPlugin
    {
        string GetWebPageContents(string url);
    }

    internal sealed class NetworkLibrary : INetworkLibrary
    {
        private readonly IFilePlugin filePlugin;
        private readonly INetworkPlugin networkPlugin;

        public NetworkLibrary(IFilePlugin filePlugin, INetworkPlugin networkPlugin)
        {
            this.filePlugin = filePlugin;
            this.networkPlugin = networkPlugin;
        }

        public string DownloadFile(string url)
        {
            string filePath = this.filePlugin.GetTemporaryFilePath();
            string contents = this.networkPlugin.GetWebPageContents(url);

            this.filePlugin.WriteContents(filePath, contents);
            return filePath;
        }

        public string GetWebPageContents(string url)
        {
            return this.networkPlugin.GetWebPageContents(url);
        }
    }
}
