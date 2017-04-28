using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization.Configuration;
using Microsoft.AspNet.Identity;

namespace AspNet.Identity.SQL
{
    /// <summary>
    /// Class that implements the key ASP.NET Identity role store iterfaces
    /// </summary>
    public class RoleStore<TRole> : IQueryableRoleStore<TRole>
        where TRole : IdentityRole
    {
        private RoleTable roleTable;
        public SQLDatabase Database { get; private set; }

        public IQueryable<TRole> Roles
        {
            get
            {
                int total = 0;
                return roleTable.GetRoles(0, int.MaxValue, out total).Cast<TRole>().AsQueryable();
            }
        }


        /// <summary>
        /// Default constructor that initializes a new SQLDatabase
        /// instance using the Default Connection string
        /// </summary>
        public RoleStore()
            : this(new SQLDatabase())
        {
            //new RoleStore<TRole>(new SQLDatabase());
        }

        /// <summary>
        /// Constructor that takes a SQLDatabase as argument 
        /// </summary>
        /// <param name="database"></param>
        public RoleStore(SQLDatabase database)
        {
            Database = database;
            roleTable = new RoleTable(database);
        }

        public Task CreateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            roleTable.Insert(role);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("user");
            }

            roleTable.Delete(role.Id);

            return Task.FromResult<Object>(null);
        }

        public Task<TRole> FindByIdAsync(string roleId)
        {
            TRole result = roleTable.GetRoleById(roleId) as TRole;

            return Task.FromResult<TRole>(result);
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            TRole result = roleTable.GetRoleByName(roleName) as TRole;

            return Task.FromResult<TRole>(result);
        }

        public Task<List<TRole>> GetRoles(int pageIndex, int pageSize, out int totalCount)
        {
            List<TRole> roleList = roleTable.GetRoles(pageIndex, pageSize, out totalCount) as List<TRole>;


            return Task.FromResult<List<TRole>>(roleList);
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("user");
            }

            roleTable.Update(role);

            return Task.FromResult<Object>(null);
        }

        public void Dispose()
        {
            if (Database != null)
            {
                Database.Dispose();
                Database = null;
            }
        }

    }
}
