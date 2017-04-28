using System;
using System.Collections.Generic;

namespace AspNet.Identity.SQL
{
    /// <summary>
    /// Class that represents the Role table in the MySQL Database
    /// </summary>
    public class RoleTable 
    {
        private SQLDatabase _database;

        /// <summary>
        /// Constructor that takes a SQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public RoleTable(SQLDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Deltes a role from the Roles table
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns></returns>
        public int Delete(string roleId)
        {
            string commandText = "delete from roles where Id=@Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Id", roleId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Inserts a new Role in the Roles table
        /// </summary>
        /// <param name="roleName">The role's name</param>
        /// <returns></returns>
        public int Insert(IdentityRole role)
        {
            string commandText = "Insert into Roles (Id, Name,Describe) values (@id, @name,@describe)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", role.Name);
            parameters.Add("@id", role.Id);
            parameters.Add("@describe", role.Describe);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Returns a role name given the roleId
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns>Role name</returns>
        public string GetRoleName(string roleId)
        {
            string commandText = "Select Name from Roles where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", roleId);

            return _database.GetStrValue(commandText, parameters);
        }

        /// <summary>
        /// Returns the role Id given a role name
        /// </summary>
        /// <param name="roleName">Role's name</param>
        /// <returns>Role's Id</returns>
        public string GetRoleId(string roleName)
        {
            string roleId = null;
            string commandText = "Select Id from Roles where Name = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", roleName } };

            var result = _database.QueryValue(commandText, parameters);
            if (result != null)
            {
                return Convert.ToString(result);
            }

            return roleId;
        }

        /// <summary>
        /// Gets the IdentityRole given the role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public IdentityRole GetRoleById(string roleId)
        {
            var roleName = GetRoleName(roleId);
            IdentityRole role = null;

            if(roleName != null)
            {
                role = new IdentityRole(roleName, roleId);
            }

            return role;

        }

        /// <summary>
        /// Gets the IdentityRole given the role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IdentityRole GetRoleByName(string roleName)
        {
            var roleId = GetRoleId(roleName);
            IdentityRole role = null;

            if (roleId != null)
            {
                role = new IdentityRole(roleName, roleId);
            }

            return role;
        }

        public int Update(IdentityRole role)
        {
            string commandText = "UPDATE roles SET Name=@Name,Describe=@Describe where Id=@Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Id", role.Id);
            parameters.Add("@Name", role.Name);
            parameters.Add("@Describe", role.Describe);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public List<IdentityRole> GetRoles(int pageIndex, int pageSize,out int totalCount)
        {
            string commandText = "SELECT COUNT(1) FROM roles where IsDelete=@isDel";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@isDel", 0);
            int.TryParse(_database.QueryValue(commandText, parameters).ToString(),out totalCount);

            List<IdentityRole> rolelList = new List<IdentityRole>();
            commandText = "select Id,Name,IsDelete,Describe from (select row_number() over (order by id) as row_num,* from roles) r where row_num between @pageIndex and @pageEnd";
            parameters.Add("@pageIndex", pageIndex);
            parameters.Add("@pageEnd", pageSize + pageIndex);
            var roles = _database.Query(commandText, parameters);

            foreach (var role in roles)
            {
                var identityRole = new IdentityRole();
                identityRole.Id = role["Id"];
                identityRole.Name = role["Name"];
                identityRole.Describe = role["Describe"];
                rolelList.Add(identityRole);
            }

            return rolelList;

            
        }
    }
}
