using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CommonUtil
{

    public class Validate
    {
        public static Regex RgxInterger = new Regex(Enums.RegexDefine.IntergerAm, RegexOptions.None);
        public static Regex RgxNumber = new Regex(Enums.RegexDefine.NumericAm, RegexOptions.None);
        public static Regex RgxDateVn = new Regex(Enums.RegexDefine.DateVN, RegexOptions.None);
        public static Regex RgxDateTimeVn = new Regex(Enums.RegexDefine.DateTimeVN, RegexOptions.None);
        public static Regex RgxDateIso = new Regex(Enums.RegexDefine.DateIso, RegexOptions.None);
        public static Regex RgxGuid = new Regex(Enums.RegexDefine.Guid, RegexOptions.IgnoreCase);

        public static bool IsInterger(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return RgxInterger.IsMatch(value);
        }

        public static int ConvertInt(string value, int defaultValue = 0)
        {
            if (IsInterger(value))
            {
                return Convert.ToInt32(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public static int? ConvertIntAlowNull(string value, int? defaultValue = null)
        {
            if (IsInterger(value))
            {
                return Convert.ToInt32(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public static byte ConvertByte(string value, byte defaultValue = 0)
        {
            if (IsInterger(value))
            {
                return Convert.ToByte(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public static byte? ConvertByteAlowNull(string value, byte? defaultValue = null)
        {
            if (IsInterger(value))
            {
                return Convert.ToByte(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public static short ConvertShort(string value, short defaultValue = 0)
        {
            if (IsInterger(value))
            {
                return Convert.ToInt16(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public static short? ConvertShortAlowNull(string value, short? defaultValue = null)
        {
            if (IsInterger(value))
            {
                return Convert.ToInt16(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public static long ConvertLong(string value, long defaultValue = 0)
        {
            if (IsInterger(value))
            {
                return Convert.ToInt64(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public static long? ConvertLongAlowNull(string value, long? defaultValue = null)
        {
            if (IsInterger(value))
            {
                return Convert.ToInt64(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public static bool IsNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return RgxNumber.IsMatch(value);
        }

        public static bool IsDecimal(string value, string dot = ".")
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return Regex.IsMatch(value, @"^\d+(\" + dot + @"\d+)?$");
        }

        public static decimal ConvertDecimal(string value, decimal defaultValue = 0)
        {
            if (IsNumber(value))
            {
                return Convert.ToDecimal(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public static decimal? ConvertDecimalAlowNull(string value, decimal? defaultValue = null)
        {
            if (IsNumber(value))
            {
                return Convert.ToDecimal(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public static double ConvertDouble(string value, double defaultValue = 0)
        {
            if (IsNumber(value))
            {
                return Convert.ToDouble(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public static double? ConvertDoubleAlowNull(string value, double? defaultValue = null)
        {
            if (IsNumber(value))
            {
                return Convert.ToDouble(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public static bool IsDateVn(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return RgxDateVn.IsMatch(value);
        }

        public static bool IsDateTimeVn(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return RgxDateTimeVn.IsMatch(value);
        }

        public static bool IsGuid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return RgxGuid.IsMatch(value);
        }

        public static DateTime ConvertDateVN(string value, DateTime? defaultValue = null)
        {
            if (IsDateVn(value))
            {
                return DateTime.ParseExact(value, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                return defaultValue.HasValue ? defaultValue.Value : DateTime.MinValue;
            }
        }

        public static DateTime ConvertDateMonthVN(string value, DateTime? defaultValue = null)
        {
            value = "01/" + value;
            if (IsDateVn(value))
            {
                return DateTime.ParseExact(value, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                return defaultValue.HasValue ? defaultValue.Value : DateTime.MinValue;
            }
        }

        public static DateTime ConvertDateTimeVN(string value, DateTime? defaultValue = null)
        {
            if (IsDateTimeVn(value))
            {
                return DateTime.ParseExact(value, "d/M/yyyy HH:mm", CultureInfo.InvariantCulture);
            }
            else
            {
                return ConvertDateVN(value, defaultValue);
            }
        }

        public static DateTime? ConvertDateVNAlowNull(string value, DateTime? defaultValue = null)
        {
            if (IsDateVn(value))
            {
                return DateTime.ParseExact(value, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                return defaultValue;
            }
        }

        public static DateTime ConvertDateVNShortAlowNull(string value, DateTime defaultValue)
        {
            if (IsDateVn(value))
            {
                return DateTime.ParseExact(value, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                return defaultValue;
            }
        }

        public static DateTime? ConvertDateTimeVNAlowNull(string value, DateTime? defaultValue = null)
        {
            if (IsDateTimeVn(value))
            {
                return DateTime.ParseExact(value, "d/M/yyyy HH:mm", CultureInfo.InvariantCulture);
            }
            else
            {
                return ConvertDateVNAlowNull(value, defaultValue);
            }
        }

        public static bool IsDateIso(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return RgxInterger.IsMatch(value);
        }

        public static DateTime ConvertDateIso(string value, DateTime? defaultValue)
        {
            if (IsDateIso(value))
            {
                return Convert.ToDateTime(value);
            }
            else
            {
                return defaultValue.HasValue ? defaultValue.Value : DateTime.Now;
            }
        }

        public static bool CheckCustom(string pattern, string value)
        {
            return Regex.IsMatch(value, pattern);
        }

        public static DateTime ConvertDate(string value, string format = "d/M/yyy", string DateSeparator = "/")
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo
            {
                ShortDatePattern = format,
                DateSeparator = DateSeparator
            };
            return Convert.ToDateTime(value, dtfi);
        }

        public static DateTime? MaxDate(DateTime? date1, DateTime? date2)
        {
            if (date1.HasValue && date2.HasValue)
            {
                if (date1.Value > date2.Value)
                {
                    return date1;
                }

                return date2;
            }
            if (date1.HasValue)
            {
                return date1;
            }

            if (date2.HasValue)
            {
                return date2;
            }

            return null;
        }

        public static DateTime? MinDate(DateTime? date1, DateTime? date2)
        {

            if (date1.HasValue && date2.HasValue)
            {
                if (date1.Value > date2.Value)
                {
                    return date2;
                }

                return date1;
            }
            if (date1.HasValue)
            {
                return date1;
            }

            if (date2.HasValue)
            {
                return date2;
            }

            return null;
        }
    }

    public class Enums
    {
        public struct FormatType
        {
            public const string FormatDateVN = "dd/MM/yyyy";
            public const string FormatDateTimeVN = "dd/MM/yyyy HH:mm";
            public const string FormatTime = "HH:mm";
            public const string Currency = "##,#.## vnđ";
            public const string TrongLuong = "##,#.## g";
            public const string Percent = "##,#.##\\%";
            public const string Integer = "##,#";
            public const string Number = "##,#.##}";
        }

        public struct FormatModel
        {
            public const string FormatDateVN = "{0:dd/MM/yyyy}";
            public const string FormatDateTimeVN = "{0:dd/MM/yyyy HH:mm}";
            public const string FormatTime = "{0:HH:mm}";
            public const string Currency = "{0:##,#.##}";
            public const string TrongLuong = "{0:##,#.## g}";
            public const string Percent = "{0:##,0.##\\%}";
            public const string Integer = "{0:##,#}";
            public const string Number = "{0:##,0.##}";
        }
        public struct RegexDefine
        {
            public const string Email = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            public const string Year = @"^\d{4}$";
            public const string Interger = @"^\d*$";//"^0|([1-9]+[0-9]*)$";
            public const string IntergerAm = @"^((\-\d+)|(\d*))$";//@"^0|(\-?[1-9]+[0-9]*)$";
            public const string Numeric = @"^(\d+(\.\d+)?)$";
            public const string NumericAm = @"^(\-?\d+(\.\d+)?)$";
            public const string PhoneNumber = @"^(\+?[0-9\s\-\.]{9,15})$";
            public const string AscII = @"^([a-zA-Z\s]+)$";
            public const string Unicode = "^([\u00c0-\u020f\u1ea0-\u1ef9a-zA-Z0-9_\\-\\.\\s]*)$";
            public const string Code = @"^[a-zA-Z0-9_\-\.]+$";
            public const string CodeVN = "^[\u00c0-\u020fa-zA-Z0-9_\\-\\.]+$";
            public const string CharacterNumber = @"^[a-zA-Z0-9]+$";
            public const string LstCodeVN = "^[\u00c0-\u020fa-zA-Z0-9_\\-\\.\\,]+$";
            public const string CardNumber = @"^[a-zA-Z0-9_ \-\.]+$";
            public const string DateVN = @"^((((31\/(0?[13578]|1[02]))|((29|30)\/(0?[1,3-9]|1[0-2])))\/(1[6-9]|[2-9]\d)?\d{2})|(29\/0?2\/(((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))|(0?[1-9]|1\d|2[0-8])\/((0?[1-9])|(1[0-2]))\/((1[6-9]|[2-9]\d)?\d{2}))$";
            public const string DateTimeVN = @"^((((31\/(0?[13578]|1[02]))|((29|30)\/(0?[1,3-9]|1[0-2])))\/(1[6-9]|[2-9]\d)?\d{2})|(29\/0?2\/(((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))|(0?[1-9]|1\d|2[0-8])\/((0?[1-9])|(1[0-2]))\/((1[6-9]|[2-9]\d)?\d{2}))\s([0-1]?[0-9]|2[0-3]):[0-5][0-9]$";
            public const string DateIso = @"^$";
            public const string MaSoThue = @"^([a-zA-Z0-9\s\-]*)$";
            public const string Time24 = "^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$";
            public const string Time12VN = "^(0?[0-9]|1[0-2]):[0-5][0-9] (SA|CH)$";
            public const string Time12EN = "^(0?[0-9]|1[0-2]):[0-5][0-9] (AM|PM)$";
            public const string Guid = "^\\{?[a-fA-F\\d]{8}-([a-fA-F\\d]{4}-){3}[a-fA-F\\d]{12}\\}?$";
        }
    }
}