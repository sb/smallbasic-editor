// <copyright file="ClockLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System;
    using System.Globalization;

    internal static partial class Libraries
    {
        private static string Get_Clock_Date()
        {
            string pattern = DateTimeFormatInfo.GetInstance(CultureInfo.CurrentCulture).ShortDatePattern;
            return DateTime.Now.ToString(pattern, CultureInfo.CurrentCulture);
        }

        private static decimal Get_Clock_Day() => DateTime.Now.Day;

        private static decimal Get_Clock_ElapsedMilliseconds() => (decimal)(DateTime.Now - new DateTime(1900, 1, 1)).TotalMilliseconds;

        private static decimal Get_Clock_Hour() => DateTime.Now.Hour;

        private static decimal Get_Clock_Millisecond() => DateTime.Now.Millisecond;

        private static decimal Get_Clock_Minute() => DateTime.Now.Minute;

        private static decimal Get_Clock_Month() => DateTime.Now.Month;

        private static decimal Get_Clock_Second() => DateTime.Now.Second;

        private static string Get_Clock_Time()
        {
            string pattern = DateTimeFormatInfo.GetInstance(CultureInfo.CurrentCulture).LongTimePattern;
            return DateTime.Now.ToString(pattern, CultureInfo.CurrentCulture);
        }

        private static string Get_Clock_WeekDay() => DateTime.Now.ToString("dddd", CultureInfo.CurrentCulture);

        private static decimal Get_Clock_Year() => DateTime.Now.Year;
    }
}
