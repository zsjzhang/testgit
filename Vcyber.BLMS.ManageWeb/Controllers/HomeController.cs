using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.ManageWeb.Models;

namespace Vcyber.BLMS.ManageWeb.Controllers
{    
    public class HomeController : Controller
    {
        [MvcAuthorize]
        public ActionResult Index()
        {
            //是否显示公告弹出框
            ViewBag.showNoticeDiv = false;
            var entity = _AppContext.NoticeInfosApp.GetNewNoticeInfo();
            if (entity != null)
                ViewBag.showNoticeDiv = true;
            return View();
        }

        [MvcAuthorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        /// <summary>
        /// 公告信息提示框
        /// </summary>
        /// <returns></returns>
        public ActionResult NoticeInfo()
        {
            var entity = _AppContext.NoticeInfosApp.GetNewNoticeInfo();
            if (entity != null)
            {
                ViewBag.Title = entity.Title;
                ViewBag.ContentInnerHtml = System.Web.HttpUtility.UrlDecode(entity.Summary);
            }
            return View();
        }

        [MvcAuthorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        [MvcAuthorize]
        public ActionResult Statistic()
        {
            //LoginForum();
            //string user = "admin";
            //string pwd = "!QAZ2wsx";
            //NameValueCollection col=new NameValueCollection();
            //col.Add("forum$ctl03$Login1$UserName", user);
            //col.Add("forum$ctl03$Login1$Password", pwd);
            //GetWebResponse("http://localhost:52612/login?g=login&returnurl=%2f", col);

            //string para = "forum$ctl03$Login1$UserName=admin&forum$ctl03$Login1$Password=pwd";
            //HttpPost("http://localhost:52612/login?g=login&returnurl=%2f", para);

            //using (var client = new WebClient())
            //{
            //    var dataToPost = Encoding.Default.GetBytes("forum$ctl03$Login1$UserName=admin&forum$ctl03$Login1$Password=!QAZ2wsx");
            //    var result = client.UploadData("http://localhost:52612/login?g=login&returnurl=%2f", "POST", dataToPost);
            //    // do something with the result
            //}

            //string user = "admin";
            //string pwd = "!QAZ2wsx";
            //NameValueCollection col=new NameValueCollection();
            //col.Add("forum$ctl03$Login1$UserName", user);
            //col.Add("forum$ctl03$Login1$Password", pwd);
            //HttpPostData1("http://localhost:52612/login?g=login&returnurl=%2f", 20000, col);

            return PartialView();
        }

        private void LoginForum()
        {
            string user = "admin";
            string pwd = "!QAZ2wsx";

            HttpClient httpClient = new HttpClient();

            // 设置请求头信息

            httpClient.DefaultRequestHeaders.Add("Host", "localhost:52612");
            httpClient.DefaultRequestHeaders.Add("Method", "Post");
            httpClient.DefaultRequestHeaders.Add("KeepAlive", "false");   // HTTP KeepAlive设为false，防止HTTP连接保持
            httpClient.DefaultRequestHeaders.Add("UserAgent",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.95 Safari/537.11");

            // 构造POST参数
            HttpContent postContent = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
               {"forum$ctl03$Login1$UserName", user},
               {"forum$ctl03$Login1$Password", pwd}
            });

            MultipartFormDataContent postContent1 = new MultipartFormDataContent();
            postContent1.Add(postContent);


            httpClient.PostAsync("http://localhost:52612/login?g=login&returnurl=%2f", postContent1)
               .ContinueWith((postTask) =>
               {
                   HttpResponseMessage response = postTask.Result;

                   // 确认响应成功，否则抛出异常
                   response.EnsureSuccessStatusCode();

                   // 异步读取响应为字符串
                   response.Content.ReadAsStringAsync().ContinueWith((readTask) => Console.WriteLine("响应网页内容：" + readTask.Result));
                   Console.WriteLine("响应是否成功：" + response.IsSuccessStatusCode);
                   Console.WriteLine("响应头信息如下：\n");
                   var headers = response.Headers;
                   foreach (var header in headers)
                   {
                       Console.WriteLine("{0}: {1}", header.Key, string.Join("", header.Value.ToList()));
                   }
               }
               );

        }

        static string GetWebResponse(string url, NameValueCollection parameters)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "multipart/form-data";
            httpWebRequest.Method = "POST";

            var sb = new StringBuilder();
            foreach (var key in parameters.AllKeys)
                sb.Append(key + "=" + parameters[key] + "&");
            sb.Length = sb.Length - 1;

            byte[] requestBytes = Encoding.UTF8.GetBytes(sb.ToString());
            httpWebRequest.ContentLength = requestBytes.Length;

            using (var requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
            }

            Task<WebResponse> responseTask = Task.Factory.FromAsync<WebResponse>(httpWebRequest.BeginGetResponse, httpWebRequest.EndGetResponse, null);
            using (var responseStream = responseTask.Result.GetResponseStream())
            {
                var reader = new StreamReader(responseStream);
                return reader.ReadToEnd();
            }
        }

