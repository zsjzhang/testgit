using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 用户基础数据
    /// </summary>
    public class UserBaseData
    {
        #region ==== 构造函数 ====

        public UserBaseData() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 用户Guid
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 性别（1=女、2=男、3=保密、4=其他）
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime BirthTime { get; set; }

        /// <summary>
        /// 爱好编号
        /// </summary>
        public List<string> Hobbys { get; set; }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 转换爱好数据类型
        /// </summary>
        /// <returns></returns>
        public string ConvertHobbys()
        {
            if (this.Hobbys != null && this.Hobbys.Count > 0)
            {
                return string.Join(";", this.Hobbys);
            }

            return string.Empty;
        }

        #endregion
    }
}
