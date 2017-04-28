using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity.SelectCondition;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 用户蓝豆操作
    /// </summary>
    public class UserBlueBeanStorager : IUserBlueBeanStorager
    {
        #region ==== 私有字段 ====

        /// <summary>
        /// 用户蓝豆过期清理时间
        /// </summary>
        private DateTime cleanTime = DateTime.MaxValue;

        #endregion

        #region ==== 构造函数 ====

        public UserBlueBeanStorager() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加用户蓝豆
        /// </summary>
        /// <param name="data"></param>
        public void Add(UserblueBean data)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into userblueBean(userId,integralSource,value,usevalue,datastate,remark,CreateTime,UpdateTime)");
            sql.Append(" values(@userId,@integralSource,@value,@usevalue,@datastate,@remark,@CreateTime,@UpdateTime)");
            DbHelp.Execute(sql.ToString(), data);
        }

        /// <summary>
        /// 获取用户总蓝豆
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetTotalBlueBean(string userId)
        {
            //this.CleanBlueBean(userId);

            StringBuilder sql = new StringBuilder();
            sql.Append(" select isnull(SUM(ISNULL(value,0)-ISNULL(usevalue,0)),0) from userblueBean ");
            sql.Append(" where userblueBean.userId=@userId and userblueBean.datastate=@datastate and userblueBean.CreateTime<@Time");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), new { userId = userId, datastate = EDataStatus.NoDelete.ToInt32(), Time = this.cleanTime });
        }

        /// <summary>
        /// 获取用户全部蓝豆记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<UserblueBean> SelectAll(string userId)
        {
            //this.CleanBlueBean(userId);

            StringBuilder sql = new StringBuilder();
            sql.Append("select * from userblueBean where userId=@userId order by userblueBean.CreateTime asc");
            return DbHelp.Query<UserblueBean>(sql.ToString(), new { userId = userId });
        }

        /// <summary>
        /// 分页获取用户蓝豆
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<UserblueBean> SelectAll(string userId, PageData pageData, out int total)
        {
            //this.CleanBlueBean(userId);

            StringBuilder sql = new StringBuilder();
            sql.Append(" select count(1) from userblueBean where userId=@userId ");
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { userId = userId });
            sql.Clear();

            sql.AppendFormat(" select top {0} * from userblueBean where userId=@userId", pageData.Size);
            sql.Append(" and userblueBean.ID not in( ");
            sql.AppendFormat(" select top {0} userblueBean.ID from userintegral where userId=@userId order by userblueBean.CreateTime asc", pageData.Index);
            sql.Append(" )order by userblueBean.CreateTime asc ");
            return DbHelp.Query<UserblueBean>(sql.ToString(), new { userId = userId });
        }

        /// <summary>
        /// 获取用户有效蓝豆
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<UserblueBean> SelectList(string userId)
        {
            // this.CleanBlueBean(userId);

            StringBuilder sql = new StringBuilder();
            sql.Append(" select * from userblueBean where userId=@userId and userblueBean.datastate=@datastate and  (ISNULL(value,0)-ISNULL(usevalue,0))>0");
            sql.Append(" and userblueBean.CreateTime<@Time order by userblueBean.CreateTime asc ");
            return DbHelp.Query<UserblueBean>(sql.ToString(), new { userId = userId, datastate = EDataStatus.NoDelete.ToInt32(), Time = DateTime.MaxValue });
        }

        /// <summary>
        /// 减去用户蓝豆
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SubBlueBean(int id, string userId, int subValue)
        {
            //this.CleanBlueBean(userId);

            if (subValue <= 0)
            {
                return false;
            }

            StringBuilder sql = new StringBuilder();
            sql.Append(" update userblueBean set usevalue=ISNULL(usevalue,0)+@subvalue where userId=@userId and ID=@Id");
            sql.Append(" and (ISNULL(value,0)-ISNULL(usevalue,0))>=@subvalue");
            return DbHelp.Execute(sql.ToString(), new { subvalue = subValue, userId = userId, Id = id }) > 0;
        }

        /// <summary>
        /// 统计用户获取蓝豆的次数
        /// </summary>
        /// <param name="ruleType">蓝豆规则类型</param>
        /// <param name="userId">用户Id</param>
        /// <param name="condition">统计条件</param>
        /// <returns></returns>
        public int Count(EBRuleType ruleType, string userId, BlueBeanCondition condition)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select count(1) from userblueBean where userblueBean.userId=@userId and userblueBean.integralSource=@integralSource");
            sql.AppendFormat(" and {0}", condition.ToWhere());
            return DbHelp.ExecuteScalar<int>(sql.ToString(), new { userId = userId, integralSource = ruleType.ToInt32().ToString() });
        }

        /// <summary>
        /// 清理用户过期蓝豆
        /// </summary>
        /// <param name="userId"></param>
        public void CleanBlueBean(string userId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update userblueBean set datastate=@datastate where userblueBean.userId=@userId and datename(mm,userblueBean.CreateTime)>@Time");
            DbHelp.Execute(sql.ToString(), new { datastate = EDataStatus.NoDelete.ToInt32(), userId = userId, Time = 12 });
        }

        public void CleanBlueBean(IEnumerable<UserblueBean> blBeans)
        {
            StringBuilder sb=new StringBuilder();
            foreach (var item in blBeans)
            {
                sb.AppendFormat("update userblueBean set usevalue={0} ,UpdateTime='{2}' where ID={1};",
                    item.usevalue, item.Id, DateTime.Now);
            }
            DbHelp.Execute(sb.ToString());
        }


        #endregion
    }
}
