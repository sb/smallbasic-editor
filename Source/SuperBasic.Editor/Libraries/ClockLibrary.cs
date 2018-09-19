// <copyright file="ClockLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using System.Globalization;
    using SuperBasic.Compiler.Runtime;

    public sealed class ClockLibrary : IClockLibrary
    {
        public string Date => DateTime.Now.ToString(DateTimeFormatInfo.GetInstance(CultureInfo.CurrentCulture).ShortDatePattern, CultureInfo.CurrentCulture);

        public decimal Day => DateTime.Now.Day;

        public decimal ElapsedMilliseconds => (decimal)(DateTime.Now - new DateTime(1900, 1, 1)).TotalMilliseconds;

        public decimal Hour => DateTime.Now.Hour;

        public decimal Millisecond => DateTime.Now.Millisecond;

        public decimal Minute => DateTime.Now.Minute;

        public decimal Month => DateTime.Now.Month;

        public decimal Second => DateTime.Now.Second;

        public string Time => DateTime.Now.ToString(DateTimeFormatInfo.GetInstance(CultureInfo.CurrentCulture).LongTimePattern, CultureInfo.CurrentCulture);

        public string WeekDay => DateTime.Now.ToString("dddd", CultureInfo.CurrentCulture);

        public decimal Year => DateTime.Now.Year;
    }
}
