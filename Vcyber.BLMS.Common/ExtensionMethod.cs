using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common
{
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using System.Web.Mvc;

    public static class ExtensionMethod
    {
        /// <summary>
        ///如果传入参数为0、null、空字符串、空格则 返回 空字符串，否则返回原来的值
        /// </summary>
        /// <param name="convert"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToEmpty(this string s, object obj)
        {
            string strObj = obj + string.Empty;

            if (string.IsNullOrWhiteSpace(strObj) || strObj == "0")
            {
                return string.Empty;
            }
            else
            {
                return strObj;
            }
        }



        public static SelectList ToSelectList<T>(this T enumeration, string selected)
        {
            var source = Enum.GetValues(typeof(T));

            var items = new Dictionary<object, string>();

            var displayAttributeType = typeof(DisplayAttribute);

            foreach (var value in source)
            {
                FieldInfo field = value.GetType().GetField(value.ToString());

                DisplayAttribute attrs =
                    (DisplayAttribute)field.GetCustomAttributes(displayAttributeType, false).FirstOrDefault();

                items.Add(value, attrs != null ? attrs.GetName() : value.ToString());
            }

            return new SelectList(items, "Key", "Value", selected);
        }

        public static string DisplayName(this Enum value)
        {
            Type enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            MemberInfo member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            var outString = ((DisplayAttribute)attrs[0]).Name;

            if (((DisplayAttribute)attrs[0]).ResourceType != null)
            {
                outString = ((DisplayAttribute)attrs[0]).GetName();
            }

            return outString;
        }


        public static List<SelectListItem> ToSelectList<T>(this List<T> Items, Func<T, string> getKey, Func<T, string> getValue, string selectedValue, string noSelection, bool search = false)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            if (search)
            {
                items.Add(new SelectListItem { Selected = true, Value = "-1", Text = string.Format("-- {0} --", noSelection) });
            }

            foreach (var item in Items)
            {
                items.Add(new SelectListItem
                {
                    Text = getKey(item),
                    Value = getValue(item),
                    Selected = selectedValue == getValue(item)
                });
            }

            return items
                .OrderBy(l => l.Text)
                .ToList();
        }

        public static string GetErrors(this System.Web.Http.ModelBinding.ModelStateDictionary ms)
        {
            return ms.Values.SelectMany(modelState => modelState.Errors).Aggregate(string.Empty, (current, error) => current + ((string.IsNullOrEmpty(error.ErrorMessage)) ? error.Exception.Message : error.ErrorMessage) + ";");
        }

        public static TD Copy<TS, TD>(this TS instance)
            where TS : class,new()
            where TD : class,new()
        {
            Type sType = typeof(TS);
            Type dType = typeof(TD);
            TD td = new TD();

            PropertyInfo[] spros = sType.GetProperties();
            PropertyInfo[] dpros = dType.GetProperties();

            foreach (PropertyInfo ditem in dpros)
            {
                foreach (PropertyInfo sitem in spros)
                {
                    if (ditem.Name.Equals(sitem.Name)&&ditem.GetType()==sitem.GetType())
                    {
                        ditem.SetValue(td, sitem.GetValue(instance));
                    }
                }
            }

            return td;
        }
    }
}
