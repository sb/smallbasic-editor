// <copyright file="TreeComposer.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Components
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Blazor;
    using Microsoft.AspNetCore.Blazor.Components;
    using Microsoft.AspNetCore.Blazor.RenderTree;
    using SuperBasic.Utilities;

    public sealed class TreeComposer
    {
        private readonly RenderTreeBuilder builder;

        private int sequence;

        public TreeComposer(RenderTreeBuilder builder)
        {
            this.builder = builder;
            this.sequence = 0;
        }

        public void Element(string name, Action<ElementRef> capture = null, Dictionary<string, object> attributes = null, Action body = null)
        {
            this.builder.OpenElement(this.sequence++, name);

            if (!attributes.IsDefault())
            {
                foreach (var pair in attributes)
                {
                    this.builder.AddAttribute(this.sequence++, pair.Key, pair.Value);
                }
            }

            if (!capture.IsDefault())
            {
                this.builder.AddElementReferenceCapture(this.sequence++, capture);
            }

            if (!body.IsDefault())
            {
                body();
            }

            this.builder.CloseElement();
        }

        public void Text(string value)
        {
            this.builder.AddMarkupContent(this.sequence++, value);
        }

        public void Inject<TComponent>(Dictionary<string, object> parameters = null)
            where TComponent : IComponent
        {
            this.builder.OpenComponent<TComponent>(this.sequence++);

            if (!parameters.IsDefault())
            {
                foreach (var pair in parameters)
                {
                    this.builder.AddAttribute(this.sequence++, pair.Key, pair.Value);
                }
            }

            this.builder.CloseComponent();
        }
    }
}
