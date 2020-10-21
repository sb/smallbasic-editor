// <copyright file="TextDisplay.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Components.Display
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Blazor;
    using SmallBasic.Editor.Components.Layout;
    using SmallBasic.Editor.Interop;
    using SmallBasic.Editor.Store;
    using SmallBasic.Utilities;

    internal enum AcceptedInputMode
    {
        None,
        Numbers,
        Strings
    }

    public sealed class TextDisplay : SmallBasicComponent
    {
        private readonly List<OutputChunk> outputChunks = new List<OutputChunk>();

        private ElementRef inputFieldRef;
        private ElementRef textDisplayRef;
        private string inputBuffer = string.Empty;
        private string backgroundColor = "black";
        private AcceptedInputMode mode = AcceptedInputMode.None;

        public TextDisplay()
        {
            this.mode = AcceptedInputMode.None;
            TextDisplayStore.SetDisplay(this);
        }

        public string Title { get; set; }

        internal async Task AppendOutput(OutputChunk chunk)
        {
            this.outputChunks.Add(chunk);
            // Important to prevent th UI from freezing
            await Task.Delay(1).ConfigureAwait(false);
            await JSInterop.Layout.ScrollIntoView(this.inputFieldRef).ConfigureAwait(false);
            this.StateHasChanged();
        }

        internal void Clear()
        {
            this.outputChunks.Clear();
            this.StateHasChanged();
        }

        internal async Task SetInputMode(AcceptedInputMode mode)
        {
            this.mode = mode;

            if (this.mode != AcceptedInputMode.None)
            {
                await JSInterop.Layout.Focus(this.textDisplayRef).ConfigureAwait(false);
            }

            this.StateHasChanged();
        }

        internal void SetBackgroundColor(string hexColor)
        {
            this.backgroundColor = hexColor;
            this.StateHasChanged();
        }

        internal static void Inject(TreeComposer composer)
        {
            composer.Inject<TextDisplay>();
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            composer.Element(
                name: "text-display",
                capture: element => this.textDisplayRef = element,
                styles: new Dictionary<string, string>
                {
                    { "background-color", this.backgroundColor }
                },
                attributes: new Dictionary<string, string>
                {
                    { "tabindex", "0" }, // required to receive focus
                },
                events: new TreeComposer.Events
                {
                    OnKeyDownAsync = args => this.AcceptInput(args.Key)
                },
                body: () =>
                {
                    if (!string.IsNullOrEmpty(this.Title))
                    {
                        composer.Element(
                            name: "title",
                            body: () => composer.Text(this.Title));
                    }
                    foreach (var chunk in this.outputChunks)
                    {
                        composer.Element(
                            name: "span",
                            body: () => composer.Text(chunk.Text),
                            styles: new Dictionary<string, string>
                            {
                                { "color", chunk.HexColor }
                            });

                        if (chunk.AppendNewLine)
                        {
                            composer.Element("br");
                        }
                    }

                    composer.Element("input-field", capture: element => this.inputFieldRef = element, body: () =>
                    {
                        if (this.mode != AcceptedInputMode.None)
                        {
                            composer.Element("span", body: () => composer.Text(this.inputBuffer));
                            composer.Element(name: "span", body: () =>
                            {
                                composer.Element("cursor", body: () =>
                                {
                                    composer.Markup("&#x2588;");
                                });
                            });
                        }
                    });
                });
        }

        private async Task AcceptInput(string key)
        {
            if (this.mode == AcceptedInputMode.None)
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

                        TextDisplayStore.NotifyTextInput(this.inputBuffer);
                        await this.AppendOutput(new OutputChunk(this.inputBuffer, "gray", appendNewLine: true)).ConfigureAwait(false);
                        this.inputBuffer = string.Empty;
                        break;
                    }

                default:
                    {
                        if (key.Length == 1)
                        {
                            char ch = key[0];
                            switch (this.mode)
                            {
                                case AcceptedInputMode.Numbers:
                                    bool validNumber = char.IsDigit(ch)
                                        // first char can be '-' for negative numbers
                                        || (ch == '-' && this.inputBuffer.Length < 1)
                                        // decimal numbers can contain one '.'
                                        || (ch == '.' && !this.inputBuffer.Contains('.'));
                                    if (!validNumber)
                                    {
                                        return;
                                    }

                                    break;

                                case AcceptedInputMode.Strings:
                                    break;

                                default:
                                    throw ExceptionUtilities.UnexpectedValue(this.mode);
                            }

                            this.inputBuffer += key;
                        }

                        break;
                    }
            }

            this.StateHasChanged();
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
