using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Vcyber.BLMS.ManageWeb.Models
{
    /// <summary>
    /// 相应信息
    /// </summary>
    public class ResponseMessage
    {
        #region ==== 私有字段 ====

        private int status;

        private string message;

        #endregion

        #region ==== 构造函数 ====

        public ResponseMessage() { }

        public ResponseMessage(HttpStatusCode code, string message)
        {
            this.status = (int)code;
            this.message = message;
        }

        public ResponseMessage(HttpStatusCode code, object data)
        {
            this.status = (int)code;
            this.Data = data;
        }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get { return this.message; }
            set
            {
                this.message = value;
            }
        }

        public object Data { get; set; }


        public List<string> Errors { get; set; }

        #endregion
    }
}