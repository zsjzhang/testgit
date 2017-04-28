using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common.Web
{
    /// <summary>
    /// Web 辅助类
    /// </summary>
    public class WebUtils
    {
        #region ==== 公共方法 ====

        /// <summary>
        /// 将实例对象序列化成Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ObjToJson<T>(T data)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(data.GetType());

                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, data);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将Json格式string 反序列为指定实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T JsonToObj<T>(string json, T defaultValue)
        {
            try
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));

                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    object obj = serializer.ReadObject(ms);

                    return (T)Convert.ChangeType(obj, typeof(T));
                }
            }
            catch
            {
                return defaultValue;
            }
        }

        public static T XmlToObj<T>(string xml, T defaultValue)
        {
            try
            {
                System.Runtime.Serialization.DataContractSerializer serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T));

                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    object obj = serializer.ReadObject(ms);

                    return (T)Convert.ChangeType(obj, typeof(T));
                }
            }
            catch
            {
                return defaultValue;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object XmlDeserialize<T>(string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    System.Xml.Serialization.XmlSerializer xmldes = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }
        public static string ObjToXml<T>(T data)
        {
            System.Runtime.Serialization.DataContractSerializer serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, data);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encodingName">编号格式：utf-8</param>
        /// <param name="htmlUrl">请求Url</param>
        /// <returns></returns>
        public static string GET_WebRequestHTML(string encodingName, string htmlUrl, Dictionary<string, string> header)
        {
            string html = string.Empty;
            HttpWebResponse httpWebResponse = null;
            Stream responseStream = null;
            StreamReader streamReader = null;
            try
            {
                Encoding encoding = Encoding.GetEncoding(encodingName);
                WebRequest webRequest = WebRequest.Create(htmlUrl);

                if (header != null && header.Count > 0)
                {
                    foreach (var keyName in header.Keys)
                    {
                        webRequest.Headers.Add(keyName, header[keyName]);
                    }
                }

                httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
                responseStream = httpWebResponse.GetResponseStream();
                streamReader = new StreamReader(responseStream, encoding);

                html = streamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                LogService.Instance.Error(ex.Message, ex);
            }
            finally 
            {
                if (httpWebResponse != null)
                {
                    httpWebResponse.Close();                    
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (streamReader != null)
                {
                    streamReader.Close();
                }                
            }

            return html;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encodingName">编号格式：utf-8</param>
        /// <param name="htmlUrl">请求Url</param>
        /// <returns></returns>
        public static string GET_WebRequestHTML(string encodingName, string htmlUrl)
        {
            return GET_WebRequestHTML(encodingName,htmlUrl,null);
        }

        /// <summary>
        /// 将网址类容转换成文本字符串 post请求
        /// </summary>
        /// <param name="data">要post的数据</param>
        /// <param name="url">目标url</param>
        /// <returns>服务器响应</returns>
        public static string POST_HttpWebRequestHTML(string encodingName, string htmlUrl, string postData, Dictionary<string, string> header)
        {
            string html = string.Empty;

            try
            {
                Encoding encoding = Encoding.GetEncoding(encodingName);

                byte[] bytesToPost = encoding.GetBytes(postData);

                WebRequest webRequest = WebRequest.Create(htmlUrl);
                HttpWebRequest httpRequest = webRequest as System.Net.HttpWebRequest;

                if (header!=null&&header.Count>0)
                {
                    foreach (var keyName in header.Keys)
                    {
                        httpRequest.Headers.Add(keyName,header[keyName]);
                    }
                }

                httpRequest.Method = "POST";
                httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                httpRequest.ContentType = "application/json";
                httpRequest.ContentLength = bytesToPost.Length;
                httpRequest.Timeout = 15000;
                httpRequest.ReadWriteTimeout = 15000;
                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(bytesToPost, 0, bytesToPost.Length);
                requestStream.Close();

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, encoding);

                html = streamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                LogService.Instance.Error(ex.Message, ex);
            }

            return html;
        }

        /// <summary>
        /// 将网址类容转换成文本字符串 post请求
        /// </summary>
        /// <param name="data">要post的数据</param>
        /// <param name="url">目标url</param>
        /// <returns>服务器响应</returns>
        public static string POST_HttpWebRequestHTML(string encodingName,string htmlUrl,string postData)
        {
            return POST_HttpWebRequestHTML(encodingName,htmlUrl,postData,null);
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
            }
            else
            {
                return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
        }
        /// <summary>
        /// 数据来源线索
        /// 增加数据来源线索明细
        /// </summary>
        /// <returns></returns>
        public static string Device()
        {
            var userAgent = System.Web.HttpContext.Current.Request.UserAgent.ToLower();
            //是否包含微信的字符串
            var device = "blms_pc_web";

            var deviceFrom = System.Web.HttpContext.Current.Session["baseFrom"];
            if (deviceFrom != null && deviceFrom.ToString() != "")
            {
                device += ("_" + deviceFrom.ToString());
            }
            return device;
        }
        /// <summary>
        /// 去掉字符串中的Html标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ReplaceHtmlTag(string html)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");
            return strText;
        }
        #endregion
    }
}
