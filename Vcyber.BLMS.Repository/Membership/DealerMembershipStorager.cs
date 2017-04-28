
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNet.Identity.SQL;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Repository.CarService;

namespace Vcyber.BLMS.Repository
{
    public class DealerMembershipStorager : IDealerMembershipStorager
    {
        public IEnumerable<Membership> SelectMemberList(string phoneNumber, string identityNumber, string vin, string dealerId, string startTime, string endTime, string userType, string CarCategory, string PaperWork, int pageIndex, int pageSize, out int totalCount)
        {
//            string sql = @"      select * from(  SELECT ROW_NUMBER() over(order by m.CreateTime desc)as rowNUm,  M.Id,M.UserName,C.CustName,M.RealName,M.Email,M.PhoneNumber,M.Age,C.Gender,M.City,M.Area,R.CarCategory,d.DealerId,m.MLevel as a
//	                             ,CASE WHEN MLevel = 1 THEN '注册会员' WHEN MLevel = 10  THEN '普卡会员' WHEN MLevel = 11  THEN ' 银卡会员' WHEN MLevel = 12  THEN '金卡会员' END MLevel,R.VIN,M.IdentityNumber,M.[No],M.CreateTime,M.CreatedPerson,M.NickName,M.PayNumber,kz.PaperWork
//                           FROM Membership M 
//                           INNER JOIN IF_Customer C ON M.IdentityNumber = C.IdentityNumber
//				                            INNER JOIN IF_Car R ON C.CustId = R.CustId 
//				                            INNER JOIN MembershipDealer D ON M.Id = D.MembershipId
//                                            left JOIN Membership_Schedule kz ON M.Id = kz.MembershipId
//where 1=1 and M.MLevel<>1 {2}) as a   where rownum between {0} and  {1} order by createtime desc;";

            string sql = @"      select * from(  SELECT ROW_NUMBER() over(order by m.CreateTime desc)as rowNUm,  M.Id,M.UserName,C.CustName,M.RealName,M.Email,M.PhoneNumber,M.Age,M.City,M.Area,C.Gender,dbo.ConcatFuc(C.CustId) as CarCategory,d.DealerId,m.MLevel as a
	                             ,CASE WHEN MLevel = 1 THEN '注册会员' WHEN MLevel = 10  THEN '普卡会员' WHEN MLevel = 11  THEN ' 银卡会员' WHEN MLevel = 12  THEN '金卡会员' END MLevel,M.IdentityNumber,M.[No],M.CreateTime,M.CreatedPerson,M.NickName,M.PayNumber,
                                   ISNULL(test.shenyu,0) Shenyuintegral,ISNULL(test.shiyong,0) Shiyongintegral,ISNULL(test.total,0) TotalIntegral  
                           FROM Membership M 
                            INNER JOIN IF_Customer C ON M.IdentityNumber = C.IdentityNumber and C.IdentityNumber is not null and C.IdentityNumber<>'' 
                         {3}
				                     INNER JOIN MembershipDealer D ON M.Id = D.MembershipId  
                                      left JOIN IntegralWarn test ON test.userid=m.Id 
                                         {4} 
where 1=1 and M.MLevel<>1 {2}) as a   where rownum between {0} and  {1} order by createtime desc;";


            string ifcar = "";
            string condtion = string.Empty;

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                condtion += " AND M.UserName =@phoneNumber";
            }

            if (!string.IsNullOrEmpty(identityNumber))
            {
                condtion += " AND M.IdentityNumber=@identityNumber";
            }

            if (!string.IsNullOrEmpty(vin))
            {
                ifcar = " INNER JOIN IF_Car R ON C.CustId = R.CustId  ";
                condtion += " AND R.VIN = '" + vin + "'";
            }

            if (!string.IsNullOrEmpty(dealerId))
            {
                condtion += " AND D.DealerId = @DealerId";
            }

            if (!string.IsNullOrEmpty(startTime))
            {
                condtion += " AND M.CreateTime >= @StartTime";
            }

            if (!string.IsNullOrEmpty(endTime))
            {
                condtion += " AND M.CreateTime <= @EndTime";
            }

            if (!string.IsNullOrEmpty(userType))
            {
                condtion += " AND M.MLevel = @UserType ";
            }

