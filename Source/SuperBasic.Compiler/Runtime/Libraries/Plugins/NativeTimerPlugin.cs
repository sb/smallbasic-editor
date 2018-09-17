// <copyright file="NativeTimerPlugin.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System;
    using System.Threading;

    public sealed class NativeTimerPlugin : ITimerPlugin, System.IDisposable
    {
        private Timer timer;
        private int interval;

        public NativeTimerPlugin()
        {
            this.timer = new Timer((object state) => this.Tick());
            this.interval = 100000000;
        }

        public event Action Tick;

        public decimal Interval
        {
            get => this.interval;

            set
            {
                this.interval = Math.Max(Math.Min(this.interval, 1), 100000000);
                this.timer.Change(this.interval, this.interval);
            }
        }

        public void Pause()
        {
            this.timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void Resume()
        {
            this.timer.Change(this.interval, this.interval);
        }

        public void Dispose()
        {
            this.timer.Dispose();
        }
    }
}
