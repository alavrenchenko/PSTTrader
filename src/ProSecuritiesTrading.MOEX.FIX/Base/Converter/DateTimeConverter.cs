/*
   Copyright (C) 2016 Alexey Lavrenchenko (http://prosecuritiestrading.com/)

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Converter
{
    public static class DateTimeConverter
    {
        public static readonly byte[] NumeralsStringASCIIBytes;

        private const long TicksPerMillisecond = 10000;
        private const long TicksPerSecond = TicksPerMillisecond * 1000;
        private const long TicksPerMinute = TicksPerSecond * 60;
        private const long TicksPerHour = TicksPerMinute * 60;
        private const long TicksPerDay = TicksPerHour * 24;

        // Number of milliseconds per time unit
        private const int MillisPerSecond = 1000;
        private const int MillisPerMinute = MillisPerSecond * 60;
        private const int MillisPerHour = MillisPerMinute * 60;
        private const int MillisPerDay = MillisPerHour * 24;

        private const long MaxSeconds = Int64.MaxValue / TicksPerSecond;
        private const long MinSeconds = Int64.MinValue / TicksPerSecond;

        // Number of days in a non-leap year
        private const int DaysPerYear = 365;
        // Number of days in 4 years
        private const int DaysPer4Years = DaysPerYear * 4 + 1;
        // Number of days in 100 years
        private const int DaysPer100Years = DaysPer4Years * 25 - 1;
        // Number of days in 400 years
        private const int DaysPer400Years = DaysPer100Years * 4 + 1;

        // Number of days from 1/1/0001 to 12/31/9999
        private const int DaysTo10000 = DaysPer400Years * 25 - 366;

        private const long MinTicks = 0;
        private const long MaxTicks = DaysTo10000 * TicksPerDay - 1;

        private static readonly int[] DaysToMonth365 = {
            0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365};
        private static readonly int[] DaysToMonth366 = {
            0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366};

        static DateTimeConverter()
        {
            NumeralsStringASCIIBytes = ConverterBase.NumeralsStringASCIIBytes;
        }

        /// <summary>
        /// Format: yyyyMMdd-HH:mm:ss.fff
        /// </summary>
        public static byte[] GetBytes(Int64 ticks)
        {
            int n = (int)(ticks / TicksPerDay);
            int y400 = n / DaysPer400Years;
            n -= y400 * DaysPer400Years;
            int y100 = n / DaysPer100Years;

            if (y100 == 4) y100 = 3;

            n -= y100 * DaysPer100Years;
            int y4 = n / DaysPer4Years;
            n -= y4 * DaysPer4Years;
            int y1 = n / DaysPerYear;

            if (y1 == 4) y1 = 3;

            int year = y400 * 400 + y100 * 100 + y4 * 4 + y1 + 1;

            n -= y1 * DaysPerYear;
            bool leapYear = y1 == 3 && (y4 != 24 || y100 == 3);
            int[] days = leapYear ? DaysToMonth366 : DaysToMonth365;
            int m = n >> 5 + 1;

            while (n >= days[m]) m++;

            int month = m;
            int day = n - days[m - 1] + 1;
            int hour = (int)((ticks / TicksPerHour) % 24);
            int minute = (int)((ticks / TicksPerMinute) % 60);
            int second = (int)((ticks / TicksPerSecond) % 60);
            int millisecond = (int)((ticks / TicksPerMillisecond) % 1000);

            byte[] bytes = new byte[21];

            bytes[3] = NumeralsStringASCIIBytes[year % 10];
            year /= 10;
            bytes[2] = NumeralsStringASCIIBytes[year % 10];
            year /= 10;
            bytes[1] = NumeralsStringASCIIBytes[year % 10];
            year /= 10;
            bytes[0] = NumeralsStringASCIIBytes[year % 10];

            bytes[5] = NumeralsStringASCIIBytes[month % 10];
            month /= 10;
            bytes[4] = NumeralsStringASCIIBytes[month % 10];

            bytes[7] = NumeralsStringASCIIBytes[day % 10];
            day /= 10;
            bytes[6] = NumeralsStringASCIIBytes[day % 10];

            bytes[8] = 45;

            bytes[10] = NumeralsStringASCIIBytes[hour % 10];
            hour /= 10;
            bytes[9] = NumeralsStringASCIIBytes[hour % 10];

            bytes[11] = 58;

            bytes[13] = NumeralsStringASCIIBytes[minute % 10];
            minute /= 10;
            bytes[12] = NumeralsStringASCIIBytes[minute % 10];

            bytes[14] = 58;

            bytes[16] = NumeralsStringASCIIBytes[second % 10];
            second /= 10;
            bytes[15] = NumeralsStringASCIIBytes[second % 10];

            bytes[17] = 46;

            bytes[20] = NumeralsStringASCIIBytes[millisecond % 10];
            millisecond /= 10;
            bytes[19] = NumeralsStringASCIIBytes[millisecond % 10];
            millisecond /= 10;
            bytes[18] = NumeralsStringASCIIBytes[millisecond % 10];

            return bytes;
        }

        /// <summary>
        /// UTCTimestamp. Format: yyyyMMdd-HH:mm:ss.fff
        /// </summary>
        public static bool ParseDateTime(byte[] bytes, out Int64 ticks)
        {
            ticks = 0;
            int length = bytes.Length;

            if ((length != 21) || (bytes[8] != 45) || (bytes[11] != 58) || (bytes[14] != 58) || (bytes[17] != 46))
            {
                return false;
            }

            byte byteValue;
            int index = 0;
            int year = 0;
            int month = 0;
            int day = 0;
            int hour = 0;
            int minute = 0;
            int second = 0;
            int millisecond = 0;

            do
            {
                byteValue = bytes[index];

                if ((byteValue < 48) || (byteValue > 57))
                {
                    return false;
                }

                year = (year * 10) + (byteValue - 48);
                index++;
            }
            while (index < 4);

            do
            {
                byteValue = bytes[index];

                if ((byteValue < 48) || (byteValue > 57))
                {
                    return false;
                }

                month = (month * 10) + (byteValue - 48);
                index++;
            }
            while (index < 6);

            do
            {
                byteValue = bytes[index];

                if ((byteValue < 48) || (byteValue > 57))
                {
                    return false;
                }

                day = (day * 10) + (byteValue - 48);
                index++;
            }
            while (index < 8);

            index++;

            do
            {
                byteValue = bytes[index];

                if ((byteValue < 48) || (byteValue > 57))
                {
                    return false;
                }

                hour = (hour * 10) + (byteValue - 48);
                index++;
            }
            while (index < 11);

            index++;

            do
            {
                byteValue = bytes[index];

                if ((byteValue < 48) || (byteValue > 57))
                {
                    return false;
                }

                minute = (minute * 10) + (byteValue - 48);
                index++;
            }
            while (index < 14);

            index++;

            do
            {
                byteValue = bytes[index];

                if ((byteValue < 48) || (byteValue > 57))
                {
                    return false;
                }

                second = (second * 10) + (byteValue - 48);
                index++;
            }
            while (index < 17);

            index++;

            do
            {
                byteValue = bytes[index];

                if ((byteValue < 48) || (byteValue > 57))
                {
                    return false;
                }

                millisecond = (millisecond * 10) + (byteValue - 48);
                index++;
            }
            while (index < 21);

            if ((year < 1) || (year > 9999) || (month < 1) || (month > 12) || (hour > 23) || (minute > 59) || (second > 59) || (millisecond > 999))
            {
                return false;
            }

            bool isLeapYear = ((year % 4 == 0) && ((year % 100 != 0) || (year % 400 == 0)));

            int[] days = (isLeapYear == true) ? DaysToMonth366 : DaysToMonth365;

            if (day > (days[month] - days[month - 1]))
            {
                return false;
            }

            int y = year - 1;
            int n = y * 365 + y / 4 - y / 100 + y / 400 + days[month - 1] + day - 1;

            Int64 dateToTicks = n * TicksPerDay;
            Int64 totalSeconds = (Int64)hour * 3600 + (Int64)minute * 60 + (Int64)second;

            if ((totalSeconds > MaxSeconds) || (totalSeconds < MinSeconds))
            {
                return false;
            }

            Int64 timeToTicks = totalSeconds * TicksPerSecond;

            Int64 dateTimeTicks = (dateToTicks + timeToTicks) + (millisecond * TicksPerMillisecond);

            if ((dateTimeTicks < MinTicks) || (dateTimeTicks > MaxTicks))
            {
                return false;
            }

            ticks = dateTimeTicks;

            return true;
        }

        /// <summary>
        /// LocalMktDate, Date. Format: yyyyMMdd
        /// </summary>
        public static bool ParseDate(byte[] bytes, out Int64 ticks)
        {
            ticks = 0;
            int length = bytes.Length;

            if (length != 8)
            {
                return false;
            }

            byte byteValue;
            int index = 0;
            int year = 0;
            int month = 0;
            int day = 0;

            do
            {
                byteValue = bytes[index];

                if ((byteValue < 48) || (byteValue > 57))
                {
                    return false;
                }

                year = (year * 10) + (byteValue - 48);
                index++;
            }
            while (index < 4);

            do
            {
                byteValue = bytes[index];

                if ((byteValue < 48) || (byteValue > 57))
                {
                    return false;
                }

                month = (month * 10) + (byteValue - 48);
                index++;
            }
            while (index < 6);

            do
            {
                byteValue = bytes[index];

                if ((byteValue < 48) || (byteValue > 57))
                {
                    return false;
                }

                day = (day * 10) + (byteValue - 48);
                index++;
            }
            while (index < 8);

            if ((year < 1) || (year > 9999) || (month < 1) || (month > 12))
            {
                return false;
            }

            bool isLeapYear = ((year % 4 == 0) && ((year % 100 != 0) || (year % 400 == 0)));

            int[] days = (isLeapYear == true) ? DaysToMonth366 : DaysToMonth365;

            if (day > (days[month] - days[month - 1]))
            {
                return false;
            }

            int y = year - 1;
            int n = y * 365 + y / 4 - y / 100 + y / 400 + days[month - 1] + day - 1;

            Int64 dateToTicks = n * TicksPerDay;

            if ((dateToTicks < MinTicks) || (dateToTicks > MaxTicks))
            {
                return false;
            }

            ticks = dateToTicks;

            return true;
        }

        /// <summary>
        /// Format: yyyyMMdd-HHmmss
        /// </summary>
        public static byte[] CreateTestReqID()
        {
            Int64 ticks = DateTime.UtcNow.Ticks;
            int n = (int)(ticks / TicksPerDay);
            int y400 = n / DaysPer400Years;
            n -= y400 * DaysPer400Years;
            int y100 = n / DaysPer100Years;

            if (y100 == 4) y100 = 3;

            n -= y100 * DaysPer100Years;
            int y4 = n / DaysPer4Years;
            n -= y4 * DaysPer4Years;
            int y1 = n / DaysPerYear;

            if (y1 == 4) y1 = 3;

            int year = y400 * 400 + y100 * 100 + y4 * 4 + y1 + 1;

            n -= y1 * DaysPerYear;
            bool leapYear = y1 == 3 && (y4 != 24 || y100 == 3);
            int[] days = leapYear ? DaysToMonth366 : DaysToMonth365;
            int m = n >> 5 + 1;

            while (n >= days[m]) m++;

            int month = m;
            int day = n - days[m - 1] + 1;
            int hour = (int)((ticks / TicksPerHour) % 24);
            int minute = (int)((ticks / TicksPerMinute) % 60);
            int second = (int)((ticks / TicksPerSecond) % 60);

            byte[] bytes = new byte[15];

            bytes[3] = NumeralsStringASCIIBytes[year % 10];
            year /= 10;
            bytes[2] = NumeralsStringASCIIBytes[year % 10];
            year /= 10;
            bytes[1] = NumeralsStringASCIIBytes[year % 10];
            year /= 10;
            bytes[0] = NumeralsStringASCIIBytes[year % 10];

            bytes[5] = NumeralsStringASCIIBytes[month % 10];
            month /= 10;
            bytes[4] = NumeralsStringASCIIBytes[month % 10];

            bytes[7] = NumeralsStringASCIIBytes[day % 10];
            day /= 10;
            bytes[6] = NumeralsStringASCIIBytes[day % 10];

            bytes[8] = 45;

            bytes[10] = NumeralsStringASCIIBytes[hour % 10];
            hour /= 10;
            bytes[9] = NumeralsStringASCIIBytes[hour % 10];

            bytes[13] = NumeralsStringASCIIBytes[minute % 10];
            minute /= 10;
            bytes[12] = NumeralsStringASCIIBytes[minute % 10];

            bytes[16] = NumeralsStringASCIIBytes[second % 10];
            second /= 10;
            bytes[15] = NumeralsStringASCIIBytes[second % 10];

            return bytes;
        }
    }
}
