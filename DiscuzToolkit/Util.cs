using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace Discuz.Toolkit
{
    public class Util
    {
        private const string LINE = "\r\n";

        private static Dictionary<int, XmlSerializer> serializer_dict = new Dictionary<int, XmlSerializer>();

        //private DNTParam VersionParam = DNTParam.Create("v", "1.0");
        private string api_key;
        private string secret;
        private string url;
        private bool use_json;

        private static XmlSerializer ErrorSerializer
        {
            get
            {
                return GetSerializer(typeof(Error));
            }
        }

        public Util(string api_key, string secret, string url)
        {
            this.api_key = api_key;
            this.secret = secret;
            this.url = url;
        }

        public bool UseJson
        {
            get { return use_json; }
            set { use_json = value; }
        }

        internal string SharedSecret
        {
            get { return secret; }
            set { secret = value; }
        }

        internal string ApiKey
        {
            get { return api_key; }
        }

        internal string Url
        {
            get { return url; }
            set { url = value; }
        }

        public T GetResponse<T>(string method_name, params DiscuzParam[] parameters)
        {
            //string url = FormatGetUrl(method_name, parameters);
            //byte[] response_bytes = GetResponseBytes(url);

            DiscuzParam[] signed = Sign(method_name, parameters);

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < signed.Length; i++)
            {
                if (i > 0)
                    builder.Append("&");

                builder.Append(signed[i].ToEncodedString());
            }

            byte[] response_bytes = GetResponseBytes(Url, method_name, builder.ToString());


            XmlSerializer response_serializer = GetSerializer(typeof(T));
            try
            {
                T response = (T)response_serializer.Deserialize(new MemoryStream(response_bytes));
                return response;
            }
            catch
            {
                Error error = (Error)ErrorSerializer.Deserialize(new MemoryStream(response_bytes));
                throw new DiscuzException(error.ErrorCode, error.ErrorMsg);
            }
        }

        //public T GetResponse<T>(string method_name, params DiscuzParam[] parameters)
        //{
        //    string url = FormatGetUrl(method_name, parameters);
        //    byte[] response_bytes = GetResponseBytes(url);

        //    XmlSerializer response_serializer = GetSerializer(typeof(T));
        //    try
        //    {
        //        T response = (T)response_serializer.Deserialize(new MemoryStream(response_bytes));
        //        return response;
        //    }
        //    catch
        //    {
        //        Error error = (Error)ErrorSerializer.Deserialize(new MemoryStream(response_bytes));
        //        throw new DiscuzException(error.ErrorCode, error.ErrorMsg);
        //    }
        //}

        //public XmlDocument GetResponse(string method_name, params DiscuzParam[] parameters)
        //{
        //    string url = FormatGetUrl(method_name, parameters);
        //    byte[] response_bytes = GetResponseBytes(url);

        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(Encoding.Default.GetString(response_bytes));

        //    return doc;
        //}


        //public static byte[] GetResponseBytes(string url)
        //{
        //    WebRequest request = HttpWebRequest.Create(url);
        //    WebResponse response = null;

        //    try
        //    {
        //        response = request.GetResponse();
        //        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
        //        {
        //            return Encoding.UTF8.GetBytes(reader.ReadToEnd());
        //        }
        //    }
        //    finally
        //    {
        //        if (response != null)
        //            response.Close();
        //    }
        //}

        public static byte[] GetResponseBytes(string apiUrl, string method_name, string postData)
        {


            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(apiUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            request.Timeout = 20000;

            HttpWebResponse response = null;

            try
            {
                StreamWriter swRequestWriter = new StreamWriter(request.GetRequestStream());
                swRequestWriter.Write(postData);
                if (swRequestWriter != null)
                    swRequestWriter.Close();

                response = (HttpWebResponse) request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    var s = reader.ReadToEnd();
                    return Encoding.UTF8.GetBytes(s);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }


        private string FormatGetUrl(string method_name, params DiscuzParam[] parameters)
        {
            DiscuzParam[] signed = Sign(method_name, parameters);

            StringBuilder builder = new StringBuilder(Url);

            for (int i = 0; i < signed.Length; i++)
            {
                if (i > 0)
                    builder.Append("&");

                builder.Append(signed[i].ToString());
            }

            return builder.ToString();
        }
        
        public static XmlSerializer GetSerializer(Type t)
        {
            int type_hash = t.GetHashCode();

            if (!serializer_dict.ContainsKey(type_hash))
                serializer_dict.Add(type_hash, new XmlSerializer(t));

            return serializer_dict[type_hash];
        }

        public DiscuzParam[] Sign(string method_name, DiscuzParam[] parameters)
        {
            List<DiscuzParam> list = new List<DiscuzParam>(parameters);
            list.Add(DiscuzParam.Create("method", method_name));
            list.Add(DiscuzParam.Create("api_key", api_key));
            list.Sort();

            StringBuilder values = new StringBuilder();

            foreach (DiscuzParam param in list)
            {
                if (!string.IsNullOrEmpty(param.Value))
                    values.Append(param.ToString());
            }

            values.Append(secret);

            byte[] md5_result = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(values.ToString()));

            StringBuilder sig_builder = new StringBuilder();

            foreach (byte b in md5_result)
                sig_builder.Append(b.ToString("x2"));

            list.Add(DiscuzParam.Create("sig", sig_builder.ToString()));

            return list.ToArray();
        }

        public static int GetIntFromString(string input)
        {
            try
            {
                return int.Parse(input);
            }
            catch
            {
                return 0;
            }
        }

        public static bool GetBoolFromString(string input)
        {
            try
            {
                return bool.Parse(input);
            }
            catch
            {
                return false;
            }

        }

    }
}
