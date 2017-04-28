using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.Common
{
    using Vcyber.BLMS.Common;

    public class ReturnObject
    {

       
        public string Code { get; set; }

        public string Message { get; set; }

        public object Content { get; set; }

        public string ReIntegralType { get; set; }

        public string UserId { get; set; }
      
        public ReturnObject()
        {
            this.Code = "200";
            this.Message = CommonUtilitys.GetMessage(Code);
        }

        public ReturnObject(string code)
        {
            this.Code = code;
            this.Message = CommonUtilitys.GetMessage(code);
        }

        public ReturnObject(int code)
        {
            this.Code = code.ToString();
            this.Message = CommonUtilitys.GetMessage(Code);
        }

        //public ReturnObject(OperateStatusEnum code)
        //{
        //    this.Code = ((int)code).ToString();
        //    this.Message = CommonUtilitys.GetMessage(Code);
        //}

        public ReturnObject(string code, object content)
            : this(code)
        {
            Content = content;
        }

        public ReturnObject(string code, string message, object content)
        {
            Code = code;
            Message = message;
            Content = content;
        }

        public ReturnObject(object content)
            : this("200")
        {
            Content = content;
        }

        public ReturnObject(ReturnResult result)
        {
            this.Code = result.IsSuccess ? "200" : "500";
            this.Message = result.Message;
            this.Content = result.Data;
        }


        public ReturnObject(string code, string message, object content, string reIntegralType):this(code,message ,content )
        {

            ReIntegralType = reIntegralType;
        }
        public ReturnObject(string code, string message, object content, string reIntegralType,string userId)
            : this(code, message, content)
        {
            UserId = userId;
            ReIntegralType = reIntegralType;
        }

    }
}
