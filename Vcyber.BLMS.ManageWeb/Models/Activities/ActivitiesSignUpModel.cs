using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class ActivitiesSignUpModel
    {
        public int Id { get; set; }

        public int ActivitiesId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string CreateTime { get; set; }

        public int IsDeleted { get; set; }
    }
}