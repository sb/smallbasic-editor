// <copyright file="TimerLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using System.Threading;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Utilities;

    internal sealed class TimerLibrary : ITimerLibrary, IDisposable
    {
        private const int MaxInterval = 100000000;

        private readonly Timer timer;

        private int interval;

        public TimerLibrary()
        {
            this.timer = new Timer((object state) =>
            {
                if (!this.Tick.IsDefault())
                {
                    this.Tick();
                }
            });

            this.interval = MaxInterval;
            this.Pause();
        }

        public event Action Tick;

        public decimal Get_Interval() => this.interval;

        public void Set_Interval(decimal value)
        {
            this.interval = Math.Max(Math.Min((int)value, MaxInterval), 10);
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
