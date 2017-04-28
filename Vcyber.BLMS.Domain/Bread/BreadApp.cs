using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 面包业务逻辑
    /// </summary>
    public class BreadApp : IBreadApp
    {
        #region ==== 构造函数 ====

        public BreadApp() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 蓝豆面包
        /// </summary>
        /// <param name="ruleType">篮豆规则类型</param>
        /// <param name="userId">用户Id</param>
        /// <param name="level">用户级别</param>
        /// <param name="blueBeanValue">获得的蓝豆值</param>
        /// <returns>true:成功</returns>
        public bool BlueBeanBread(EBRuleType ruleType, string userId, MemshipLevel level, out int blueBeanValue)
        {
            blueBeanValue = 0;
            //BlueBeanRule ruleData = _DbSession.BlueBeanRuleStorager.SelecRuletOne(ruleType);

            //if (ruleData != null)
            //{
            //    BlueBeanCondition condition = new BlueBeanCondition((EAcquireMode)ruleData.AcquireMode);
            //    int count = _DbSession.UserBlueBeanStorager.Count(ruleType, userId, condition);

            //    if (ruleData.MaxCount == 0 || count < ruleData.MaxCount)//0:表示无次数限制
            //    {
            //        if (ruleData.RuleUsers != null && ruleData.RuleUsers.Count() > 0)
            //        {
            //            EBUserGrade grade = this.ConvertUserGrade(level);
            //            var ruleUsers = ruleData.RuleUsers.Where<BlueBeanRuleUser>((d) => { return d.UserGrade == grade; });

            //            if (ruleUsers != null && ruleUsers.Count() > 0)
            //            {
            //                BlueBeanRuleUser ruleUser = ruleUsers.ToList<BlueBeanRuleUser>()[0];
            //                _DbSession.UserBlueBeanStorager.Add(new UserblueBean() { CreateTime = DateTime.Now, integralSource = ruleType.ToInt32().ToString(), UpdateTime = DateTime.Now, remark = ruleData.Remark, userId = userId, value = ruleUser.BlueBeanValue });
            //                blueBeanValue = ruleUser.BlueBeanValue;
            //                return true;
            //            }
            //        }
            //    }
            //}

            return true;
        }

        /// <summary>
        /// 经验值面包
        /// </summary>
        /// <param name="ruleType">经验值规则类型</param>
        /// <param name="userId"></param>
        /// <param name="empiricValue">获得的经验值</param>
        /// <returns>true:成功</returns>
        public bool EmpiricBread(EEmpiricRule ruleType, string userId, out int empiricValue)
        {
            empiricValue = 0;
            //UserEmpiricRule ruleData = _DbSession.UserEmpiricRuleStorager.SelectOne(ruleType);

            //if (ruleData != null)
            //{
            //    UserEmpiricCondition condition = new UserEmpiricCondition((EAcquireMode)ruleData.AcquireMode);
            //    int count = _DbSession.UserEmpiricStorager.Count(ruleType, userId, condition);

            //    if (ruleData.MaxCount == 0 || count < ruleData.MaxCount)//0:表示无次数限制
            //    {
            //        _DbSession.UserEmpiricStorager.Add(new UserEmpiricRecord() { CreateTime = DateTime.Now, Remark=ruleType.ToString(), DataState=0, SourceId=ruleType.ToInt32().ToString(), UserId=userId, Value=ruleData.Value, UseValue=0 });
            //        empiricValue = ruleData.Value;
            //        return true;
            //    }
            //}

            return true;
        }

        /// <summary>
        /// 会员是否可以获取经验值面包
        /// </summary>
        /// <param name="ruleType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsEmiricBread(EEmpiricRule ruleType, string userId,out int count)
        {
            count = 0;
            UserEmpiricRule ruleData = _DbSession.UserEmpiricRuleStorager.SelectOne(ruleType);

            if (ruleData != null)
            {
                UserEmpiricCondition condition = new UserEmpiricCondition((EAcquireMode)ruleData.AcquireMode);
                count = _DbSession.UserEmpiricStorager.Count(ruleType, userId, condition);

                if (ruleData.MaxCount == 0 || count < ruleData.MaxCount)//0:表示无次数限制
                {
                    count = count;
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region ==== 私有方法 ====

        private EBUserGrade ConvertUserGrade(MemshipLevel level)
        {
            EBUserGrade grade = EBUserGrade.YX;

            switch (level)
            {
                case MemshipLevel.OneStar: grade = EBUserGrade.YX;
                    break;
                case MemshipLevel.CommonCard: grade = EBUserGrade.pk;
                    break;
                case MemshipLevel.SilverCard: grade = EBUserGrade.yk;
                    break;
                case MemshipLevel.GoldCard: grade = EBUserGrade.jk;
                    break;
                default:
                    break;
            }

            return grade;
        }

        #endregion
    }
}
