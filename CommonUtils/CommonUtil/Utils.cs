using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

namespace CommonUtil
{
    public static class Utils
    {
        /// <summary>
        /// Obtains the maximum double amog a set of double.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static double Max(params double[] numbers) => numbers.Max();
        /// <summary>
        /// Obtains the minimum double among a set of doubles.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static double Min(params double[] numbers) => numbers.Min();
        /// <summary>
        /// Obtains the maximum decimal among a set of decimals.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static decimal Max(params decimal[] numbers) => numbers.Max();
        /// <summary>
        /// Obtains the minimum decimal among a set of decimals.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static decimal Min(params decimal[] numbers) => numbers.Min();
        /// <summary>
        /// Tolerance to control division between numbers.
        /// </summary>
        public static double Tolerance = 1e-15;

        /// <summary>
        /// format money
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static string formatNumber(object money, string format = Enums.FormatModel.Currency)
        {
            if (money.ToString().Equals("0") || long.Parse(money.ToString().Replace(",", "").Replace(".", "")) == 0)
                return format.Equals(Enums.FormatModel.Currency) ? "0 đ" : "0";
            return String.Format(format, money);
        }

        /// <summary>
        /// Chuyển dự liệu từ list ra table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="FHead">True - Lay dislay name cua field vao row1</param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(IList<T> data, bool FHead = false)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)

                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            if (FHead)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.DisplayName ?? row[prop.Name];
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)

                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                table.Rows.Add(row);
            }

            return table;
        }

        public static bool stringEquals(string a, string b)
        {
            if (string.IsNullOrEmpty(a) && string.IsNullOrEmpty(b))
                return true;
            if (string.IsNullOrEmpty(a))
                return false;
            if (string.IsNullOrEmpty(b))
                return false;
            return string.Equals(a, b);
        }

        public static List<T> DeepClone<T>(List<T> obj)
        {
            List<T> objResult = null;
            string ms = JsonConvert.SerializeObject(obj);
            objResult = JsonConvert.DeserializeObject<List<T>>(ms);
            return objResult;
        }

        public static string ReplaceFirst(this string text, string oldStr, string newStr)
        {
            int pos = text.IndexOf(oldStr);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + newStr + text.Substring(pos + oldStr.Length);
            //var regex = new Regex(Regex.Escape(oldStr));
            //return regex.Replace(text, newStr, 1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="compare"></param>
        /// <param name="obj"></param>
        /// <param name="chekHours"></param>
        /// <returns>
        /// 1 : compare lơn hơn obj;
        /// -1 : compare nhỏ hơn obj;
        /// 0 : compare băng obj
        /// </returns>
        public static int CompareDate(this DateTime compare, DateTime obj, bool chekHours = false)
        {
            if (compare.Year > obj.Year)
                return 1;
            if (compare.Year < obj.Year)
                return -1;
            if (compare.Month > obj.Month)
                return 1;
            if (compare.Month < obj.Month)
                return -1;
            if (compare.Day > obj.Day)
                return 1;
            if (compare.Day < obj.Day)
                return -1;
            if (chekHours)
            {
                if (compare.Hour > obj.Hour)
                    return 1;
                if (compare.Hour < obj.Hour)
                    return -1;
                if (compare.Minute > obj.Minute)
                    return 1;
                if (compare.Minute < obj.Minute)
                    return -1;
                if (compare.Second > obj.Second)
                    return 1;
                if (compare.Second < obj.Second)
                    return -1;
            }
            return 0;
        }
        public static string PadCenter(this string s, int width, char c)
        {
            if (s == null || width <= s.Length) return s;

            int padding = width - s.Length;
            return s.PadLeft(s.Length + padding / 2, c).PadRight(width, c);
        }

        const string temp = "-----------------------------------------------------------------------";
        public static string GetSpaceByLevel(int level)
        {
            if (level <= 0)
                return string.Empty;
            return temp.Substring(0, level);
        }

        private static readonly string[] VietnameseSigns = new string[]
        {
            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };

        public static string RemoveSign4VietnameseString(string str)
        {
            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi
            if (string.IsNullOrEmpty(str))
                return str;

            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)

                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }

            return str;
        }

        public static string ToStringSearch(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            return RemoveSign4VietnameseString(str.ToLower());
        }
        public static string GetFriendlyTitle(string title, bool remapToAscii = false, int maxlength = 80)
        {
            if (title == null)
            {
                return string.Empty;
            }

            int length = title.Length;
            bool prevdash = false;
            StringBuilder stringBuilder = new StringBuilder(length);
            char c;

            for (int i = 0; i < length; ++i)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    stringBuilder.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    stringBuilder.Append((char)(c | 32));
                    prevdash = false;
                }
                else if ((c == ' ') || (c == ',') || (c == '.') || (c == '/') ||
                    (c == '\\') || (c == '-') || (c == '_') || (c == '='))
                {
                    if (!prevdash && (stringBuilder.Length > 0))
                    {
                        stringBuilder.Append(' ');
                        prevdash = true;
                    }
                }
                else if (c >= 128)
                {
                    int previousLength = stringBuilder.Length;

                    if (remapToAscii)
                    {
                        stringBuilder.Append(RemapInternationalCharToAscii(c));
                    }
                    else
                    {
                        stringBuilder.Append(c);
                    }

                    if (previousLength != stringBuilder.Length)
                    {
                        prevdash = false;
                    }
                }

                if (i == maxlength)
                {
                    break;
                }
            }

            if (prevdash)
            {
                return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
            }
            else
            {
                return stringBuilder.ToString();
            }
        }

        private static string RemapInternationalCharToAscii(char character)
        {
            string s = character.ToString().ToLowerInvariant();
            if ("àåáâäãåąāáàạảãâấầậẩẫăắằặẳẵ".Contains(s))
            {
                return "a";
            }
            else if ("èéêëęéèẹẻẽêếềệểễ".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïıíìịỉĩ".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőðóòọỏõôốồộổỗơớờợởỡ".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭůúùụủũưứừựửữ".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿýỳỵỷỹ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (s.Equals('ř'))
            {
                return "r";
            }
            else if (s.Equals('ł'))
            {
                return "l";
            }
            else if ("đđ".Contains(s))
            {
                return "d";
            }
            else if (s.Equals('ß'))
            {
                return "ss";
            }
            else if (s.Equals('Þ'))
            {
                return "th";
            }
            else if (s.Equals('ĥ'))
            {
                return "h";
            }
            else if (s.Equals('ĵ'))
            {
                return "j";
            }
            else
            {
                return string.Empty;
            }
        }

        private static char[] tcvnchars = {
            'µ', '¸', '¶', '·', '¹',
            '¨', '»', '¾', '¼', '½', 'Æ',
            '©', 'Ç', 'Ê', 'È', 'É', 'Ë',
            '®', 'Ì', 'Ð', 'Î', 'Ï', 'Ñ',
            'ª', 'Ò', 'Õ', 'Ó', 'Ô', 'Ö',
            '×', 'Ý', 'Ø', 'Ü', 'Þ',
            'ß', 'ã', 'á', 'â', 'ä',
            '«', 'å', 'è', 'æ', 'ç', 'é',
            '¬', 'ê', 'í', 'ë', 'ì', 'î',
            'ï', 'ó', 'ñ', 'ò', 'ô',
            '­', 'õ', 'ø', 'ö', '÷', 'ù',
            'ú', 'ý', 'û', 'ü', 'þ',
            '¡', '¢', '§', '£', '¤', '¥', '¦'
        };

        private static char[] unichars = {
            'à', 'á', 'ả', 'ã', 'ạ',
            'ă', 'ằ', 'ắ', 'ẳ', 'ẵ', 'ặ',
            'â', 'ầ', 'ấ', 'ẩ', 'ẫ', 'ậ',
            'đ', 'è', 'é', 'ẻ', 'ẽ', 'ẹ',
            'ê', 'ề', 'ế', 'ể', 'ễ', 'ệ',
            'ì', 'í', 'ỉ', 'ĩ', 'ị',
            'ò', 'ó', 'ỏ', 'õ', 'ọ',
            'ô', 'ồ', 'ố', 'ổ', 'ỗ', 'ộ',
            'ơ', 'ờ', 'ớ', 'ở', 'ỡ', 'ợ',
            'ù', 'ú', 'ủ', 'ũ', 'ụ',
            'ư', 'ừ', 'ứ', 'ử', 'ữ', 'ự',
            'ỳ', 'ý', 'ỷ', 'ỹ', 'ỵ',
            'Ă', 'Â', 'Đ', 'Ê', 'Ô', 'Ơ', 'Ư'
        };

        private static char[] convertTable;


        #region readXml

        public static void ReadXml(string pathFile)
        {
            XmlTextReader reader = new XmlTextReader(pathFile);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        break;

                    case XmlNodeType.Text: //Display the text in each element.
                        break;

                    case XmlNodeType.EndElement: //Display the end of the element.
                        break;
                }
            }
        }

        #endregion readXml
    }
}
