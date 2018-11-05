// <copyright file="TimerLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using System.Threading;
    using SuperBasic.Compiler.Runtime;

    internal sealed class TimerLibrary : ITimerLibrary, IDisposable
    {
        private readonly Timer timer;
        private int interval;

        public TimerLibrary()
        {
            this.timer = new Timer((object state) => this.Tick());
            this.interval = 100000000;
        }

        public event Action Tick;

        public decimal Get_Interval() => this.interval;

        public void Set_Interval(decimal value)
        {
            this.interval = Math.Max(Math.Min((int)value, 1), 100000000);
            this.timer.Change(this.interval, this.interval);
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
