using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Globalization;
using System.Reflection;

namespace GharchNews.Utilities
{
    public static class ConvertorDateToShamsi
    {
        private static CultureInfo _culture;

        public static CultureInfo GetPersianCulture()
        {
            if (_culture == null)
            {
                _culture = new CultureInfo("fa-IR");
                DateTimeFormatInfo formatInfo = _culture.DateTimeFormat;
                formatInfo.AbbreviatedDayNames = new string[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
                formatInfo.DayNames = new string[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنج شنبه", "جمعه", "شنبه" };
                var monthNames = new[]
                {
                    "فروردین","اردیبهشت","خرداد","تیر","مرداد","شهریور","مهر","آبان","آذر","دی","بهمن","اسفند", ""
                };
                formatInfo.AbbreviatedMonthNames =
                    formatInfo.MonthNames =
                    formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;
                formatInfo.AMDesignator = "ق.ظ";
                formatInfo.PMDesignator = "ب.ظ";
                formatInfo.ShortDatePattern = "yyyy/MM/dd";
                formatInfo.LongDatePattern = "dddd, dd MMMM,yyyy";
                formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;
                System.Globalization.Calendar cal = new PersianCalendar();

                FieldInfo fieldInfo = _culture.GetType().GetField("calender", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                    fieldInfo.SetValue(_culture, cal);

                _culture.NumberFormat.NumberDecimalSeparator = "/";
                _culture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
                _culture.NumberFormat.NumberNegativePattern = 0;
            }
            return _culture;
        }

    public static string ToShamsi(this DateTime date, string format = "dddd dd MMMM yyyy")
    {
        return date.ToString(format, GetPersianCulture());
    }
}
}