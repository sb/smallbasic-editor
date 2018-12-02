// <copyright file="TextDisplay.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components.Display
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Blazor;
    using SuperBasic.Editor.Components.Layout;
    using SuperBasic.Editor.Interop;
    using SuperBasic.Utilities;

    internal enum AcceptedInputKind
    {
        None,
        Numbers,
        Strings
    }

    public sealed class TextDisplay : SuperBasicComponent, IDisposable
    {
        private ElementRef textDisplayRef;
        private ElementRef inputFieldRef;

        private bool isInitialized = false;
        private string inputBuffer = string.Empty;
        private List<OutputChunk> outputChunks = new List<OutputChunk>();

        public TextDisplay()
        {
            this.AcceptedInput = AcceptedInputKind.None;
            StaticStore.SetTextDisplay(this);
        }

        public event Action<string> InputReceived;

        internal AcceptedInputKind AcceptedInput { get; set; }

        public void Dispose()
        {
            JSInterop.TextDisplay.Dispose().ConfigureAwait(false);
        }

        public void AcceptCharacter(string key)
        {
            if (this.AcceptedInput == AcceptedInputKind.None)
            {
                return;
            }

            switch (key)
            {
                case "Backspace":
                    {
                        if (!this.inputBuffer.Any())
                        {
                            return;
                        }

                        this.inputBuffer = this.inputBuffer.Substring(0, this.inputBuffer.Length - 1);
                        break;
                    }

                case "Enter":
                    {
                        if (!this.inputBuffer.Any())
                        {
                            return;
                        }

                        this.InputReceived(this.inputBuffer);
                        this.outputChunks.Add(new OutputChunk(this.inputBuffer, "gray", appendNewLine: true));
                        this.inputBuffer = string.Empty;
                        break;
                    }

                default:
                    {
                        Debug.Assert(key.Length == 1, "Forgot to handle another key?");
                        char ch = key[0];

                        switch (this.AcceptedInput)
                        {
                            case AcceptedInputKind.Numbers:
                                if (!char.IsDigit(ch) || !decimal.TryParse(this.inputBuffer + key, out _))
                                {
                                    return;
                                }

                                break;

                            case AcceptedInputKind.Strings:
                                break;

                            default:
                                throw ExceptionUtilities.UnexpectedValue(this.AcceptedInput);
                        }

                        this.inputBuffer += key;
                        break;
                    }
            }

            this.StateHasChanged();
        }

        public void AppendOutput(OutputChunk chunk)
        {
            this.outputChunks.Add(chunk);
            this.StateHasChanged();
        }

        public void Clear()
        {
            this.outputChunks.Clear();
            this.StateHasChanged();
        }

        internal static void Inject(TreeComposer composer)
        {
            composer.Inject<TextDisplay>();
        }

        protected override async Task OnAfterRenderAsync()
        {
            if (!this.isInitialized)
            {
                await JSInterop.TextDisplay.Initialize(this.textDisplayRef).ConfigureAwait(false);
                this.isInitialized = true;
            }

            await JSInterop.TextDisplay.ScrollTo(this.inputFieldRef).ConfigureAwait(false);
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            composer.Element("text-display", capture: element => this.textDisplayRef = element, body: () =>
            {
                foreach (var chunk in this.outputChunks)
                {
                    composer.Element("span", body: () => composer.Text(chunk.Text), attributes: new Dictionary<string, object>
                    {
                        { "style", $"color: {chunk.HexColor}" }
                    });

                    if (chunk.AppendNewLine)
                    {
                        composer.Element("br");
                    }
                }

                composer.Element("input-field", capture: element => this.inputFieldRef = element, body: () =>
                {
                    if (this.AcceptedInput != AcceptedInputKind.None)
                    {
                        composer.Element("span", body: () => composer.Text(this.inputBuffer));
                        composer.Element(name: "span", body: () =>
                        {
                            composer.Element("cursor", body: () =>
                            {
                                composer.Text("&#x2588;");
                            });
                        });
                    }
                });
            });
        }
    }

    public class OutputChunk
    {
        public OutputChunk(string text, string hexColor, bool appendNewLine)
        {
            this.Text = text;
            this.HexColor = hexColor;
            this.AppendNewLine = appendNewLine;
        }

        public string Text { get; private set; }

        public string HexColor { get; private set; }

        public bool AppendNewLine { get; set; }
    }
}
