using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class FunctionUrlModel
    { /// <summary>
        /// UrlId
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 功能Id
        /// </summary>
        public int FunId { get; set; }

        public string Action { get; set; }
        public string Controller { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDel { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
    }
}