using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using AspNet.Identity.SQL;
using System.Collections.Generic;

namespace Vcyber.BLMS.ManageWeb.Models
{
    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明

            //RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(ApplicationDbContext.Create());
            //ClaimsIdentity claim = userIdentity as ClaimsIdentity;

            //foreach (var role in claim.Claims.Where(c => c.Type == ClaimTypes.Role))
            //{
            //    var roleModel = await roleStore.FindByNameAsync(role.Value);
            //    role.Properties.Add("Id", roleModel.Id);
            //    role.Properties.Add("Name", roleModel.Name);
            //}

            //Claim claim = new Claim(ClaimTypes.Role, "abc");
            //userIdentity.AddClaim(claim);

            //var roles = await manager.GetRolesAsync(userIdentity.GetUserId());
            //foreach (string role in roles)
            //{
            //    userIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            //}

            return userIdentity;
        }
    }

    public class ApplicationDbContext : SQLDatabase
    {
        public ApplicationDbContext(string connectionName)
            : base(connectionName)
        {
        }

        public static ApplicationDbContext Create()

        {
            return new ApplicationDbContext("DbConnection");
        }

    }
}