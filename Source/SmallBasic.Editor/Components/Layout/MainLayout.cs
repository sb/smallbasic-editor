// <copyright file="MainLayout.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Components.Layout
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using SmallBasic.Editor.Components.Toolbox;
    using SmallBasic.Editor.Interop;
    using SmallBasic.Utilities;
    using SmallBasic.Utilities.Resources;

    public abstract class MainLayout : SmallBasicComponent
    {
        private static readonly IReadOnlyDictionary<string, string> HeaderLinks = new Dictionary<string, string>
        {
            { EditorResources.Header_TutorialsLink, "https://smallbasic-publicwebsite.azurewebsites.net/tutorials" },
            { EditorResources.Header_DocumentationLink, "https://smallbasic-publicwebsite.azurewebsites.net/docs" }
        };

        protected override Task OnInitAsync()
        {
            return JSInterop.Layout.InitializeWebView(CultureInfo.CurrentCulture.Name, EditorResources.ApplicationTitle);
        }

        protected sealed override void ComposeTree(TreeComposer composer)
        {
            composer.Element("main-layout", body: () =>
            {
                composer.Element("header-row", body: () =>
                {
                    composer.Element("logo-area", body: () =>
                    {
                        composer.Element("logo");
                    });

                    composer.Element("header-links", body: () =>
                    {
                        foreach (var pair in HeaderLinks)
                        {
                            composer.Element(
                                name: "header-link",
                                events: new TreeComposer.Events
                                {
                                    OnClickAsync = args => OpenExtrernalLink(pair.Value)
                                },
                                body: () =>
                                {
                                    composer.Text(pair.Key);
                                });
                        }
                    });
                });

                Actions.Row(composer, left: () => this.ComposeLeftActions(composer), right: () => this.ComposeRightActions(composer));

                composer.Element("page-contents", body: () =>
                {
                    this.ComposeBody(composer);
                });
            });
        }

        protected virtual void ComposeBody(TreeComposer composer)
        {
        }

        protected virtual void ComposeLeftActions(TreeComposer composer)
        {
        }

        protected virtual void ComposeRightActions(TreeComposer composer)
        {
        }

        private static Task OpenExtrernalLink(string url)
        {
#if IsBuildingForDesktop
            return Bridge.Process.OpenExternalLink(url);
#else
            return JSInterop.Layout.OpenExternalLink(url);
#endif
        }
    }
}
