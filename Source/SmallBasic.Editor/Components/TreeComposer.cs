// <copyright file="TreeComposer.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Components
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Blazor;
    using Microsoft.AspNetCore.Blazor.Components;
    using Microsoft.AspNetCore.Blazor.RenderTree;
    using SmallBasic.Utilities;

    public sealed class TreeComposer
    {
        private readonly RenderTreeBuilder builder;

        private int sequence;

        public TreeComposer(RenderTreeBuilder builder)
        {
            this.builder = builder;
            this.sequence = 0;
        }

        public void Element(
            string name,
            IReadOnlyDictionary<string, string> attributes = null,
            IReadOnlyDictionary<string, string> styles = null,
            Action<ElementRef> capture = null,
            Events events = null,
            Action body = null)
        {
            this.builder.OpenElement(this.sequence++, name);

            if (!attributes.IsDefault())
            {
                foreach (var pair in attributes)
                {
                    this.builder.AddAttribute(this.sequence++, pair.Key, pair.Value);
                }
            }

            if (!events.IsDefault())
            {
                events.Compose(this);
            }

            if (!styles.IsDefault())
            {
                string styleString = string.Join("; ", styles.Select(pair => $"{pair.Key}: {pair.Value}"));
                this.builder.AddAttribute(this.sequence++, "style", styleString);
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
            this.builder.AddContent(this.sequence++, value);
        }

        public void Markup(string value)
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

        public sealed class Events
        {
            public Action<UIMouseEventArgs> OnClick { get; set; }

            public Func<UIMouseEventArgs, Task> OnClickAsync { get; set; }

            public Action<UIChangeEventArgs> OnInput { get; set; }

            public Func<UIWheelEventArgs, Task> OnMouseWheelAsync { get; set; }

            public Action<UIKeyboardEventArgs> OnKeyDown { get; set; }

            public Func<UIKeyboardEventArgs, Task> OnKeyDownAsync { get; set; }

            public Action<UIKeyboardEventArgs> OnKeyUp { get; set; }

            public Action<UIMouseEventArgs> OnMouseDown { get; set; }

            public Action<UIMouseEventArgs> OnMouseUp { get; set; }

            public Action<UIMouseEventArgs> OnMouseMove { get; set; }

            public void Compose(TreeComposer composer)
            {
                if (!this.OnClick.IsDefault())
                {
                    Debug.Assert(this.OnClickAsync.IsDefault(), "Cannot set both sync and async versions of this event");
                    composer.builder.AddAttribute(composer.sequence++, "onclick", BindMethods.GetEventHandlerValue(this.OnClick));
                }

                if (!this.OnClickAsync.IsDefault())
                {
                    Debug.Assert(this.OnClick.IsDefault(), "Cannot set both sync and async versions of this event");
                    composer.builder.AddAttribute(composer.sequence++, "onclick", BindMethods.GetEventHandlerValue(this.OnClickAsync));
                }

                if (!this.OnInput.IsDefault())
                {
                    composer.builder.AddAttribute(composer.sequence++, "oninput", BindMethods.GetEventHandlerValue(this.OnInput));
                }

                if (!this.OnMouseWheelAsync.IsDefault())
                {
                    composer.builder.AddAttribute(composer.sequence++, "onmousewheel", BindMethods.GetEventHandlerValue(this.OnMouseWheelAsync));
                }

                if (!this.OnKeyDown.IsDefault())
                {
                    Debug.Assert(this.OnKeyDownAsync.IsDefault(), "Cannot set both sync and async versions of this event");
                    composer.builder.AddAttribute(composer.sequence++, "onkeydown", BindMethods.GetEventHandlerValue(this.OnKeyDown));
                }

                if (!this.OnKeyDownAsync.IsDefault())
                {
                    Debug.Assert(this.OnKeyDown.IsDefault(), "Cannot set both sync and async versions of this event");
                    composer.builder.AddAttribute(composer.sequence++, "onkeydown", BindMethods.GetEventHandlerValue(this.OnKeyDownAsync));
                }

                if (!this.OnKeyUp.IsDefault())
                {
                    composer.builder.AddAttribute(composer.sequence++, "onkeyup", BindMethods.GetEventHandlerValue(this.OnKeyUp));
                }

                if (!this.OnMouseDown.IsDefault())
                {
                    composer.builder.AddAttribute(composer.sequence++, "onmousedown", BindMethods.GetEventHandlerValue(this.OnMouseDown));
                }

                if (!this.OnMouseUp.IsDefault())
                {
                    composer.builder.AddAttribute(composer.sequence++, "onmouseup", BindMethods.GetEventHandlerValue(this.OnMouseUp));
                }

                if (!this.OnMouseMove.IsDefault())
                {
                    composer.builder.AddAttribute(composer.sequence++, "onmousemove", BindMethods.GetEventHandlerValue(this.OnMouseMove));
                }
            }
        }
    }
}
