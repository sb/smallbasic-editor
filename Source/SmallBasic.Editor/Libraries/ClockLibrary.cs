// <copyright file="ClockLibrary.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries
{
    using System;
    using System.Globalization;
    using SmallBasic.Compiler.Runtime;

    internal sealed class ClockLibrary : IClockLibrary
    {
        public string Get_Date() => DateTime.Now.ToString(DateTimeFormatInfo.GetInstance(CultureInfo.CurrentCulture).ShortDatePattern, CultureInfo.CurrentCulture);

        public decimal Get_Day() => DateTime.Now.Day;

        public decimal Get_ElapsedMilliseconds() => (decimal)(DateTime.Now - new DateTime(1900, 1, 1)).TotalMilliseconds;

        public decimal Get_Hour() => DateTime.Now.Hour;

        public decimal Get_Millisecond() => DateTime.Now.Millisecond;

        public decimal Get_Minute() => DateTime.Now.Minute;

        public decimal Get_Month() => DateTime.Now.Month;

        public decimal Get_Second() => DateTime.Now.Second;

        public string Get_Time() => DateTime.Now.ToString(DateTimeFormatInfo.GetInstance(CultureInfo.CurrentCulture).LongTimePattern, CultureInfo.CurrentCulture);

        public string Get_WeekDay() => DateTime.Now.ToString("dddd", CultureInfo.CurrentCulture);

        public decimal Get_Year() => DateTime.Now.Year;
    }
}
