using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 问卷访客表
    /// </summary>
    public class QuestionnaireVisitor
    {
        public QuestionnaireVisitor()
        { }

        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string VName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Provency { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 县区
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 邮寄地址
        /// </summary>
        public string MailAddress { get; set; }

        /// <summary>
        /// 问卷id
        /// </summary>
        public int QuestionnaireId { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }


        /// <summary>
        /// 学历
        /// </summary>
        public string Education { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        public string CarModels { get; set; }

        /// <summary>
        /// 问卷答案提交时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 数据来源
        /// </summary>
        public string VSource { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 是否为会员
        /// </summary>
        public bool IsMember { get; set; }

        #region ==== 外接字段 ====
        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IdentityNumber { get;set;}

        /// <summary>
        /// 会员等级
        /// </summary>
        public int MLevel { get; set; }

        /// <summary>
        /// 注册来源
        /// </summary>
        public string CreatedPerson { get; set; }
        #endregion
    }
}
