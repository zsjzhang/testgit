using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Vcyber.BLMS.Common.Web
{    
    /// <summary>
    /// http操作类
    /// </summary>
    public class HttpUtil
    {
        private HttpClient _client;
        private static HttpUtil _instance;
        //锁定对象
        private static object _lock = new object();
        public HttpUtil()
        {
            _client = new HttpClient();
        }
        /// <summary>
        /// 单例
        /// </summary>        
        public static HttpUtil GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new HttpUtil();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// 创建
        /// </summary>
        public string Post(string requestUri, string requestJson)
        {            
            HttpContent httpContent = new StringContent(requestJson);            
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            HttpResponseMessage response = _client.PostAsync(requestUri, httpContent).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            return result;
        }
        /// <summary>
        /// 创建
        /// </summary>
        public T Post<T, E>(string requestUri, E model) where T : new()
        {
            var obj = WebUtils.JsonToObj<T>(Post(requestUri, WebUtils.ObjToJson<E>(model)),new T());
            return obj;
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }

        public static string Post(string requestUrl, string requestData, string contentType, string certPath = "", string certPwd="")
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 1000;
                //设置https验证方式
                if (requestUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(requestUrl);

                request.Method = "POST";
                request.Timeout = 10000;

                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);              //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                request.ContentType = contentType;
                byte[] data = System.Text.Encoding.UTF8.GetBytes(requestData);
                request.ContentLength = data.Length;

                //是否使用证书
                if (!string.IsNullOrEmpty(certPath))
                {
                    X509Certificate2 cert = new X509Certificate2(certPath, certPwd, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
                    request.ClientCertificates.Add(cert);           
                }

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                result = e.Message;
                log4net.LogManager.GetLogger("http_util_post_method").Error(e);
                System.Threading.Thread.ResetAbort();
            }
            catch (Exception e)
            {
                result = e.Message;
                log4net.LogManager.GetLogger("http_util_post_method").Error(e);
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        /// <summary>
        /// 处理http GET请求，返回数据
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <returns>http GET成功后返回的数据，失败抛WebException异常</returns>
        public static string Get(string url)
        {
            System.GC.Collect();
            string result = "";

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            //请求url以获取数据
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 1000;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";

                //设置代理
                //WebProxy proxy = new WebProxy();
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);
                //request.Proxy = proxy;

                //获取服务器返回
                response = (HttpWebResponse)request.GetResponse();

                //获取HTTP返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                log4net.LogManager.GetLogger("http_util_get_method").Error("Thread - caught ThreadAbortException - resetting.");
                log4net.LogManager.GetLogger("http_util_get_method").Error(string.Format("Exception message: {0}", e.Message));
                System.Threading.Thread.ResetAbort();
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("http_util_get_method").Error(e);
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }
    }
}