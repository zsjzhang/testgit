using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{

    public class SSIQuestion
    {

        public string section_name { get; set; }

        public string section_tel { get; set; }

        public string section_car { get; set; }

        public string section_color { get; set; }

        public string N1 { get; set; }
        public string N2 { get; set; }
        public string N3 { get; set; }
        public string N4 { get; set; }
        public string N5 { get; set; }
        public string N6 { get; set; }
        public string N7 { get; set; }
        public string N8 { get; set; }
        public string N9 { get; set; }
        public string N10 { get; set; }

        public string Q1 {
            get { return "请问您进4S店后2分钟内是否有销售人员主动接待"; }
        }
        public string Q2
        {
            get { return "请问看车时，销售顾问是否向您演示新车上的各项功能，并邀请您动手操作"; }
        }
        public string Q3
        {
            get { return "请问销售顾问是否用纸质或其他形式的报价单向您清楚的说明价格组成情况"; }
        }
        public string Q4
        {
            get { return "请问您是否在购车的4S店处试乘试驾过您买的同种车型"; }
        }
        public string Q5
        {
            get { return "请问销售顾问是否主动介绍了衍生产品，如保险、精品、信贷、延保业务"; }
        }
        public string Q6
        {
            get { return "请问您到店提车时，是否有服务顾问帮助您了解后续保养维修事宜"; }
        }
        public string Q7
        {
            get { return "请问4S店是否为您举行了交车仪式"; }
        }
        public string Q8
        {
            get { return "请问您到店提车时，销售顾问是否向您介绍了快速入门手册、车辆各项功能的操作方法"; }
        }
        public string Q9
        {
            get { return "请问4S店是否介绍了会员俱乐部政策并推荐入会"; }
        }
        public string Q10
        {
            get { return "请问在您提车回家后，4S店是否在24小时之内向您致电，并询问车辆的使用情况"; }
        }
    }


    public class Questionnaire
    {
        #region ==== 构造函数 ====
        public Questionnaire() { }
        #endregion

        #region ==== 公用属性 ====
        public int Id { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// 备用标题
        /// </summary>
        public string AlternateTheme { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// 该问卷开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 该问卷结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 该问卷是第几期
        /// </summary>
        public int PeriodicalCount { get; set; }

        /// <summary>
        /// 完成该问卷可获得的蓝豆数量
        /// </summary>
        public int BlueBeanCount { get; set; }

        /// <summary>
        /// 该问卷当前状态：0 删除，1 未开始，2 已开始，3 已结束
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 问卷创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 主题图片路径
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 首页图片
        /// </summary>
        public string SyImage { get; set; }

        /// <summary>
        /// 往期问卷列表页的说明文字
        /// </summary>
        public string LbRemarks { get; set; }

        /// <summary>
        /// 问卷类型 
        /// </summary>
        public int QType { get; set; }
        #endregion
    }
}
