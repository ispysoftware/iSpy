using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Drawing;
using System.Linq;
using System.Text;

namespace iSpyApplication
{
    public static class Extensions
    {
        public static Uri SetPort(this Uri uri, int newPort)
        {
            var builder = new UriBuilder(uri) { Port = newPort };
            return builder.Uri;
        }
        public static string ToBase64(this IEnumerable<byte> bytes, bool insertLineBreaks = false)
        {
            if (bytes == null)
            {
                return null;
            }
            var opt = insertLineBreaks ? Base64FormattingOptions.InsertLineBreaks : Base64FormattingOptions.None;
            return Convert.ToBase64String(bytes.ToArray(), opt);
        }
        public static string ToBase64(this byte[] bytes, bool insertLineBreaks = false)
        {
            if (bytes == null)
            {
                return null;
            }
            var opt = insertLineBreaks ? Base64FormattingOptions.InsertLineBreaks : Base64FormattingOptions.None;
            return Convert.ToBase64String(bytes, opt);
        }
        public static byte[] FromBase64(this string base64)
        {
            if (base64 == null)
            {
                return null;
            }
            return Convert.FromBase64String(base64);
        }
        public static byte[] ToUtf8(this string str)
        {
            if (str == null)
            {
                return null;
            }
            return Encoding.UTF8.GetBytes(str);
        }
        public static string FromUtf8(this byte[] utf8)
        {
            if (utf8 == null)
            {
                return null;
            }
            return Encoding.UTF8.GetString(utf8);
        }
        public static string FromUtf8(this byte[] utf8, int index, int count)
        {
            return Encoding.UTF8.GetString(utf8, index, count);
        }
        public static string FromUtf8(this byte[] utf8, int count)
        {
            return Encoding.UTF8.GetString(utf8, 0, count);
        }

        public static byte[] ToAscii(this string str)
        {
            if (str == null)
            {
                return null;
            }
            return Encoding.ASCII.GetBytes(str);
        }
        public static string FromAscii(this byte[] ascii)
        {
            if (ascii == null)
            {
                return null;
            }
            return Encoding.ASCII.GetString(ascii);
        }


        private static readonly Dictionary<string, Color> Colours = new Dictionary<string, Color>();

        public static bool IsValidEmail(this string email)
        {
            var message = new MailMessage();
            bool f = false;
            try
            {
                message.To.Add(email); //use built in validator
            }
            catch
            {
                f = true;
            }
            message.Dispose();
            return !f;
        }

        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search, StringComparison.Ordinal);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static string JsonSafe(this string text)
        {
            if (text == null) text = "";
            return text.Replace(@"\", @"\\").Replace("\"", "\\\"");
        }

        public static Color ToColor(this string colorRGB)
        {
            if (Colours.ContainsKey(colorRGB))
                return Colours[colorRGB];

            string[] cols = colorRGB.Split(',');
            var c = Color.FromArgb(Convert.ToInt16(cols[0]), Convert.ToInt16(cols[1]), Convert.ToInt16(cols[2]));
            
            try
            {
                Colours.Add(colorRGB, c);
            }
            catch
            {
                //multiple threads can add colours in simultaneously
            }
            
            return c;

        }

        public static String ToRGBString(this Color color)
        {
            return color.R + "," + color.G + "," + color.B;
        }

        public static bool Has<T>(this Enum type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }

        public static bool Is<T>(this Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }


        public static T Add<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    $"Could not append value from enumerated type '{typeof (T).Name}'.", ex);
            }
        }


        public static T Remove<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    $"Could not remove value from enumerated type '{typeof (T).Name}'.", ex);
            }
        }
    }

}