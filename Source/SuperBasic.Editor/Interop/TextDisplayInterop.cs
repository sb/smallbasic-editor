// <copyright file="TextDisplayInterop.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Interop
{
    using System.Threading.Tasks;
    using SuperBasic.Editor.Components;

    public class TextDisplayInterop : ITextDisplayInterop
    {
        public Task AcceptInput(string key)
        {
            StaticStore.TextDisplay.AcceptCharacter(key);
            return Task.CompletedTask;
        }
    }
}