            if (!string.IsNullOrEmpty(CarCategory))
            {
                if (string.IsNullOrWhiteSpace(ifcar))
                {
                    ifcar = "  INNER JOIN IF_Car R ON C.CustId = R.CustId  ";
                }
                condtion += " AND R.CarCategory = @CarCategory ";
            }
            string ptablework = "";
            if (!string.IsNullOrEmpty(PaperWork))
            {
                ptablework = "   left JOIN Membership_Schedule kz ON M.Id = kz.MembershipId ";
                condtion += " AND kz.PaperWork = @PaperWork ";
            }

            sql = string.Format(sql, (pageIndex - 1) * pageSize + 1, pageSize * pageIndex, condtion,ifcar,ptablework);


            string sqlCount = @"SELECT COUNT(1) FROM Membership M 
                 INNER JOIN IF_Customer C ON M.IdentityNumber = C.IdentityNumber and C.IdentityNumber is not null and C.IdentityNumber<>'' 
				{0}
				INNER JOIN MembershipDealer D ON M.Id = D.MembershipId
                {1} 
				WHERE 1=1 and M.MLevel<>1" + condtion;

            totalCount = DbHelp.ExecuteScalar<int>(string.Format(sqlCount, ifcar, ptablework), new { DealerId = dealerId, StartTime = startTime, EndTime = endTime, UserType = userType, CarCategory = CarCategory, PaperWork = PaperWork, phoneNumber = phoneNumber, identityNumber = identityNumber });

