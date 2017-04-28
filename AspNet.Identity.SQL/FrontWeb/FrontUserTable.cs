using System;
using System.Collections.Generic;
using System.Linq;
using AspNet.Identity.SQL.FrontWeb.QueryEntity;
using System.Text;
using System.Windows.Forms;
using AspNet.Identity.SQL.FrontWeb;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace AspNet.Identity.SQL
{
    /// <summary>
    /// Class that represents the Users table in the MySQL Database
    /// </summary>
    public class FrontUserTable<TUser>
        where TUser : FrontIdentityUser
    {
        private SQLDatabase _database;

        public FrontUserTable()
            : this(new SQLDatabase())
        {

        }

        /// <summary>
        /// Constructor that takes a SQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public FrontUserTable(SQLDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Returns the user's name given a user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserName(string userId)
        {
            string commandText = "Select UserName from membership where Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Id", userId } };

            return _database.GetStrValue(commandText, parameters);
        }

        /// <summary>
        /// Returns a User ID given a user name
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public string GetUserId(string userName)
        {
            string commandText = "Select Id from membership where UserName = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

            return _database.GetStrValue(commandText, parameters);
        }

        /// <summary>
        /// Returns an TUser given the user's id
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public TUser GetUserById(string userId)
        {
            TUser user = null;
            //string commandText = "Select * from membership where id = @id";

            string commandText = "Select * from membership left join Membership_Schedule on Membership.Id=Membership_Schedule.MembershipId  where id =@id";
            //string commandText = "Select * from membership inner join Membership_Schedule  on membership.Id=Membership_Schedule.MembershipId inner join IF_Customer on membership.IdentityNumber=IF_Customer.IdentityNumber inner join IF_Car  on IF_Customer.CustId=IF_Car.CustId where membership.Id = @id"; 
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

            var rows = _database.Query(commandText, parameters);
            if (rows != null && rows.Count == 1)
            {
                var row = rows[0];
                user = (TUser)Activator.CreateInstance(typeof(TUser));
                //个人信息
                user.Id = row["Id"];
                user.UserName = row["UserName"];
                user.RealName = row["RealName"];
                user.CreatedPerson = row["CreatedPerson"];
                user.MLevel = row["MLevel"] == null ? 1 : int.Parse(row["MLevel"]);
                user.Status = int.Parse(row["Status"]);
                user.VIN = row["VIN"];
                user.ApprovalStatus = int.Parse(row["ApprovalStatus"]);
                user.PhoneNumber = row["PhoneNumber"];
                user.IdentityNumber = row["IdentityNumber"];
                user.Gender = row["Gender"];
                user.GenderName = string.IsNullOrEmpty(row["Gender"]) ? null : row["Gender"] == "1" ? "男" : "女";
                user.Provency = row["Provency"];
                user.City = row["City"];
                user.Area = row["Area"];
                user.Address = row["Address"];
                user.PasswordHash = string.IsNullOrEmpty(row["PasswordHash"]) ? null : row["PasswordHash"];
                user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
                user.Email = string.IsNullOrEmpty(row["Email"]) ? null : row["Email"];
                user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true : false;
                user.PhoneNumber = string.IsNullOrEmpty(row["PhoneNumber"]) ? null : row["PhoneNumber"];
                user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
                user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
                user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
                user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
                user.CreateTime = row["CreateTime"];
                user.FaceImage = row["FaceImage"];
                user.MType = string.IsNullOrEmpty(row["MType"]) ? 1 : int.Parse(row["MType"]);
                user.IsPay = string.IsNullOrEmpty(row["IsPay"]) ? 0 : int.Parse(row["IsPay"]);
                user.SystemMType = string.IsNullOrEmpty(row["SystemMType"]) ? 1 : int.Parse(row["SystemMType"]);
                user.ActiveWay = string.IsNullOrEmpty(row["ActiveWay"]) ? 1 : int.Parse(row["ActiveWay"]);
                user.IsNeedModifyPw = string.IsNullOrEmpty(row["IsNeedModifyPw"]) ? 0 : int.Parse(row["IsNeedModifyPw"]);
                user.NickName = row["NickName"];
                user.PayNumber = row["PayNumber"];
                user.No = row["No"];
                user.Interest = row["Interest"];
                user.UserType = row["UserType"];
                user.AuthenticationTime = string.IsNullOrEmpty(row["AuthenticationTime"]) ? Convert.ToDateTime("1900-01-01"): Convert.ToDateTime(row["AuthenticationTime"]);
                user.AuthenticationSource = row["AuthenticationSource"];
                if (!string.IsNullOrEmpty(row["Birthday"]))
                {
                    user.Birthday = DateTime.Parse(row["Birthday"]).ToShortDateString();
                }
                //详细信息
                user.MembershipId = row["MembershipId"];
                user.ZipCode = row["ZipCode"];
                user.TelePhone = row["TelePhone"];
                user.PaperWork = row["PaperWork"];
                user.Educational = row["Educational"];
                user.Job = row["Job"];
                user.Office = row["Office"];
                user.Industry = row["Industry"];
                user.Remark = row["Remark"];
                user.IsMarriage = row["IsMarriage"];
                user.MarriageDay = row["MarriageDay"];
                user.MainContact = row["MainContact"];
                user.MainTelePhone = row["MainTelePhone"];
                user.OrganizationCode = row["OrganizationCode"];
                if (!string.IsNullOrEmpty(row["SendSms"]))
                {
                    user.SendSms = int.Parse(row["SendSms"]);
                }
                if (!string.IsNullOrEmpty(row["MakePhone"]))
                {
                    user.MakePhone = int.Parse(row["MakePhone"]);
                }
                if (!string.IsNullOrEmpty(row["SendLetter"]))
                {
                    user.SendLetter = int.Parse(row["SendLetter"]);
                }
                if (!string.IsNullOrEmpty(row["SendEmail"]))
                {
                    user.SendEmail = int.Parse(row["SendEmail"]);
                }
                user.TransactionTime = row["TransactionTime"];
                //新增字段
                if (!string.IsNullOrEmpty(row["MLevelBeginDate"]))
                {
                    user.MLevelBeginDate = DateTime.Parse(row["MLevelBeginDate"]);
                }
                if (!string.IsNullOrEmpty(row["MLevelInvalidDate"]))
                {
                    user.MLevelInvalidDate = DateTime.Parse(row["MLevelInvalidDate"]);
                }
                user.RankID = int.Parse(row["RankID"]);
                if (!string.IsNullOrEmpty(row["Amount"]))
                {
                    user.Amount = decimal.Parse(row["Amount"]);
                }
            }

            return user;
        }


        /// <summary>
        /// Returns a User ID given a user name
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public TUser GetUserByIdentityNunmber(string identityNunmber)
        {
            TUser user = null;
            string commandText = "Select * from membership where IdentityNumber = @identityNunmber";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@identityNunmber", identityNunmber } };

            var rows = _database.Query(commandText, parameters);
            if (rows != null && rows.Count == 1)
            {
                var row = rows[0];
                user = (TUser)Activator.CreateInstance(typeof(TUser));
                user.Id = row["Id"];
                user.UserName = row["UserName"];
                user.RealName = row["RealName"];
                user.CreatedPerson = row["CreatedPerson"];
                user.MLevel = row["MLevel"] == null ? 1 : int.Parse(row["MLevel"]);
                user.Status = int.Parse(row["Status"]);
                user.VIN = row["VIN"];
                user.ApprovalStatus = int.Parse(row["ApprovalStatus"]);
                user.PhoneNumber = row["PhoneNumber"];
                user.IdentityNumber = row["IdentityNumber"];
                user.Gender = row["Gender"];
                user.GenderName = string.IsNullOrEmpty(row["Gender"]) ? null : row["Gender"] == "1" ? "男" : "女";
                user.Provency = row["Provency"];
                user.City = row["City"];
                user.Area = row["Area"];
                user.Address = row["Address"];
                user.PasswordHash = string.IsNullOrEmpty(row["PasswordHash"]) ? null : row["PasswordHash"];
                user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
                user.Email = string.IsNullOrEmpty(row["Email"]) ? null : row["Email"];
                user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true : false;
                user.PhoneNumber = string.IsNullOrEmpty(row["PhoneNumber"]) ? null : row["PhoneNumber"];
                user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
                user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
                user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
                user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
                user.CreateTime = row["CreateTime"];
                user.FaceImage = row["FaceImage"];
                user.MType = string.IsNullOrEmpty(row["MType"]) ? 1 : int.Parse(row["MType"]);
                user.IsPay = string.IsNullOrEmpty(row["IsPay"]) ? 0 : int.Parse(row["IsPay"]);
                user.SystemMType = string.IsNullOrEmpty(row["SystemMType"]) ? 1 : int.Parse(row["SystemMType"]);
                user.ActiveWay = string.IsNullOrEmpty(row["ActiveWay"]) ? 1 : int.Parse(row["ActiveWay"]);
                user.IsNeedModifyPw = string.IsNullOrEmpty(row["IsNeedModifyPw"]) ? 0 : int.Parse(row["IsNeedModifyPw"]);
                user.NickName = row["NickName"];
                user.PayNumber = row["PayNumber"];
                user.Birthday = row["Birthday"];
                user.No = row["No"];
                //新增字段
                //user.AuthenticationTime = string.IsNullOrEmpty(row["AuthenticationTime"]) ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(row["AuthenticationTime"]);
                //20170303确认把车主认证字段的值修改为当前时间
                user.AuthenticationTime = string.IsNullOrEmpty(row["AuthenticationTime"]) ? DateTime.Now : Convert.ToDateTime(row["AuthenticationTime"]);
                user.AuthenticationSource = row["AuthenticationSource"];
                if (!string.IsNullOrEmpty(row["MLevelBeginDate"]))
                {
                    user.MLevelBeginDate = DateTime.Parse(row["MLevelBeginDate"]);
                }
                if (!string.IsNullOrEmpty(row["MLevelInvalidDate"]))
                {
                    user.MLevelInvalidDate = DateTime.Parse(row["MLevelInvalidDate"]);
                }

                if (!string.IsNullOrEmpty(row["Amount"]))
                {
                    user.Amount = decimal.Parse(row["Amount"]);
                }

            }

            return user;
        }

        //修改积分状态
        public int UpdateUserintegral(TUser user)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(@"update userintegral set datastate=@datastate,UpdateTime=GETDATE() from Membership join userintegral on Membership.Id=userintegral.userId WHERE Membership.Id = @userid;" +
               "update UserIntegralRecord set datastate=@datastate,UpdateTime=GETDATE() WHERE UserIntegralRecord.userid = @userid");
           // stringBuilder.AppendFormat(@"update UserIntegralRecord set datastate=@datastate from Membership join UserIntegralRecord on Membership.Id=UserIntegralRecord.userId WHERE Membership.Id = @userid");
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters = new Dictionary<string, object>
            {
                {"@datastate", user.datastate},
                 {"@userId", user.Id}
            };
            //parameters.Add("@datastate", user.datastate);
            //parameters.Add("@userId", user.Id);
            var commandText = stringBuilder;
            return _database.Execute(commandText.ToString(), parameters);
        }

        public TUser FindByIdUserintegral(string userId)
        {
            TUser user = null;
            string commandText = "Select membership.*,Membership_Schedule.*,j.datastate,j.userid from membership left join Membership_Schedule on Membership.Id=Membership_Schedule.MembershipId left join userintegral j on Membership.Id=j.userid where Membership.Id =@id";
            //string commandText = "Select * from membership inner join Membership_Schedule  on membership.Id=Membership_Schedule.MembershipId inner join IF_Customer on membership.IdentityNumber=IF_Customer.IdentityNumber inner join IF_Car  on IF_Customer.CustId=IF_Car.CustId where membership.Id = @id"; 
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

            var rows = _database.Query(commandText, parameters);
            if (rows != null)
            {
                var row = rows[0];
                user = (TUser)Activator.CreateInstance(typeof(TUser));
                //个人信息
                user.Id = row["Id"];
                user.UserName = row["UserName"];
                user.RealName = row["RealName"];
                user.CreatedPerson = row["CreatedPerson"];
                user.MLevel = row["MLevel"] == null ? 1 : int.Parse(row["MLevel"]);
                user.Status = int.Parse(row["Status"]);
                user.datastate = int.Parse(row["datastate"]);
                user.VIN = row["VIN"];
                user.ApprovalStatus = int.Parse(row["ApprovalStatus"]);
                user.PhoneNumber = row["PhoneNumber"];
                user.IdentityNumber = row["IdentityNumber"];
                user.Gender = row["Gender"];
                user.GenderName = string.IsNullOrEmpty(row["Gender"]) ? null : row["Gender"] == "1" ? "男" : "女";
                user.Provency = row["Provency"];
                user.City = row["City"];
                user.Area = row["Area"];
                user.Address = row["Address"];
                user.PasswordHash = string.IsNullOrEmpty(row["PasswordHash"]) ? null : row["PasswordHash"];
                user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
                user.Email = string.IsNullOrEmpty(row["Email"]) ? null : row["Email"];
                user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true : false;
                user.PhoneNumber = string.IsNullOrEmpty(row["PhoneNumber"]) ? null : row["PhoneNumber"];
                user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
                user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
                user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
                user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
                user.CreateTime = row["CreateTime"];
                user.FaceImage = row["FaceImage"];
                user.MType = string.IsNullOrEmpty(row["MType"]) ? 1 : int.Parse(row["MType"]);
                user.IsPay = string.IsNullOrEmpty(row["IsPay"]) ? 0 : int.Parse(row["IsPay"]);
                user.SystemMType = string.IsNullOrEmpty(row["SystemMType"]) ? 1 : int.Parse(row["SystemMType"]);
                user.ActiveWay = string.IsNullOrEmpty(row["ActiveWay"]) ? 1 : int.Parse(row["ActiveWay"]);
                user.IsNeedModifyPw = string.IsNullOrEmpty(row["IsNeedModifyPw"]) ? 0 : int.Parse(row["IsNeedModifyPw"]);
                user.NickName = row["NickName"];
                user.PayNumber = row["PayNumber"];
                user.No = row["No"];
                user.Interest = row["Interest"];
                user.UserType = row["UserType"];
                user.AuthenticationTime = string.IsNullOrEmpty(row["AuthenticationTime"]) ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(row["AuthenticationTime"]);
                user.AuthenticationSource = row["AuthenticationSource"];
                if (!string.IsNullOrEmpty(row["Birthday"]))
                {
                    user.Birthday = DateTime.Parse(row["Birthday"]).ToShortDateString();
                }
                //详细信息
                user.MembershipId = row["MembershipId"];
                user.ZipCode = row["ZipCode"];
                user.TelePhone = row["TelePhone"];
                user.PaperWork = row["PaperWork"];
                user.Educational = row["Educational"];
                user.Job = row["Job"];
                user.Office = row["Office"];
                user.Industry = row["Industry"];
                user.Remark = row["Remark"];
                user.IsMarriage = row["IsMarriage"];
                user.MarriageDay = row["MarriageDay"];
                user.MainContact = row["MainContact"];
                user.MainTelePhone = row["MainTelePhone"];
                user.OrganizationCode = row["OrganizationCode"];
                if (!string.IsNullOrEmpty(row["SendSms"]))
                {
                    user.SendSms = int.Parse(row["SendSms"]);
                }
                if (!string.IsNullOrEmpty(row["MakePhone"]))
                {
                    user.MakePhone = int.Parse(row["MakePhone"]);
                }
                if (!string.IsNullOrEmpty(row["SendLetter"]))
                {
                    user.SendLetter = int.Parse(row["SendLetter"]);
                }
                if (!string.IsNullOrEmpty(row["SendEmail"]))
                {
                    user.SendEmail = int.Parse(row["SendEmail"]);
                }
                user.TransactionTime = row["TransactionTime"];
                //新增字段
                if (!string.IsNullOrEmpty(row["MLevelBeginDate"]))
                {
                    user.MLevelBeginDate = DateTime.Parse(row["MLevelBeginDate"]);
                }
                if (!string.IsNullOrEmpty(row["MLevelInvalidDate"]))
                {
                    user.MLevelInvalidDate = DateTime.Parse(row["MLevelInvalidDate"]);
                }
                user.RankID = int.Parse(row["RankID"]);
                if (!string.IsNullOrEmpty(row["Amount"]))
                {
                    user.Amount = decimal.Parse(row["Amount"]);
                }
            }

            return user;
        }


        /// <summary>
        /// Returns a list of TUser instances given a user name
        /// </summary>
        /// <param name="userName">User's name</param>
        /// <returns></returns>
        public List<TUser> GetUserByName(string userName)
        {
            List<TUser> users = new List<TUser>();
            string commandText = "Select * from membership where UserName = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

            var rows = _database.Query(commandText, parameters);
            foreach (var row in rows)
            {
                TUser user = (TUser)Activator.CreateInstance(typeof(TUser));
                user.Id = row["Id"];
                user.UserName = row["UserName"];
                user.RealName = row["RealName"];
                user.Status = int.Parse(row["Status"]);
                user.CreatedPerson = row["CreatedPerson"];
                user.MLevel = row["MLevel"] == null ? 1 : int.Parse(row["MLevel"]);
                user.VIN = row["VIN"];
                user.No = row["No"];
                user.ApprovalStatus = int.Parse(row["ApprovalStatus"]);
                user.PhoneNumber = row["PhoneNumber"];
                user.IdentityNumber = row["IdentityNumber"];
                user.Gender = row["Gender"];
                user.Provency = row["Provency"];
                user.City = row["City"];
                user.Area = row["Area"];
                user.Address = row["Address"];
                user.PasswordHash = string.IsNullOrEmpty(row["PasswordHash"]) ? null : row["PasswordHash"];
                user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
                user.Email = string.IsNullOrEmpty(row["Email"]) ? null : row["Email"];
                user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true : false;
                user.PhoneNumber = string.IsNullOrEmpty(row["PhoneNumber"]) ? null : row["PhoneNumber"];
                user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
                user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
                user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
                user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
                user.FaceImage = row["FaceImage"];
                user.MType = string.IsNullOrEmpty(row["MType"]) ? 1 : int.Parse(row["MType"]);
                user.IsPay = string.IsNullOrEmpty(row["IsPay"]) ? 0 : int.Parse(row["IsPay"]);
                user.SystemMType = string.IsNullOrEmpty(row["SystemMType"]) ? 1 : int.Parse(row["SystemMType"]);
                user.ActiveWay = string.IsNullOrEmpty(row["ActiveWay"]) ? 1 : int.Parse(row["ActiveWay"]);
                user.IsNeedModifyPw = string.IsNullOrEmpty(row["IsNeedModifyPw"]) ? 0 : int.Parse(row["IsNeedModifyPw"]);
                user.NickName = row["NickName"];
                user.PayNumber = row["PayNumber"];
                user.UserType = row["UserType"];
                user.Birthday = row["Birthday"];

                user.AuthenticationTime = string.IsNullOrEmpty(row["AuthenticationTime"]) ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(row["AuthenticationTime"]);
                user.AuthenticationSource = row["AuthenticationSource"];
                if (!string.IsNullOrEmpty(row["MLevelBeginDate"]))
                {
                    user.MLevelBeginDate = DateTime.Parse(row["MLevelBeginDate"]);
                }
                if (!string.IsNullOrEmpty(row["MLevelInvalidDate"]))
                {
                    user.MLevelInvalidDate = DateTime.Parse(row["MLevelInvalidDate"]);
                }

                if (!string.IsNullOrEmpty(row["Amount"]))
                {
                    user.Amount = decimal.Parse(row["Amount"]);
                }

                users.Add(user);
            }

            return users;
        }

        /// <summary>
        /// 查询匹配的登陆人信息
        /// </summary>
        /// <param name="value">登陆值</param>
        /// <returns></returns>
        public List<TUser> FindLogin(string value)
        {
            List<TUser> dataResult = new List<TUser>();
            string sql = "Select * from membership where  Membership.UserName=@value and Status=1";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@value", value);
            var rows = _database.Query(sql, parameters);
            //if (rows == null || rows.Count == 0)
            //{
            //    string sql1 = "Select * from membership where  Membership.UserName=@value and Status=1";
            //    Dictionary<string, object> parameters1 = new Dictionary<string, object>();
            //    parameters1.Add("@value", value);
            //    rows = _database.Query(sql1, parameters1);
            //}
            foreach (var row in rows)
            {
                TUser user = (TUser)Activator.CreateInstance(typeof(TUser));
                user.Id = row["Id"];
                user.UserName = row["UserName"];
                user.RealName = row["RealName"];
                user.Status = int.Parse(row["Status"]);
                user.CreatedPerson = row["CreatedPerson"];
                user.MLevel = row["MLevel"] == null ? 1 : int.Parse(row["MLevel"]);
                user.VIN = row["VIN"];
                user.No = row["No"];
                user.ApprovalStatus = int.Parse(row["ApprovalStatus"]);
                user.PhoneNumber = row["PhoneNumber"];
                user.IdentityNumber = row["IdentityNumber"];
                //user.Gender = int.Parse(row["Gender"]);
                user.Provency = row["Provency"];
                user.City = row["City"];
                user.Area = row["Area"];
                user.Address = row["Address"];
                user.PasswordHash = string.IsNullOrEmpty(row["PasswordHash"]) ? null : row["PasswordHash"];
                user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
                user.Email = string.IsNullOrEmpty(row["Email"]) ? null : row["Email"];
                user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true : false;
                user.PhoneNumber = string.IsNullOrEmpty(row["PhoneNumber"]) ? null : row["PhoneNumber"];
                user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
                user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
                user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
                user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
                user.FaceImage = row["FaceImage"];
                user.MType = string.IsNullOrEmpty(row["MType"]) ? 1 : int.Parse(row["MType"]);
                user.IsPay = string.IsNullOrEmpty(row["IsPay"]) ? 0 : int.Parse(row["IsPay"]);
                user.SystemMType = string.IsNullOrEmpty(row["SystemMType"]) ? 1 : int.Parse(row["SystemMType"]);
                user.ActiveWay = string.IsNullOrEmpty(row["ActiveWay"]) ? 1 : int.Parse(row["ActiveWay"]);
                user.IsNeedModifyPw = string.IsNullOrEmpty(row["IsNeedModifyPw"]) ? 0 : int.Parse(row["IsNeedModifyPw"]);
                user.NickName = row["NickName"];
                user.PayNumber = row["PayNumber"];
                user.UserType = row["UserType"];
                user.AuthenticationTime = string.IsNullOrEmpty(row["AuthenticationTime"]) ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(row["AuthenticationTime"]);
                user.AuthenticationSource = row["AuthenticationSource"];
                if (!string.IsNullOrEmpty(row["MLevelBeginDate"]))
                {
                    user.MLevelBeginDate = DateTime.Parse(row["MLevelBeginDate"]);
                }
                if (!string.IsNullOrEmpty(row["MLevelInvalidDate"]))
                {
                    user.MLevelInvalidDate = DateTime.Parse(row["MLevelInvalidDate"]);
                }

                if (!string.IsNullOrEmpty(row["Amount"]))
                {
                    user.Amount = decimal.Parse(row["Amount"]);
                }
                dataResult.Add(user);
            }

            return dataResult;
        }

        public List<TUser> GetUserByEmail(string email)
        {
            List<TUser> users = new List<TUser>();
            string commandText = "Select * from membership where email = @email";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@email", email } };

            var rows = _database.Query(commandText, parameters);
            foreach (var row in rows)
            {
                TUser user = (TUser)Activator.CreateInstance(typeof(TUser));
                user.Id = row["Id"];
                user.UserName = row["UserName"];
                user.RealName = row["RealName"];
                user.Status = int.Parse(row["Status"]);
                user.CreatedPerson = row["CreatedPerson"];
                user.VIN = row["VIN"];
                user.No = row["No"];
                user.MLevel = row["MLevel"] == null ? 1 : int.Parse(row["MLevel"]);
                user.ApprovalStatus = int.Parse(row["ApprovalStatus"]);
                user.PhoneNumber = row["PhoneNumber"];
                user.IdentityNumber = row["IdentityNumber"];
                //user.Gender = int.Parse(row["Gender"]);
                user.Provency = row["Provency"];
                user.City = row["City"];
                user.Area = row["Area"];
                user.Address = row["Address"];
                user.PasswordHash = string.IsNullOrEmpty(row["PasswordHash"]) ? null : row["PasswordHash"];
                user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
                user.Email = string.IsNullOrEmpty(row["Email"]) ? null : row["Email"];
                user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true : false;
                user.PhoneNumber = string.IsNullOrEmpty(row["PhoneNumber"]) ? null : row["PhoneNumber"];
                user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
                user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
                user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
                user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
                user.FaceImage = row["FaceImage"];
                user.MType = string.IsNullOrEmpty(row["MType"]) ? 1 : int.Parse(row["MType"]);
                user.IsPay = string.IsNullOrEmpty(row["IsPay"]) ? 0 : int.Parse(row["IsPay"]);
                user.SystemMType = string.IsNullOrEmpty(row["SystemMType"]) ? 1 : int.Parse(row["SystemMType"]);
                user.ActiveWay = string.IsNullOrEmpty(row["ActiveWay"]) ? 1 : int.Parse(row["ActiveWay"]);
                user.IsNeedModifyPw = string.IsNullOrEmpty(row["IsNeedModifyPw"]) ? 0 : int.Parse(row["IsNeedModifyPw"]);
                user.NickName = row["NickName"];
                user.PayNumber = row["PayNumber"];
                user.AuthenticationTime = string.IsNullOrEmpty(row["AuthenticationTime"]) ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(row["AuthenticationTime"]);
                user.AuthenticationSource = row["AuthenticationSource"];
                if (!string.IsNullOrEmpty(row["MLevelBeginDate"]))
                {
                    user.MLevelBeginDate = DateTime.Parse(row["MLevelBeginDate"]);
                }
                if (!string.IsNullOrEmpty(row["MLevelInvalidDate"]))
                {
                    user.MLevelInvalidDate = DateTime.Parse(row["MLevelInvalidDate"]);
                }

                if (!string.IsNullOrEmpty(row["Amount"]))
                {
                    user.Amount = decimal.Parse(row["Amount"]);
                }
                users.Add(user);
            }

            return users;
        }

        public List<TUser> GetUserByNickName(string nickName)
        {
            List<TUser> users = new List<TUser>();
            string commandText = "Select * from membership where nickName = @nickName";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@nickName", nickName } };

            var rows = _database.Query(commandText, parameters);
            foreach (var row in rows)
            {
                TUser user = (TUser)Activator.CreateInstance(typeof(TUser));
                user.Id = row["Id"];
                user.UserName = row["UserName"];
                user.RealName = row["RealName"];
                user.Status = int.Parse(row["Status"]);
                user.CreatedPerson = row["CreatedPerson"];
                user.VIN = row["VIN"];
                user.No = row["No"];
                user.ApprovalStatus = int.Parse(row["ApprovalStatus"]);
                user.PhoneNumber = row["PhoneNumber"];
                user.IdentityNumber = row["IdentityNumber"];
                user.MLevel = row["MLevel"] == null ? 1 : int.Parse(row["MLevel"]);
                //user.Gender = int.Parse(row["Gender"]);
                user.Provency = row["Provency"];
                user.City = row["City"];
                user.Area = row["Area"];
                user.Address = row["Address"];
                user.PasswordHash = string.IsNullOrEmpty(row["PasswordHash"]) ? null : row["PasswordHash"];
                user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
                user.Email = string.IsNullOrEmpty(row["Email"]) ? null : row["Email"];
                user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true : false;
                user.PhoneNumber = string.IsNullOrEmpty(row["PhoneNumber"]) ? null : row["PhoneNumber"];
                user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
                user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
                user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
                user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
                user.FaceImage = row["FaceImage"];
                user.MType = string.IsNullOrEmpty(row["MType"]) ? 1 : int.Parse(row["MType"]);
                user.IsPay = string.IsNullOrEmpty(row["IsPay"]) ? 0 : int.Parse(row["IsPay"]);
                user.SystemMType = string.IsNullOrEmpty(row["SystemMType"]) ? 1 : int.Parse(row["SystemMType"]);
                user.ActiveWay = string.IsNullOrEmpty(row["ActiveWay"]) ? 1 : int.Parse(row["ActiveWay"]);
                user.IsNeedModifyPw = string.IsNullOrEmpty(row["IsNeedModifyPw"]) ? 0 : int.Parse(row["IsNeedModifyPw"]);
                user.NickName = row["NickName"];
                user.PayNumber = row["PayNumber"];
                user.AuthenticationTime = string.IsNullOrEmpty(row["AuthenticationTime"]) ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(row["AuthenticationTime"]);
                user.AuthenticationSource = row["AuthenticationSource"];
                if (!string.IsNullOrEmpty(row["MLevelBeginDate"]))
                {
                    user.MLevelBeginDate = DateTime.Parse(row["MLevelBeginDate"]);
                }
                if (!string.IsNullOrEmpty(row["MLevelInvalidDate"]))
                {
                    user.MLevelInvalidDate = DateTime.Parse(row["MLevelInvalidDate"]);
                }

                if (!string.IsNullOrEmpty(row["Amount"]))
                {
                    user.Amount = decimal.Parse(row["Amount"]);
                }
                users.Add(user);
            }

            return users;
        }

        /// <summary>
        /// Return the user's password hash
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public string GetPasswordHash(string userId)
        {
            string commandText = "Select PasswordHash from membership where Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Id", userId);

            var passHash = _database.GetStrValue(commandText, parameters);
            if (string.IsNullOrEmpty(passHash))
            {
                return null;
            }

            return passHash;
        }

        /// <summary>
        /// Sets the user's password hash
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public int SetPasswordHash(string userId, string passwordHash)
        {
            string commandText = "Update membership set PasswordHash = @pwdHash where Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@pwdHash", passwordHash);
            parameters.Add("@Id", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Returns the user's security stamp
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSecurityStamp(string userId)
        {
            string commandText = "Select SecurityStamp from membership where Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Id", userId } };
            var result = _database.GetStrValue(commandText, parameters);

            return result;
        }

        /// <summary>
        /// 根据VIN查询会员等级
        /// </summary>
        /// <param name="vin"></param>
        /// <param name="phone">购车电话</param>
        /// <returns></returns>
        public List<Dictionary<string, string>> FindLevel(string vin)
        {
            string sql = "select * from IF_Car where VIN like '%" + vin + "'";

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            //return int.Parse(_database.QueryValue(sql, parameters).ToString()) > 0;

            return _database.Query(sql, parameters);
        }



        /// <summary>
        /// Inserts a new user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Insert(TUser user)
        {
            /*const string commandText = @" IF NOT EXISTS(select * from Membership where IdentityNumber=@IdentityNumber ) Insert into Membership (UserName,RealName, Id, VIN, PasswordHash, SecurityStamp,Email,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed,IdentityNumber, AccessFailedCount,LockoutEnabled,LockoutEndDateUtc,TwoFactorEnabled,IsDel,Status,CreateTime,CreatedPerson,FaceImage,MType,IsPay,ApprovalStatus,ActiveWay,IsNeedModifyPw,NickName,PayNumber,MLevel,No,MLevelBeginDate,MLevelInvalidDate,Amount,SystemMType,Birthday)
                values (@name,@realname, @id, @VIN, @pwdHash, @SecStamp,@email,@emailconfirmed,@phonenumber,@phonenumberconfirmed,@IdentityNumber,@accesscount,@lockoutenabled,@lockoutenddate,@twofactorenabled,@IsDel,@Status,@CreateTime,@CreatedPerson,@FaceImage,@MType,@IsPay,@ApprovalStatus,@ActiveWay,@IsNeedModifyPw,@NickName,@PayNumber,@MLevel,@No,@MLevelBeginDate,@MLevelInvalidDate ,@Amount,@SystemMType,@Birthday )";
            var parameters = new Dictionary<string, object>
            {
                {"@name", user.UserName},
                {"@Birthday",user.Birthday},
                {"@realname", user.RealName},
                {"@id", user.Id},
                {"@VIN", user.VIN},
                {"@pwdHash", user.PasswordHash},
                {"@SecStamp", user.SecurityStamp},
                {"@email", user.Email},
                {"@emailconfirmed", user.EmailConfirmed},
                {"@phonenumber", user.PhoneNumber},
                {"@phonenumberconfirmed", user.PhoneNumberConfirmed},
                {"@IdentityNumber", user.IdentityNumber},
                {"@accesscount", user.AccessFailedCount},
                {"@lockoutenabled", user.LockoutEnabled},
                {"@lockoutenddate", user.LockoutEndDateUtc},
                {"@twofactorenabled", user.TwoFactorEnabled},
                {"@IsDel", 0},
                {"@Status", 1},
                {"@CreateTime", DateTime.Now},
                {"@CreatedPerson", user.CreatedPerson},
                {"@FaceImage",user.FaceImage},
                {"@MType",user.MType},
                {"@IsPay",user.IsPay},
                {"@ApprovalStatus", user.ApprovalStatus},
                {"@ActiveWay", user.ActiveWay},
                {"@IsNeedModifyPw", user.IsNeedModifyPw},
                {"@NickName", user.NickName},
                {"@PayNumber", user.PayNumber},
                {"@MLevel",user.MLevel},
                {"@No",user.No},
                {"@MLevelBeginDate",user.MLevelBeginDate},
                {"@MLevelInvalidDate",user.MLevelInvalidDate},
                {"@Amount",user.Amount },
                {"@SystemMType",user.SystemMType }
            };
            return _database.Execute(commandText, parameters);*/
            try
            {
            user.CreateTime = DateTime.Now.ToString();
                string commandText=string.Empty;
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

        //public int InsertMembershipToForum(TUser user)
        //{
        //    System.Web.Security.MembershipCreateStatus status;

        //    var provider = new YAF.Providers.Membership.YafMembershipProvider();
        //    var forumUser = provider.CreateUser(user.PhoneNumber, user.Password, user.Email, user.PhoneNumber, user.PhoneNumber, true, null,
        //        out status);
        //    // create the user in the YAF DB as well as sync roles...
        //    int? userID = YAF.Core.RoleMembershipHelper.CreateForumUser(forumUser, 1);

        //    return (int)status;
        //}

        /// <summary>
        /// 通过中间表，检查
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool CheckMembershipValid(TUser user)
        {
            const string commandText = @"select * from IF_Customer cus inner join IF_Car car on cus.custid=car.id where cus.CustMobile=@CustMobile and cus.IdentityNumber=@IdentityNumber and car.VIN=@VIN";
            var parameters = new Dictionary<string, object>
            {
                {"@CustMobile", user.PhoneNumber},
                {"@IdentityNumber", user.IdentityNumber},
                {"@VIN", user.VIN}
            };
            return _database.Execute(commandText, parameters) > 0;
        }

        /// <summary>
        /// 更新会员账号状态
        /// </summary>
        /// <param name="user">TUser</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateMembershipStatus(TUser user, string status)
        {
            if (status == null) throw new ArgumentNullException("status");
            const string commandText = @"update Membership set Status=@Status where phonenumber=@phonenumber and VIN=@VIN";
            var parameters = new Dictionary<string, object>
            {
                {"@Status", status},
                {"@phonenumber", user.PhoneNumber},
                {"@VIN", user.VIN}
            };
            return _database.Execute(commandText, parameters) > 0;
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        private int Delete(string userId)
        {
            const string commandText = "Delete from membership where Id = @Id";
            var parameters = new Dictionary<string, object> { { "@Id", userId } };
            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Delete(TUser user)
        {
            return Delete(user.Id);
        }

        /// <summary>
        /// Updates a user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update(TUser user)
        {
            string commandText = @"
UPDATE [Membership]
   SET [UserName] =             @UserName
      ,[RealName] =             @RealName
      ,[Email] =                @Email
      ,[EmailConfirmed] =       @EmailConfirmed
      ,[PasswordHash] =         @PasswordHash
      ,[SecurityStamp] =        @SecurityStamp
      ,[PhoneNumber] =          @PhoneNumber
      ,[PhoneNumberConfirmed] = @PhoneNumberConfirmed
      ,[TwoFactorEnabled] =     @TwoFactorEnabled
      ,[LockoutEndDateUtc] =    @LockoutEndDateUtc
      ,[LockoutEnabled] =       @LockoutEnabled
      ,[AccessFailedCount] =    @AccessFailedCount
      ,[IsDel] =                @IsDel
      ,[Age] =                  @Age
      ,[Birthday] =             @Birthday
      ,[ApprovalStatus] =       @ApprovalStatus
      ,[MLevel] =               @MLevel
      ,[VIN] =                  @VIN
      ,[IdentityNumber] =       @IdentityNumber
      ,[Gender] =               @Gender
      ,[FaceImage] =            @FaceImage
      ,[Status] =               @Status
      ,[Provency] =             @Provency
      ,[City] =                 @City
      ,[Area] =                 @Area
      ,[Address] =              @Address
      ,[CreatedPerson] =        @CreatedPerson
      ,[UpdateTime] =           @UpdateTime
      ,[No] =                   @No
      ,[MType] =                @MType
      ,[SystemMType] =          @SystemMType
      ,[IsPay] =                @IsPay
      ,[ActiveWay] =            @ActiveWay
      ,[IsNeedModifyPw] =       @IsNeedModifyPw
      ,[NickName] =             @NickName
      ,[PayNumber] =            @PayNumber 
      ,[Amount]=@Amount
      ,[AuthenticationTime]=    @AuthenticationTime
      ,[AuthenticationSource]=  @AuthenticationSource
";
            if (user.AuthenticationTime.Year != 1900)
            {
                if (user.MLevel == 11 || user.MLevel == 12)
                {
                    commandText += string.Format(",[MLevelBeginDate]=@MLevelBeginDate,[MLevelInvalidDate]=@MLevelInvalidDate");
                }
                else
                {
                    commandText += string.Format(",[MLevelBeginDate]=@MLevelBeginDate");                
                }                
            }
            commandText += " WHERE Id = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserName", user.UserName);
            parameters.Add("@RealName", user.RealName);
            parameters.Add("@Email", user.Email);
            parameters.Add("@EmailConfirmed", user.EmailConfirmed);
            parameters.Add("@PasswordHash", user.PasswordHash);
            parameters.Add("@SecurityStamp", user.SecurityStamp);
            parameters.Add("@PhoneNumber", user.PhoneNumber);
            parameters.Add("@PhoneNumberConfirmed", user.PhoneNumberConfirmed);
            parameters.Add("@TwoFactorEnabled", user.TwoFactorEnabled);
            parameters.Add("@LockoutEndDateUtc", user.LockoutEndDateUtc);
            parameters.Add("@LockoutEnabled", user.LockoutEnabled);
            parameters.Add("@AccessFailedCount", user.AccessFailedCount);
            parameters.Add("@IsDel", user.IsDel);
            parameters.Add("@Age", user.Age);
            parameters.Add("@Birthday", user.Birthday);
            parameters.Add("@ApprovalStatus", user.ApprovalStatus);
            parameters.Add("@MLevel", user.MLevel);
            parameters.Add("@VIN", user.VIN);
            parameters.Add("@IdentityNumber", user.IdentityNumber);
            parameters.Add("@Gender", user.Gender);
            parameters.Add("@FaceImage", user.FaceImage);
            parameters.Add("@Status", user.Status);
            parameters.Add("@Provency", user.Provency);
            parameters.Add("@City", user.City);
            parameters.Add("@Area", user.Area);
            parameters.Add("@Address", user.Address);
            parameters.Add("@CreatedPerson", user.CreatedPerson);
            parameters.Add("@UpdateTime", DateTime.Now);
            parameters.Add("@No", user.No);
            parameters.Add("@MType", user.MType);
            parameters.Add("@SystemMType", user.SystemMType);
            parameters.Add("@IsPay", user.IsPay);
            parameters.Add("@ActiveWay", user.ActiveWay);
            parameters.Add("@IsNeedModifyPw", user.IsNeedModifyPw);
            parameters.Add("@NickName", user.NickName);
            parameters.Add("@PayNumber", user.PayNumber);
            parameters.Add("@userId", user.Id);
            if (user.AuthenticationTime.Year != 1900)
            {
                if (user.MLevel == 11 || user.MLevel == 12)
                {
                    parameters.Add("@MLevelBeginDate", user.AuthenticationTime.Date);
                    parameters.Add("@MLevelInvalidDate", user.AuthenticationTime.AddYears(1).Date);
                }
                else {
                    parameters.Add("@MLevelBeginDate", user.AuthenticationTime.Date);
                }                
            }
            //parameters.Add("@MLevelBeginDate", user.MLevelBeginDate);
            //parameters.Add("@MLevelInvalidDate", user.MLevelInvalidDate);
            parameters.Add("@Amount", user.Amount);
            parameters.Add("@AuthenticationTime", user.AuthenticationTime);
            parameters.Add("@AuthenticationSource", user.AuthenticationSource);
            return _database.Execute(commandText, parameters);
        }


        public int Update2(TUser user)
        {
            string commandText = @"
        UPDATE [UpMembership_Procedure]
    SET [UserName] =             @UserName
      ,[RealName] =             @RealName
      ,[Email] =                @Email
      ,[Birthday] =             @Birthday
      ,[Gender] =               @Gender
      ,[Provency] =             @Provency
      ,[City] =                 @City
      ,[Area] =                 @Area
      ,[Address] =              @Address
       ,[TelePhone] =            @TelePhone
       ,[TransactionTime] =      @TransactionTime
       ,[Job] =                  @Job
       ,[Office] =              @Office
      ,[IsMarriage] =            @IsMarriage
      ,[MarriageDay] =           @MarriageDay
      ,[Educational] =           @Educational
      ,[Remark] =                @Remark
       ,[UpdateTime] =           @UpdateTime
        WHERE Id =                @userId
";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@UserName", user.UserName);
            parameters.Add("@RealName", user.RealName);
            parameters.Add("@Email", user.Email);
            parameters.Add("@Birthday", user.Birthday);
            parameters.Add("@Gender", user.Gender);
            parameters.Add("@Provency", user.Provency);
            parameters.Add("@City", user.City);
            parameters.Add("@Area", user.Area);
            parameters.Add("@Address", user.Address);
            parameters.Add("@TelePhone", user.TelePhone);
            parameters.Add("@TransactionTime", user.TransactionTime);
            parameters.Add("@Job", user.Job);
            parameters.Add("@Office", user.Office);
            parameters.Add("@IsMarriage", user.IsMarriage);
            parameters.Add("@MarriageDay", user.MarriageDay);
            parameters.Add("@Educational", user.Educational);
            parameters.Add("@UpdateTime", DateTime.Now);
            parameters.Add("@Remark", user.Remark);
            parameters.Add("@userId", user.Id);
            return _database.Execute(commandText, parameters);
        }
        public bool CreateMembershipRequest(string membershipId, string identityNumber, string dealerId, string memo, string dataSource)
        {
            //var commandText = new StringBuilder("select cardealer.dealerid from [IF_Customer] cust " +
            //    "inner join IF_Car car on cust.custid=car.custid " +
            //    "inner join IF_CarDealerShip cardealer on car.dealerid=cardealer.dealerid " +
            //    "where cust.identitynumber=@identityNumber and car.CarCategory=@CarCategory");
            //var parameters = new Dictionary<string, object>
            //{
            //    {"@identityNumber", identityNumber},
            //    {"@CarCategory", CommonConst.SONATA9},
            //};

            //var dealerList = new List<string>();
            //_database.Query(commandText.ToString(), parameters).ForEach((e) =>
            //{
            //    dealerList.Add(e["dealerid"].ToString());
            //});

            //if (dealerList.Count > 0)
            //{
            var insertText = new StringBuilder("INSERT INTO [dbo].[MembershipRequest] ([MembershipId],[DealerId],[Status],[CreateTime],[UpdateTime],[Memo],[DataSource]) VALUES (@MembershipId,@DealerId,@Status,@CreateTime,@UpdateTime,@Memo,@DataSource)");
            var parameters1 = new Dictionary<string, object>
            {
                {"@MembershipId", membershipId},
                {"@DealerId", dealerId},
                {"@Status", 1},
                {"@CreateTime", DateTime.Now},
                {"@UpdateTime", DateTime.Now},
                {"@Memo", memo},
                {"@DataSource",dataSource}
            };
            return _database.Execute(insertText.ToString(), parameters1) > 0;
            //}
            //return false;
        }


        /// <summary>
        /// 获取经销商入会的需要审核的信息
        /// </summary>
        /// <param name="dealerId">经销商ID</param>
        /// <param name="identityNumber">会员证件号码</param>
        /// <returns>经销商入会的需要审核的信息列表</returns>
        public List<MembershipRequestApproval> GetApprovingMembershipList(string dealerId, string identityNumber)
        {
            string sql = @"  select  a.id as ApprovalId ,a.DealerId,a.MembershipId,a.Status as ApprStatus from MembershipRequest a join Membership b on a.membershipid=b.id  
                             where   b.ispay!=1  and  a.DealerId =@DealerId
                             and b.IdentityNumber=@IdentityNumber";
            var parameters = new Dictionary<string, object>
            {
                {"@DealerId", dealerId},
                {"@IdentityNumber", identityNumber},

            };
            var approvalList = _database.Query(sql, parameters);
            var userList = new List<MembershipRequestApproval>();
            foreach (var li in approvalList)
            {
                var app = new MembershipRequestApproval
                {
                    MembershipId = li["MembershipId"],
                    ApprovalId = li["ApprovalId"],
                    Status = li["ApprStatus"],
                    DealerId = li["DealerId"]
                };
                userList.Add(app);
            }
            return userList;
        }




        //缴费获取积分会员
        public List<MembershipRequestApproval> FindApprovingMembership(string PayNumber, string phoneNumber, string dealerId, string IdentityNumber, string ApproveType, int skip, int count, out int totalCount, int IsPay, decimal Amount, string PaperWork, string VINNumber,string No)
        {
            var commandText1 = "select count(*)    FROM membership ms " +
                " inner join MembershipRequest msr on ms.id=msr.membershipid {0} " +
                  " left join IF_Customer cust on cust.IdentityNumber=ms.IdentityNumber " +
                 " {3} " +
                " {2} " +
                " where 1=1 {1} ";
            var whereExp = new StringBuilder();
          
            var sql = "select * from (select row_number() over (order by ms.id) as row_num,ms.id as MembershipId, cust.CustName as RealName, case when ms.IsPay = 1 then '已缴费' when  ms.IsPay = 2  then '审核中' else '未缴费' end IsPay, ms.PayNumber, ms.PhoneNumber, ms.IdentityNumber, msr.DealerId,msr.id as ApprovalId, msr.createtime as SubmitTime, msr.status as ApprStatus, msr.memo,ms.City,ms.Address,ms.Amount,ms.No  FROM membership ms " +
                " inner join MembershipRequest msr on ms.id=msr.membershipid {0} " +
                 " left join IF_Customer cust on cust.IdentityNumber=ms.IdentityNumber " +
                  " {3} " +
                " {2} " +
                " where 1=1 {1}) u " +
                " where 1=1 and row_num between @PageIndex and @PageEnd order by u.SubmitTime desc";
           
            var commandText = new StringBuilder();
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                commandText.Append(" AND PhoneNumber = @PhoneNumber ");
                whereExp.Append(" AND PhoneNumber = @PhoneNumber ");
            }
            if (!string.IsNullOrEmpty(PayNumber))
            {
                commandText.Append(" AND PayNumber = @PayNumber ");
                whereExp.Append(" AND PayNumber = @PayNumber ");
            }
            if (!string.IsNullOrEmpty(IdentityNumber))
            {
                commandText.Append(" AND IdentityNumber = @IdentityNumber ");
                whereExp.Append(" AND IdentityNumber = @IdentityNumber ");
            }
            if (!string.IsNullOrEmpty(No))
            {
                commandText.Append(" AND ms.No = @No ");
                whereExp.Append(" AND ms.No = @No ");
            }
            if (!string.IsNullOrEmpty(ApproveType))
            {
                commandText.Append(" AND msr.Status = @ApproveType ");
                whereExp.Append(" AND msr.Status = @ApproveType ");
            }

            if (IsPay == 0 || IsPay == 1 || IsPay == 2)
            {
                commandText.Append(" AND ms.IsPay = @IsPay ");
                whereExp.Append(" AND ms.IsPay = @IsPay ");
            }
            if (Amount == 50 || Amount == 100)
            {
                commandText.Append(" AND ms.Amount = @Amount ");
                whereExp.Append(" AND ms.Amount = @Amount ");
            }
            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(dealerId))
            {
                sb1.Append(" AND msr.DealerId = @DealerId ");
                sb.Append(" AND msr.DealerId = @DealerId ");
            }
            string pwroketable = "";
            if (!string.IsNullOrEmpty(PaperWork))
            {
                pwroketable = " left join Membership_Schedule kz on ms.id=kz.membershipid ";
                sb1.Append(" AND kz.PaperWork = @PaperWork ");
                sb.Append(" AND kz.PaperWork = @PaperWork ");
            }
            StringBuilder ifcar = new StringBuilder();
            if(!string.IsNullOrEmpty(VINNumber))
            {
                ifcar.Append(" left join IF_Car car on car.custid=cust.custid ");
                sb1.Append(" AND car.VIN=@VINNumber ");
                sb.Append(" AND car.VIN=@VINNumber");
            }
            sql = string.Format(sql, whereExp, sb,ifcar.ToString(),pwroketable);

            var parameters = new Dictionary<string, object>
            {
                {"@isDel", 0},
                {"@DealerId", dealerId},
                {"@PhoneNumber", phoneNumber},
                {"@IdentityNumber", IdentityNumber},
                {"@PayNumber", PayNumber},
                 
                {"@PaperWork", PaperWork},
                {"@IsPay", IsPay},
                {"@Amount", Amount},
                {"@VINNumber",VINNumber},
                {"@No",No},
                {"@ApproveType", ApproveType},
                {"@PageIndex", skip + 1},
                {"@PageEnd", count + skip}
            };
            commandText1 = string.Format(commandText1, commandText, sb1,ifcar.ToString(),pwroketable);
            int.TryParse(_database.QueryValue(commandText1, parameters).ToString(), out totalCount);
            var approvalList = _database.Query(sql.ToString(), parameters);
            var userList = new List<MembershipRequestApproval>();
            foreach (var li in approvalList)
            {
                var app = new MembershipRequestApproval
                {
                    MembershipId = li["MembershipId"],
                    ApprovalId = li["ApprovalId"],
                    RealName = li["RealName"],
                    //VIN = li["VIN"],
                    IdentityNumber = li["IdentityNumber"],
                    PhoneNumber = li["PhoneNumber"],
                    SubmitTime = li["SubmitTime"],
                    Status = li["ApprStatus"],
                    IsPay = li["IsPay"].ToString(),
                    Amount = Convert.ToDecimal(li["Amount"]),
                    PayNumber = li["PayNumber"],
                    No = li["No"],
                    City = li["City"],
                    Address = li["Address"],
                    DealerId = li["DealerId"]
                };
                userList.Add(app);
            }
            return userList;
        }

        private int ResourceLy(string identity)
        {

            CarTypeReturnIntegral CarType = _AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(identity);
            string type = CarType.ToString();
            if (string.Equals(type, "DS", StringComparison.InvariantCultureIgnoreCase) || string.Equals(type, "NoDS", StringComparison.InvariantCultureIgnoreCase))
            {
                return 9;
            }
            else if (string.Equals(type, "DSAdd", StringComparison.InvariantCultureIgnoreCase) || string.Equals(type, "NoDSAdd", StringComparison.InvariantCultureIgnoreCase))
            {
                return 10;
            }
            return 0;

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

        public bool ManagerMembershipRequest(string id, out string message, string phone, string SubmitTime)
        {
            message = null;
            string membershipId = DbHelp.ExecuteScalar<string>(
                "select membershipid from MembershipRequest a join Membership b on a.membershipid=b.id  where a.id=@id and  b.ispay!=1", new { id = id });
            if (string.IsNullOrEmpty(membershipId))
            {
                message = "通过认证失败,该会员已经交费认证成功！";
                return false;
            }
            var userInfo = this.GetUserById(membershipId);
            List<Car> Vins = new List<Car>();
            var intergration = _AppContext.CarServiceUserApp.GetJoinIntegralByIdentity(userInfo.IdentityNumber, Convert.ToDateTime(SubmitTime), Vins);//

           

            var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(userInfo.IdentityNumber).ToList<Car>();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(
                @"update Membership set ApprovalStatus=2, IsPay=1 where id='{0}'and status=1;update MembershipRequest set status=3 where id=@id and status=1;",
                membershipId);

            bool flag = IsPersonalUser(userInfo.IdentityNumber);
            if (flag)
            {
                if (Vins.Count() > 0)
                {
                    _AppContext.UserIntegralApp.AddVin(Vins, userInfo.IdentityNumber, userInfo.Id);
                }

                stringBuilder.AppendFormat(
           @"INSERT INTO  userintegral 
           (userId
           ,integralSource
           ,value
           ,usevalue
           ,datastate
           ,remark
           ,CreateTime
           ,UpdateTime
           ,IsSend
           ,IntegralBeginDate
           ,IntegralInvalidDate)
     VALUES
           ('{0}'
           ,'{2}'
           ,{1}
           ,0
           ,0
           ,'{3}'
           ,getdate()
           ,GETDATE()
           ,'1'
           , CONVERT(varchar(12), getdate(), 111 ),
           CONVERT(varchar(12), DATEADD(yyyy,2,getdate()), 111 ))", membershipId, intergration, (carList.Count == 1 ? 9 : 10), (carList.Count == 1 ? "新购" : "增购"));

                //插入统计记录表
                stringBuilder.AppendFormat(
           @"INSERT INTO  UserIntegralRecord 
           (userId
           ,integralSource
           ,value
           ,usevalue
           ,datastate
           ,remark
           ,CreateTime
           ,UpdateTime
           ,IsSend
           ,IntegralBeginDate
           ,IntegralInvalidDate)
     VALUES
           ('{0}'
           ,'{2}'
           ,{1}
           ,0
           ,0
           ,'{3}'
           ,getdate()
           ,GETDATE()
           ,'1'
           , CONVERT(varchar(12), getdate(), 111 ),
           CONVERT(varchar(12), DATEADD(yyyy,2,getdate()), 111 ))", membershipId, intergration, (carList.Count == 1 ? 9 : 10), "缴费返积分");
            }

            var commandText = stringBuilder;
            var parameters = new Dictionary<string, object>
            {
                {"@id", id}
            };
            bool result = true;
            result = _database.Execute(commandText.ToString(), parameters) > 0;
            if (flag)
            {
                _AppContext.UserMessageRecordApp.Insert(new UserMessageRecord()
                {
                    MsgType = MessageType.IntegralConsum,
                    MsgContent = string.Format("您好，您已成功领取增换购缴费返积分{0}", intergration),
                    UserId = membershipId

                });

                if (result)
                {

                    _AppContext.SMSApp.SendSMS(ESmsType.通过认证, phone, new string[] { (intergration / 10) + "" });

                }

            }

            if (!result)
                message = "通过认证失败,未找到此会员或会员状态不正确!";
            return result;

        }

        public bool ApprovalMembershipRequestman(string id, out string message, string phone)
        {
            message = null;
            string membershipId = DbHelp.ExecuteScalar<string>(
                "select membershipid from MembershipRequest a join Membership b on a.membershipid=b.id  where a.id=@id and  b.ispay!=1", new { id = id });
            if (string.IsNullOrEmpty(membershipId))
            {
                message = "通过认证失败,该会员已经交费认证成功！";
                return false;
            }
            var userInfo = this.GetUserById(membershipId);
            var intergration = _AppContext.CarServiceUserApp.GetIntegrationByBuyCarPayMoney(userInfo.IdentityNumber);
            int value = ResourceLy(userInfo.IdentityNumber);
            var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(userInfo.IdentityNumber).ToList<Car>();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(
                @"update Membership set ApprovalStatus=2, IsPay=1 where id='{0}'and status=1;update MembershipRequest set status=3 where id=@id and status=1;",
                membershipId);

            bool flag = IsPersonalUser(userInfo.IdentityNumber);
            if (flag)
            {
                stringBuilder.AppendFormat(
           @"INSERT INTO  userintegral 
           (userId
           ,integralSource
           ,value
           ,usevalue
           ,datastate
           ,remark
           ,CreateTime
           ,UpdateTime
           ,IsSend
           ,IntegralBeginDate
           ,IntegralInvalidDate)
     VALUES
           ('{0}'
           ,'{2}'
           ,{1}
           ,0
           ,0
           ,'{3}'
           ,getdate()
           ,GETDATE()
           ,'1'
           , CONVERT(varchar(12), getdate(), 111 ),
           CONVERT(varchar(12), DATEADD(yyyy,2,getdate()), 111 ))", membershipId, intergration, (carList.Count == 1 ? 9 : 10), (carList.Count == 1 ? "新购" : "增购"));

                //插入统计记录表
                stringBuilder.AppendFormat(
           @"INSERT INTO  UserIntegralRecord 
           (userId
           ,integralSource
           ,value
           ,usevalue
           ,datastate
           ,remark
           ,CreateTime
           ,UpdateTime
           ,IsSend
           ,IntegralBeginDate
           ,IntegralInvalidDate)
     VALUES
           ('{0}'
           ,'{2}'
           ,{1}
           ,0
           ,0
           ,'{3}'
           ,getdate()
           ,GETDATE()
           ,'1'
           , CONVERT(varchar(12), getdate(), 111 ),
           CONVERT(varchar(12), DATEADD(yyyy,2,getdate()), 111 ))", membershipId, intergration, (carList.Count == 1 ? 9 : 10), "缴费返积分");
            }

            var commandText = stringBuilder;
            var parameters = new Dictionary<string, object>
            {
                {"@id", id}
            };
            bool result = true;
            result = _database.Execute(commandText.ToString(), parameters) > 0;
            if (flag)
            {
            _AppContext.UserMessageRecordApp.Insert(new UserMessageRecord()
            {
                MsgType = MessageType.IntegralConsum,
                MsgContent = string.Format("您好，您已成功领取增换购缴费返积分{0}", intergration),
                UserId = membershipId

            });

            if (result)
            {
              
                _AppContext.SMSApp.SendSMS(ESmsType.通过认证, phone, new string[] { (intergration / 10) + "" });
               
            }
                
            }

            if (!result)
                message = "通过认证失败,未找到此会员或会员状态不正确!";
            return result;

        }
        public bool ApprovalMembershipRequest(string id, out string message)
        {
            message = null;
            string membershipId = DbHelp.ExecuteScalar<string>(
                "select membershipid from MembershipRequest a join Membership b on a.membershipid=b.id  where a.id=@id and  b.ispay!=1", new { id = id });
            if (string.IsNullOrEmpty(membershipId))
            {
                message = "通过认证失败,该会员已经交费认证成功！";
                return false;
            }
            var userInfo = this.GetUserById(membershipId);
            var intergration = _AppContext.CarServiceUserApp.GetIntegrationByBuyCarPayMoney(userInfo.IdentityNumber);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(
                @"update Membership set ApprovalStatus=2, IsPay=1 where id='{0}'and status=1;update MembershipRequest set status=3 where id=@id and status=1;
INSERT INTO  userintegral
           (userId
           ,integralSource
           ,value
           ,usevalue
           ,datastate
           ,remark
           ,CreateTime
           ,UpdateTime
           ,IsSend
           ,IntegralBeginDate
           ,IntegralInvalidDate)
     VALUES
           ('{0}'
           ,'0'
           ,{1}
           ,0
           ,0
           ,'增换购缴费返积分'
           ,getdate()
           ,GETDATE()
           ,'1'
           , CONVERT(varchar(12), getdate(), 111 ),
           CONVERT(varchar(12), DATEADD(yyyy,2,getdate()), 111 ))", membershipId, intergration);
            var commandText = stringBuilder;
            var parameters = new Dictionary<string, object>
            {
                {"@id", id}
            };
            _AppContext.UserMessageRecordApp.Insert(new UserMessageRecord()
            {
                MsgType = MessageType.IntegralConsum,
                MsgContent = string.Format("您好，您已成功领取增换购缴费返积分{0}", intergration),
                UserId = membershipId

            });
            bool result = _database.Execute(commandText.ToString(), parameters) > 0;
            if (!result)
                message = "通过认证失败,未找到此会员或会员状态不正确!";
            return result;

        }

        public bool RejectMembershipRequest(string id, out string message)
        {
            message = null;
            var commandText = new StringBuilder("update MembershipRequest set status=4 where membershipid=@membershipid and status=1;" +
                                                "update Membership set ApprovalStatus=4 where id=@membershipid and status=1; ");
            var parameters = new Dictionary<string, object>
            {
                {"@membershipid", id}
            };
            bool result = _database.Execute(commandText.ToString(), parameters) > 0;
            if (!result)
                message = "拒绝认证失败,未找到此会员或会员状态为已拒绝!";
            return _database.Execute(commandText.ToString(), parameters) > 0;
        }

        public List<TUser> GetExtraUsers(FrontUserQueryEntity query, out int totalCount)
        {
            var whereExp = new StringBuilder();
            var commandText = new StringBuilder("SELECT COUNT(1) FROM membership ms inner join IF_Customer ifcus on ms.identitynumber=ifcus.identitynumber " +
                                                "inner join IF_Car ifcar on ifcus.custid=ifcar.custid where ifcar.vin=@VIN and ms.IsDel=@isDel {0}");
            var sql = new StringBuilder("select * from (select row_number() over (order by ms.id) as row_num,ms.* from membership  ms inner join IF_Customer ifcus on ms.identitynumber=ifcus.identitynumber " +
                                        "inner join IF_Car ifcar on ifcus.custid=ifcar.custid where ifcar.vin=@VIN and ms.IsDel=@isDel {0}) u " +
                                        "where row_num between @PageIndex and @PageEnd	");
            if (!string.IsNullOrEmpty(query.MLevel) && query.MLevel != "-1")
            {
                whereExp.Append(" AND ms.MLevel = @MLevel ");
            }
            if (!string.IsNullOrEmpty(query.Status) && query.Status != "-1")
            {
                whereExp.Append(" AND Status=@Status ");
            }
            if (query.IsSonata9)
            {
                whereExp.Append(" AND [No] is not null");
            }
            if (query.BeginTime != null && query.EndTime != null)
            {
                whereExp.AppendFormat(" AND ms.CreateTime >='{0}' AND ms.CreateTime <='{1}' ", query.BeginTime, query.EndTime);
            }
            if (!string.IsNullOrEmpty(query.IDCard))
            {
                whereExp.AppendFormat(" AND IdentityNumber = @IdentityNumber ");
            }
            if (!string.IsNullOrEmpty(query.IsTmall))
            {
                if (query.IsTmall == "1")
                    whereExp.AppendFormat(" AND payNumber is not null ");
                else//"2"
                    whereExp.AppendFormat(" AND payNumber is null ");
            }
            if (!string.IsNullOrEmpty(query.PayNumber))
            {
                whereExp.AppendFormat(" AND payNumber = @PayNumber ");
            }
            var parameters = new Dictionary<string, object>
            {
                {"@isDel", 0},
                {"@RealName", query.RealName},
                {"@NickName", query.NickName},
                {"@VIN", query.VIN},
                {"@MLevel", query.MLevel},
                {"@Status", query.Status},
                {"@PayNumber",query.PayNumber},
                {"@PageIndex", query.Skip + 1},
                {"@PageEnd", query.Count + query.Skip},
                {"@IdentityNumber", query.IDCard}
            };
            int.TryParse(_database.QueryValue(string.Format(commandText.ToString(), whereExp.ToString()), parameters).ToString(), out totalCount);
            var users = _database.Query(string.Format(sql.ToString(), whereExp), parameters);

            var userList = new List<TUser>();
            foreach (var user in users)
            {
                var identityUser = (TUser)Activator.CreateInstance(typeof(TUser));
                identityUser.Id = user["Id"];
                identityUser.UserName = user["UserName"];
                identityUser.RealName = user["RealName"];
                identityUser.IdentityNumber = user["IdentityNumber"];
                identityUser.Email = user["Email"];
                identityUser.PhoneNumber = user["PhoneNumber"];
                identityUser.CreateTime = user["CreateTime"];
                identityUser.Status = int.Parse(user["Status"]);
                identityUser.MLevel = (int.Parse(user["MLevel"]) == 3 && !string.IsNullOrEmpty(user["No"])) ? 9 : int.Parse(user["MLevel"]);
                identityUser.CreatedPerson = user["CreatedPerson"];
                identityUser.VIN = user["VIN"];
                identityUser.MType = string.IsNullOrEmpty(user["MType"]) ? 1 : int.Parse(user["MType"]);
                identityUser.SystemMType = string.IsNullOrEmpty(user["SystemMType"]) ? 1 : int.Parse(user["SystemMType"]);
                identityUser.IsPay = string.IsNullOrEmpty(user["IsPay"]) ? 0 : int.Parse(user["IsPay"]);
                identityUser.ApprovalStatus = string.IsNullOrEmpty(user["ApprovalStatus"]) ? 0 : int.Parse(user["ApprovalStatus"]);
                identityUser.ActiveWay = string.IsNullOrEmpty(user["ActiveWay"]) ? 1 : int.Parse(user["ActiveWay"]);
                identityUser.IsNeedModifyPw = string.IsNullOrEmpty(user["IsNeedModifyPw"]) ? 0 : int.Parse(user["IsNeedModifyPw"]);
                identityUser.NickName = user["NickName"];
                identityUser.PayNumber = user["PayNumber"];
                identityUser.Age = int.Parse(user["Age"]);
                identityUser.GenderName = string.IsNullOrEmpty(user["Gender"]) ? null : user["Gender"] == "1" ? "男" : "女"; ;
                identityUser.City = user["City"];
                identityUser.Area = user["Area"];
                identityUser.No = user["No"];
                if (!string.IsNullOrEmpty(user["MLevelBeginDate"]))
                {
                    identityUser.MLevelBeginDate = DateTime.Parse(user["MLevelBeginDate"]);
                }
                if (!string.IsNullOrEmpty(user["MLevelInvalidDate"]))
                {
                    identityUser.MLevelInvalidDate = DateTime.Parse(user["MLevelInvalidDate"]);
                }

                if (!string.IsNullOrEmpty(user["Amount"]))
                {
                    identityUser.Amount = decimal.Parse(user["Amount"]);
                }
                userList.Add(identityUser);
            }
            return userList;
        }

        public List<TUser> ExportGetUsers(FrontUserQueryEntity query, out int totalCount)
        {
            totalCount = 0;
            var commandText = new StringBuilder();
            var whereExp = new StringBuilder();
            var sql = new StringBuilder();



            if (string.IsNullOrEmpty(query.DealerId))
            {
               
                sql.Append(@"select 
                        MS.Id,MS.Amount,MS.NickName,ms.PhoneNumber,MS.MLevelBeginDate,ms.Status,ms.IsPay,ms.Area,MS.MLevelInvalidDate,ms.Email,ms.UserName,ms.RealName,ms.MType,MS.No,MS.PayNumber,MS.IdentityNumber,MS.CreateTime,MS.IsDel,MS.MLevel,
                        ms.CreatedPerson,MS.Age,
						--R.Gender
						ms.Gender,ms.ApprovalStatus,MS.SystemMType,MS.UserType,
						--R.City
						ms.City,ms.AuthenticationTime ,ms.AuthenticationSource,R.AccntType,IF_Car.VIN
                        from Membership ms 
						LEFT JOIN IF_Customer R ON ms.IdentityNumber=R.IdentityNumber  and  ms.IdentityNumber = R.IdentityNumber and R.IdentityNumber is not null and R.IdentityNumber<>'' 
                         left join IF_Car on R.CustId=IF_Car.CustId 
                        {1} 
                        {3}
                      {2}
                        where 1=1 and IsDel=@isDel {0}");
            }
            else
            {
               
                sql.Append(@"select MS.Amount,MS.NickName,MS.Id,ms.PhoneNumber,MS.MLevelBeginDate,
                            ms.Status,MS.MLevelInvalidDate,ms.IsPay,ms.Area,ms.Email,ms.UserName,ms.RealName,ms.MType,MS.No,
                            MS.PayNumber,MS.IdentityNumber,MS.CreateTime,MS.IsDel,MS.MLevel,ms.CreatedPerson,MS.Age,
							--R.Gender
							ms.Gender
							,ms.ApprovalStatus,
                            MS.SystemMType,MS.UserType,
							--R.City
							ms.City,ms.AuthenticationTime,ms.AuthenticationSource,R.AccntType,IF_Car.VIN
							from Membership ms 
                            LEFT JOIN IF_Customer R ON ms.IdentityNumber=R.IdentityNumber    and  ms.IdentityNumber = R.IdentityNumber and R.IdentityNumber is not null and R.IdentityNumber<>'' 
                         left join IF_Car on R.CustId=IF_Car.CustId
                              {1} 
                            {3}
                            {2}
                            where 1=1  and IsDel=@isDel {0}");


            }
            if (!string.IsNullOrEmpty(query.MLevel) && query.MLevel != "-1")
            {
                whereExp.Append(" AND MLevel = @MLevel ");
            }
            if (!string.IsNullOrEmpty(query.Identity))
            {
                whereExp.Append(" AND (PhoneNumber = @PhoneNumber) ");
            }
            if (!string.IsNullOrEmpty(query.NickName))
            {
                whereExp.Append(" AND NickName=@NickName ");
            }
            string ifcar = "";
            if (!string.IsNullOrEmpty(query.VIN) || query.BuyTimeStart != null || query.BuyTimeEnd != null)
            {
                if (!string.IsNullOrEmpty(query.VIN))
                {
                    whereExp.Append(" AND IF_Car.VIN=@VIN ");
                }
            }
            string deartable = "";
            if (!string.IsNullOrEmpty(query.DealerId))
            {
                deartable = " LEFT JOIN MembershipDealer D ON D.MembershipId=MS.Id ";
                whereExp.Append(" AND D.DealerId=@DealerId");
            }
            string pwroktable = "";
            if (!string.IsNullOrEmpty(query.PaperWork))
            {
                pwroktable = "  LEFT JOIN Membership_Schedule kz on ms.id=kz.MembershipId  ";
                whereExp.Append(" AND kz.PaperWork=@PaperWork ");
            }
            //add 企业客户-个人客户
            if (!string.IsNullOrWhiteSpace(query.isComValue) && query.isComValue != "-1")
            {
                whereExp.Append(" AND r.AccntType=@isComValue ");
            }

            if (!string.IsNullOrEmpty(query.RealName))
            {
                whereExp.Append(" AND NickName=@NickName ");
            }
            if (!string.IsNullOrEmpty(query.Status) && query.Status != "-1")
            {
                whereExp.Append(" AND ms.Status=@Status ");
            }
            //if (query.IsSonata9)
            //{
            //    whereExp.Append(" AND [No] is not null");
            //}
            if ((!string.IsNullOrEmpty(query.UserType)) && query.UserType != "ALL")
            {
                whereExp.Append(" AND NO IS NOT NULL AND (UserType = @UserType OR UserType = 'TOP') ");
            }

            if (query.UserType == "ALL")
            {
                whereExp.Append(" AND NO IS NOT NULL ");
            }

            if (query.BeginTime != null)
            {
                whereExp.AppendFormat(" AND ms.CreateTime >='{0}'  ", query.BeginTime);
            }

            if (query.EndTime != null)
            {
                whereExp.AppendFormat("  AND ms.CreateTime <'{0}' ", query.EndTime.Value.AddDays(1));
            }

            if (query.AuthenticationTimeStart != null)
            {
                whereExp.AppendFormat(" AND ms.AuthenticationTime >='{0}'  ", query.AuthenticationTimeStart);
            }
            if (query.AuthenticationTimeEnd != null)
            {
                whereExp.AppendFormat(" AND ms.AuthenticationTime <'{0}'  ", query.AuthenticationTimeEnd.Value.AddDays(1));
            }
            if (query.BuyTimeStart != null)
            {
                whereExp.AppendFormat(" AND IF_Car.BuyTime >='{0}'  ", query.BuyTimeStart);
            }
            if (query.BuyTimeEnd != null)
            {
                whereExp.AppendFormat(" AND IF_Car.BuyTime <'{0}'  ", query.BuyTimeEnd.Value.AddDays(1));
            }

            if (!string.IsNullOrEmpty(query.IDCard))
            {
                whereExp.AppendFormat(" AND MS.IdentityNumber = @IdentityNumber ");
            }
            if (!string.IsNullOrEmpty(query.IsTmall))
            {
                if (query.IsTmall == "1")
                    whereExp.AppendFormat(" AND payNumber is not null ");
                else//"2"
                    whereExp.AppendFormat(" AND payNumber is null AND IsPay <> 0 ");
            }
            if (!string.IsNullOrEmpty(query.PayNumber))
            {
                whereExp.AppendFormat(" AND payNumber = @PayNumber ");
            }
            if (!string.IsNullOrEmpty(query.AuthenticationSource))
            {
                string strWhereSql = "";
                if (query.AuthenticationSource.Equals("app", StringComparison.OrdinalIgnoreCase))
                {
                    strWhereSql = @"'xjfw-app','app2', 'member_app','music_app','30518_appchezhu','blms_web',
'mls-pc-app','mls-wap-app','mls-wx-app',
                    'blms_wap_yuena_app'";
                    whereExp.AppendFormat(" AND AuthenticationSource in ({0}) ", strWhereSql);
                }
                if (query.AuthenticationSource.Equals("wx", StringComparison.OrdinalIgnoreCase))
                {
                    strWhereSql =
                        @"'xjfw-wx' ,'xjfwwx','blms_wechat','wx','mls-pc-wxnews','mls-wap-wxnews','mls-wx-wxfanspush','mls-wx-wxmenu','mls-wx-wxnews'
,'mls-wx-wxshare','blms_wechat_yuena','blms_wechat_yuena_weixin'";
                    whereExp.AppendFormat(" AND AuthenticationSource in ({0}) ", strWhereSql);
                }
                if (query.AuthenticationSource.Equals("pc", StringComparison.OrdinalIgnoreCase))
                {
                    strWhereSql = @"'member_web_edm','member_web_bmwebsite','edm2','member_web_weibo','music_edm','magazine',
'xjfw-pc','xjfwpc','undefined','opc07e5ad3f3fe297e','blms','blms_questionnaire','other2','blms_pc_web'";
                    whereExp.AppendFormat(" AND AuthenticationSource in ({0}) ", strWhereSql);
                }
                if (query.AuthenticationSource.Equals("jxs", StringComparison.OrdinalIgnoreCase))
                {
                    whereExp.AppendFormat(" AND AuthenticationSource like 'D%' ");
                }
            }
            if (!string.IsNullOrEmpty(query.CreatedPerson))
            {
                string strWhereSql = "";
                if (query.CreatedPerson.Equals("app", StringComparison.OrdinalIgnoreCase))
                {
                    strWhereSql = @"'xjfw-app','app2', 'member_app','music_app','30518_appchezhu','blms_web',
'mls-pc-app','mls-wap-app','mls-wx-app',
                    'blms_wap_yuena_app'";
                    whereExp.AppendFormat(" AND CreatedPerson in ({0}) ", strWhereSql);
                }
                if (query.CreatedPerson.Equals("wx", StringComparison.OrdinalIgnoreCase))
                {
                    strWhereSql =
                        @"'xjfw-wx' ,'xjfwwx','blms_wechat','wx','mls-pc-wxnews','mls-wap-wxnews','mls-wx-wxfanspush','mls-wx-wxmenu','mls-wx-wxnews'
,'mls-wx-wxshare','blms_wechat_yuena','blms_wechat_yuena_weixin'";
                    whereExp.AppendFormat(" AND CreatedPerson in ({0}) ", strWhereSql);
                }
                if (query.CreatedPerson.Equals("pc", StringComparison.OrdinalIgnoreCase))
                {
                    strWhereSql = @"'member_web_edm','member_web_bmwebsite','edm2','member_web_weibo','music_edm','magazine',
'xjfw-pc','xjfwpc','undefined','opc07e5ad3f3fe297e','blms','blms_questionnaire','other2','blms_pc_web'";
                    whereExp.AppendFormat(" AND CreatedPerson in ({0}) ", strWhereSql);
                }
                if (query.CreatedPerson.Equals("jxs", StringComparison.OrdinalIgnoreCase))
                {

                    whereExp.AppendFormat(" AND CreatedPerson  like 'D%'");
                }
            }
            var parameters = new Dictionary<string, object>
            {
                {"@isDel", 0},
                {"@PhoneNumber",query.Identity},
                //{"@RealName",query.RealName},
                {"@NickName",query.NickName},
                {"@MLevel",query.MLevel},
                {"@DealerId",query.DealerId},
                {"@VIN",query.VIN},
                {"@Status",query.Status},
                {"@UserType",query.UserType},
                {"@PayNumber",query.PayNumber},
                {"@PaperWork",query.PaperWork},
                {"@IdentityNumber", query.IDCard},
                {"@isComValue",query.isComValue},
                {"@AuthenticationSource",query.AuthenticationSource},
                {"@CreatedPerson",query.CreatedPerson}
            };
            var filter = whereExp.ToString();
            if (string.IsNullOrEmpty(query.DealerId))
            {
                filter = filter.Replace("AND D.DealerId=@VIN", "");
            }

            // int.TryParse(_database.QueryValue(string.Format(commandText.ToString(), whereExp.ToString()), parameters).ToString(), out totalCount);
            var users = _database.Query(string.Format(sql.ToString(), whereExp.ToString(), ifcar, pwroktable, deartable), parameters);

            var userList = new List<TUser>();
            foreach (var user in users)
            {
                var identityUser = (TUser)Activator.CreateInstance(typeof(TUser));
                identityUser.Id = user["Id"];
                identityUser.UserName = user["UserName"];
                if (!string.IsNullOrEmpty(user["AuthenticationTime"]))
                {
                    identityUser.AuthenticationTime = DateTime.Parse(user["AuthenticationTime"]);
                }
                identityUser.AuthenticationSource = user["AuthenticationSource"];
                identityUser.AccntType = user["AccntType"];
                identityUser.IdentityNumber = user["IdentityNumber"];
                identityUser.Email = user["Email"];
                identityUser.PhoneNumber = user["PhoneNumber"];
                identityUser.CreateTime = user["CreateTime"];
                identityUser.Status = int.Parse(user["Status"]);
                identityUser.MLevel = string.IsNullOrEmpty(user["MLevel"]) ? 1 : int.Parse(user["MLevel"]);//(int.Parse(user["MLevel"]) == 3 && !string.IsNullOrEmpty(user["No"])) ? 9 : int.Parse(user["MLevel"]);
                identityUser.CreatedPerson = user["CreatedPerson"];
                identityUser.MType = string.IsNullOrEmpty(user["MType"]) ? 1 : int.Parse(user["MType"]);
                identityUser.SystemMType = string.IsNullOrEmpty(user["SystemMType"]) ? 1 : int.Parse(user["SystemMType"]);
                identityUser.IsPay = string.IsNullOrEmpty(user["IsPay"]) ? 0 : int.Parse(user["IsPay"]);
                identityUser.ApprovalStatus = string.IsNullOrEmpty(user["ApprovalStatus"]) ? 0 : int.Parse(user["ApprovalStatus"]);
                identityUser.NickName = user["NickName"];
                identityUser.RealName = user["RealName"];
                identityUser.PayNumber = user["PayNumber"];
                identityUser.No = user["No"];
                identityUser.UserType = user["UserType"];
                identityUser.Gender = user["Gender"];
                identityUser.GenderName = string.IsNullOrEmpty(user["Gender"]) ? null : user["Gender"] == "1" ? "男" : "女";
                identityUser.City = user["City"];
                identityUser.Area = user["Area"];
                identityUser.Age = Convert.ToInt32(user["Age"]);
                identityUser.VIN = user["VIN"];
                if (!string.IsNullOrEmpty(user["MLevelBeginDate"]))
                {
                    identityUser.MLevelBeginDate = DateTime.Parse(user["MLevelBeginDate"]);
                }
                if (!string.IsNullOrEmpty(user["MLevelInvalidDate"]))
                {
                    identityUser.MLevelInvalidDate = DateTime.Parse(user["MLevelInvalidDate"]);
                }

                if (!string.IsNullOrEmpty(user["Amount"]))
                {
                    identityUser.Amount = decimal.Parse(user["Amount"]);
                }
                userList.Add(identityUser);
            }
            return userList;
        }

        public List<TUser> GetUsers(FrontUserQueryEntity query, out int totalCount)
        {
            var commandText = new StringBuilder();   
            var whereExp = new StringBuilder();
            var sql = new StringBuilder();



            if (string.IsNullOrEmpty(query.DealerId))
            {
                commandText.Append(@"SELECT count(1) FROM  Membership ms 
						LEFT JOIN IF_Customer R ON MS.IdentityNumber=R.IdentityNumber and  MS.IdentityNumber = R.IdentityNumber 
--and R.IdentityNumber is not null and R.IdentityNumber<>'' 
                        -- left join IF_Car on R.CustId=IF_Car.CustId
                           {1} 
                        {3}
                       {2}
                        where 1=1 and IsDel=0 {0}");
                sql.Append(@"SELECT * FROM (select row_number() over (order by ms.CreateTime desc ) as row_num, 
                        MS.Id,MS.Amount,MS.NickName,ms.PhoneNumber,MS.MLevelBeginDate,ms.Status,ms.IsPay,ms.Area,MS.MLevelInvalidDate,ms.Email,ms.UserName,ms.RealName,ms.MType,MS.No,MS.PayNumber,MS.IdentityNumber,MS.CreateTime,MS.IsDel,MS.MLevel,
                        ms.CreatedPerson,MS.Age,
						--R.Gender
						ms.Gender,ms.ApprovalStatus,MS.SystemMType,MS.UserType,
						--R.City
						ms.City,ms.AuthenticationTime ,ms.AuthenticationSource,R.AccntType
                        from Membership ms 
						LEFT JOIN IF_Customer R ON ms.IdentityNumber=R.IdentityNumber  and  ms.IdentityNumber = R.IdentityNumber and R.IdentityNumber is not null and R.IdentityNumber<>'' 
                        -- left join IF_Car on R.CustId=IF_Car.CustId 
                        {1} 
                        {3}
                      {2}
                        where 1=1 and IsDel=@isDel {0}) u " +
                           "where row_num between @PageIndex and @PageEnd ");
            }
            else
            {
                commandText.Append(
                    @"select count(1)
                        from Membership ms 
                        LEFT JOIN IF_Customer R ON MS.IdentityNumber=R.IdentityNumber   and  ms.IdentityNumber = R.IdentityNumber 
--and R.IdentityNumber is not null and R.IdentityNumber<>'' 
  -- left join IF_Car on R.CustId=IF_Car.CustId
     {1} 
                        {3}
                       {2}
                         where ms.IsDel=@isDel  {0}");

                sql.Append(@"SELECT * FROM (select row_number() over (order by ms.CreateTime desc) as row_num,MS.Amount,MS.NickName,MS.Id,ms.PhoneNumber,MS.MLevelBeginDate,
                            ms.Status,MS.MLevelInvalidDate,ms.IsPay,ms.Area,ms.Email,ms.UserName,ms.RealName,ms.MType,MS.No,
                            MS.PayNumber,MS.IdentityNumber,MS.CreateTime,MS.IsDel,MS.MLevel,ms.CreatedPerson,MS.Age,
							--R.Gender
							ms.Gender
							,ms.ApprovalStatus,
                            MS.SystemMType,MS.UserType,
							--R.City
							ms.City,ms.AuthenticationTime,ms.AuthenticationSource,R.AccntType
							from Membership ms 
                            LEFT JOIN IF_Customer R ON ms.IdentityNumber=R.IdentityNumber    and  ms.IdentityNumber = R.IdentityNumber and R.IdentityNumber is not null and R.IdentityNumber<>'' 
 -- left join IF_Car on R.CustId=IF_Car.CustId
                              {1} 
                            {3}
                            {2}
                            where 1=1  and IsDel=@isDel {0}) u " +
                           "where row_num between @PageIndex and @PageEnd ");


            }
            if (!string.IsNullOrEmpty(query.MLevel) && query.MLevel != "-1")
            {
                whereExp.Append(" AND MLevel = @MLevel ");
            }
            if (!string.IsNullOrEmpty(query.Identity))
            {
                whereExp.Append(" AND (PhoneNumber = @PhoneNumber) ");
            }
            if (!string.IsNullOrEmpty(query.NickName))
            {
                whereExp.Append(" AND NickName=@NickName ");
            }
            string ifcar = "";
            if (!string.IsNullOrEmpty(query.VIN) || query.BuyTimeStart!=null || query.BuyTimeEnd!=null)
            {
                ifcar = " left join IF_Car on R.CustId=IF_Car.CustId ";
                if(!string.IsNullOrEmpty(query.VIN))
                { 
                whereExp.Append(" AND IF_Car.VIN=@VIN ");
                }
            }
            string deartable = "";
            if (!string.IsNullOrEmpty(query.DealerId))
            {
                deartable = " LEFT JOIN MembershipDealer D ON D.MembershipId=MS.Id ";
                whereExp.Append(" AND D.DealerId=@DealerId");
            }
            string pwroktable = "";
            if (!string.IsNullOrEmpty(query.PaperWork))
            {
                pwroktable = "  LEFT JOIN Membership_Schedule kz on ms.id=kz.MembershipId  ";
                whereExp.Append(" AND kz.PaperWork=@PaperWork ");
            }
            //add 企业客户-个人客户
            if (!string.IsNullOrWhiteSpace(query.isComValue) && query.isComValue!="-1")
            {
                whereExp.Append(" AND r.AccntType=@isComValue ");
            }

            if (!string.IsNullOrEmpty(query.RealName))
            {
                whereExp.Append(" AND NickName=@NickName ");
            }
            if (!string.IsNullOrEmpty(query.Status) && query.Status != "-1")
            {
                whereExp.Append(" AND ms.Status=@Status ");
            }
            //if (query.IsSonata9)
            //{
            //    whereExp.Append(" AND [No] is not null");
            //}
            if ((!string.IsNullOrEmpty(query.UserType)) && query.UserType != "ALL")
            {
                whereExp.Append(" AND NO IS NOT NULL AND (UserType = @UserType OR UserType = 'TOP') ");
            }

            if (query.UserType == "ALL")
            {
                whereExp.Append(" AND NO IS NOT NULL ");
            }

            if (query.BeginTime != null )
            {
                whereExp.AppendFormat(" AND ms.CreateTime >='{0}'  ", query.BeginTime);
            }

            if (query.EndTime != null)
            {
                whereExp.AppendFormat("  AND ms.CreateTime <'{0}' ", query.EndTime.Value.AddDays(1));
            }

            if (query.AuthenticationTimeStart != null)
            {
                whereExp.AppendFormat(" AND ms.AuthenticationTime >='{0}'  ", query.AuthenticationTimeStart);
            }
            if (query.AuthenticationTimeEnd != null)
            {
                whereExp.AppendFormat(" AND ms.AuthenticationTime <'{0}'  ", query.AuthenticationTimeEnd.Value.AddDays(1));
            }
            if (query.BuyTimeStart!=null)
            {
                whereExp.AppendFormat(" AND IF_Car.BuyTime >='{0}'  ", query.BuyTimeStart);
            }
            if (query.BuyTimeEnd != null)
            {
                whereExp.AppendFormat(" AND IF_Car.BuyTime <'{0}'  ", query.BuyTimeEnd.Value.AddDays(1));
            }

            if (!string.IsNullOrEmpty(query.IDCard))
            {
                whereExp.AppendFormat(" AND MS.IdentityNumber = @IdentityNumber ");
            }
            //会员卡号
            if (!string.IsNullOrEmpty(query.No))
            {
                whereExp.AppendFormat(" AND MS.No = @No ");
            }
            if (!string.IsNullOrEmpty(query.IsTmall))
            {
                if (query.IsTmall == "1")
                    whereExp.AppendFormat(" AND payNumber is not null ");
                else//"2"
                    whereExp.AppendFormat(" AND payNumber is null AND IsPay <> 0 ");
            }
            if (!string.IsNullOrEmpty(query.PayNumber))
            {
                whereExp.AppendFormat(" AND payNumber = @PayNumber ");
            }
            if (!string.IsNullOrEmpty(query.AuthenticationSource))
            {

                string strWhereSql = "";
                if (query.AuthenticationSource.Equals("app", StringComparison.OrdinalIgnoreCase))
                {
                    strWhereSql = @"'xjfw-app','app2', 'member_app','music_app','30518_appchezhu','blms_web',
'mls-pc-app','mls-wap-app','mls-wx-app',
                    'blms_wap_yuena_app'";
                    whereExp.AppendFormat(" AND AuthenticationSource in ({0}) ", strWhereSql);
                }
                if (query.AuthenticationSource.Equals("wx", StringComparison.OrdinalIgnoreCase))
                {
                    whereExp.AppendFormat(@" AND (AuthenticationSource like 'blms_wechat%' or  AuthenticationSource in ('wxss_shsbyq','linghu','member_wx','weixin2','xjfw-wx','xjfwwx',
                         'music_wx','mls-wx-wxshare','mls-wx-wxmenu','mls-wx-wxnews','mls-wx-wxfanspush'))");
                        
                }
                if (query.AuthenticationSource.Equals("pc", StringComparison.OrdinalIgnoreCase))
                {


                    strWhereSql = @"'member_web_edm','member_web_bmwebsite','edm2','member_web_weibo','music_edm','magazine',
'xjfw-pc','xjfwpc','undefined','opc07e5ad3f3fe297e','blms','blms_questionnaire','other2','blms_pc_web','',
'blms_web_pc','xjfw','admin','cs_questionnaire','member_dealer','memberstrip','musical_web','music_sms','memberlottery',
'member_sms','blms_pc_web_yuena_hyundai','blms_pc_web_yuena_weixin'";


                    whereExp.AppendFormat(" AND AuthenticationSource in ({0}) ", strWhereSql);



                }
                if (query.AuthenticationSource.Equals("jxs", StringComparison.OrdinalIgnoreCase))
                {
                    whereExp.AppendFormat(" AND AuthenticationSource like 'D%' ");
                }
                if (query.AuthenticationSource.Equals("wap",StringComparison.OrdinalIgnoreCase))
                {
                    whereExp.AppendFormat(@" and  AuthenticationSource in ('blms_wap_nov_hyundai',
                        'blms_wap_nov','blms_wap_annum_weixinnews','blms_wap_yuena','blms_wap_tk','xjfw-wap','xjfwwap',
                        'blms_wap_yuena_sms','blms_wap_annum_app','blms_wap_annum_sms','blms_wap_annum_weixin','blms_wap_nov_sms',
                        'wap2','mls-wap-wxnews','blms_wap_yuena_weixin','blms_wap','blms_wap_yuena_sharenormal','blms_wap_nov_app',
                        'blms_wap_yuena_wxmenu'
                        ) 
");
                }
            }
            if (!string.IsNullOrEmpty(query.CreatedPerson))
            {
                string strWhereSql = "";
                if (query.CreatedPerson.Equals("app", StringComparison.OrdinalIgnoreCase))
                {
                    strWhereSql = @"'xjfw-app','app2', 'member_app','music_app','30518_appchezhu','blms_web',
'mls-pc-app','mls-wap-app','mls-wx-app',
                    'blms_wap_yuena_app'";
                    whereExp.AppendFormat(" AND CreatedPerson in ({0}) ", strWhereSql);
                }
                if (query.CreatedPerson.Equals("wx", StringComparison.OrdinalIgnoreCase))
                {
                    
                    whereExp.AppendFormat(@" AND (CreatedPerson like 'blms_wechat%' or ( CreatedPerson in ('wxss_shsbyq','linghu','member_wx','weixin2','xjfw-wx','xjfwwx',
                         'music_wx','mls-wx-wxshare','mls-wx-wxmenu','mls-wx-wxnews','mls-wx-wxfanspush' ) ))");
                        
                }
                if (query.CreatedPerson.Equals("pc", StringComparison.OrdinalIgnoreCase))
                {
                    
                    strWhereSql =
                        @"'member_web_edm','member_web_bmwebsite','edm2','member_web_weibo','music_edm','magazine',
'xjfw-pc','xjfwpc','undefined','opc07e5ad3f3fe297e','blms','blms_questionnaire','other2','blms_pc_web','',
'blms_web_pc','xjfw','admin','cs_questionnaire','member_dealer','memberstrip','musical_web','music_sms','memberlottery',
'member_sms','blms_pc_web_yuena_hyundai','blms_pc_web_yuena_weixin'";
                    whereExp.AppendFormat(" AND (CreatedPerson in ({0}) or CreatedPerson is null)", strWhereSql);
                }
                if (query.CreatedPerson.Equals("jxs", StringComparison.OrdinalIgnoreCase))
                {

                    whereExp.AppendFormat(" AND CreatedPerson  like 'D%'");
                }
                if (query.CreatedPerson.Equals("wap", StringComparison.OrdinalIgnoreCase))
                {
                    whereExp.AppendFormat(@" and  CreatedPerson in ('blms_wap_nov_hyundai',
                        'blms_wap_nov','blms_wap_annum_weixinnews','blms_wap_yuena','blms_wap_tk','xjfw-wap','xjfwwap',
                        'blms_wap_yuena_sms','blms_wap_annum_app','blms_wap_annum_sms','blms_wap_annum_weixin','blms_wap_nov_sms',
                        'wap2','mls-wap-wxnews','blms_wap_yuena_weixin','blms_wap','blms_wap_yuena_sharenormal','blms_wap_nov_app',
                        'blms_wap_yuena_wxmenu'
                        ) ");

                }
            }
            var parameters = new Dictionary<string, object>
            {
                {"@isDel", 0},
                {"@PhoneNumber",query.Identity},
                //{"@RealName",query.RealName},
                {"@NickName",query.NickName},
                {"@MLevel",query.MLevel},
                {"@DealerId",query.DealerId},
                {"@VIN",query.VIN},
                {"@Status",query.Status},
                {"@UserType",query.UserType},
                {"@PayNumber",query.PayNumber},
                {"@PaperWork",query.PaperWork},
                {"@PageIndex", query.Skip + 1},
                {"@PageEnd", query.Count + query.Skip},
                {"@IdentityNumber", query.IDCard},
                {"@isComValue",query.isComValue},
                {"@AuthenticationSource",query.AuthenticationSource},
                {"@CreatedPerson",query.CreatedPerson},
                {"@No",query.No}
            };
            var filter = whereExp.ToString();
            if (string.IsNullOrEmpty(query.DealerId))
            {
                filter = filter.Replace("AND D.DealerId=@VIN", "");
            }
            totalCount = Convert.ToInt32(_database.QueryValue(string.Format(commandText.ToString(), filter, ifcar, pwroktable, deartable), parameters));

            // int.TryParse(_database.QueryValue(string.Format(commandText.ToString(), whereExp.ToString()), parameters).ToString(), out totalCount);
            var users = _database.Query(string.Format(sql.ToString(), whereExp.ToString(), ifcar, pwroktable, deartable), parameters);

            var userList = new List<TUser>();
            foreach (var user in users)
            {
                var identityUser = (TUser)Activator.CreateInstance(typeof(TUser));
                identityUser.Id = user["Id"];
                identityUser.UserName = user["UserName"];
                if (!string.IsNullOrEmpty(user["AuthenticationTime"]))
                {
                    identityUser.AuthenticationTime = DateTime.Parse(user["AuthenticationTime"]);
                }
                identityUser.AuthenticationSource = user["AuthenticationSource"];
                identityUser.AccntType = user["AccntType"];
                identityUser.IdentityNumber = user["IdentityNumber"];
                identityUser.Email = user["Email"];
                identityUser.PhoneNumber = user["PhoneNumber"];
                identityUser.CreateTime = user["CreateTime"];
                identityUser.Status = int.Parse(user["Status"]);
                identityUser.MLevel = string.IsNullOrEmpty(user["MLevel"]) ? 1 : int.Parse(user["MLevel"]);//(int.Parse(user["MLevel"]) == 3 && !string.IsNullOrEmpty(user["No"])) ? 9 : int.Parse(user["MLevel"]);
                identityUser.CreatedPerson = user["CreatedPerson"];
                identityUser.MType = string.IsNullOrEmpty(user["MType"]) ? 1 : int.Parse(user["MType"]);
                identityUser.SystemMType = string.IsNullOrEmpty(user["SystemMType"]) ? 1 : int.Parse(user["SystemMType"]);
                identityUser.IsPay = string.IsNullOrEmpty(user["IsPay"]) ? 0 : int.Parse(user["IsPay"]);
                identityUser.ApprovalStatus = string.IsNullOrEmpty(user["ApprovalStatus"]) ? 0 : int.Parse(user["ApprovalStatus"]);
                identityUser.NickName = user["NickName"];
                identityUser.RealName = user["RealName"];
                identityUser.PayNumber = user["PayNumber"];
                identityUser.No = user["No"];
                identityUser.UserType = user["UserType"];
                identityUser.Gender = user["Gender"];
                identityUser.GenderName = string.IsNullOrEmpty(user["Gender"]) ? null : user["Gender"] == "1" ? "男" : "女";
                identityUser.City = user["City"];
                identityUser.Area = user["Area"];
                identityUser.Age = Convert.ToInt32(user["Age"]);

                if (!string.IsNullOrEmpty(user["MLevelBeginDate"]))
                {
                    identityUser.MLevelBeginDate = DateTime.Parse(user["MLevelBeginDate"]);
                }
                if (!string.IsNullOrEmpty(user["MLevelInvalidDate"]))
                {
                    identityUser.MLevelInvalidDate = DateTime.Parse(user["MLevelInvalidDate"]);
                }

                if (!string.IsNullOrEmpty(user["Amount"]))
                {
                    identityUser.Amount = decimal.Parse(user["Amount"]);
                }
                userList.Add(identityUser);
            }
            return userList;
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
            return _database.Execute(commandText.ToString(), parameters) > 0;
        }

        public bool AddMembershipSchedule(string membershipId, string dealerId)
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
            return _database.Execute(commandText.ToString(), parameters) > 0;
        }


        public bool DeleteMembershipDealerRecord(string membershipId)
        {
            var commandText =
                new StringBuilder(
                    "Delete MembershipDealer WHERE MembershipId =  @MembershipId ");
            var parameters = new Dictionary<string, object>
            {
                {"@MembershipId", membershipId}
            };
            return _database.Execute(commandText.ToString(), parameters) > 0;
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
            int.TryParse(_database.QueryValue(commandText.ToString(), parameters).ToString(), out count);
            return count > 0;
        }

        public List<MembershipRequestFailed> GetMembershipRequestFailedList(int? status, string phone, int start, int count, out int totalCount)
        {
            var appendStr = "";
            if (status != null && status > 0)
            {
                appendStr = string.Format("and rf.status={0}", status);
            }

            if (!string.IsNullOrEmpty(phone))
            {
                appendStr = appendStr + string.Format(" and rf.username={0}", phone);
            }

            var totalsql = new StringBuilder("SELECT COUNT(1) FROM MembershipRequestFailed rf where 1=1 ");
            totalsql.Append(appendStr);
            int.TryParse(_database.QueryValue(totalsql.ToString(), null).ToString(), out totalCount);

            var sql = new StringBuilder();
            sql.AppendFormat("SELECT top {0} rf.*, m.PayNumber,r.CarCategory,m.IsPay FROM MembershipRequestFailed rf inner join membership m on rf.userid=m.id and m.No is null inner join IF_Customer c on m.IdentityNumber = c.IdentityNumber inner join IF_Car r on c.CustId = r.CustId where 1=1 ", count);
            sql.Append(appendStr);
            sql.AppendFormat(" and rf.id not in (select top {0} rf.id from MembershipRequestFailed rf inner join membership m on rf.userid=m.id and m.No is null  where 1=1 ", start);
            sql.Append(appendStr);
            sql.Append(")");

            var result = _database.Query(sql.ToString(), null);

            var list = new List<MembershipRequestFailed>();
            foreach (var item in result)
            {
                var request = new MembershipRequestFailed();
                request.Id = Int32.Parse(item["Id"]);
                request.UserName = item["UserName"];
                request.UserId = item["UserId"];
                request.IdentityNumber = item["IdentityNumber"];
                request.RequestTime = item["RequestTime"];
                request.Status = int.Parse(item["Status"]);
                request.OperationTime = item["OperationTime"];
                request.VIN = item["VIN"];
                request.Operator = item["Operator"];
                request.PayNumber = item["PayNumber"];
                request.CarCategory = item["CarCategory"];
                list.Add(request);
            }
            return list;
        }

        public List<MembershipRequestFailed> GetMembershipRequestFailedListAll(int? status)
        {
            var appendStr = "";
            if (status != null && status > 0)
            {
                appendStr = string.Format("and rf.status={0}", status);
            }
            var sql = new StringBuilder();
            sql.AppendFormat("SELECT rf.*, m.PayNumber,r.CarCategory,m.IsPay FROM MembershipRequestFailed rf inner join membership m on rf.userid=m.id inner join IF_Customer c on m.IdentityNumber = c.IdentityNumber inner join IF_Car r on c.CustId = r.CustId where 1=1 ");
            sql.Append(appendStr);

            var result = _database.Query(sql.ToString(), null);

            var list = new List<MembershipRequestFailed>();
            foreach (var item in result)
            {
                var request = new MembershipRequestFailed();
                request.Id = Int32.Parse(item["Id"]);
                request.UserName = item["UserName"];
                request.UserId = item["UserId"];
                request.IdentityNumber = item["IdentityNumber"];
                request.RequestTime = item["RequestTime"];
                request.Status = int.Parse(item["Status"]);
                request.OperationTime = item["OperationTime"];
                request.VIN = item["VIN"];
                request.Operator = item["Operator"];
                request.PayNumber = item["PayNumber"];
                request.CarCategory = item["CarCategory"];
                list.Add(request);
            }
            return list;
        }

        public bool UpdateMembershipRequestStatus(string id, string operatorName, out string message)
        {
            message = null;
            var commandText = new StringBuilder("update MembershipRequestFailed set status=2,OperationTime=@OperationTime,Operator=@Operator where userId=@userId;update Membership set MonitorScheduleFail = 0,MonitorCarScheduleStartTime = null where id = @userId");
            var parameters = new Dictionary<string, object>
            {
                {"@userId", id},
                  {"@OperationTime",DateTime.Now},
                 {"@Operator",operatorName}
            };

            return _database.Execute(commandText.ToString(), parameters) > 0;
        }

        public bool Activate(string id, string operatorName, out string message)
        {
            message = null;
            var commandText = new StringBuilder("update Membership set ApprovalStatus=2 where Id=@userId;" +
                                                "update MembershipRequestFailed set status=3,OperationTime=@OperationTime,Operator=@Operator where userId=@userId; ");
            var parameters = new Dictionary<string, object>
            {
                {"@userId", id} ,
                {"@OperationTime",DateTime.Now},
                 {"@Operator",operatorName}
            };
            bool result = _database.Execute(commandText.ToString(), parameters) > 0;
            if (!result)
                message = "重新激活失败,未找到此会员!";
            return _database.Execute(commandText.ToString(), parameters) > 0;
        }

        public bool UpdateIdentityNumber(string id, string identityNmuber, string operatorName, out string message)
        {
            message = null;
            if (IsIdentityNumberRepeate(identityNmuber, id))
            {
                message = "此身份证在系统中已存在";
                return false;
            }
            var commandText = new StringBuilder("update Membership set MonitorScheduleFail=@MonitorScheduleFail,MonitorCarScheduleStartTime=@MonitorCarScheduleStartTime where Id=@userId;" +
                                                "update MembershipRequestFailed set status=2,IdentityNumber=@identityNmuber,OperationTime=@OperationTime,Operator=@Operator where userId=@userId; ");

            var parameters = new Dictionary<string, object>
            {
                {"@userId", id},
                {"@identityNmuber",identityNmuber},
                {"@MonitorScheduleFail",0}, //更新身份证号后,继续轮询匹配车辆
                {"@MonitorCarScheduleStartTime", DateTime.Now},
                {"@OperationTime",DateTime.Now},
                {"@Operator",operatorName}
            }; ;
            bool result = _database.Execute(commandText.ToString(), parameters) > 0;
            if (!result)
                message = "重新激活失败!";
            //result = _database.Execute(commandText.ToString(), parameters) > 0;
            //if (!result)
            //    message = "修改身份证失败!";
            //return _database.Execute(commandText.ToString(), parameters) > 0;
            return result;
        }

        public bool UpdateIdentityNumberBy4S(string id, string identityNumber, string operatorName, out string message)
        {
            message = null;
            if (IsIdentityNumberRepeate(identityNumber, id))
            {
                message = "此身份证在系统已存在";
                return false;
            }
            var commandText = new StringBuilder("update Membership set identityNumber=@identityNumber, updateTime=@updateTime where Id=@userId ");

            var parameters = new Dictionary<string, object>
            {
                {"@userId", id},
                {"@identityNumber",identityNumber},
                {"@updateTime",DateTime.Now}
            };
            bool result = _database.Execute(commandText.ToString(), parameters) > 0;
            if (!result)
                message = "修改身份证失败!";
            else
                message = "修改成功!";
            return result;
        }

        public bool IsIdentityNumberRepeate(string identityNumber)
        {
            var commandText = new StringBuilder("select count(*) from membership where identitynumber=@identityNmuber");
            var parameters = new Dictionary<string, object>
            {
                {"@identityNmuber",identityNumber}
               
            };
            return int.Parse(_database.QueryValue(commandText.ToString(), parameters).ToString()) > 0;
        }

        public bool IsIdentityNumberId(string identityNumber, string Id)
        {
            var commandText = new StringBuilder("select count(*) from membership where identitynumber=@identityNmuber and Id=@Id");
            var parameters = new Dictionary<string, object>
            {
                {"@identityNmuber",identityNumber},
                {"@Id",Id}
               
            };
            return int.Parse(_database.QueryValue(commandText.ToString(), parameters).ToString()) > 0;
        }
        public bool IsIdentityNumberRepeate(string identityNumber, string id)
        {
            var commandText = new StringBuilder("select count(*) from membership where identitynumber=@identityNmuber and Id!=@id ");
            var parameters = new Dictionary<string, object>
            {
                {"@identityNmuber",identityNumber},
                {"@id",id}
            };
            return int.Parse(_database.QueryValue(commandText.ToString(), parameters).ToString()) > 0;
        }

        /// <summary>
        /// 检查昵称是否存在
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        internal bool CheckNickNameIsExist(string NickName, string id)
        {
            var commandText = new StringBuilder("select count(*) from membership where UserName=@NickName and Id!=@id ");
            var parameters = new Dictionary<string, object>
            {
                {"@nickname",NickName},
                 {"@id",id}
            };
            return int.Parse(_database.QueryValue(commandText.ToString(), parameters).ToString()) > 0;
        }
        /// <summary>
        /// 检用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        internal bool CheckUserNameIsExist1(string UserName, string id)
        {
            var commandText = new StringBuilder("select count(*) from membership where UserName=@UserName and Id!=@id ");
            var parameters = new Dictionary<string, object>
            {
                {"@UserName",UserName},
                 {"@id",id}
            };
            return int.Parse(_database.QueryValue(commandText.ToString(), parameters).ToString()) > 0;
        }
        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        internal bool CheckNickNameIsExist(string NickName)
        {
            var commandText = new StringBuilder("select count(*) from membership where nickname=@NickName");
            var parameters = new Dictionary<string, object>
            {
                {"@nickname",NickName}
                
            };
            return int.Parse(_database.QueryValue(commandText.ToString(), parameters).ToString()) > 0;
        }

        //自己检查手机号是否存在
        internal bool CheckPhoneNumberIsExist(string phoneNumber)
        {
            var commandText = new StringBuilder("select count(*) from membership where phoneNumber=@phoneNumber ");
            var parameters = new Dictionary<string, object>
            {
                {"@phoneNumber",phoneNumber}
               
            };
            return int.Parse(_database.QueryValue(commandText.ToString(), parameters).ToString()) > 0;
        }
        //自己检查手机号是否存在
        internal bool CheckPhoneNumberIsExist(string phoneNumber, string id)
        {
            var commandText = new StringBuilder("select count(*) from membership where phoneNumber=@phoneNumber and Id!=@id ");
            var parameters = new Dictionary<string, object>
            {
                {"@phoneNumber",phoneNumber},
                {"@id",id}
            };
            return int.Parse(_database.QueryValue(commandText.ToString(), parameters).ToString()) > 0;
        }
        //自己检查邮箱否存在
        internal bool CheckEmailIsExist(string email, string id)
        {
            var commandText = new StringBuilder("select count(*) from membership where Email=@email and Id!=@id ");
            var parameters = new Dictionary<string, object>
            {
                {"@email",email},
               {"@id",id}
            };
            return int.Parse(_database.QueryValue(commandText.ToString(), parameters).ToString()) > 0;
        }
        //自己检查邮箱否存在
        internal bool CheckEmailIsExist(string email)
        {
            var commandText = new StringBuilder("select count(*) from membership where Email=@email");
            var parameters = new Dictionary<string, object>
            {
                {"@email",email}
             
            };
            return int.Parse(_database.QueryValue(commandText.ToString(), parameters).ToString()) > 0;
        }
        /// <summary>
        /// 检查此手机号是否存在
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        internal bool CheckUserNameIsExist(string phoneNumber)
        {
            var commandText = new StringBuilder("select count(Id) from membership where username=@username ");
            var parameters = new Dictionary<string, object>
            {
                {"@username",phoneNumber}
            };
            return int.Parse(_database.QueryValue(commandText.ToString(), parameters).ToString()) > 0;
        }


        /// <summary>
        /// 检查用户名密码是否存在
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        internal bool CheckLoginIsExist(string nickName, string Password)
        {

            //var checkname = new StringBuilder("select count(*) from membership where nickname=@nickName");
            //var checkpers = new Dictionary<string, object>
            //{
            //    {"@nickname",nickName} 
            //};
            //if (checkpers==null)
            //{
            //    return int.Parse(_database.QueryValue(checkname.ToString(), checkpers).ToString()) < 0;
            //}
            var commandText = new StringBuilder("select count(*) from membership where nickname=@nickName and PasswordHash=@Password");
            var parameters = new Dictionary<string, object>
            {
                {"@nickname",nickName},
                {"@Password",Password}
            };

            return int.Parse(_database.QueryValue(commandText.ToString(), parameters).ToString()) > 0;
        }
        /// <summary>
        /// 检查用户名密码是否存在,返回用户id
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public List<TUser> GetUserByLogin(string nickName, string Password)
        {
            List<TUser> users = new List<TUser>();
            string commandText = "Select * from membership where nickName = @nickName";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@nickName", nickName },{"@Password",Password}
            };

            var rows = _database.Query(commandText, parameters);
            foreach (var row in rows)
            {
                TUser user = (TUser)Activator.CreateInstance(typeof(TUser));
                user.Id = row["Id"];

                user.UserName = row["UserName"];
                user.RealName = row["RealName"];
                user.Status = int.Parse(row["Status"]);
                user.CreatedPerson = row["CreatedPerson"];
                user.VIN = row["VIN"];
                user.No = row["No"];
                user.ApprovalStatus = int.Parse(row["ApprovalStatus"]);
                user.PhoneNumber = row["PhoneNumber"];
                user.IdentityNumber = row["IdentityNumber"];
                user.MLevel = row["MLevel"] == null ? 1 : int.Parse(row["MLevel"]);
                //user.Gender = int.Parse(row["Gender"]);
                user.Provency = row["Provency"];
                user.City = row["City"];
                user.Area = row["Area"];
                user.Address = row["Address"];
                user.PasswordHash = string.IsNullOrEmpty(row["PasswordHash"]) ? null : row["PasswordHash"];
                user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
                user.Email = string.IsNullOrEmpty(row["Email"]) ? null : row["Email"];
                user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true : false;
                user.PhoneNumber = string.IsNullOrEmpty(row["PhoneNumber"]) ? null : row["PhoneNumber"];
                user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
                user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
                user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
                user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
                user.FaceImage = row["FaceImage"];
                user.MType = string.IsNullOrEmpty(row["MType"]) ? 1 : int.Parse(row["MType"]);
                user.IsPay = string.IsNullOrEmpty(row["IsPay"]) ? 0 : int.Parse(row["IsPay"]);
                user.SystemMType = string.IsNullOrEmpty(row["SystemMType"]) ? 1 : int.Parse(row["SystemMType"]);
                user.ActiveWay = string.IsNullOrEmpty(row["ActiveWay"]) ? 1 : int.Parse(row["ActiveWay"]);
                user.IsNeedModifyPw = string.IsNullOrEmpty(row["IsNeedModifyPw"]) ? 0 : int.Parse(row["IsNeedModifyPw"]);
                user.NickName = row["NickName"];
                user.PayNumber = row["PayNumber"];

                users.Add(user);
            }

            return users;
        }
        /// <summary>
        /// 修改手机号
        /// </summary>
        /// <param name="id"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="operatorName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool updatePhoneNumberModal(string id, string phoneNumber, string operatorName, out string message)
        {
            message = null;
            //验证新手机号是否存在
            var commandText = new StringBuilder("select count(*) from membership where PhoneNumber=@PhoneNumber or UserName=@PhoneNumber ");
            var parameters = new Dictionary<string, object>
            {
                {"@PhoneNumber",phoneNumber}
            };
            var isFind = int.Parse(_database.QueryValue(commandText.ToString(), parameters).ToString()) > 0;

            if (isFind)
            {
                message = "此手机号在系统已存在";
                return false;
            }
            commandText = new StringBuilder("update Membership set PhoneNumber=@PhoneNumber, UserName=@PhoneNumber, updateTime=@updateTime where Id=@userId ");

            parameters = new Dictionary<string, object>
            {
                {"@userId", id},
                {"@PhoneNumber",phoneNumber},
                {"@updateTime",DateTime.Now}
            };
            bool result = _database.Execute(commandText.ToString(), parameters) > 0;
            if (!result)
                message = "修改手机号失败!";
            return result;
        }

        /// <summary>
        /// 检查用户信息附表中是否存在
        /// </summary>
        /// <param name="MembershipId"></param>
        /// <returns></returns>
        internal bool CheckMembershipIdIsExist(string MembershipId)
        {
            var commandText = new StringBuilder("select count(*) from Membership_Schedule where MembershipId=@membershipid ");
            var parameters = new Dictionary<string, object>
            {
                {"@membershipid",MembershipId}
            };
            return int.Parse(_database.QueryValue(commandText.ToString(), parameters).ToString()) > 0;
        }

        /// <summary>
        /// 新增Or修改用户详细信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update_Or_Insert_Membership_Schedule(TUser user)
        {
            string commandText = "";
            if (!string.IsNullOrEmpty(user.Id) && CheckMembershipIdIsExist(user.Id))
            {
                commandText = @"UPDATE [Membership_Schedule] SET ZipCode=@ZipCode,TelePhone=@TelePhone,PaperWork=@PaperWork,Educational=@Educational,Job=@Job,Office=@Office,Industry=@Industry,Remark=@Remark,IsMarriage=@IsMarriage,MarriageDay=@MarriageDay,MainContact=@MainContact,MainTelePhone=@MainTelePhone,OrganizationCode=@OrganizationCode,SendSms=@SendSms,MakePhone=@MakePhone,SendLetter=@SendLetter,SendEmail=@SendEmail,TransactionTime=@TransactionTime WHERE MembershipId = @MembershipId ";
            }
            else
            {
                commandText = @"Insert Into Membership_Schedule(ZipCode,TelePhone,PaperWork,Educational,Job,Office,Industry,Remark,IsMarriage,MarriageDay,MainContact,MainTelePhone,OrganizationCode,SendSms,MakePhone,SendLetter,SendEmail,TransactionTime,MembershipId) values(@ZipCode,@TelePhone,@PaperWork,@Educational,@Job,@Office,@Industry,@Remark,@IsMarriage,@MarriageDay,@MainContact,@MainTelePhone,@OrganizationCode,@SendSms,@MakePhone,@SendLetter,@SendEmail,@TransactionTime,@MembershipId)";
            }
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ZipCode", user.ZipCode);
            parameters.Add("@TelePhone", user.TelePhone);
            parameters.Add("@PaperWork", user.PaperWork);
            parameters.Add("@Educational", user.Educational);
            parameters.Add("@Job", user.Job);
            parameters.Add("@Office", user.Office);
            parameters.Add("@Industry", user.Industry);

            parameters.Add("@Remark", user.Remark);
            parameters.Add("@IsMarriage", user.IsMarriage);
            parameters.Add("@MarriageDay", user.MarriageDay);
            parameters.Add("@MainContact", user.MainContact);
            parameters.Add("@MainTelePhone", user.MainTelePhone);
            parameters.Add("@OrganizationCode", user.OrganizationCode);

            parameters.Add("@SendSms", user.SendSms);
            parameters.Add("@MakePhone", user.MakePhone);
            parameters.Add("@SendLetter", user.SendLetter);
            parameters.Add("@SendEmail", user.SendEmail);
            parameters.Add("@TransactionTime", user.TransactionTime);

            parameters.Add("@MembershipId", user.Id);
            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// add Paperwork Value to Membership_Schedule 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public void AddPaperworkToMembership_Schedule(TUser user)
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
            _database.Execute(commandText, parameters);
        }




    }


     


    /// <summary>
    /// 会员等级信息
    /// </summary>
    public class MembershipLevel
    {
        #region ==== 构造函数 ====

        public MembershipLevel() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 会员等级
        /// </summary>
        public int MLevel { get; set; }


        /// <summary>
        /// 会员编号
        /// </summary>
        public string No { get; set; }

        #endregion
    }
}
