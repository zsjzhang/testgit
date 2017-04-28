namespace Vcyber.BLMS.WebApi.Common
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Results;

    using Microsoft.Ajax.Utilities;

    using Vcyber.BLMS.Entity.Common;
    using Vcyber.BLMS.WebApi.Common;
    using System;

    public static class ApiControllerExtension
    {
        /// <summary>
        /// 获取Header内容
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetHeader(this ApiController controller, string key)
        {
            return controller.ActionContext.Request.GetHeader(key);
        }

        /// <summary>
        /// 返回数据（json格式）
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="dataResult">数据集</param>
        /// <returns></returns>
        public static IHttpActionResult DataResult(this ApiController instance, string status)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            message.Headers.Add("resultcode", status);
            return new ResponseMessageResult(message);
        }

        /// <summary>
        /// 返回数据（json格式）
        /// </summary>
        /// <typeparam name="T">数据结果集类型</typeparam>
        /// <param name="instance"></param>
        /// <param name="dataResult">数据集</param>
        /// <param name="status">数据集状态</param>
        /// <returns></returns>
        public static IHttpActionResult DataResult<T>(this ApiController instance, T dataResult, string status)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            message.Headers.Add("resultcode", status);
            message.Content = new ObjectContent<T>(dataResult, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
            return new ResponseMessageResult(message);
        }

        /// <summary>
        /// 返回数据（json格式）
        /// </summary>
        /// <typeparam name="T">数据结果集类型</typeparam>
        /// <param name="instance"></param>
        /// <param name="dataResult">数据集</param>
        /// <param name="code">http 响应状态</param>
        /// <returns></returns>
        public static IHttpActionResult DataResult<T>(this ApiController instance, T dataResult, HttpStatusCode code)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            message.StatusCode = code;
            message.Content = new ObjectContent<T>(dataResult, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
            return new ResponseMessageResult(message);
        }

        /// <summary>
        /// 获取请求设备的key
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string GetSysCode(this ApiController instance)
        {
            try
            {
                var values = instance.Request.Headers.GetValues("appkey");

                if (values != null)
                {
                    foreach (var item in values)
                    {
                        return item;
                    }
                }

                return "blms";
            }
            catch (Exception ex)
            {
                return "blms";
            }
            
        }
    }
}