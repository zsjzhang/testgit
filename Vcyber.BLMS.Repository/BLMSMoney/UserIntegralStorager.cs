using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity.BLMSMoney;
using Vcyber.BLMS.Entity.Generated;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 用户积分操作
    /// </summary>
    public class UserIntegralStorager : IUserIntegralStorager
    {

        public int AddVin(List<Car> Vins, string identityNumber, string userid)
        {
            StringBuilder sql = new StringBuilder();
            if (Vins != null && Vins.Count > 0)
            {
                foreach (Car item in Vins)
                {
                    sql.AppendFormat(" 	INSERT INTO UserCarIntegralRecord values ('{0}','{1}','{2}','{3}','{4}',getdate()) ;", userid, identityNumber, item.CarCategory, item.VIN, item.Userintegral);
                }

                return DbHelp.Execute(sql.ToString());
            }

            return 0;
        }

        #region ==== 私有字段 ====

        /// <summary>
        /// 用户积分过期清理时间
        /// </summary>
        private DateTime cleanTime = DateTime.MaxValue;

        #endregion

        #region ==== 构造函数 ====

        public UserIntegralStorager() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加用户积分
        /// </summary>
        /// <param name="data"></param>
        public void Add(UserIntegral data)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into userintegral(userId,integralSource,value,usevalue,datastate,remark,CreateTime,UpdateTime,IntegralBeginDate,IntegralInvalidDate)");
            sql.Append(" values(@userId,@integralSource,@value,@usevalue,@datastate,@remark,@CreateTime,@UpdateTime,@IntegralBeginDate,@IntegralInvalidDate)");
            DbHelp.Execute(sql.ToString(), new
            {
                userId = data.userId,
                integralSource = data.integralSource,
                value = data.value,
                usevalue = data.usevalue,
                datastate = data.datastate,
                remark = data.remark,
                CreateTime = data.CreateTime,
                UpdateTime = data.UpdateTime,
                IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
            });
        }
        /// <summary>
        /// 向积分记录表中添加用户的积分记录
        /// </summary>
        public void AddUserIntegralRecord(UserIntegral data)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into UserIntegralRecord(userId,integralSource,value,datastate,remark,CreateTime,ProductName,UpdateTime,IntegralBeginDate,IntegralInvalidDate )");
            sql.Append(" values(@userId,@integralSource,@value,@datastate,@remark,@CreateTime,@ProductName,@UpdateTime,@IntegralBeginDate,@IntegralInvalidDate)");

            DbHelp.Execute(sql.ToString(), new
            {
                userId=data.userId,
                integralSource=data.integralSource,
                value=data.value,
                ProductName = data.ProductName,//添加商品名称add by wangchunrong 20161205
                datastate =data.datastate,
                remark=data.remark,
                CreateTime=data.CreateTime,
                IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                UpdateTime = DateTime.Now,
            });
        }

        /// <summary>
        /// 获取用户总积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetTotalIntegral(string userId)
        {
            //this.CleanIntegral(userId);

            StringBuilder sql = new StringBuilder();
            sql.Append(" select isnull(SUM(ISNULL(value,0)-ISNULL(usevalue,0)),0) from userintegral ");
            sql.Append(" where userintegral.userId=@userId and userintegral.datastate=@datastate and userintegral.CreateTime<@Time");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), new { userId = userId, datastate = EDataStatus.NoDelete.ToInt32(), Time = this.cleanTime });
        }

        /// <summary>
        /// 获取用户积分（总，使用，剩余）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<IntegraInfo> GetIntegralIn(string userId)
        {
            //this.CleanIntegral(userId);

            StringBuilder sql = new StringBuilder();
            sql.Append(" select SUM(ISNULL(value,0)) as total,SUM(ISNULL(usevalue,0)) as usevalue,isnull(SUM(ISNULL(value,0)-ISNULL(usevalue,0)),0) as Surplus from userintegral ");
            sql.Append(" where userintegral.userId=@userId and userintegral.datastate=@datastate and userintegral.CreateTime<@Time");
            return DbHelp.Query<IntegraInfo>(sql.ToString(), new { userId = userId, datastate = EDataStatus.NoDelete.ToInt32(), Time = this.cleanTime });
        }

        /// <summary>
        /// 获取用户全部积分记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<UserIntegral> SelectAll(string userId)
        {
            //this.CleanIntegral(userId);

            StringBuilder sql = new StringBuilder();
            sql.Append("select * from userintegral where userId=@userId order by userintegral.CreateTime asc");
            return DbHelp.Query<UserIntegral>(sql.ToString(), new { userId = userId });
        }

        /// <summary>
        /// 分页获取用户积分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<UserIntegral> SelectAll(string userId, PageData pageData, out int total)
        {
            //this.CleanIntegral(userId);

            StringBuilder sql = new StringBuilder();
            sql.Append(" select count(1) from userintegral where userId=@userId ");
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { userId = userId });
            sql.Clear();

            sql.AppendFormat(" select top {0} * from userintegral where userId=@userId", pageData.Size);
            sql.Append(" and userintegral.ID not in( ");
            sql.AppendFormat(" select top {0} userintegral.ID from userintegral where userId=@userId order by userintegral.CreateTime asc", pageData.Index);
            sql.Append(" )order by userintegral.CreateTime asc ");
            return DbHelp.Query<UserIntegral>(sql.ToString(), new { userId = userId });
        }
        /// <summary>
        /// 查询用户的积分记录
        /// 查询userintegralrecord表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<UserIntegral> SelectIntegralRecord(string userId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from userintegralrecord where userId=@userId order by userintegral.CreateTime asc");
            return DbHelp.Query<UserIntegral>(sql.ToString(), new { userId = userId });

        }
        /// <summary>
        /// 分页获取用户积分记录
        /// 查询userintegralrecord表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<UserIntegral> SelectIntegralRecordPage(string userId, int start, int end, out int total)
        {
            //this.CleanIntegral(userId);

            StringBuilder sql = new StringBuilder();
            sql.Append(" select count(1) from UserIntegralRecord where userId=@userId  and datastate=0  ");
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { userId = userId });
            sql.Clear();
            sql.AppendFormat(@" select * from ( select  * ,  ROW_NUMBER ()  over ( order by  CreateTime  desc )   as  rows  from  UserIntegralRecord   where userId =@userID  and  datastate=0 ) page
   where      page.rows  >= {0}  and  page .rows <=  {1} ", start, end);
            return DbHelp.Query<UserIntegral>(sql.ToString(), new { userId = userId });
        }

        /// <summary>
        /// 根据用户UserId获取消费积分明细分页(旧方法)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
   //     public IEnumerable<UserIntegral> SelectUserIntegral(string userId, int pageIndex, int pageSize, out int total)
   //     {

   //         StringBuilder sql = new StringBuilder();
   //         sql.Append(" select count(1) from userintegral where userId=@userId  and datastate=0  ");
   //         total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { userId = userId });
   //         sql.Clear();
   //         sql.AppendFormat(@" select * from ( select  * ,  ROW_NUMBER ()  over ( order by  CreateTime  desc )   as  rows  from  userintegral   where userId =@userID  and  datastate=0 ) page
   //where      page.rows  >= {0}  and  page .rows <=  {1} ", (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
   //         return DbHelp.Query<UserIntegral>(sql.ToString(), new { userId = userId });

   //     }

  


        /// <summary>
        /// 获取用户有效积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<UserIntegral> SelectList(string userId)
        {
            // this.CleanIntegral(userId);

            StringBuilder sql = new StringBuilder();
            sql.Append(" select * from userintegral where userId=@userId and userintegral.datastate=@datastate and  (ISNULL(value,0)-ISNULL(usevalue,0))>0");
            sql.Append(" and userintegral.CreateTime<@Time order by userintegral.CreateTime asc ");
            return DbHelp.Query<UserIntegral>(sql.ToString(), new { userId = userId, datastate = EDataStatus.NoDelete.ToInt32(), Time = DateTime.MaxValue });
        }

        /// <summary>
        /// 减去用户积分
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SubIntegral(int id, string userId, int subValue)
        {
            //this.CleanIntegral(userId);

            if (subValue < 0)
            {
                return false;
            }

            StringBuilder sql = new StringBuilder();
            sql.Append(" update userintegral set usevalue=ISNULL(usevalue,0)+@subvalue where userId=@userId and ID=@Id");
            sql.Append(" and (ISNULL(value,0)-ISNULL(usevalue,0))>=@subvalue");
            return DbHelp.Execute(sql.ToString(), new { subvalue = subValue, userId = userId, Id = id }) > 0;
        }

        /// <summary>
        /// 清理用户过期积分
        /// </summary>
        /// <param name="userId"></param>
        public void CleanIntegral(string userId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update userintegral set datastate=@datastate where userintegral.userId=@userId and datediff(yyyy,userintegral.CreateTime,@Time)>=5");
            DbHelp.Execute(sql.ToString(), new { datastate = EDataStatus.NoDelete.ToInt32(), userId = userId, Time = DateTime.Now });
        }

        /// <summary>
        /// 删除用户积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int DeleteUserIntegral(string userId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Delete from userintegral where userintegral.userId=@userId");
            return DbHelp.Execute(sql.ToString(), new { userId = userId });

        }

        static bool IsUserBirthDay(string identitynumber)
        {
            Regex reg = new Regex(@"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$");
            if (!reg.IsMatch(identitynumber))
            {
                return false;
            }
            if (identitynumber.Length == 15)
            {
                return Convert.ToInt32(identitynumber.Substring(8, 2)) == DateTime.Now.Month;
            }
            if (identitynumber.Length == 18)
            {
                return Convert.ToInt32(identitynumber.Substring(10, 2)) == DateTime.Now.Month;
            }
            return true;
        }
        public double GetUserIntegralDiscount(string userId)
        {
            var membership = DbHelp.QueryOne<Vcyber.BLMS.Entity.Membership>(@"select MLevel ,Birthday,IdentityNumber from Membership where Id=@userId",
                new { userId = userId });

            int IntegralDouble = 1;

            if (DateTime.Now.CompareTo(Convert.ToDateTime("2017-2-16")) > 0 && DateTime.Now.CompareTo(Convert.ToDateTime("2017-4-1")) < 0)//活动期间（2.17~3.31）购车的车主返厂，消费返双倍积分
            {
                if (membership != null && !string.IsNullOrWhiteSpace(membership.IdentityNumber))
                {
                    IEnumerable<Car> cars = _DbSession.CarServiceUserStorager.SelectCarstByIdentity(membership.IdentityNumber);

                    if (cars != null && cars.Count() > 0)
                    {
                        
                        bool flag =
                        cars.Count(
                           i =>
                               Convert.ToDateTime(i.BuyTime.Value).Month < 4 && Convert.ToDateTime(i.BuyTime.Value).Year!=2017) > 0;
                        IntegralDouble = flag ? 2 : 1;
                    }
                }
            }
            

            double discount = 0;
            ///普卡
            if (membership.MLevel == "10")
            {
                discount = 0.1;
            }
            ///银卡
            else if (membership.MLevel == "11")
            {
                discount = 0.15;
            }
            ///金卡
            else if (membership.MLevel == "12")
            {
                discount = 0.2;

                if (IsUserBirthDay(membership.IdentityNumber))
                {
                    discount = 0.5;
                }
            }
            return discount * IntegralDouble;
        }


        #endregion


        public int GetUserIntegralBybuyCarType(string idNumber, BuyCarType buyCarType, int paymentMoney)
        {
            var isDs = _DbSession.DealerMembershipStorager.IsDsCarTypeByIdNumber(idNumber);
            if (isDs)
            {
                if (buyCarType == BuyCarType.First)
                {
                    if (paymentMoney == 100)
                        return 4000;
                    else
                    {
                        throw new Exception("不支持的缴费金额");
                    }
                }
                else if (buyCarType == BuyCarType.Increase)
                {
                    if (paymentMoney == 100)
                        return 7000;
                    else
                    {
                        throw new Exception("不支持的缴费金额");
                    }
                }
                else
                {
                    throw new Exception("不支持的BuyCarType");
                }
            }
            else
            {
                if (buyCarType == BuyCarType.First)
                {
                    if (paymentMoney == 50)
                        return 2000;
                    else
                    {
                        throw new Exception("不支持的缴费金额");
                    }
                }
                else if (buyCarType == BuyCarType.Increase)
                {
                    if (paymentMoney == 50)
                        return 4000;
                    else
                    {
                        throw new Exception("不支持的缴费金额");
                    }
                }
                else
                {
                    throw new Exception("不支持的BuyCarType");
                }
            }
        }

        /// <summary>
        /// 蓝缤会员积分明细
        /// </summary>
        /// <param name="userId">会员ID</param>
        /// <param name="bgDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>蓝缤会员积分明细列表</returns>
        public IEnumerable<UserIntegral> GetUserIntegralList(string userId, DateTime bgDate, DateTime endDate)
        {
            string sql = @"   select userId,integralSource,value,usevalue,datastate,remark,CreateTime,UpdateTime,IntegralBeginDate,IntegralInvalidDate from userintegral
                           where  userId =@userId  {0}";//AND CreateTime  between @bgDate and @endDate
            var whereExp = new StringBuilder();
            //时间段可以为空
            string a = bgDate.Year.ToString();
            //如果时间段为空，返回的值为9999
            if (bgDate.Year != 9999 || endDate.Year != 9999)
            {
                whereExp.Append(" AND CreateTime  between @bgDate and @endDate");
            }
            sql = string.Format(sql, whereExp.ToString());
            return DbHelp.Query<UserIntegral>(sql, new { userId = userId, bgDate = bgDate, endDate = endDate });
        }

        //获取积分明细
        public UserIntegralRecordDetail UserIntegralDetailByUserID(string userid)
        {
            UserIntegralRecordDetail RecordDetail = new UserIntegralRecordDetail();
            StringBuilder sql = new StringBuilder();
            //查询积分使用记录
            sql.Append(" select value,usevalue as surplus,CreateTime,ProductName,remark as IntegralType from UserIntegralRecord where userId=@userId and datastate=0 order by CreateTime desc");//case when value<0 then '消费' else '新增'  end as IntegralType

            #region 时间参数
            //if (!string.IsNullOrWhiteSpace(startTime))
            //{
            //    sql.Append(" and CreateTime>=@startTime ");
            //}
            //if (!string.IsNullOrWhiteSpace(endTime))
            //{
            //    endTime = Convert.ToDateTime(endTime).AddDays(1).ToString("yyyy-MM-dd");
            //    sql.Append("  and CreateTime<@endTime order by CreateTime desc");
            //}
            #endregion
            //消费明细
            RecordDetail.UserIntegrals = DbHelp.Query<UserIntegralRecord>(sql.ToString(), new { userId = userid }).ToList();
            //获取总积分和剩余积分
            StringBuilder sqlTotal = new StringBuilder();
            sqlTotal.Append(@"select  SUM(ISNULL(value,0)) as TotalIntegral,isnull(SUM(ISNULL(value,0)-ISNULL(usevalue,0)),0) as ResidueIntegral from userintegral 
            where userintegral.userId = @userId and userintegral.datastate = 0 ");
            UserIntegralRecordDetail total = DbHelp.QueryOne<UserIntegralRecordDetail>(sqlTotal.ToString(), new { userId = userid });
            //总积分
            RecordDetail.TotalIntegral = total.TotalIntegral;
            //剩余积分
            RecordDetail.ResidueIntegral = total.ResidueIntegral;

            //字典类实现
            //Dictionary<int, int> result = new Dictionary<int, int>();
            //result = GetShengyu(RecordDetail);

            int temp = 0;

            for (int i = 0; i < RecordDetail.UserIntegrals.Count; i++)
            {
                var item = RecordDetail.UserIntegrals[i];
                if (i == 0)
                {
                    //当循环第一条的时候，第一条的余额就是剩余的总余额（RecordDetail.ResidueIntegral）
                    temp = RecordDetail.ResidueIntegral - (RecordDetail.UserIntegrals[i].value);//前一条记录积分
                    item.surplus = RecordDetail.ResidueIntegral;//当前条的剩余积分
                }
                else
                {
                    //当循环到>1的时候，当前条的剩余积分就是temp
                    item.surplus = temp;//当前条的剩余积分
                    temp = item.surplus - (RecordDetail.UserIntegrals[i].value);//计算下一条的剩余积分
                }

            }
            return RecordDetail;

        }

        /// <summary>
        /// 根据userId获取该用户的总积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserIntegral GetUserintegralByUserId(string userId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select SUM(isnull(value,0))-SUM(isnull(usevalue,0)) as value from userintegral where datastate=0 and userId=@userId ");
            return DbHelp.QueryOne<UserIntegral>(sql.ToString(), new { userId = userId });
        }

        /// <summary>
        /// 根据用户UserId获取消费积分明细分页
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<UserIntegralRecord> SelectUserIntegral(string userId, int pageIndex, int pageSize, out int total,out int pageSurplus)
        {
            #region
            //StringBuilder sql = new StringBuilder();
            //sql.Append(" select count(1) from userintegral where userId=@userId  and datastate=0  ");
            //total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { userId = userId });
            //sql.Clear();
            //sql.AppendFormat(@" select * from ( select  * ,  ROW_NUMBER ()  over ( order by  CreateTime  desc )   as  rows  from  userintegral   where userId =@userID  and  datastate=0 ) page
            //                     where page.rows  >= {0}  and  page .rows <=  {1} ", (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
            //return DbHelp.Query<UserIntegral>(sql.ToString(), new { userId = userId });
            #endregion

            UserIntegralRecordDetail RecordDetail = new UserIntegralRecordDetail();
            StringBuilder sql = new StringBuilder();
            //查询积分使用记录
            //sql.Append(@" select page.*
            //                from (
            //                     select value,usevalue as surplus,CreateTime,ProductName,remark as IntegralType,
            //                            ROW_NUMBER () over ( order by CreateTime desc ) as rows
            //                       from UserIntegralRecord 
            //                      where userId=@userId 
            //                        and datastate=0 
            //                      order by CreateTime desc) page where page.rows  >= {0}  and  page .rows <=  {1} ", (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
            sql.Append(@" select value,usevalue as surplus,CreateTime,ProductName,remark as IntegralType,
                                        ROW_NUMBER () over ( order by CreateTime desc ) as rows
                                   from UserIntegralRecord 
                                  where userId=@userId 
                                    and datastate=0 
                                  order by CreateTime desc");

            //消费明细
            RecordDetail.UserIntegrals = DbHelp.Query<UserIntegralRecord>(sql.ToString(), new { userId = userId }).ToList();
            //获取总积分和剩余积分
            StringBuilder sqlTotal = new StringBuilder();
            sqlTotal.Append(@"select  SUM(ISNULL(value,0)) as TotalIntegral,isnull(SUM(ISNULL(value,0)-ISNULL(usevalue,0)),0) as ResidueIntegral from userintegral 
            where userintegral.userId = @userId and userintegral.datastate = 0 ");
            UserIntegralRecordDetail total1 = DbHelp.QueryOne<UserIntegralRecordDetail>(sqlTotal.ToString(), new { userId = userId });
            //总积分
            RecordDetail.TotalIntegral = total1.TotalIntegral;
            //剩余积分
            RecordDetail.ResidueIntegral = total1.ResidueIntegral;

            int temp = 0;
            int tempcount = 0;
            for (int i = 0; i < RecordDetail.UserIntegrals.Count; i++)
            {
                var item = RecordDetail.UserIntegrals[i];
                if (i == 0)
                {
                    //当循环第一条的时候，第一条的余额就是剩余的总余额（RecordDetail.ResidueIntegral）
                    temp = RecordDetail.ResidueIntegral - (RecordDetail.UserIntegrals[i].value);//前一条记录积分
                    item.surplus = RecordDetail.ResidueIntegral;//当前条的剩余积分
                    tempcount = item.surplus;
                }
                else
                {
                    //当循环到>1的时候，当前条的剩余积分就是temp
                    item.surplus = temp;//当前条的剩余积分
                    temp = item.surplus - (RecordDetail.UserIntegrals[i].value);//计算下一条的剩余积分
                    tempcount = temp;
                }
            }
            StringBuilder sql1 = new StringBuilder();
            sql1.Append(@" select count(1) 
                            from UserIntegralRecord 
                           where userId=@userId 
                             and datastate=0 ");
            total = DbHelp.ExecuteScalar<int>(sql1.ToString(), new { userId = userId });
            pageSurplus = tempcount;//每页最后一条记录的剩余积分
            return RecordDetail.UserIntegrals.AsEnumerable();
        }

        /// <summary>
        /// DMS修改消费工单
        /// </summary>
        /// <param name="data"></param>
        public void DmsUpdateOrder(Consuem data)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update CS_Consume set PurchaseCost = @PurchaseCost,PointCost = @PointCost,TotalCost = @TotalCost,ConsumePoints = @ConsumePoints,RewardPoints = @RewardPoints");
            sql.Append(" where DMSOrderNo = @DMSOrderNo and UserId = @UserId"); 
            DbHelp.Execute(sql.ToString(), new
            {
                UserId = data.UserId,
                ConsumePoints = data.ConsumePoints,//Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                RewardPoints = data.RewardPoints, //Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                UpdateTime = DateTime.Now,
                PurchaseCost = data.PurchaseCost,
                PointCost = data.PointCost,
                TotalCost = data.TotalCost,
                DMSOrderNo = data.DMSOrderNo
            });
        }

        /// <summary>
        /// DMS取消消费工单
        /// </summary>
        /// <param name="data"></param>
        public void DmsCancelOrder(string OrderNo)
        {
            //StringBuilder sql = new StringBuilder();
            //sql.Append("insert into CS_Consume  select  UserId,UserName,OrderNo,Phone,DealerId,DealerName,ScheduleOrderNo,ConsumeType,PartCost,MaterialCost,LaborCost");
            //sql.Append(" ,PurchaseCost* (-1),PointCost* (-1),TotalCost * (-1),ConsumePoints * (-1),RewardPoints * (-1),PaperOrder,ApproveStatus,PointStatus,ConsumeDate,CreateTime,CreateId,CreateName");
            //sql.Append(",getdate(),UpdateId,UpdateName, Comment,IdentityNumber,vin,Settlement,SettlementState,PaperOrderCost,MLevel from CS_Consume where OrderNo = @OrderNo");
            //DbHelp.Execute(sql.ToString(), new
            //{
            //    OrderNo = @OrderNo
            //});
            BMDb db = BMDb.GetInstance();
            using (var scope = db.GetTransaction())
            {
                db.Execute(string.Format(@"insert into CS_Consume  select  UserId,UserName,OrderNo,Phone,DealerId,DealerName,ScheduleOrderNo,ConsumeType,PartCost,MaterialCost,LaborCost
                    ,PurchaseCost* (-1),PointCost* (-1),TotalCost * (-1),ConsumePoints * (-1),RewardPoints * (-1),PaperOrder,ApproveStatus,PointStatus,ConsumeDate,CreateTime,CreateId,CreateName
                 ,getdate(),UpdateId,UpdateName, Comment,IdentityNumber,vin,Settlement,SettlementState,PaperOrderCost,MLevel,DMSOrderNo from CS_Consume where DMSOrderNo = @0;
                  insert into History_CS_Consume  select  UserId,UserName,OrderNo,DMSOrderNo,Phone,DealerId,DealerName,ScheduleOrderNo,ConsumeType,PartCost,MaterialCost,LaborCost
                    ,PurchaseCost,PointCost,TotalCost,ConsumePoints,RewardPoints,PaperOrder,ApproveStatus,PointStatus,ConsumeDate,CreateTime,CreateId,CreateName
                 ,getdate(),UpdateId,UpdateName, Comment,IdentityNumber,vin,Settlement,SettlementState,PaperOrderCost,MLevel from CS_Consume where DMSOrderNo = @0;
                "), OrderNo);
                scope.Complete();
            }
        }
    }
}
