using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public    class FittingValidate
    {

 //        Id  int not null identity (1,1) primary key,
 //Code varchar(50)  null,
 //UserAddress varchar(100) null,
 //Longitude  float null,
 //Latitude  float null,
 //Altitude  float null,
 //Userid varchar(50) null,
 //Ctype   varchar(20) null,
 //Result  int  null,
 //Createtime datetime null

        public int Id { get; set; }
        public string Code { get; set; }
        public string UserAddress { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public float Altitude { get; set; }
        public string Userid { get; set; }
        public string Ctype { get; set; }
        public string  Result { get; set; }

        public DateTime Createtime { get; set; }
    }
}