            return DbHelp.Query<Membership>(sql, new { DealerId = dealerId, StartTime = startTime, EndTime = endTime, UserType = userType, CarCategory = CarCategory, PaperWork = PaperWork, PageSize = pageSize, PageCount = (pageSize * pageIndex),phoneNumber = phoneNumber, identityNumber = identityNumber  });
        }

        public IEnumerable<MemberSonata> SelectMemberListNoJoin(string custName, string phoneNumber, string identityNumber, string vin, string dealerId, string startTime, string endTime, int pageIndex, int pageSize, out int totalCount)
        {
            string sql = @"SELECT C.Id CarId,C.CustId,C.DealerId,M.CustMobile,M.CustName,C.VIN,M.IdentityNumber,M.Gender,C.BuyTime,M.City,M.Address
                            FROM IF_Car C,IF_Customer M,				                          
	                                (SELECT TOP {0} MM.CarId FROM (          
		                                SELECT TOP {1} M.CarId FROM (                      
				                            SELECT X.* FROM (
					                            SELECT C.Id CarId,S.Id MembershipId
					                            FROM IF_Car C 
					                            INNER JOIN IF_Customer M ON C.CustId = M.CustId
					                            LEFT JOIN Membership S ON S.IdentityNumber = M.IdentityNumber 
					                            WHERE  M.CustName NOT like 'D%' {2} ";

            string condtion = string.Empty;

            if (!string.IsNullOrEmpty(custName))
            {
                condtion += " AND M.CustName LIKE '%" + custName + "%'";
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                condtion += " AND M.CustMobile LIKE '%" + phoneNumber + "%'";
            }

            if (!string.IsNullOrEmpty(identityNumber))
            {
                condtion += " AND M.IdentityNumber LIKE '%" + identityNumber + "%'";
            }

            if (!string.IsNullOrEmpty(vin))
            {
                condtion += " AND C.VIN LIKE '%" + vin + "%'";
            }

            if (!string.IsNullOrEmpty(dealerId))
            {
                condtion += " AND C.DealerId = @DealerId";
            }

            if (!string.IsNullOrEmpty(startTime))
            {
                condtion += " AND C.BuyTime >= @StartTime";
            }

            if (!string.IsNullOrEmpty(endTime))
            {
                condtion += " AND C.BuyTime <= @EndTime";
            }

            sql = string.Format(sql, pageSize, pageSize * pageIndex, condtion);

            sql += @"                   ) X	WHERE X.MembershipId IS NULL
		                          ) M ORDER BY M.CarId DESC
		                        ) MM ORDER BY MM.CarId ASC
	                         )M2 
                        WHERE C.Id = M2.CarId AND M.CustId = C.CustId ORDER BY C.BuyTime DESC";

            string sqlCount = @"SELECT COUNT(*) FROM (
                                    SELECT C.Id CarId,C.CustId,S.Id MembershipId,C.DealerId,M.CustMobile,M.CustName,C.VIN,M.IdentityNumber,M.Gender,C.BuyTime,M.City,M.Address
                                    FROM IF_Car C 
                                    INNER JOIN IF_Customer M ON C.CustId = M.CustId
                                    LEFT JOIN Membership S ON S.IdentityNumber = M.IdentityNumber 
                                   " + condtion;

            sqlCount += " ) X WHERE X.MembershipId IS NULL";

            totalCount = DbHelp.ExecuteScalar<int>(sqlCount, new { DealerId = dealerId, StartTime = startTime, EndTime = endTime });

            return DbHelp.Query<MemberSonata>(sql, new { DealerId = dealerId, StartTime = startTime, EndTime = endTime, PageSize = pageSize, PageCount = (pageSize * pageIndex) });
        }

        public IEnumerable<MemberSonata> SelectMemberListAll(string status, string custName, string phoneNumber, string identityNumber, string vin, string startTime, string endTime, string dealerId, string selectCanJoin, string userType, string PaperWork, int pageIndex, int pageSize, out int totalCount, int IsPay, decimal Amount)
        {
            string sql = @"SELECT X.* FROM (                      
					                            SELECT ROW_NUMBER() over(order by S.id) as rowNUm,C.Id CarId,C.CustId,C.DealerId,
                                                 M.CustMobile,M.CustName,C.VIN,M.Gender,S.No,S.Id MembershipId,M.City,M.Address,S.Amount,
												 c.BuyTime,m.IdentityNumber,
												  CASE S.IsPay when '0'then '未缴费' when '1'then '已缴费' when '2' then '审核中' else '未缴费' end IsPayState,
												  CASE M.Agree WHEN '0' THEN '未调查' WHEN '1' THEN '调查中' WHEN '2' THEN '同意入会' ELSE '未调查' END IsCanJoin 
					                            FROM IF_Car C 
					                            INNER JOIN IF_Customer M ON C.CustId = M.CustId
					                            LEFT JOIN Membership S ON S.IdentityNumber = M.IdentityNumber 
                                               {4}
					                            WHERE 1=1 {2}  {3}       ) X	WHERE  rownum between {0} and  {1}  and 1=1  
";

            string condtion = string.Empty;
            string sql_condtion = string.Empty;
            string pworktable = "";
            if (!string.IsNullOrEmpty(custName))
            {
                condtion += " AND M.CustName =@custName";
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                condtion += " AND M.CustMobile = @phoneNumber";
            }

            if (!string.IsNullOrEmpty(identityNumber))
            {
                condtion += " AND M.IdentityNumber =@identityNumber";
            }

            if (!string.IsNullOrEmpty(vin))
            {
                condtion += " AND C.VIN = @VIN";
            }

            if (!string.IsNullOrEmpty(dealerId))
            {
                condtion += " AND C.DealerId = @DealerId";
            }

            if (!string.IsNullOrEmpty(startTime))
            {
                condtion += " AND C.BuyTime >= @StartTime";
            }

            if (!string.IsNullOrEmpty(endTime))
            {
                condtion += " AND C.BuyTime <= @EndTime";
            }

            if (!string.IsNullOrEmpty(selectCanJoin))
            {
                condtion += " AND M.Agree = @Agree ";
            }

            if (!string.IsNullOrEmpty(userType))
            {
                condtion += " AND C.CarCategory = @UserType ";
            }

            if (!string.IsNullOrEmpty(PaperWork))
            {
                pworktable = "   LEFT JOIN Membership_Schedule kz ON S.id =kz.MembershipId ";
                condtion += " AND kz.PaperWork = @PaperWork ";
            }
            if (IsPay != -1)
            {
                if ((status == "2" ||  status=="0") && IsPay==0)
                {
                    condtion += " AND (S.IsPay = @IsPay or  s.IsPay is null)";
                }
                else { 
                condtion += " AND S.IsPay = @IsPay";
                }
            }
            if (Amount != -1)
            {
                condtion += " AND S.Amount = @Amount";
            }
            //    sql = string.Format(sql, pageSize, pageSize * pageIndex, condtion);

            // sql += @"       ) X	WHERE 1=1 ";

            if (status == "1")
            {
                sql_condtion += " AND S.Id IS NOT NULL";
            }

            if (status == "2")
            {
                sql_condtion += " AND S.Id IS NULL";
            }

            //            sql += @"
            //		                         
            //		                       
            //	                         )M2 
            //                        WHERE C.Id = M2.CarId AND M.CustId = C.CustId AND and  rownum between {0} and  {1} ORDER BY MembershipId";

            string sqlCount = @"SELECT COUNT(1) FROM (
                                    SELECT S.Id MembershipId
                                    FROM IF_Car C 
                                    INNER JOIN IF_Customer M ON C.CustId = M.CustId
                                    LEFT JOIN Membership S ON S.IdentityNumber = M.IdentityNumber
                                    {0}  WHERE 1=1 
                                    " + condtion;

            sqlCount += " ) X WHERE 1=1";

            if (status == "1")
            {
                sqlCount += " AND X.MembershipId IS NOT NULL";
            }

            if (status == "2")
            {
                sqlCount += " AND X.MembershipId IS NULL";
            }

            totalCount = DbHelp.ExecuteScalar<int>(string.Format(sqlCount, pworktable), new { DealerId = dealerId, StartTime = startTime, EndTime = endTime, Agree = selectCanJoin, UserType = userType, IsPay = IsPay, Amount = Amount, vin = vin, IdentityNumber = identityNumber, PaperWork = PaperWork, custName = custName, phoneNumber = phoneNumber });

            sql = string.Format(sql, (pageIndex - 1) * pageSize + 1, pageSize * pageIndex, condtion, sql_condtion, pworktable);
            return DbHelp.Query<MemberSonata>(sql, new { DealerId = dealerId, StartTime = startTime, EndTime = endTime, Agree = selectCanJoin, UserType = userType, PageSize = pageSize, PageCount = (pageSize * pageIndex), IsPay = IsPay, Amount = Amount, vin = vin, IdentityNumber = identityNumber, PaperWork = PaperWork, custName = custName, phoneNumber = phoneNumber });
        }
        /// <summary>
        /// 定级（临时）
        /// </summary>
        /// <param name="pid">身份证号</param>
        /// <returns>级别</returns>
        public string GetLevel(string pid)
        {
            string mlevel = "1";
            CarServiceUserStorager car = new CarServiceUserStorager();
            IEnumerable<Car> cars = car.CarsByPID(pid);
            var totalCars = cars.Count();
            var afterCars = cars.Where(i => i.BuyTime.Value >= DateTime.Parse("2016-04-01")).Count();
            if (afterCars > 0)
            {
                if (totalCars == 1)
                {
                    mlevel = "10";
                }
                else if (totalCars == 2)
                {
                    mlevel = "11";
                }
                else
                {
                    mlevel = "12";
                }
            }
            else if (afterCars == 0 && totalCars > 0)
            {
                mlevel = "10";
            }
            return mlevel;
        }

        public bool AddMembershipDealerRecord(string membershipId, string dealerId)
        {
            if (IsExsitMembershipDealer(membershipId, dealerId))
                return true;
            var commandText =
                new StringBuilder(
                    "insert into MembershipDealer(MembershipId,DealerId,CreatedTime) values(@MembershipId,@DealerId,GetDate()) ");
            var parameters = new Dictionary<string, object>
            {
                {"@MembershipId", membershipId},
                {"@DealerId",dealerId}
            };
            return DbHelp.ExecuteScalar<int>(commandText.ToString(), parameters) > 0;
        }

        private bool IsExsitMembershipDealer(string membershipId, string dealerId)
        {
            int count = 0;
            var commandText = new StringBuilder("select count(*) from MembershipDealer where MembershipId=@MembershipId and DealerId=@DealerId");
            var parameters = new Dictionary<string, object>
            {
                {"@MembershipId", membershipId},
                {"@DealerId",dealerId}
            };
            int.TryParse(DbHelp.ExecuteScalar<int>(commandText.ToString(), parameters).ToString(), out count);
            return count > 0;
        }

        public void AddPaperworkToMembership_Schedule(UserModel user)
        {
            string commandText = "";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@PaperWork", user.PaperWork);
            parameters.Add("@MembershipId", user.Id);
            if (!string.IsNullOrEmpty(user.Id) && !string.IsNullOrEmpty(user.PaperWork) && !CheckMembershipIdIsExist(user.Id))
            {
                commandText = @"INSERT INTO dbo.Membership_Schedule(MembershipId,PaperWork,Remark,TransactionTime)
                VALUES(@MembershipId,@PaperWork,@Remark,GETDATE())";
                parameters.Add("@Remark", user.Remark);
            }
            else
            {
                commandText = @"UPDATE [Membership_Schedule] SET PaperWork=@PaperWork  where MembershipId=@MembershipId ";
            }
            DbHelp.Execute(commandText, parameters);
        }

        internal bool CheckMembershipIdIsExist(string MembershipId)
        {
            var commandText = new StringBuilder("select count(*) from Membership_Schedule where MembershipId=@membershipid ");
            var parameters = new Dictionary<string, object>
            {
                {"@membershipid",MembershipId}
            };
            return int.Parse(DbHelp.ExecuteScalar<int>(commandText.ToString(), parameters).ToString()) > 0;
        }

        public int InsertUser(UserModel user)
        {
            
            try
            {
                user.CreateTime = DateTime.Now.ToString();
                string commandText = string.Empty;
                if (!string.IsNullOrEmpty(user.Mid))
                {
                    commandText = @"IF NOT EXISTS(select * from Membership where IdentityNumber=@IdentityNumber  AND IdentityNumber IS NOT NULL AND IdentityNumber <> '' )
  Insert into Membership (UserName,RealName, Id, VIN, PasswordHash, SecurityStamp,Email,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed,IdentityNumber, AccessFailedCount,LockoutEnabled,LockoutEndDateUtc,TwoFactorEnabled,IsDel,Status,CreateTime,CreatedPerson,FaceImage,MType,IsPay,ApprovalStatus,ActiveWay,IsNeedModifyPw,NickName,PayNumber,MLevel,No,MLevelBeginDate,MLevelInvalidDate,Amount,SystemMType,Birthday,Mid,AuthenticationTime,AuthenticationSource,age,Gender)
                values (@UserName,@RealName, @Id, @VIN, @PasswordHash, @SecurityStamp,@Email,@EmailConfirmed,@PhoneNumber,@PhoneNumberConfirmed,@IdentityNumber, @AccessFailedCount,@LockoutEnabled,@LockoutEndDateUtc,@TwoFactorEnabled,0,1,@CreateTime,@CreatedPerson,@FaceImage,@MType,@IsPay,@ApprovalStatus,@ActiveWay,@IsNeedModifyPw,@NickName,@PayNumber,@MLevel,@No,@MLevelBeginDate,@MLevelInvalidDate,@Amount,@SystemMType,@Birthday,@IdentityNumber,@AuthenticationTime,@AuthenticationSource,@age,@Gender)";
                }
                else
                {
                    commandText = @"IF NOT EXISTS(select * from Membership where IdentityNumber=@IdentityNumber  AND IdentityNumber IS NOT NULL AND IdentityNumber <> '' )
  Insert into Membership (UserName,RealName, Id, VIN, PasswordHash, SecurityStamp,Email,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed,IdentityNumber, AccessFailedCount,LockoutEnabled,LockoutEndDateUtc,TwoFactorEnabled,IsDel,Status,CreateTime,CreatedPerson,FaceImage,MType,IsPay,ApprovalStatus,ActiveWay,IsNeedModifyPw,NickName,PayNumber,MLevel,No,MLevelBeginDate,MLevelInvalidDate,Amount,SystemMType,Birthday,AuthenticationTime,AuthenticationSource,age,Gender)
                values (@UserName,@RealName, @Id, @VIN, @PasswordHash, @SecurityStamp,@Email,@EmailConfirmed,@PhoneNumber,@PhoneNumberConfirmed,@IdentityNumber, @AccessFailedCount,@LockoutEnabled,@LockoutEndDateUtc,@TwoFactorEnabled,0,1,@CreateTime,@CreatedPerson,@FaceImage,@MType,@IsPay,@ApprovalStatus,@ActiveWay,@IsNeedModifyPw,@NickName,@PayNumber,@MLevel,@No,@MLevelBeginDate,@MLevelInvalidDate,@Amount,@SystemMType,@Birthday,@AuthenticationTime,@AuthenticationSource,@age,@Gender)";
                }
                return DbHelp.Execute(commandText, user);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public string GetFirstRegistMLevelByIdNumber(string idNumber)
        {
            string mlevel = "1";
            CarServiceUserStorager car = new CarServiceUserStorager();
            IEnumerable<Car> cars= car.SelectCarListByIdentity(idNumber);
            var totalCars = cars.Count();
            var afterCars = cars.Where(i=>i.BuyTime.Value>=DateTime.Parse("2016-04-01")).Count();
            if (afterCars>0)
            {
                if (totalCars==1)
                {
                    mlevel = "10";
                }
                else if (totalCars == 2)
                {
                    mlevel = "11";
                }
                else
                {
                    mlevel = "12";
                }
            }
            else if (afterCars == 0 && totalCars>0)
            {
                mlevel = "10";
            }
            return mlevel;
        }

        private static IList<string> dsCarType = new List<string>()
        {
            "ix25",
             "ix35",
              "全新胜达",
               "全新途胜",
                "索纳塔8",
                 "索纳塔9",
                  "名图",
        };
        /// <summary>
        /// 根据身份证号判断用的车型是否是D+S
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        public bool IsDsCarTypeByIdNumber(string idNumber)
        {
            bool isDs = false;
            string sql =
                @"  select distinct CarCategory from IF_Car a with (nolock) inner join IF_Customer b with (nolock) on a.CustId=b.CustId  where b.IdentityNumber=@IdentityNumber";
            IEnumerable<string> carCategoryList = DbHelp.Query<string>(sql, new { IdentityNumber = idNumber });
            foreach (var item in carCategoryList)
            {
                if (dsCarType.Contains(item))
                {
                    isDs = true;
                    break;
                }
            }
            return isDs;
        }

        /// <summary>
        /// 根据手机号码获取会员信息
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <returns></returns>
        public IEnumerable<Membership> SelectMemberListByphoneNumber(string phoneNumber)
        {
            string sql = "  select id,UserName,PhoneNumber,MLevel from  Membership  where UserName=@UserName";
            return DbHelp.Query<Membership>(sql, new { UserName = phoneNumber });
        }


        public Membership GetMembershipByUserId(string userId)
        {
            var commandText = @"select id,UserName,PhoneNumber from  Membership  where id=@id";
            return DbHelp.QueryOne<Membership>(commandText, new { id = userId });
        }


        public bool IsPersonalUser(string idNumber)
        {
            bool IsPersonal = false;
            string sql = "select  AccntType    from  IF_Customer   where IdentityNumber=@IdentityNumber";

            string accntType = DbHelp.ExecuteScalar<string>(sql, new { IdentityNumber = idNumber });
            if (!string.IsNullOrEmpty(accntType))
            {
                if (accntType == "个人客户")
                {
                    IsPersonal = true;
                }
                else
                {
                    IsPersonal = false;
                }
            }
            return IsPersonal;
        }

        public bool IsComUser(string idNumber)
        {
            bool IsPersonal = false;
            string sql = "select  AccntType    from  IF_Customer   where IdentityNumber=@IdentityNumber";

            string accntType = DbHelp.ExecuteScalar<string>(sql, new { IdentityNumber = idNumber });
            if (!string.IsNullOrEmpty(accntType))
            {
                if (accntType == "公司客户")
                {
                    IsPersonal = true;
                }
                else
                {
                    IsPersonal = false;
                }
            }
            return IsPersonal;
        }


        /// <summary>
        /// 根据证件号码获取会员信息
        /// </summary>
        /// <param name="idNumber">证件号码</param>
        /// <returns></returns>
        public IEnumerable<Membership> GetMemberListByIdentityNumber(string idNumber)
        {
            string sql = "  select id,UserName,PhoneNumber from  Membership  where IdentityNumber=@IdentityNumber";
            return DbHelp.Query<Membership>(sql, new { IdentityNumber = idNumber });
        }

        /// <summary>
        /// 根据userId获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Membership GetMembershipMsgByUserId(string userId)
        {
            var commandText = @"select * from Membership where Id=@id";
            return DbHelp.QueryOne<Membership>(commandText, new { id = userId });
        }
    }
}
