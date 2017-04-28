using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common 
{
    /// <summary>
    /// 枚举扩张
    /// </summary>
    public static class EnumExtension
    { 
        public static string GetDiscribe<T>(this T instance)
        {
            System.Reflection.FieldInfo fieldInfo = instance.GetType().GetField(instance.ToString());
            if (fieldInfo != null)
            {
                object[] attris = fieldInfo.GetCustomAttributes(typeof (EnumDescribeAttribute), false);

                if (attris != null && attris.Length > 0 && attris[0].GetType() == typeof (EnumDescribeAttribute))
                {
                    return ((EnumDescribeAttribute) attris[0]).Describe;
                }
            }

            return string.Empty;
        }
    }
}
