using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
using log4net;
namespace Vcyber.BLMS.Repository
{
    public class SNCardStorager : ISNCardStorager
    {
        private ILog log = LogManager.GetLogger(typeof(SNCardStorager));
        /// <summary>
        /// 根据服务码查询服务码信息
        /// </summary>
        /// <param name="snCode">服务码</param>
        /// <returns>服务码信息</returns>
        public SNCard SelectSNCardByCode(string snCode)
        {
            string sql = "SELECT s.*,m.PhoneNumber FROM SNCard s LEFT JOIN Membership m ON s.UserId = m.Id WHERE s.SNCode = @SNCode";
            return DbHelp.QueryOne<SNCard>(sql, new { @SNCode = snCode });
        }

        /// <summary>
        /// 获取用户持有的服务编码
        /// 17-01-12增加下发时间条件用于计算满足条件的用户每年享有的免费服务次数
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>结果集</returns>
        public IEnumerable<SNCard> SelectSNCardByUser(string userId)
        {
            //string sql = "SELECT * FROM SNCard WHERE UserId = @UserId";
            string sql = @"SELECT * FROM SNCard WHERE UserId = @UserId 
            AND SendTime BETWEEN 
            (SELECT  MLevelBeginDate from Membership   WHERE Id = @UserId ) 
            and (SELECT MLevelInvalidDate from Membership  WHERE Id = @UserId)";
            return DbHelp.Query<SNCard>(sql, new { @UserId = userId });
        }

        /// <summary>
        /// 获取单个候机服务券详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SNCard GetCardByUserDetails(string userid, string id)
        {
            string sql = "SELECT * FROM SNCard WHERE UserId = @UserId and Id=@id";
            return DbHelp.QueryOne<SNCard>(sql, new { @UserId = userid, @id = id });
        }

        /// <summary>
        /// 获取指定数量的服务编码
        /// </summary>
        /// <param name="number">数量</param>
        /// <returns>结果集</returns>
        public IEnumerable<SNCard> GetSNCard(int number)
        {
            string sql = string.Format("SELECT TOP {0} * FROM SNCard WHERE Status = 1 ORDER BY Id", number);
            return DbHelp.Query<SNCard>(sql);
        }

        /// <summary>
        /// 下发服务编码
        /// </summary>
        /// <param name="userId">用户编码</param>
        /// <param name="id">服务编码ID</param>
        /// <returns>执行结果</returns>
        public bool SendSNCard(string userId, int id, int sendType, int airportId, string phoneNumber, string dataSource)
        {
            string sql = "UPDATE SNCard SET Status = 2, SendTime = getdate(), ValidateTime = getdate() + 7, UserId = @UserId, SendType = @SendType, AirportId = @AirportId, RecivePhoneNumber = @RecivePhoneNumber, DataSource = @DataSource WHERE Id = @Id";
            log.Debug(string.Format("UPDATE SNCard SET Status = 2, SendTime = getdate(), ValidateTime = getdate() + 7, UserId = {0}, SendType = {1}, AirportId ={2}, RecivePhoneNumber = {3}, DataSource = {4} WHERE Id = {5}", userId,sendType ,airportId ,phoneNumber , dataSource ,id));
            return DbHelp.Execute(sql, new { @UserId = userId, @Id = id, @SendType = sendType, @AirportId = airportId, @RecivePhoneNumber = phoneNumber, @DataSource = dataSource }) > 0;
        }

        /// <summary>
        /// 使用服务码
        /// </summary>
        /// <param name="snCode">服务码</param>
        /// <returns>执行结果</returns>
        public bool UseSNCard(ReadCheckinDataItemDetail record)
        {
            string sql = "UPDATE SNCard SET Status = 3, UseTime = @UseTime WHERE SNCode = @SNCode";
            return DbHelp.Execute(sql, new { @UseTime = record.checkintime, @SNCode = record.code }) > 0;
        }

