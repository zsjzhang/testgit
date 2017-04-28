using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    public class WXUpdateMembershipModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string FaceImage { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string GenderName { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 兴趣
        /// </summary>
        public List<string> Interest { get; set; }

        /// <summary>
        /// Vin
        /// </summary>
        public string Vin { get; set; }


        #region ==== 附件属性 ====

        /// <summary>
        /// 证件类型（身份证 = 1, 护照 = 2,  军官证 = 3）
        /// </summary>
        public string NPaperWork
        {
            get;
            set;
        }

        /// <summary>
        /// 学历（其他=-1，高中以下 = 1, 中专 =2, 大专 = 3, 大学 = 4,学士 = 5, 硕士 = 6, 博士=7）
        /// </summary>
        public string NEducational
        {
            get;
            set;
        }
        /// <summary>
        /// 职业（其他=-1，娱乐、服务 = 1, 广告、营销、公关= 2, 建筑 = 3,政府、军事、公共服务 = 4, 教师= 5, 教育/培训= 6, 旅游交通 = 7,消费品 = 8, 电信 = 9, 能源、采矿 = 10, 航天 = 11, 计算机网络 = 12）
        /// </summary>
        public string NJob
        {
            get;
            set;
        }
        /// <summary>
        /// 职位（ 其他=-1， 普通职员=1,政府官员 = 2, 私营企业主 = 3, 部门经理 =4, 董事长 = 5, 高级管理人员 = 6）
        /// </summary>
        public string NOffice
        {
            get;
            set;
        }

        /// <summary>
        /// 所属行业
        /// </summary>
        public string NIndustry
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string NRemark
        {
            get;
            set;
        }
        /// <summary>
        /// 婚姻状况（0：未婚，1：已婚）
        /// </summary>
        public string NIsMarriage
        {
            get;
            set;
        }
        /// <summary>
        /// 婚姻纪念日
        /// </summary>
        public string NMarriageDay
        {
            get;
            set;
        }
        /// <summary>
        /// 主要联系方式（1：电话；2：邮件）
        /// </summary>
        public string NMainContact
        {
            get;
            set;
        }
        /// <summary>
        /// 主要联系电话
        /// </summary>
        public string NMainTelePhone
        {
            get;
            set;
        }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string NOrganizationCode
        {
            get;
            set;
        }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Provency { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 家庭电话
        /// </summary>
        public string NTelePhone { get; set; }

        /// <summary>
        /// 购车时间
        /// </summary>
        public string NTransactionTime { get; set; }

        #endregion
    }
}