// <copyright file="LibrariesCollection.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using SuperBasic.Compiler.Runtime;

    public sealed class LibrariesCollection : IEngineLibraries
    {
        public LibrariesCollection()
        {
            // TODO: implement the rest
            this.Array = new ArrayLibrary();
            this.Clock = new ClockLibrary();
            this.Controls = new ControlsLibrary();
        }

        public IArrayLibrary Array { get; private set; }

        public IClockLibrary Clock { get; private set; }

        public IControlsLibrary Controls { get; private set; }

        public IDesktopLibrary Desktop => throw new System.InvalidOperationException();

        public IDictionaryLibrary Dictionary => throw new System.InvalidOperationException();

        public IFileLibrary File => throw new System.InvalidOperationException();

        public IFlickrLibrary Flickr => throw new System.InvalidOperationException();

        public IGraphicsWindowLibrary GraphicsWindow => throw new System.InvalidOperationException();

        public IImageListLibrary ImageList => throw new System.InvalidOperationException();

        public IMathLibrary Math => throw new System.InvalidOperationException();

        public IMouseLibrary Mouse => throw new System.InvalidOperationException();

        public INetworkLibrary Network => throw new System.InvalidOperationException();

        public IProgramLibrary Program => throw new System.InvalidOperationException();

        public IShapesLibrary Shapes => throw new System.InvalidOperationException();

        public ISoundLibrary Sound => throw new System.InvalidOperationException();

        public IStackLibrary Stack => throw new System.InvalidOperationException();

        public ITextLibrary Text => throw new System.InvalidOperationException();

        public ITextWindowLibrary TextWindow => throw new System.InvalidOperationException();

        public ITimerLibrary Timer => throw new System.InvalidOperationException();

        public ITurtleLibrary Turtle => throw new System.InvalidOperationException();
    }
}
