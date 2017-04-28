using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    public class RecommendViewModel
    {
        public string OpenId { get; set; }

        //被推荐人的信息
        public string Name { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class RecommendViewModel_SB
    {
        [Required]
        public string OpenId { get; set; }

        //被推荐人的信息
        public string Name1 { get; set; }

        public string PhoneNumber1 { get; set; }

        public string Name2 { get; set; }

        public string PhoneNumber2 { get; set; }

        public string Name3 { get; set; }

        public string PhoneNumber3 { get; set; }
    }
}