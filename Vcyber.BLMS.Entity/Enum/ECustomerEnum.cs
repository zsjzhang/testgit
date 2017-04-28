using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 
    /// </summary>
    public enum ECustomerEnum
    {

    }

    /// <summary>
    /// 证件类型
    /// </summary>
    public enum ECustomerIdentificationType
    {
        /// <summary>
        /// 身份证
        /// </summary>
        [EnumDescribe("身份证")]
        IdentityCard = 1,

        /// <summary>
        /// 护照
        /// </summary>
        [EnumDescribe("护照")]
        Passport = 2,

        ///// <summary>
        ///// 驾驶证
        ///// </summary>
        //[EnumDescribe("驾驶证")]
        //DriverLicense = 3

        ///// <summary>
        ///// 警官证
        ///// </summary>
        //[EnumDescribe("警官证")]
        //PoliceOfficer = 3,

        ///// <summary>
        ///// 士兵证
        ///// </summary>
        //[EnumDescribe("士兵证")]
        //Soldier = 4,

        /// <summary>
        /// 军官证
        /// </summary>
        [EnumDescribe("军官证")]
        Officer = 3
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum ECustomerGenderType
    {
        /// <summary>
        /// 女
        /// </summary>
        [EnumDescribe("女")]
        Women = 0,

        /// <summary>
        /// 男
        /// </summary>
        [EnumDescribe("男")]
        Man = 1
    }

    /// <summary>
    /// 婚姻状况
    /// </summary>
    public enum ECustomerMarriageType
    {
        /// <summary>
        /// 未婚
        /// </summary>
        [EnumDescribe("未婚")]
        No = 0,

        /// <summary>
        /// 已婚
        /// </summary>
        [EnumDescribe("已婚")]
        Is = 1
    }

    /// <summary>
    /// 行业
    /// </summary>
    public enum ECustomerIndustry
    {
        /// <summary>
        /// 农、林、牧、渔、水利业
        /// </summary>
        [EnumDescribe("农、林、牧、渔、水利业")]
        Agricultural = 1,

        /// <summary>
        /// 工业
        /// </summary>
        [EnumDescribe("工业")]
        Industry = 2,

        /// <summary>
        /// 地质普查和勘探业
        /// </summary>
        [EnumDescribe("地质普查和勘探业")]
        Exploration = 3,

        /// <summary>
        /// 建筑业
        /// </summary>
        [EnumDescribe("建筑业")]
        Construction = 4,

        /// <summary>
        /// 交通运输业、邮电通信业
        /// </summary>
        [EnumDescribe("交通运输业、邮电通信业")]
        Transportation = 5,

        /// <summary>
        /// 商业、公共饮食业、物资供应和仓储业
        /// </summary>
        [EnumDescribe("商业、公共饮食业、物资供应和仓储业")]
        Business = 6,

        /// <summary>
        /// 房地产管理、公用事业、居民服务和咨询服务业
        /// </summary>
        [EnumDescribe("房地产管理、公用事业、居民服务和咨询服务业")]
        Consulting = 7,

        /// <summary>
        /// 卫生、体育和社会福利事业
        /// </summary>
        [EnumDescribe("卫生、体育和社会福利事业")]
        Health = 8,

        /// <summary>
        /// 教育、文化艺术和广播电视业
        /// </summary>
        [EnumDescribe("教育、文化艺术和广播电视业")]
        Education = 9,

        /// <summary>
        /// 科学研究和综合技术服务业
        /// </summary>
        [EnumDescribe("科学研究和综合技术服务业")]
        Scientific = 10,

        /// <summary>
        /// 金融、保险业
        /// </summary>
        [EnumDescribe("金融、保险业")]
        Finance = 11,

        /// <summary>
        /// 国家机关、党政机关和社会团体
        /// </summary>
        [EnumDescribe("国家机关、党政机关和社会团体")]
        Agency = 12

    }

    /// <summary>
    /// 职业
    /// </summary>
    public enum ECustomerJob
    {
        /// <summary>
        /// 娱乐、服务
        /// </summary>
        [EnumDescribe("娱乐、服务")]
        Entertainment = 1,

        /// <summary>
        /// 广告、营销、公关
        /// </summary>
        [EnumDescribe("广告、营销、公关")]
        Advertising = 2,

        /// <summary>
        /// 建筑
        /// </summary>
        [EnumDescribe("建筑")]
        Architecture = 3,

        /// <summary>
        /// 政府、军事、公共服务
        /// </summary>
        [EnumDescribe("政府、军事、公共服务")]
        Organ = 4,

        /// <summary>
        /// 教师
        /// </summary>
        [EnumDescribe("教师")]
        Teacher = 5,

        /// <summary>
        /// 教育/培训
        /// </summary>
        [EnumDescribe("教育、培训")]
        Education = 6,

        /// <summary>
        /// 旅游交通
        /// </summary>
        [EnumDescribe("旅游交通")]
        Traveling = 7,

        /// <summary>
        /// 消费品
        /// </summary>
        [EnumDescribe("消费品")]
        Consumer = 8,

        /// <summary>
        /// 电信
        /// </summary>
        [EnumDescribe("电信")]
        Telecom = 9,

        /// <summary>
        /// 能源、采矿
        /// </summary>
        [EnumDescribe("能源、采矿")]
        Energy = 10,

        /// <summary>
        /// 航天
        /// </summary>
        [EnumDescribe("航天")]
        Space = 11,

        /// <summary>
        /// 计算机网络
        /// </summary>
        [EnumDescribe("计算机网络")]
        IT = 12
    }

    /// <summary>
    /// 学历
    /// </summary>
    public enum ECustomerEducational
    {
        /// <summary>
        /// 高中以下
        /// </summary>
        [EnumDescribe("高中以下")]
        MiddleSchool = 1,

        /// <summary>
        /// 中专
        /// </summary>
        [EnumDescribe("中专")]
        Polytechnic =2,

        /// <summary>
        /// 大专
        /// </summary>
        [EnumDescribe("大专")]
        JuniorCollege = 3,

        /// <summary>
        /// 大学
        /// </summary>
        [EnumDescribe("大学")]
        University = 4,

        /// <summary>
        /// 学士
        /// </summary>
        [EnumDescribe("学士")]
        Bachelor = 5,

        /// <summary>
        /// 硕士
        /// </summary>
        [EnumDescribe("硕士")]
        Master = 6,

        /// <summary>
        /// 博士
        /// </summary>
        [EnumDescribe("博士")]
        Dr = 7
    }

    /// <summary>
    /// 主要联系方式
    /// </summary>
    public enum ECustomerMainContact
    {
        /// <summary>
        /// 未婚
        /// </summary>
        [EnumDescribe("电话")]
        Phone = 1,

        /// <summary>
        /// 已婚
        /// </summary>
        [EnumDescribe("邮件")]
        Email = 2
    }

    /// <summary>
    /// 职务
    /// </summary>
    public enum ECustomerPost
    {
        /// <summary>
        /// 普通职员
        /// </summary>
        [EnumDescribe("普通职员")]
        Clerk=1,

        /// <summary>
        /// 政府官员
        /// </summary>
        [EnumDescribe("政府官员")]
        Officer = 2,

        /// <summary>
        /// 私营企业主
        /// </summary>
        [EnumDescribe("私营企业主")]
        Entrepreneur = 3,

        /// <summary>
        /// 部门经理
        /// </summary>
        [EnumDescribe("部门经理")]
        DepartmentManager =4,

        /// <summary>
        /// 董事长
        /// </summary>
        [EnumDescribe("董事长")]
        President = 5,

        /// <summary>
        /// 高级管理人员
        /// </summary>
        [EnumDescribe("高级管理人员")]
        Management = 6
    }

}
