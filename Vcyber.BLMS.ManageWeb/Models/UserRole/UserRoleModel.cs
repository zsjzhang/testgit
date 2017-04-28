using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class UserRoleModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<UserRole> Roles { get; set; }
    }

    public class UserRole
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsChecked { get; set; }
    }
}