        /// <summary>
        /// 添加使用记录
        /// </summary>
        /// <param name="record">机场核销数据</param>
        /// <returns>保存结果</returns>
        public bool AddUsedRecord(ReadCheckinDataItemDetail record, string batchNo)
        {
            string sql = "INSERT INTO SNUsedRecord(SNCode,UseTime,UseAdd,BatchNo) VALUES(@SNCode,@UseTime,@UseAdd,@BatchNo)";
            return DbHelp.Execute(sql, new { @SNCode = record.code, @UseTime = record.checkintime, @UseAdd = record.info, @BatchNo = batchNo }) > 0;
        }

        //分页查询
        public IEnumerable<SNCard> SelectSNCard_Old(string phoneNumber, int status, string iscallcenter, string start, string end, int pageIndex, int pageSize, out int totalCount)
        {
            string sql = @"SELECT w1.*,w2.PhoneNumber,w2.IdentityNumber,RealName,(SELECT A.AirportName FROM Airport A WHERE A.Id = W1.AirportId) AirportName,(SELECT B.UseAdd FROM snusedrecord B WHERE B.SNCode = W1.SNCode) UseAdd FROM sncard w1, 
                            (
                                SELECT TOP {0} ID,phonenumber,IdentityNumber,RealName FROM 
                                (
                                    SELECT TOP {1} ss.* FROM (
			                            SELECT s.*,u.phonenumber,u.IdentityNumber,u.RealName FROM sncard s LEFT JOIN membership u ON s.userId = u.id WHERE 1 = 1 ";

            sql = string.Format(sql, pageSize, pageSize * pageIndex);

            string sqlCondtion = " ";

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                sqlCondtion += " AND u.phonenumber = @PhoneNumber ";
            }

            if (status != 0)
            {
                sqlCondtion += " AND s.Status = @Status ";
            }

            if (!string.IsNullOrEmpty(iscallcenter))
            {
                sqlCondtion += " AND s.IsCallCenter = @IsCallCenter ";
            }

            if (!string.IsNullOrEmpty(start))
            {
                sqlCondtion += "AND s.UseTime >= @Start ";
            }

            if (!string.IsNullOrEmpty(end))
            {
                sqlCondtion += "AND s.UseTime <= @End ";
            }

            sql = sql + sqlCondtion;

            sql += @"
			                            ) ss
                                    ORDER BY ss.Id DESC
                                ) w ORDER BY w.Id ASC
                            ) w2 WHERE w1.ID = w2.ID ORDER BY w1.Id DESC";

            totalCount = DbHelp.ExecuteScalar<int>("SELECT count(*) FROM sncard s LEFT JOIN membership u ON s.userId = u.id WHERE 1 = 1 " + sqlCondtion, new { PhoneNumber = phoneNumber, Status = status, IsCallCenter = iscallcenter, Start = start, End = end });

            return DbHelp.Query<SNCard>(sql, new { PhoneNumber = phoneNumber, Status = status, IsCallCenter = iscallcenter, Start = start, End = end, PageSize = pageSize, PageCount = (pageSize * pageIndex) });
        }

        public IEnumerable<SNCard> SelectSNCard(string phoneNumber, int status, string iscallcenter, string start, string end, int pageIndex, int pageSize, out int totalCount)
        {
            string sql = @"SELECT s.*,u.phonenumber,u.IdentityNumber,u.RealName,A.AirportName,B.UseAdd
                                FROM sncard s 
                                LEFT JOIN membership u ON s.userId = u.id 
                                LEFT JOIN Airport A ON A.Id = s.AirportId 
                                LEFT JOIN snusedrecord B ON B.SNCode = s.SNCode
                                WHERE 1 = 1 ";

            string sqlCondtion = " ";

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                sqlCondtion += " AND u.phonenumber = @PhoneNumber ";
            }

            if (status != 0)
            {
                sqlCondtion += " AND s.Status = @Status ";
            }

            if (!string.IsNullOrEmpty(iscallcenter))
            {
                sqlCondtion += " AND s.IsCallCenter = @IsCallCenter ";
            }

            if (!string.IsNullOrEmpty(start))
            {
                sqlCondtion += "AND s.UseTime >= @Start ";
            }

