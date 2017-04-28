using System;
using System.Collections.Generic;
using System.Text;

namespace AspNet.Identity.SQL
{
    /// <summary>
    /// Class that represents the Users table in the MySQL Database
    /// </summary>
    public class UserTable<TUser>
        where TUser :IdentityUser
    {
        private SQLDatabase _database;

        /// <summary>
        /// Constructor that takes a SQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserTable(SQLDatabase database)
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
            string commandText = "Select Name from Users where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

            return _database.GetStrValue(commandText, parameters);
        }

        /// <summary>
        /// Returns a User ID given a user name
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public string GetUserId(string userName)
        {
            string commandText = "Select Id from Users where UserName = @name";
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
            string commandText = "Select * from Users where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

            var rows = _database.Query(commandText, parameters);
            if (rows != null && rows.Count == 1)
            {
                var row = rows[0];
                user = (TUser)Activator.CreateInstance(typeof(TUser));
                user.Id = row["Id"];
                user.UserName = row["UserName"];
                user.Status = int.Parse(row["Status"]);
                user.PasswordHash = string.IsNullOrEmpty(row["PasswordHash"]) ? null : row["PasswordHash"];
                user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
                user.Email = string.IsNullOrEmpty(row["Email"]) ? null : row["Email"];
                user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true:false;
                user.PhoneNumber = string.IsNullOrEmpty(row["PhoneNumber"]) ? null : row["PhoneNumber"];
                user.Department = string.IsNullOrEmpty(row["Department"]) ? null : row["Department"];
                user.DealerId = string.IsNullOrEmpty(row["DealerId"]) ? null : row["DealerId"];
                user.DealerName = string.IsNullOrEmpty(row["DealerName"]) ? null : row["DealerName"];
                user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
                user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
                user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
                user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
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
            string commandText = "Select * from Users where UserName = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

            var rows = _database.Query(commandText, parameters);
            foreach(var row in rows)
            {
                TUser user = (TUser)Activator.CreateInstance(typeof(TUser));
                user.Id = row["Id"];
                user.UserName = row["UserName"];
                user.PasswordHash = string.IsNullOrEmpty(row["PasswordHash"]) ? null : row["PasswordHash"];
                user.SecurityStamp = string.IsNullOrEmpty(row["SecurityStamp"]) ? null : row["SecurityStamp"];
                user.Email = string.IsNullOrEmpty(row["Email"]) ? null : row["Email"];
                user.EmailConfirmed = row["EmailConfirmed"] == "1" ? true : false;
                user.PhoneNumber = string.IsNullOrEmpty(row["PhoneNumber"]) ? null : row["PhoneNumber"];
                user.PhoneNumberConfirmed = row["PhoneNumberConfirmed"] == "1" ? true : false;
                user.LockoutEnabled = row["LockoutEnabled"] == "1" ? true : false;
                user.TwoFactorEnabled = row["TwoFactorEnabled"] == "1" ? true : false;
                user.LockoutEndDateUtc = string.IsNullOrEmpty(row["LockoutEndDateUtc"]) ? DateTime.Now : DateTime.Parse(row["LockoutEndDateUtc"]);
                user.AccessFailedCount = string.IsNullOrEmpty(row["AccessFailedCount"]) ? 0 : int.Parse(row["AccessFailedCount"]);
                users.Add(user);
            }

            return users;
        }

        public List<TUser> GetUserByEmail(string email)
        {
            return null;
        }

        /// <summary>
        /// Return the user's password hash
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public string GetPasswordHash(string userId)
        {
            string commandText = "Select PasswordHash from Users where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", userId);

            var passHash = _database.GetStrValue(commandText, parameters);
            if(string.IsNullOrEmpty(passHash))
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
            string commandText = "Update Users set PasswordHash = @pwdHash where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@pwdHash", passwordHash);
            parameters.Add("@id", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Returns the user's security stamp
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSecurityStamp(string userId)
        {
            string commandText = "Select SecurityStamp from Users where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };
            var result = _database.GetStrValue(commandText, parameters);

            return result;
        }

        /// <summary>
        /// Inserts a new user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Insert(TUser user)
        {
            string commandText = @"Insert into Users (UserName, Id, PasswordHash, SecurityStamp,Email,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed, AccessFailedCount,LockoutEnabled,LockoutEndDateUtc,TwoFactorEnabled,IsDel,Status,CreateTime,Department,DealerId,DealerName)
                values (@name, @id, @pwdHash, @SecStamp,@email,@emailconfirmed,@phonenumber,@phonenumberconfirmed,@accesscount,@lockoutenabled,@lockoutenddate,@twofactorenabled,@IsDel,@Status,@CreateTime,@Department,@DealerId,@DealerName)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", user.UserName);
            parameters.Add("@id", user.Id);
            parameters.Add("@pwdHash", user.PasswordHash);
            parameters.Add("@SecStamp", user.SecurityStamp);
            parameters.Add("@email", user.Email);
            parameters.Add("@emailconfirmed", user.EmailConfirmed);
            parameters.Add("@phonenumber", user.UserName);
            parameters.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
            parameters.Add("@accesscount", user.AccessFailedCount);
            parameters.Add("@lockoutenabled", user.LockoutEnabled);
            parameters.Add("@lockoutenddate", user.LockoutEndDateUtc);
            parameters.Add("@twofactorenabled", user.TwoFactorEnabled);
            parameters.Add("@IsDel", 0);
            parameters.Add("@Status", user.Status);
            parameters.Add("@CreateTime", DateTime.Now);
            if (user.Department == "-1")
                user.Department = string.Empty;
            parameters.Add("@Department",user.Department);
            parameters.Add("@DealerId", user.DealerId);
            parameters.Add("@DealerName", user.DealerName);
            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        private int Delete(string userId)
        {
            string commandText = "Update Users set IsDel = @IsDel where Id = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);
            parameters.Add("@IsDel", 1);
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
            string commandText = @"Update Users set UserName = @userName, PasswordHash = @pswHash, SecurityStamp = @secStamp, 
                Email=@email, EmailConfirmed=@emailconfirmed, PhoneNumber=@phonenumber, PhoneNumberConfirmed=@phonenumberconfirmed,
                AccessFailedCount=@accesscount, LockoutEnabled=@lockoutenabled, LockoutEndDateUtc=@lockoutenddate, TwoFactorEnabled=@twofactorenabled,IsDel=@IsDel,Status=@Status,Department=@Department
                WHERE Id = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userName", user.UserName);
            parameters.Add("@pswHash", user.PasswordHash);
            parameters.Add("@secStamp", user.SecurityStamp);
            parameters.Add("@userId", user.Id);
            parameters.Add("@email", user.Email);
            parameters.Add("@emailconfirmed", user.EmailConfirmed);
            parameters.Add("@phonenumber", user.PhoneNumber);
            parameters.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
            parameters.Add("@accesscount", user.AccessFailedCount);
            parameters.Add("@lockoutenabled", user.LockoutEnabled);
            parameters.Add("@lockoutenddate", user.LockoutEndDateUtc);
            parameters.Add("@twofactorenabled", user.TwoFactorEnabled);

            parameters.Add("@IsDel", 0);
            parameters.Add("@Status", user.Status);
            parameters.Add("@Department", user.Department);
            return _database.Execute(commandText, parameters);
        }

        public List<TUser> GetUsers(int pageIndex, int pageSize,string userName, string userRole, out int totalCount)
        {
            var whereExp = new StringBuilder();
            string commandText = @"SELECT COUNT(1) FROM Users u inner join UserRoles ur on u.Id = ur.UserId
                                        inner join Roles r on ur.RoleId = r.Id where IsDel=@isDel{0}";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@isDel", 0);

            if (!string.IsNullOrEmpty(userName))
            {
                whereExp.Append(" and UPPER(UserName) like @userName");

                parameters.Add("@userName", "%" + userName.ToUpper() + "%");
            }

            if (!string.IsNullOrEmpty(userRole))
            {
                whereExp.Append(" and UPPER(r.Id) = @userRole");

                parameters.Add("@userRole", userRole.ToUpper());
            }

            int.TryParse(_database.QueryValue(string.Format(commandText.ToString(), whereExp.ToString()), parameters).ToString(), out totalCount);

            List<TUser> userList = new List<TUser>();
            commandText =
                @"select * from (select row_number() over (order by u.id) as row_num,u.* from Users U inner join UserRoles ur on u.Id = ur.UserId
inner join Roles r on ur.RoleId = r.Id where u.IsDel=@isDel{0} ) u where row_num between @PageIndex and @PageEnd";


            
            parameters.Add("@PageIndex", pageIndex+1);
            parameters.Add("@PageEnd", pageSize + pageIndex);
            var users = _database.Query(string.Format(commandText.ToString(), whereExp.ToString()), parameters);

            foreach (var user in users)
            {
                var identityUser = (TUser)Activator.CreateInstance(typeof(TUser));
                identityUser.Id = user["Id"];
                identityUser.UserName = user["UserName"];
                identityUser.Email = user["Email"];
                identityUser.Department = user["Department"];
                identityUser.PhoneNumber = user["PhoneNumber"];
                identityUser.CreateTime = user["CreateTime"];
                identityUser.Status = int.Parse(user["Status"]);
                userList.Add(identityUser);
            }

            return userList;


        }
    }
}
