using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.FrontWeb.Models
{
    /// <summary>
    /// 用户基本信息
    /// </summary>
    public class UserBaseInfoV
    {
        #region ==== 构造函数 ====

        public UserBaseInfoV() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public string BirthTime { get; set; }

        /// <summary>
        /// 爱好（多个爱好已“;”分割）
        /// </summary>
        public string Hobby { get; set; }

        /// <summary>
        /// 性别（1=女、2=男、3=保密、4=其他）
        /// </summary>
        public int Sex { get; set; }
        
        #endregion

        #region ==== 公共方法 ====

        public bool ValidatePara(out string message)
        {
            message = string.Empty;

            DateTime birthTime;

            if (!DateTime.TryParse(this.BirthTime,out birthTime)||birthTime>DateTime.Now.AddYears(-18))
            {
                message = "出生日期不能小于18岁。";
                return false;
            }

            if (string.IsNullOrEmpty(this.Hobby))
            {
                message = "爱好必填";
                return false;
            }

            if (this.Sex!=1&&this.Sex!=2&&this.Sex!=3)
            {
                message = "性别必填";
                return false;
            }

            return true;
        }

        #endregion
    }
}