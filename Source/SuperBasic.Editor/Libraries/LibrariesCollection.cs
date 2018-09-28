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

        public IDesktopLibrary Desktop => throw new System.NotImplementedException();

        public IDictionaryLibrary Dictionary => throw new System.NotImplementedException();

        public IFileLibrary File => throw new System.NotImplementedException();

        public IFlickrLibrary Flickr => throw new System.NotImplementedException();

        public IGraphicsWindowLibrary GraphicsWindow => throw new System.NotImplementedException();

        public IImageListLibrary ImageList => throw new System.NotImplementedException();

        public IMathLibrary Math => throw new System.NotImplementedException();

        public IMouseLibrary Mouse => throw new System.NotImplementedException();

        public INetworkLibrary Network => throw new System.NotImplementedException();

        public IProgramLibrary Program => throw new System.NotImplementedException();

        public IShapesLibrary Shapes => throw new System.NotImplementedException();

        public ISoundLibrary Sound => throw new System.NotImplementedException();

        public IStackLibrary Stack => throw new System.NotImplementedException();

        public ITextLibrary Text => throw new System.NotImplementedException();

        public ITextWindowLibrary TextWindow => throw new System.NotImplementedException();

        public ITimerLibrary Timer => throw new System.NotImplementedException();

        public ITurtleLibrary Turtle => throw new System.NotImplementedException();
    }
}
