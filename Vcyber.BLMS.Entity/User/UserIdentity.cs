using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 用户身份认证信息
    /// </summary>
    public class UserIdentity
    {
        #region ==== 构造函数 ====

        public UserIdentity() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户身份证编号
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 用户证件有效时间
        /// </summary>
        public DateTime IdentityValidTime { get; set; }

        /// <summary>
        /// 身份证图片
        /// </summary>
        public List<string> IdentityImgs { get; set; }

        /// <summary>
        /// 身份是否确认（1=确认、2=未确认）
        /// </summary>
        public int IdentityConfirmed { get; set; }

        #endregion

        #region ==== 公共方法 ====

        public string ConvertImg()
        {
            if (this.IdentityImgs!=null&&this.IdentityImgs.Count>0)
            {
                return string.Join(";",this.IdentityImgs);
            }

            return string.Empty;
        }

        #endregion
    }
}