            if (!string.IsNullOrEmpty(end))
            {
                sqlCondtion += "AND s.UseTime <= @End ";
            }

            sql = sql + sqlCondtion;

            sql += " ORDER BY s.sendtime desc";

            totalCount = DbHelp.ExecuteScalar<int>("SELECT count(*) FROM sncard s LEFT JOIN membership u ON s.userId = u.id WHERE 1 = 1 " + sqlCondtion, new { PhoneNumber = phoneNumber, Status = status, IsCallCenter = iscallcenter, Start = start, End = end });

            return DbHelp.Query<SNCard>(sql, new { PhoneNumber = phoneNumber, Status = status, IsCallCenter = iscallcenter, Start = start, End = end, PageSize = pageSize, PageCount = (pageSize * pageIndex) });
        }

        /// <summary>
        /// 卡券统计
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="status"></param>
        /// <param name="noSend"></param>
        public IEnumerable<SNCard> SelectSNCardCount(string phoneNumber, int status, out int noSend)
        {
            string sqlCondtion = " ";

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                sqlCondtion += " AND u.phonenumber = @PhoneNumber ";
            }

            if (status != 0)
            {
                sqlCondtion += " AND s.Status = @Status ";
            }

            var cards = DbHelp.Query<SNCard>("SELECT s.*,u.PhoneNumber,IdentityNumber,RealName,(SELECT A.AirportName FROM Airport A WHERE A.Id = s.AirportId) AirportName,(SELECT B.UseAdd FROM snusedrecord B WHERE B.SNCode = s.SNCode) UseAdd FROM sncard s LEFT JOIN membership u ON s.userId = u.id WHERE 1 = 1 " + sqlCondtion, new
            {
                PhoneNumber = phoneNumber,
                Status = status
            });

            noSend = cards.Count(x => x.Status == (int)ESNCardStatus.Created);

            return cards;

        }

        /// <summary>
        /// 增加卡券
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool AddSNCard(SNCard card)
        {
            string sql = "INSERT INTO SNCard(SNCode,Status,CreateTime) VALUES(@SNCode,@Status,@CreateTime)";

            return DbHelp.Execute(sql, card) > 0;
        }

        /// <summary>
        /// 查询服务券的使用信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public SNUsedRecord SelectCardUsedRecord(string code)
        {
            string sql = "SELECT * FROM SNUsedRecord WHERE SNCode = @SNCode";
            return DbHelp.QueryOne<SNUsedRecord>(sql, new { @SNCode = code });
        }

        /// <summary>
        /// 返回所有机场所在省
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> SelectProvince()
        {
            string sql = "SELECT DISTINCT Province FROM Airport Order by Province";
            return DbHelp.Query<string>(sql);
        }

        /// <summary>
        /// 返回所有机场所在城市
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> SelectCityByProvince(string province)
        {
            string sql = "SELECT DISTINCT City FROM Airport WHERE 1 = 1 ";

            if (!string.IsNullOrEmpty(province))
                sql += "AND Province = @Province ";

            return DbHelp.Query<string>(sql, new { @Province = province });
        }

        /// <summary>
        /// 查询所有机场列表
        /// </summary>
        /// <returns>机场列表</returns>
        public IEnumerable<Airport> SelectAirportList()
        {
            string sql = "SELECT DISTINCT Province,City,AirportName FROM Airport";
            return DbHelp.Query<Airport>(sql);
        }

        /// <summary>
        /// 根据省、市查询机场
        /// </summary>
        /// <param name="province">省</param>
        /// <param name="city">市</param>
        /// <returns></returns>
        public IEnumerable<Airport> SelectAirportList(string province, string city)
        {
            string sql = "SELECT DISTINCT Province,City,AirportName FROM Airport WHERE 1 = 1 ";

            if (!string.IsNullOrEmpty(province))
                sql += "AND Province = @Province ";

            if (!string.IsNullOrEmpty(city))
                sql += "AND City = @City ";

            sql += " Order by Province";

            return DbHelp.Query<Airport>(sql, new { @Province = province, @City = city });
        }

        /// <summary>
        /// 根据机场名称查询机场候机室列表
        /// </summary>
        /// <param name="airportName">机场名称</param>
        /// <returns>机场候机室列表</returns>
        public IEnumerable<Airport> SelectAirportRoomList(string airportName)
        {
            string sql = "SELECT * FROM Airport WHERE AirportName = @AirportName";

            return DbHelp.Query<Airport>(sql, new { @AirportName = airportName });
        }

        /// <summary>
        /// 获取所有机场候车室
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Airport> AllAirportRoomList()
        {
            string sql = "SELECT * FROM Airport ";

            return DbHelp.Query<Airport>(sql);
        }

        /// <summary>
        /// 查询机场详细信息
        /// </summary>
        /// <param name="id">机场ID</param>
        /// <returns>机场信息</returns>
        public Airport SelectAirport(int id)
        {
            string sql = "SELECT * FROM Airport WHERE Id = @Id";

            return DbHelp.QueryOne<Airport>(sql, new { @Id = id });
        }

        public IEnumerable<Airport> SelectAirportRoomList(string province, string city, string airport)
        {
            string sql = "SELECT * FROM Airport WHERE 1 = 1 ";

            if (!string.IsNullOrEmpty(province))
                sql += "AND Province = @Province ";

            if (!string.IsNullOrEmpty(city))
                sql += "AND City = @City ";

            if (!string.IsNullOrEmpty(airport))
                sql += "AND AirportName = @Airport ";

            sql += " Order by Province";

            return DbHelp.Query<Airport>(sql, new { @Province = province, @City = city, @Airport = airport });
        }

        /// <summary>
        /// 用户轮询
        /// </summary>
        /// <param name="identityNumber">身份证号</param>
        /// <returns></returns>
        public bool MembershipMonitor(string identityNumber)
        {
            try
            {
                DbHelp.Execute("dbo.MembershipMonitor_Test", new { @IdentityNo = identityNumber }, null, null, System.Data.CommandType.StoredProcedure);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 用户轮询积分
        /// </summary>
        /// <param name="identityNumber">身份证号</param>
        /// <returns></returns>
        public bool MembershipMonitorIntegral(string identityNumber)
        {
            try
            {
                DbHelp.Execute("dbo.MembershipMonitorCarIntegral_Test", new { @IdentityNo = identityNumber }, null, null, System.Data.CommandType.StoredProcedure);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 预约候机
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public IEnumerable<SNCard> AirportServiceList(string phone)
        {
            string sql = @"select s.SNCode,SendTime,s.Status,s.SendType,s.UseTime,u.UseAdd AirportName from SNCard s
                            left join Airport on s.AirportId=Airport.Id 
                            left join Membership on s.UserId=Membership.Id 
                            left join SNUsedRecord u on s.sncode = u.sncode where s.Status>1 and Membership.UserName=@phone";
            //sql = string.Format(sql, phone);
            return DbHelp.Query<SNCard>(sql, new { @phone = phone });
        }

        public bool AddAirport(Airport airport)
        {
            string sql = @"INSERT INTO Airport(Province,City,AirportName,AirportAddress,AirportRoomType,AirportRoomName) 
                            VALUES(@Province,@City,@AirportName,@AirportAddress,@AirportRoomType,@AirportRoomName)";

            return DbHelp.Execute(sql, airport) > 0;
        }

        public bool UpdateAirport(Airport airport)
        {
            string sql = @"UPDATE Airport SET Province = @Province,City = @City,AirportName = @AirportName,AirportAddress = @AirportAddress,
                        AirportRoomType = @AirportRoomType,AirportRoomName = @AirportRoomName WHERE Id = @Id ";

            return DbHelp.Execute(sql, airport) > 0;
        }

        public bool DeleteAirport(int Id)
        {
            string sql = "Delete FROM Airport WHERE Id = @Id ";

            return DbHelp.Execute(sql, new { @Id = Id }) > 0;
        }

    }
}
