using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Vcyber.BLMS.ManageWeb.Helper
{
    public class XmlHelper
    {
        /// <summary>
        /// Deserialize xml file to one object
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Deserialize(Stream str , Type type)
        {
            XmlSerializer xmlSer = new XmlSerializer(type);

            object result;
            try
            {
                result = xmlSer.Deserialize(str);
            }
            catch (Exception innerException)
            {
                throw new ApplicationException(string.Format("Couldn't parse XML: '{0}'", innerException));
            }

            str.Close();

            return result;
        }

    }
}