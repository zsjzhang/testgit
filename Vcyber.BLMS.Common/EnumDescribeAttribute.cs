using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common 
{
    /// <summary>
    /// 枚举描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum|AttributeTargets.Field|AttributeTargets.Property,Inherited=false,AllowMultiple=false)]
    public class EnumDescribeAttribute:Attribute
    {

        #region ==== 构造函数 ====

        public EnumDescribeAttribute()
        { }

        public EnumDescribeAttribute(string discribe)
        {
            this.Describe = discribe;
        }

        #endregion

        #region ==== 公共属性 ====

        public string Describe { get; set; }

        #endregion
    }
}