        static string HttpPost(string url, string Parameters)
        {
            var req = System.Net.WebRequest.Create(url);

            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Parameters);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null)
                return null;
            var sr = new StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        private static string HttpPostData1(string url, int timeOut, NameValueCollection stringDict)
        {
            string responseContent;
            var memStream = new MemoryStream();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            // 边界符  
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            // 边界符  
            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            // 最后的结束符  
            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

            // 设置属性  
            webRequest.Method = "POST";
            webRequest.Timeout = timeOut;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            // 写入文件  
            const string filePartHeader =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                 "Content-Type: application/octet-stream\r\n\r\n";

            
            var buffer = new byte[1024];
            int bytesRead; // =0  

            
            // 写入字符串的Key  
            var stringKeyHeader = "\r\n--" + boundary +
                                   "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                   "\r\n\r\n{1}\r\n";

            foreach (byte[] formitembytes in from string key in stringDict.Keys
                                             select string.Format(stringKeyHeader, key, stringDict[key])
                                                 into formitem
                                                 select Encoding.UTF8.GetBytes(formitem))
            {
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

            // 写入最后的结束边界符  
            memStream.Write(endBoundary, 0, endBoundary.Length);

            webRequest.ContentLength = memStream.Length;

            var requestStream = webRequest.GetRequestStream();

            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

            using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(),
                                                            Encoding.GetEncoding("utf-8")))
            {
                responseContent = httpStreamReader.ReadToEnd();
            }

            httpWebResponse.Close();
            webRequest.Abort();

            return responseContent;
        }  

        private static string HttpPostData(string url, int timeOut, string fileKeyName,
                                    string filePath, NameValueCollection stringDict)
        {
            string responseContent;
            var memStream = new MemoryStream();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            // 边界符  
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            // 边界符  
            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            // 最后的结束符  
            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

            // 设置属性  
            webRequest.Method = "POST";
            webRequest.Timeout = timeOut;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            // 写入文件  
            const string filePartHeader =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                 "Content-Type: application/octet-stream\r\n\r\n";
            var header = string.Format(filePartHeader, fileKeyName, filePath);
            var headerbytes = Encoding.UTF8.GetBytes(header);

            memStream.Write(beginBoundary, 0, beginBoundary.Length);
            memStream.Write(headerbytes, 0, headerbytes.Length);

            var buffer = new byte[1024];
            int bytesRead; // =0  

            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                memStream.Write(buffer, 0, bytesRead);
            }

            // 写入字符串的Key  
            var stringKeyHeader = "\r\n--" + boundary +
                                   "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                   "\r\n\r\n{1}\r\n";

            foreach (byte[] formitembytes in from string key in stringDict.Keys
                                             select string.Format(stringKeyHeader, key, stringDict[key])
                                                 into formitem
                                                 select Encoding.UTF8.GetBytes(formitem))
            {
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

            // 写入最后的结束边界符  
            memStream.Write(endBoundary, 0, endBoundary.Length);

            webRequest.ContentLength = memStream.Length;

            var requestStream = webRequest.GetRequestStream();

            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

            using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(),
                                                            Encoding.GetEncoding("utf-8")))
            {
                responseContent = httpStreamReader.ReadToEnd();
            }

            fileStream.Close();
            httpWebResponse.Close();
            webRequest.Abort();

            return responseContent;
        }  
    }
}