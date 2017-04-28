using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Repository.CarService
{
    using System.Configuration;

    using AspNet.Identity.SQL;

    using PetaPoco;

    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.IRepository.CarService;
    using Vcyber.BLMS.Repository.Entity.Generated;

    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.Entity;
    using Vcyber.BLMS.Entity.SelectCondition;

    public class CarServiceUserStorager : ICarServiceUserStorager
    {
        public bool IsSonataUser(string identity)
        {
            var car = PocoHelper.CurrentDb().Query<IFCar>(@"
select a.* from IF_Car a
join IF_Customer b
on	a.CustId = b.CustId
where (a.CarCategory=@0 or a.CarCategory=@1 or a.CarCategory=@2)
and	b.IdentityNumber=@3 and b.IdentityNumber <> '' and b.IdentityNumber is not null", CommonConst.SONATA9, CommonConst.SONATA9_1, CommonConst.SONATA9_2, identity);

            return car != null && car.Any();
        }

        public bool IsTlcUser(string identityNumber)
        {
            var car = PocoHelper.CurrentDb().Query<IFCar>(@"
select a.* from IF_Car a
join IF_Customer b
on	a.CustId = b.CustId
where a.CarCategory=@0
and	b.IdentityNumber=@1 and b.IdentityNumber <> '' and b.IdentityNumber is not null", CommonConst.TLC, identityNumber);

            return car != null && car.Any();
        }

        public int GetFreeServiceCount(string identity, EDMSServiceType orderType)
        {
            //string _orderType = orderType.ToString().Replace(" ", string.Empty);//数据库中的字段是数字开头的，把枚举中的下划线去掉，不然无法匹配数据库内容
            //var frontUserStore = new FrontUserStore<FrontIdentityUser>();
            switch (orderType)
            {
                case EDMSServiceType.Care:
                    {
                        var order = PocoHelper.CurrentDb().ExecuteScalar<int>(@"
select count(1) from IF_Car a
join IF_Customer b
on	a.CustId = b.CustId
join IF_RepairReport c
on	b.IdentityNumber=c.IdentityNumber
and	a.VIN = c.VINCode
where c.ServiceType =@0
and b.IdentityNumber=@1", orderType.DisplayName(), identity);
                        return 1 - order;
                    }
                case EDMSServiceType.Home2Home:
                    {
                        var order = PocoHelper.CurrentDb().ExecuteScalar<int>(@"select count(1) from IF_Car a join IF_Customer b on a.CustId=b.CustId join Membership m on b.IdentityNumber=m.IdentityNumber join CS_SonataService c on m.Id=c.UserId where c.OrderType=@0 and b.IdentityNumber=@1 and a.BuyTime=@2", orderType, identity, DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd"));
                        return 1 - order;
                    }
                case EDMSServiceType.FreeCheck:
                    {
                        #region ==== 原始代码 ====

                        //                        var order = PocoHelper.CurrentDb().ExecuteScalar<int>(@"
                        //select count(1) from IF_Car a
                        //join IF_Customer b
                        //on	a.CustId = b.CustId
                        //join IF_RepairReport c
                        //on	b.IdentityNumber=c.IdentityNumber
                        //and	a.VIN = c.VINCode
                        //where c.ServiceType =@0
                        //and b.IdentityNumber=@1
                        //and a.BuyTime>@2", orderType.DisplayName(), identity, DateTime.Now.ToString("yyyy-MM-dd"));//DateTime.Now.AddYears(-3).ToString("yyyy-MM-dd")

                        //                        return 3 - order;

                        #endregion

                        #region ==== 新逻辑 ====

                        StringBuilder sql = new StringBuilder();
                        sql.Append(" select count(1) from IF_Car a join IF_Customer b on	a.CustId = b.CustId ");
                        sql.Append(" where b.IdentityNumber=@0 and  DATEDIFF(year,a.BuyTime,@1)>=3");

                        int count = PocoHelper.CurrentDb().ExecuteScalar<int>(sql.ToString(), identity, DateTime.Now);
                        sql.Clear();

                        if (count > 0)//购车时间是否超过三年
                        {
                            return 0;
                        }

                        sql.Append(" select count(1) from IF_Car a");
                        sql.Append(" join IF_Customer b on	a.CustId = b.CustId");
                        sql.Append(" join IF_RepairReport c on b.IdentityNumber=c.IdentityNumber and	a.VIN = c.VINCode");
                        sql.Append(" where c.ServiceType =@0 and b.IdentityNumber=@1");
                        sql.Append(" and( DATEDIFF(year,a.BuyTime,@2)=0 or DATEDIFF(year,a.BuyTime,@2)=1 or");
                        sql.Append(" DATEDIFF(year,a.BuyTime,@2)=2) ");

                        count = PocoHelper.CurrentDb().ExecuteScalar<int>(sql.ToString(), orderType.DisplayName(), identity, DateTime.Now);
                        return count > 3 ? 0 : 3 - count;

                        #endregion

                    }
                case EDMSServiceType.LongDistanceTravel:
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append(" select count(1) from IF_Car a");
                        sql.Append(" join IF_Customer b on	a.CustId = b.CustId");
                        sql.Append(" join IF_RepairReport c on b.IdentityNumber=c.IdentityNumber and	a.VIN = c.VINCode");
                        sql.Append(" where c.ServiceType =@0 and b.IdentityNumber=@1");

                        int count = PocoHelper.CurrentDb().ExecuteScalar<int>(sql.ToString(), orderType.DisplayName(), identity);
                        sql.Clear();

                        if (count >= 2)
                        {
                            return 0;
                        }

                        sql.Append(" select count(1) from IF_Car a");
                        sql.Append(" join IF_Customer b on	a.CustId = b.CustId");
                        sql.Append(" join IF_RepairReport c on b.IdentityNumber=c.IdentityNumber and	a.VIN = c.VINCode");
                        sql.Append(" where c.ServiceType =@0 and b.IdentityNumber=@1");
                        sql.Append(" and DATEDIFF(year,C.RepairTime,@2)=0");

                        count = PocoHelper.CurrentDb().ExecuteScalar<int>(sql.ToString(), orderType.DisplayName(), identity, DateTime.Now);
                        return 1 - count;
                    }
                default:
                    return 0;
            }
        }
        public List<FreeServiceRecord> GetFreeServiceRecord(string identity, EDMSServiceType orderType)
        {
            List<FreeServiceRecord> list = new List<FreeServiceRecord>();
            list = PocoHelper.CurrentDb().Query<FreeServiceRecord>(@"
select c.*,m.IdentityNumber,m.MLevelBeginDate,m.MLevelInvalidDate from  Membership m  join CS_SonataService c on m.Id=c.UserId where c.OrderType=@0 and m.IdentityNumber=@1 ", (int)orderType, identity).ToList();
            return list;
        }

        public IEnumerable<Car> SelectCarListByUserId(string userId)
        {
            string sql = @"SELECT c.[Id]
      ,c.[CarCategory]
      ,c.[CarType]
      ,c.[DealerId]
      ,c.[DealerName]
      ,c.[LicencePlate]
      ,c.[VIN]
      ,c.[Mileage]
      ,c.[CustId]
      ,c.[BuyTime]
      ,M.accnttype 
      ,'" + ConfigurationManager.AppSettings["ImgPath"] + @"'+MD.PictureUrl   as PictureUrl
      ,'" + ConfigurationManager.AppSettings["ImgPath"] + @"'+MD.PictureUrl2   as PictureUrl2
      FROM IF_Car C
      join	IF_Customer M
      on	C.CustId = M.CustId and m.IdentityNumber <> '' and m.IdentityNumber is not null
      join  MemberShip U
      on    M.IdentityNumber = U.IdentityNumber
      left join	MD_CarPicture MD
      on	C.CarCategory=MD.CarCategory
      where U.Id = @UserId";

            return DbHelp.Query<Car>(sql, new { @UserId = userId });
        }

        public IEnumerable<Car> SelectCarstByIdentity(string identityNumber)
        {
            string sql = @"SELECT c.[Id]
      ,c.[CarCategory]
      ,c.[CarType]
      ,c.[DealerId]
      ,c.[DealerName]
      ,c.[LicencePlate]
      ,c.[VIN]
      ,c.[Mileage]
      ,c.[CustId]
      ,c.[BuyTime]
      FROM IF_Car C
      join	IF_Customer M
      on	C.CustId = M.CustId      
      where	M.IdentityNumber = @IdentityNumber";
            return DbHelp.Query<Car>(sql, new { @IdentityNumber = identityNumber });
        }

        public IEnumerable<Car> SelectCarListByIdentity(string identityNumber)
        {
            string sql = @"SELECT c.[Id]
      ,c.[CarCategory]
      ,c.[CarType]
      ,c.[DealerId]
      ,c.[DealerName]
      ,c.[LicencePlate]
      ,c.[VIN]
      ,c.[Mileage]
      ,c.[CustId]
      ,c.[BuyTime]
      ,'" + ConfigurationManager.AppSettings["ImgPath"] +
         @"'+MD.PictureUrl   as PictureUrl
      FROM IF_Car C
      join	IF_Customer M
      on	C.CustId = M.CustId      
      left join	MD_CarPicture MD
      on	C.CarCategory=MD.CarCategory
      where	M.IdentityNumber = @IdentityNumber";
            return DbHelp.Query<Car>(sql, new { @IdentityNumber = identityNumber });
        }


        public IEnumerable<Car> SelectCarListByIdentityOld(string identityNumber)
        {
            string sql = @"SELECT c.[Id]
      ,c.[CarCategory]
      ,c.[CarType]
      ,c.[DealerId]
      ,c.[DealerName]
      ,c.[LicencePlate]
      ,c.[VIN]
      ,c.[Mileage]
      ,c.[CustId]
      ,c.[BuyTime] 
      FROM blms.dbo.IF_Car C
      join	blms.dbo.IF_Customer M
      on	C.CustId = M.CustId      
      where	M.IdentityNumber = @IdentityNumber and m.IdentityNumber <> '' and m.IdentityNumber is not null";
            return DbHelp.Query<Car>(sql, new { @IdentityNumber = identityNumber });
        }




        /// <summary>
        /// 查询证件号下的车辆
        /// </summary>
        /// <param name="pid">证件号</param>
        /// <returns>车辆列表</returns>
        public IEnumerable<Car> CarsByPID(string pid)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT icz.[Id],icz.[CarCategory],icz.[CarType],icz.[DealerId],icz.[DealerName],");
            sql.Append("icz.[LicencePlate],icz.[VIN],icz.[Mileage],icz.[CustId],icz.[BuyTime],");
            sql.AppendFormat("'{0}'+mcp.PictureUrl AS PictureUrl", ConfigurationManager.AppSettings["ImgPath"]);
            sql.Append(" FROM dbo.IF_Car AS icz");
            sql.Append(" INNER JOIN dbo.IF_Customer AS icz2 ON icz2.CustId = icz.CustId");
            sql.Append(" LEFT JOIN dbo.MD_CarPicture AS mcp ON mcp.CarCategory = icz.CarCategory");
            sql.Append(" WHERE icz2.IdentityNumber = @IdentityNumber");
            return DbHelp.Query<Car>(sql.ToString(), new { @IdentityNumber = pid });
        }
        /// <summary>
        /// 查询用户名下的车辆
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>车辆列表</returns>
        public IEnumerable<Car> CarsByUserId(string userId)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT icz.[Id],icz.[CarCategory],icz.[CarType],icz.[DealerId],icz.[DealerName],");
            sql.Append("icz.[LicencePlate],icz.[VIN],icz.[Mileage],icz.[CustId],icz.[BuyTime],");
            sql.AppendFormat("'{0}'+mcp.PictureUrl AS PictureUrl,", ConfigurationManager.AppSettings["ImgPath"]);
            sql.AppendFormat("'{0}'+mcp.PictureUrl2 AS PictureUrl2", ConfigurationManager.AppSettings["ImgPath"]);
            sql.Append(" FROM dbo.IF_Car AS icz");
            sql.Append(" INNER JOIN dbo.IF_Customer AS icz2 ON icz2.CustId = icz.CustId");
            sql.Append(" INNER JOIN dbo.Membership AS m ON m.IdentityNumber = icz2.IdentityNumber");
            sql.Append(" LEFT JOIN dbo.MD_CarPicture AS mcp ON mcp.CarCategory = icz.CarCategory");
            sql.Append(" WHERE m.Id = @UserId");
            return DbHelp.Query<Car>(sql.ToString(), new { @UserId = userId });
        }
        public IFCustomer GetCustomer(string identityNumber)
        {
            string sql = @"select * from IF_Customer c where c.identityNumber=@identityNumber and c.IdentityNumber <> '' and c.IdentityNumber is not null";
            return DbHelp.QueryOne<IFCustomer>(sql, new { @identityNumber = identityNumber });
        }

        public bool UpdateCarInfo(string vin, string LicencePlate, string Mileage)
        {
            string sql = "update IF_Car set LicencePlate = @LicencePlate,Mileage = @Mileage where VIN = @VIN";
            return DbHelp.Execute(sql, new { @LicencePlate = LicencePlate, @Mileage = Mileage, @VIN = vin }) > 0;
        }
        public bool UpdateCarInfo2(string vin, string LicencePlate, string Mileage)
        {
            string sql = "update IF_Car set LicencePlate = @LicencePlate,Mileage = @Mileage where VIN = @VIN";
            return DbHelp.Execute(sql, new { @LicencePlate = LicencePlate, @Mileage = Mileage, @VIN = vin }) > 0;
        }

        public Car GetCarInfoByVIN(string vin)
        {
            string sql = @" select * from IF_Car c where c.VIN=@VIN ";
            return DbHelp.QueryOne<Car>(sql, new { @VIN = vin });
        }

        public Car OldGetCarInfoByVIN(string vin)
        {
            string sql = @" select * from blms.dbo.IF_Car c where c.VIN=@VIN ";
            return DbHelp.QueryOne<Car>(sql, new { @VIN = vin });
        }

        /// <summary>
        /// 查询中间表客户信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public IEnumerable<IFCustomer> FindCustomer(CustomerCondition condition)
        {
            string sql = string.Format("select * from IF_Customer where {0}", condition.ToWhere());

            if (condition.IsValidate || !string.IsNullOrEmpty(condition.CustId))
            {
                return DbHelp.Query<IFCustomer>(sql);
            }

            return new List<IFCustomer>(1);
        }

        public IEnumerable<IFCustomer> OldFindCustomer(CustomerCondition condition)
        {
            string sql = string.Format("select * from blms.dbo.IF_Customer where {0}", condition.ToWhere());

            if (condition.IsValidate)
            {
                return DbHelp.Query<IFCustomer>(sql);
            }

            return new List<IFCustomer>(1);
        }
        /// <summary>
        /// 删除中间表客户信息
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool DelCustomer(CustomerCondition condition)
        {
            string sql = "delete from blms.dbo.IF_Customer where CustId = @CustId";
            return DbHelp.Execute(sql, new { @CustId = condition.CustId }) > 0;
        }
        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <param name="cusId"></param>
        /// <returns></returns>
        public IFCustomer findCustomer(string cusId)
        {
            string sql = "select * from IF_Customer where CustId=@CustId";
            return DbHelp.QueryOne<IFCustomer>(sql, new { CustId = cusId });
        }

        public IFCustomer OldfindCustomer(string cusId)
        {
            string sql = "select * from blms.dbo.IF_Customer where CustId=@CustId";
            return DbHelp.QueryOne<IFCustomer>(sql, new { CustId = cusId });
        }

        /// <summary>
        /// 查询客户车辆信息
        /// </summary>
        /// <param name="cusId"></param>
        /// <returns></returns>
        public IEnumerable<Car> findIfCar(string cusId)
        {
            string sql = "select * from IF_Car where CustId=@CustId";
            return DbHelp.Query<Car>(sql, new { CustId = cusId });
        }

        public IEnumerable<Car> OldfindIfCar(string cusId)
        {
            string sql = "select * from blms.dbo.IF_Car where CustId=@CustId";
            return DbHelp.Query<Car>(sql, new { CustId = cusId });
        }

        public IFCustomer CreateOrGetIFCustomer(IFCustomer customer)
        {
            IFCustomer c = GetCustomer(customer.IdentityNumber);
            if (c != null && !string.IsNullOrEmpty(c.IdentityNumber))
            {
                return c;
            }
            else
            {
                int keyId = 0;

                string sql = " insert into IF_Customer(CustId,CustName,CustMobile,IdentityNumber,Gender,Email,Address,City) values(@CustId,@CustName,@CustMobile,@IdentityNumber,@Gender,@Email,@Address,@City);SELECT @@identity; ";
                keyId = DbHelp.Execute(sql, new { @CustId = customer.CustId, @CustName = customer.CustName, @CustMobile = customer.CustMobile, @IdentityNumber = customer.IdentityNumber, @Gender = customer.Gender, @Email = customer.Email, @Address = customer.Address, @City = customer.City });

                if (keyId > 0)
                    return customer;
                else
                    return null;
            }
        }

        /// <summary>
        /// 是否存在相同的身份证号
        /// </summary>
        /// <param name="identityNumber"></param>
        /// <returns></returns>
        public bool IsExistIdentity(string identityNumber)
        {
            string sql = "select count(1) from IF_Customer c where c.identityNumber=@identityNumber";
            return DbHelp.ExecuteScalar<int>(sql, new { identityNumber = identityNumber }) > 0;
        }

        public bool OldIsExistIdentity(string identityNumber)
        {
            string sql = "select count(1) from blms.dbo.IF_Customer c where c.identityNumber=@identityNumber";
            return DbHelp.ExecuteScalar<int>(sql, new { identityNumber = identityNumber }) > 0;
        }

        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="data"></param>
        public void AddCustomer(IFCustomer data)
        {
            string sql = " insert into blms.dbo.IF_Customer(CustId,CustName,CustMobile,IdentityNumber,Gender,Email,Address,City,AccntType,addTime) values(@CustId,@CustName,@CustMobile,@IdentityNumber,@Gender,@Email,@Address,@City,'个人客户',GETDATE())";
            DbHelp.Execute(sql, data);
        }

        public bool UpdateCarInfoByVIN(string vin, string custId)
        {
            string sql = " update blms.dbo.IF_Car set CustId = @CustId,addTime=CONVERT(VARCHAR(10),DATEADD(DAY,1,GETDATE()),120) where VIN = @VIN ";
            return DbHelp.Execute(sql, new { @CustId = custId, @VIN = vin }) > 0;
        }

        public bool InsertCarInfoByVIN(string vin, string custId, string CarCategory, string Buytime)
        {
            string sql = " INSERT INTO blms.dbo.IF_Car (Id ,cartype, CarCategory ,DealerId ,DealerName ,  VIN ,CustId , BuyTime , addTime , Usage ) VALUES (NEWID(),'',@CarCategory,'D0050','员工购车管理',@VIN,@CustId,@Buytime,GETDATE(),'A1')";
            return DbHelp.Execute(sql, new { @CustId = custId, @VIN = vin, @CarCategory = CarCategory, @Buytime = Buytime }) > 0;
        }

        public LevelAndNo GetUserLevelByVin(string vin)
        {
            string sql = @"SELECT M.MLevel,M.No,M.Id AS UserId FROM Membership M, IF_Customer C, IF_Car R 
                            WHERE M.IdentityNumber = C.IdentityNumber and m.IdentityNumber <> '' and m.IdentityNumber is not null AND C.CustId = R.CustId AND R.VIN = @VIN";

            return DbHelp.QueryOne<LevelAndNo>(sql, new { @VIN = vin });
        }

        public bool AddActivityJoinRecord(string uname, string mobile, string aname)
        {
            string sql = "INSERT INTO ActivityJoinRecord(UserName,PhoneNumber,ActivityName,CreateTime) VALUES(@UserName,@PhoneNumber,@ActivityName,GETDATE())";

            return DbHelp.Execute(sql, new { @UserName = uname, @PhoneNumber = mobile, @ActivityName = aname }) > 0;
        }

        public EMemshipLevelWX GetMemshipLevelWX(string userId)
        {
            EMemshipLevelWX level = EMemshipLevelWX.OneStar;
            Sql sql = new Sql(@"select a.Id, a.MLevel,a.IsPay,c.CarCategory from Membership a
join IF_Customer b
on a.IdentityNumber=b.IdentityNumber and a.IdentityNumber <> '' and a.IdentityNumber is not null
join IF_Car c
on b.CustId  = c.CustId
where a.Id=@0 ", userId);
            var result = PocoHelper.CurrentDb().Fetch<dynamic>(sql);
            if (result.Count > 0)
            {
                level = (EMemshipLevelWX)result[0].MLevel;
            }
            result.ForEach(
                    item =>
                    {
                        if (level == EMemshipLevelWX.ThreeStar
                            && (item.CarCategory == "全新途胜" || item.CarCategory == "索纳塔9" || item.CarCategory == "第九代索纳塔"))
                        {
                            if (item.IsPay == 0) level = EMemshipLevelWX.SilverNotActive;
                            else level = EMemshipLevelWX.Silver;
                        }
                    });

            return level;

        }

        public IEnumerable<CSTestDrive> GetCSTestDrive(string userid)
        {
            string sql = @"select * from CS_TestDrive where UserId=@userid";
            return DbHelp.Query<CSTestDrive>(sql, new { @userid = userid });
        }

        public IEnumerable<CSTestDrive> GetCSTestDriveByPhone(string Phone)
        {
            string sql = @"select * from CS_TestDrive where Phone=@Phone ORDER BY CreateTime DESC";
            return DbHelp.Query<CSTestDrive>(sql, new { @Phone = Phone });
        }

        public IEnumerable<CSTestDrive> GetCSTestDriveDetial(string OrderNo)
        {
            string sql = @"select * from CS_TestDrive where OrderNo=@OrderNo";
            return DbHelp.Query<CSTestDrive>(sql, new { @OrderNo = OrderNo });
        }


        public IEnumerable<CSMaintenance> GetCSMaintenance(string userid)
        {
            string sql = @"select * from CS_Maintenance where UserId=@userid";
            return DbHelp.Query<CSMaintenance>(sql, new { @userid = userid });
        }

        public IEnumerable<CSMaintenance> GetCSMaintenanceDetial(string OrderNo)
        {
            string sql = @"select * from CS_Maintenance where OrderNo=@OrderNo";
            return DbHelp.Query<CSMaintenance>(sql, new { @OrderNo = OrderNo });
        }

        /// <summary>
        /// 查询银卡会员的vin，姓名，购车日期
        /// </summary>
        /// <returns>机场列表</returns>
        public IEnumerable<GetVinCustTimeEF> GetVinCustTime()
        {
            string sql = "select IF_Car.BuyTime,IF_Car.VIN,IF_Customer.CustName from IF_Car,IF_Customer where IF_Customer.CustId=IF_Car.CustId and (IF_Car.CarCategory ='第九代索纳塔' or IF_Car.CarCategory ='全新途胜'or IF_Car.CarCategory ='索纳塔9' )  and DateDiff(dd,IF_Car.BuyTime,getdate())=0  ";
            return DbHelp.Query<GetVinCustTimeEF>(sql);
        }

    }
}
