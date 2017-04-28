using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.FrontWeb.Models
{
    public class DataResult
    {
        #region ==== 构造函数 ====

        public DataResult() {
        
        }

        #endregion

        #region ==== 公共属性 ====

        public bool Success { get; set; }
        /// <summary>
        /// 请求状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 请求信息
        /// </summary>
        public string Message { get; set; }

        #endregion

        #region ==== 公共方法 ====

        public static JsonResult CreateData(EExecuteStatus status, string messge)
        {
            JsonResult result = new JsonResult();
            result.Data = new DataResult() { Status = (int)status, Message = messge };
            return result;
        }

        #endregion
    }
}