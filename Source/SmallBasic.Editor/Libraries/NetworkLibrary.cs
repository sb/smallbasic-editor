// <copyright file="NetworkLibrary.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
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
