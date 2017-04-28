using System.Collections.Generic;

namespace AspNet.Identity.SQL
{
    /// <summary>
    /// Class that represents the UserRoles table in the MySQL Database
    /// </summary>
    public class UserRolesTable
    {
        private SQLDatabase _database;

        /// <summary>
        /// Constructor that takes a SQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserRolesTable(SQLDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Returns a list of user's roles
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<string> FindByUserId(string userId)
        {
            List<string> roles = new List<string>();
            string commandText = "Select Roles.Name from UserRoles, Roles where UserRoles.UserId = @userId and UserRoles.RoleId = Roles.Id and Roles.IsDelete = @isDel";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);
            parameters.Add("@isDel", 0);
            var rows = _database.Query(commandText, parameters);
            foreach(var row in rows)
            {
                roles.Add(row["Name"]);
            }

            return roles;
        }

        /// <summary>
        /// Deletes all roles from a user in the UserRoles table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            string commandText = "Delete from UserRoles where UserId = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("UserId", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Deletes one role from a user in the UserRoles table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId,string roleId)
        {
            string commandText = "Delete from UserRoles where UserId = @userId and RoleId = @roleId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("UserId", userId);
            parameters.Add("RoleId", roleId);
            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Deletes one role from a user in the UserRoles table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int DeleteByRoleName(string userId, string name)
        {
            string commandText = "Delete from UserRoles where UserId = @userId and RoleId in (select Id from roles where Name=@name)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("UserId", userId);
            parameters.Add("Name", name);
            return _database.Execute(commandText, parameters);
        }
        /// <summary>
        /// Inserts a new role for a user in the UserRoles table
        /// </summary>
        /// <param name="user">The User</param>
        /// <param name="roleId">The Role's id</param>
        /// <returns></returns>
        public int Insert(IdentityUser user, string roleId)
        {
            string commandText = "Insert into UserRoles (UserId, RoleId) values (@userId, @roleId)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("userId", user.Id);
            parameters.Add("roleId", roleId);

            return _database.Execute(commandText, parameters);
        }
    }
}
