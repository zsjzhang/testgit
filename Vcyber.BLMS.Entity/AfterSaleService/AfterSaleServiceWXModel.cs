using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class AfterSaleServiceWXModel
    {
        public int ret { get; set; }

        public string errorcode { get; set; }

        public string msg { get; set; }

        public AfterSaleServiceWXModelData data { get; set; }
    }

    public class AfterSaleServiceWXModelData
    {
        public int id{get;set;}

        public string tel{get;set;}

        public string openId{get;set;}

        public string cardId{get;set;}

        public string code{get;set;}

        public int cardState{get;set;}

        public string hx_openid{get;set;}

        public string hx_cardid{get;set;}

        public string remark { get; set; }

        public DateTime hx_time{get;set;}

        public DateTime createTime{get;set;}
    }
}
