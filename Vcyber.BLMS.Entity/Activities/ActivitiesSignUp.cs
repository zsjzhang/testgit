using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class ActivitiesSignUp
    {
        public int Id { get; set; }

        [Required]
        public int ActivitiesId{get;set;}
        public string UserId{get;set;}
        public string UserName{get;set;}

        public DateTime CreateTime{get;set;}

       public int IsDeleted{get;set;}


    }
}
