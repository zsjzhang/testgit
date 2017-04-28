using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common
{
   public  class FilterStr
   {

       public static readonly string FilterKeys = "or|exec|insert|select|delete|update|chr|mid|master|truncate|char|declare|join|cmd";

       public static string FilterSql(string text)
       {
           if (!string.IsNullOrEmpty(text))
           {
               string keys = FilterKeys;
               foreach (string key in keys.Split('|'))
               {
                   if ((text.ToLower().IndexOf(key) > -1) || (text.ToLower().IndexOf(key) > -1))
                   {
                       text = text.Replace(key, FormatKey(key));
                   }
               }
           }
           return text;
       }

       public static bool IsFlag<T>(T instance)
       {
           var t = typeof(T);
           foreach (var pro in t.GetProperties())
           {
               var valValue = pro.GetValue(instance, null);
               if (valValue != null && FilterSqlRedirect(valValue.ToString()))
               {
                   return true;
               }
           }
           return false;
       }

       public static bool IsFlagXss<T>(T instance)
       {
           var t = typeof(T);
           foreach (var pro in t.GetProperties())
           {
               var valValue = pro.GetValue(instance, null);
               if (valValue != null && FilterXSSRedirect(valValue.ToString()))
               {
                   return true;
               }
           }
           return false;
       }

       public static bool IsFlagXssStrSplit(string Split)
       {
           string[] arrays=Split.Split('|');
           foreach (var item in arrays)
           {
               if (FilterXSSRedirect(item))
               {
                   return true;
               }
           }
           return false;

       }

       public static bool FilterXSSRedirect(string text)
       {
           if (!string.IsNullOrEmpty(text))
           {
               string keys = FilterXss;
               foreach (string key in keys.Split('|'))
               {
                   if ((text.ToLower().IndexOf(key) > -1) || (text.ToLower().IndexOf(key) > -1))
                   {
                       return true;
                   }
               }
           }
           return false;
       }



       public static bool FilterSqlRedirect(string text)
       {
           if (!string.IsNullOrEmpty(text))
           {
               string keys = FilterKeys;
               foreach (string key in keys.Split('|'))
               {
                   if ((text.ToLower().IndexOf(key) > -1) || (text.ToLower().IndexOf(key) > -1))
                   {
                       return true;
                   }
               }
           }
           return false;
       }

       public static string FormatKey(string key)
       {
           string keyResult = key;
           foreach (char ch in key)
           {
               switch (ch)
               {
                   case 'a':
                       keyResult = keyResult.Replace(ch, 'ａ');
                       break;
                   case 'b':
                       keyResult = keyResult.Replace(ch, 'ｂ');
                       break;
                   case 'c':
                       keyResult = keyResult.Replace(ch, 'ｃ');
                       break;
                   case 'd':
                       keyResult = keyResult.Replace(ch, 'ｄ');
                       break;
                   case 'e':
                       keyResult = keyResult.Replace(ch, 'ｅ');
                       break;
                   case 'f':
                       keyResult = keyResult.Replace(ch, 'ｆ');
                       break;
                   case 'g':
                       keyResult = keyResult.Replace(ch, 'ｇ');
                       break;
                   case 'h':
                       keyResult = keyResult.Replace(ch, 'ｈ');
                       break;
                   case 'i':
                       keyResult = keyResult.Replace(ch, 'ｉ');
                       break;
                   case 'j':
                       keyResult = keyResult.Replace(ch, 'ｊ');
                       break;
                   case 'k':
                       keyResult = keyResult.Replace(ch, 'ｋ');
                       break;
                   case 'l':
                       keyResult = keyResult.Replace(ch, 'ｌ');
                       break;
                   case 'm':
                       keyResult = keyResult.Replace(ch, 'ｍ');
                       break;
                   case 'n':
                       keyResult = keyResult.Replace(ch, 'ｎ');
                       break;
                   case 'o':
                       keyResult = keyResult.Replace(ch, 'ｏ');
                       break;
                   case 'p':
                       keyResult = keyResult.Replace(ch, 'ｐ');
                       break;
                   case 'q':
                       keyResult = keyResult.Replace(ch, 'ｑ');
                       break;
                   case 'r':
                       keyResult = keyResult.Replace(ch, 'ｒ');
                       break;
                   case 's':
                       keyResult = keyResult.Replace(ch, 'ｓ');
                       break;
                   case 't':
                       keyResult = keyResult.Replace(ch, 'ｔ');
                       break;
                   case 'u':
                       keyResult = keyResult.Replace(ch, 'ｕ');
                       break;
                   case 'v':
                       keyResult = keyResult.Replace(ch, 'ｖ');
                       break;
                   case 'w':
                       keyResult = keyResult.Replace(ch, 'ｗ');
                       break;
                   case 'x':
                       keyResult = keyResult.Replace(ch, 'ｘ');
                       break;
                   case 'y':
                       keyResult = keyResult.Replace(ch, 'ｙ');
                       break;
                   case 'z':
                       keyResult = keyResult.Replace(ch, 'ｚ');
                       break;
               }
           }
           return keyResult;
       }

       public static readonly string FilterXss = "<|>|'|jAvAsCrIpT|alert|MsgBox";
        public static string FormatHTML(string str)
        {
            return str = str.Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "‘").Replace("jAvAsCrIpT", "").Replace("alert", "").Replace("MsgBox","");
        }
        public static string GetHTML(string str)
        {
            return str.Replace("&lt;", "<").Replace("&gt;", ">").Replace("‘", "'");
        }        
    }
}
