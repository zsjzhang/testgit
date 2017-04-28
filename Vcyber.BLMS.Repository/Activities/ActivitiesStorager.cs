using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class ActivitiesStorager : IActivitiesStorager
    {
        public IEnumerable<Activities> Select(int? approveStatus, int? displayStatus, int? isHot, string dealer, int pageIndex, int pageSize, out int totalCount)
        {
            StringBuilder conditionStr = new StringBuilder();
            if (!string.IsNullOrEmpty(dealer))
            {
                conditionStr.Append("and Dealer=@dealer");
            }

            if (approveStatus != null && approveStatus >= 0)
            {

                conditionStr.AppendFormat(" and IsApproved={0}", approveStatus);

            }

            if (displayStatus != null && displayStatus >= 0)
            {

                conditionStr.AppendFormat(" and IsDisplay={0}", displayStatus);

            }
            if (isHot != null && isHot >= 0)
            {

                conditionStr.AppendFormat(" and IsHot={0}", isHot);

            }

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(1) from Activities where IsDeleted=0 ");
            sql.Append(conditionStr);
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString(), new { @dealer = dealer });
            sql.Clear();

            sql.AppendFormat("SELECT top {0} * FROM Activities WHERE IsDeleted=0", pageSize);
            sql.Append(conditionStr);
            sql.AppendFormat(" and id not in (select top {0} id from Activities WHERE IsDeleted=0", pageIndex);
            sql.Append(conditionStr);
            sql.Append(" ORDER BY Priority desc,CreateTime DESC) ORDER BY Priority desc,CreateTime DESC");
            return DbHelp.Query<Activities>(sql.ToString(), new { @dealer = dealer });
        }
        /// <summary>
        /// 查询未开始的活动
        /// </summary>
        /// <param name="approveStatus"></param>
        /// <param name="displayStatus"></param>
        /// <param name="isHot"></param>
        /// <param name="dealer"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<Activities> SelectNotStart(int? approveStatus, int? displayStatus, int? isHot, string dealer, int pageIndex, int pageSize, out int totalCount)
        {
            StringBuilder conditionStr = new StringBuilder();
            if (!string.IsNullOrEmpty(dealer))
            {
                conditionStr.AppendFormat("and Dealer='{0}'", dealer);
            }

            if (approveStatus != null && approveStatus >= 0)
            {

                conditionStr.AppendFormat(" and IsApproved={0}", approveStatus);

            }
            if (displayStatus != null && displayStatus >= 0)
            {

                conditionStr.AppendFormat(" and IsDisplay={0}", displayStatus);

            }

            if (isHot != null && isHot >= 0)
            {

                conditionStr.AppendFormat(" and IsHot={0}", isHot);

            }
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(1) from Activities where IsDeleted=0 and BeginTime>'{0}'", DateTime.Now);
            sql.Append(conditionStr);
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());
            sql.Clear();

            sql.AppendFormat("SELECT top {0} * FROM Activities WHERE IsDeleted=0 and BeginTime>'{1}'", pageSize, DateTime.Now);
            sql.Append(conditionStr);
            sql.AppendFormat(" and id not in (select top {0} id from Activities WHERE IsDeleted=0 and BeginTime>'{1}'", pageIndex, DateTime.Now);
            sql.Append(conditionStr);
            sql.Append("ORDER BY Priority desc,CreateTime DESC) ORDER BY Priority desc,CreateTime DESC");
            return DbHelp.Query<Activities>(sql.ToString());
        }
        /// <summary>
        /// 查询进行中的活动
        /// </summary>
        /// <param name="approveStatus"></param>
        /// <param name="displayStatus"></param>
        /// <param name="isHot"></param>
        /// <param name="dealer"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<Activities> SelectInProgress(int? approveStatus, int? displayStatus, int? isHot, string dealer, int pageIndex, int pageSize, out int totalCount)
        {
            StringBuilder conditionStr = new StringBuilder();
            if (!string.IsNullOrEmpty(dealer))
            {
                conditionStr.AppendFormat("and Dealer='{0}'", dealer);
            }

            if (approveStatus != null && approveStatus >= 0)
            {

                conditionStr.AppendFormat(" and IsApproved={0}", approveStatus);

            }
            if (displayStatus != null && displayStatus >= 0)
            {

                conditionStr.AppendFormat(" and IsDisplay={0}", displayStatus);

            }

            if (isHot != null && isHot >= 0)
            {

                conditionStr.AppendFormat(" and IsHot={0}", isHot);

            }

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(1) from Activities where IsDeleted=0 and BeginTime<='{0}' and EndTime>='{0}'", DateTime.Now);
            sql.Append(conditionStr);
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());
            sql.Clear();

            sql.AppendFormat("SELECT top {0} * FROM Activities WHERE IsDeleted=0 and BeginTime<='{1}' and EndTime>='{1}'", pageSize, DateTime.Now);
            sql.Append(conditionStr);
            sql.AppendFormat(" and id not in (select top {0} id from Activities WHERE IsDeleted=0 and BeginTime<='{1}' and EndTime>='{1}'", pageIndex, DateTime.Now);
            sql.Append(conditionStr);
            sql.Append("ORDER BY Priority desc,CreateTime DESC) ORDER BY Priority desc,CreateTime DESC");
            return DbHelp.Query<Activities>(sql.ToString());
        }
        /// <summary>
        /// 查询已结束的活动
        /// </summary>
        /// <param name="approveStatus"></param>
        /// <param name="displayStatus"></param>
        /// <param name="isHot"></param>
        /// <param name="dealer"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<Activities> SelectFinished(int? approveStatus, int? displayStatus, int? isHot, string dealer, int pageIndex, int pageSize, out int totalCount)
        {
            StringBuilder conditionStr = new StringBuilder();
            if (!string.IsNullOrEmpty(dealer))
            {
                conditionStr.AppendFormat("and Dealer='{0}'", dealer);
            }

            if (approveStatus != null && approveStatus >= 0)
            {

                conditionStr.AppendFormat(" and IsApproved={0}", approveStatus);

            }
            if (displayStatus != null && displayStatus >= 0)
            {

                conditionStr.AppendFormat(" and IsDisplay={0}", displayStatus);

            }
            if (isHot != null && isHot >= 0)
            {

                conditionStr.AppendFormat(" and IsHot={0}", isHot);

            }

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(1) from Activities where IsDeleted=0 and EndTime<'{0}'", DateTime.Now);
            sql.Append(conditionStr);
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());
            sql.Clear();

            sql.AppendFormat("SELECT * FROM Activities WHERE IsDeleted=0 and EndTime<'{1}'", totalCount, DateTime.Now);
            sql.Append(conditionStr);
            sql.Append(" ORDER BY Priority desc,CreateTime DESC");

            return DbHelp.Query<Activities>(sql.ToString());
        }


        public Activities GetActivitiesById(int id)
        {
            string sql = "Select * From Activities WHERE Id=@Id and IsDeleted=0";
            return DbHelp.QueryOne<Activities>(sql, new { @Id = id });
        }

        public int AddActivities(Activities activities)
        {
            string sql = "Insert into Activities(Title,MajorImageUrl,Content,SignUp,BeginTime,EndTime,CreateTime,CreateBy,UpdateTime,UpdateBy,IsDeleted,Summary,IsUrl,Url,IsCarOwner,Priority,IsApproved,Dealer,IsHot,IsDisplay) values(@Title,@MajorImageUrl,@Content,@SignUp,@BeginTime,@EndTime,@CreateTime,@CreateBy,@UpdateTime,@UpdateBy,@IsDeleted,@Summary,@IsUrl,@Url,@IsCarOwner,@Priority,@IsApproved,@Dealer,@IsHot,@IsDisplay);SELECT @@identity";
            return DbHelp.ExecuteScalar<int>(sql, new
            {
                activities.Title,
                activities.MajorImageUrl,
                activities.Content,
                activities.SignUp,
                activities.BeginTime,
                activities.EndTime,
                @CreateTime = DateTime.Now,
                activities.CreateBy,
                @UpdateTime = DateTime.Now,
                activities.UpdateBy,
                @IsDeleted = 0,
                activities.Summary,
                activities.IsUrl,
                activities.Url,
                activities.IsCarOwner,
                activities.Priority,
                @IsApproved = (int)EApproveStatus.NoBegin,
                activities.Dealer,
                activities.IsHot,
                activities.IsDisplay
            });
        }

        public bool UpdateActivities(Activities activities)
        {
            string sql = "Update Activities set Title =@Title,MajorImageUrl=@MajorImageUrl,Content=@Content,SignUp=@SignUp,BeginTime=@BeginTime," +
                         "EndTime=@EndTime,UpdateTime=@UpdateTime,UpdateBy=@UpdateBy, " +
                         "IsApproved = @IsApproved,Summary=@Summary,IsUrl=@IsUrl,Url=@Url,IsCarOwner=@IsCarOwner," +
                         "Priority=@Priority,IsHot=@IsHot where Id=@Id and IsDeleted=0";

            var approvestatus = activities.IsApproved;
            var apporveBy = activities.ApprovedBy;
            var approveTime = activities.ApprovedTime;

            if (approvestatus != (int)EApproveStatus.NoBegin)
            {
                approvestatus = (int)EApproveStatus.NoBegin;
                apporveBy = activities.UpdateBy;
                approveTime = DateTime.Now;
            }
            if (approveTime <= DateTime.MinValue)
            {
                approveTime = (DateTime)SqlDateTime.MinValue;
            }
            return DbHelp.Execute(sql, new
            {
                activities.Title,
                activities.MajorImageUrl,
                activities.Content,
                activities.SignUp,
                activities.BeginTime,
                activities.EndTime,
                @UpdateTime = DateTime.Now,
                activities.UpdateBy,
                activities.Id,
                @IsApproved = approvestatus,
                @ApprovedBy = apporveBy,
                @ApprovedTime = approveTime,
                activities.Summary,
                activities.IsUrl,
                activities.Url,
                activities.IsCarOwner,
                activities.Priority,
                activities.IsHot
            }) > 0;

        }

        public bool ApprovedActivities(int id, string userId, int status)
        {
            string sql = "Update Activities set IsApproved = @IsApproved,ApprovedBy=@ApprovedBy,ApprovedTime=@ApprovedTime,UpdateTime=@UpdateTime where Id=@Id and IsDeleted=0";
            return DbHelp.Execute(sql, new { @Id = id, @IsApproved = status, @ApprovedBy = userId, @ApprovedTime = DateTime.Now, @UpdateTime = DateTime.Now }) > 0;

        }

        public bool DeleteActivities(int id, string name)
        {
            string sql = "Update Activities set IsDeleted = 1,UpdateBy=@UpdateBy,UpdateTime=@UpdateTime where Id=@Id and IsDeleted=0";
            return DbHelp.Execute(sql, new { @Id = id, @UpdateBy = name, @UpdateTime = DateTime.Now }) > 0;

        }

        /// <summary>
        /// 获取活动审批列表
        /// </summary>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<Activities> GetActivities(int status, int pageIndex, int pageSize, out int totalCount)
        {
            var whereStr = "";
            if (status >= 0)
            {
                whereStr = "IsApproved=" + status + " and";
            }
            string sql = "SELECT top {0} * FROM Activities WHERE " + whereStr +
                         " IsDeleted=0 and id not in (select top {1} id from Activities WHERE " + whereStr + " IsDeleted=0 ORDER BY CreateTime desc) ORDER BY CreateTime desc";
            sql = string.Format(sql, pageSize, pageIndex, status);
            var totalsql = string.Format("Select count(*) from Activities WHERE {0} IsDeleted=0", whereStr);
            totalCount = DbHelp.ExecuteScalar<int>(totalsql);
            return DbHelp.Query<Activities>(sql);
        }

        public bool UpdateIsDisplay(int id, int status, string operatorName)
        {
            string sql = "Update Activities set IsDisplay = @IsDisplay,UpdateTime=@UpdateTime,UpdateBy=@UpdateBy where Id=@Id and IsDeleted=0";
            return DbHelp.Execute(sql, new { @Id = id, @IsDisplay = status, @UpdateBy = operatorName, @UpdateTime = DateTime.Now }) > 0;

        }

        public bool UpdateAllDisplay(int id, int priority, int dispaly, int isHot, string operatorName)
        {
            string sql = "Update Activities set Priority = @Priority,IsDisplay=@IsDisplay,IsHot = @IsHot ,UpdateTime=@UpdateTime,UpdateBy=@UpdateBy where Id=@Id and IsDeleted=0";
            return DbHelp.Execute(sql, new { @Id = id, @Priority = priority, @IsDisplay = dispaly, @IsHot = isHot, @UpdateBy = operatorName, @UpdateTime = DateTime.Now }) > 0;

        }

        public AfterSaleServiceWXModel CheckCusomCard(string cardType, string cardNo)
        {
            AfterSaleServiceWXModel afterSaleServiceWxModel = null;
            try
            {
                string sql =
                    @"select c.Id,c.Tel,c.CreateTime,m.CardBeginDate,m.CardEndDate,m.CardName,m.CardValidityType,m.CardValidity,c.IsCancel   FROM  CustomCard  as c    left join  CustomCardInfo as m  on c.CardType=m.CardType where c.CardCode=@CardCode and c.CardType=@CardType ";
                var cardInfo = DbHelp.QueryOne<RrsUserCustomCard>(sql, new { @CardCode = cardNo, @CardType = cardType });
                if (cardInfo != null && cardInfo.Id > 0)
                {
                    afterSaleServiceWxModel = new AfterSaleServiceWXModel();
                    //判断卡券是否核销过。
                    if (cardInfo.IsCancel)
                    {
                        afterSaleServiceWxModel.msg = "该卡券已核销过，请确认";
                        afterSaleServiceWxModel.ret = 0;
                    }
                    //卡券是否在有效期内,1：固定时间；2：领取后生效

                    else if (((cardInfo.CardValidityType == (int)ECardValidityType.Fixed) && DateTime.Now >= cardInfo.CardBeginDate && DateTime.Now <= cardInfo.CardEndDate)
                       || ((cardInfo.CardValidityType == (int)ECardValidityType.After) && DateTime.Now >= cardInfo.CardBeginDate && DateTime.Now <= cardInfo.CreateTime.AddDays(cardInfo.CardValidity+1))
                       )
                    {
                        afterSaleServiceWxModel.data = new AfterSaleServiceWXModelData()
                        {
                            id = Convert.ToInt32(cardInfo.Id),
                            openId = "0000-1111-2222",
                            code = cardNo,
                            tel = cardInfo.Tel,
                            remark = cardInfo.CardName
                        };
                        afterSaleServiceWxModel.ret = 1;
                    }
                    else
                    {
                        afterSaleServiceWxModel.msg = "不在有效期内";
                        afterSaleServiceWxModel.ret = 0;
                    }

                }
                //无效的卡劵号码
                else
                {
                    afterSaleServiceWxModel.msg = "无效的卡劵号码，请确认";
                    afterSaleServiceWxModel.ret = 0;
                }
                return afterSaleServiceWxModel;
            }
            catch (Exception)
            {
                return afterSaleServiceWxModel;
            }
        }

        public AfterSaleServiceWXModel CancelCusomCard(long cardId)
        {
            AfterSaleServiceWXModel afterSaleServiceWxModel = null;
            try
            {
                string updateSql = "update CustomCard set IsCancel=1,UpdateTime=@UpdateTime where Id=@Id";
                int id = DbHelp.Execute(updateSql, new { @UpdateTime = DateTime.Now, @Id = cardId });
                if (id > 0)
                {
                    afterSaleServiceWxModel = new AfterSaleServiceWXModel();
                    afterSaleServiceWxModel.data = new AfterSaleServiceWXModelData()
                    {

                    };
                    afterSaleServiceWxModel.ret = 1;
                }
                return afterSaleServiceWxModel;
            }
            catch (Exception)
            {

                return afterSaleServiceWxModel;
            }
        }

        /// <summary>
        /// 用户核销卡券
        /// </summary>
        /// <param name="cardType"></param>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public AfterSaleServiceWXModel IsCancelCusomCard(string cardType, string cardNo)
        {
            AfterSaleServiceWXModel afterSaleServiceWxModel = null;
            try
            {
                string sql = "select Id FROM  CustomCard where CardCode=@CardCode and CardType=@CardType and IsCancel=0";
                long id = DbHelp.ExecuteScalar<long>(sql, new { @CardCode = cardNo, @CardType = cardType });
                if (id > 0)
                {
                    string updateSql = "update CustomCard set IsCancel=1,UpdateTime=@UpdateTime";
                    DbHelp.Execute(updateSql, new { @UpdateTime = DateTime.Now });
                    afterSaleServiceWxModel = new AfterSaleServiceWXModel();
                    afterSaleServiceWxModel.data = new AfterSaleServiceWXModelData()
                    {
                        code = cardNo
                    };
                }
                return afterSaleServiceWxModel;
            }
            catch (Exception)
            {

                return afterSaleServiceWxModel;
            }
        }
    }
}
