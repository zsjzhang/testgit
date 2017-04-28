using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.FrontWeb.Models
{
    public class UserCarDetailModel
    {
        public ApplicationUser CurrentUser { get; set; }

        public Car CarInfo { get; set; }
    }
}