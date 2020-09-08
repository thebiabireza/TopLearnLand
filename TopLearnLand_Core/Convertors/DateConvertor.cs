﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TopLearnLand_Core.Convertors
{
    public static class DateConvertor
    {
        public static string ToShamsiDate(this DateTime date)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            return persianCalendar.GetYear(date) + "/" + persianCalendar.GetMonth(date).ToString("00") + "/" +
                   persianCalendar.GetDayOfMonth(date).ToString("00");
        }
    }
}
