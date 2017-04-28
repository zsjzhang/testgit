using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Vcyber.BLMS.Common.City
{
    [Serializable]
    [XmlRoot("Areas")]
    public class Areas
    {
        [XmlElement("Area")]
        public List<Area> AreaList { get; set; }
    }

    [DataContract]
    public class Area
    {

        [XmlElement("ID")]
        [DataMember]
        public int ID { get; set; }

        [XmlElement("code")]
        [DataMember]
        public string Code { get; set; }

        [XmlElement("letter")]
        [DataMember]
        public string Letter { get; set; }

        [XmlElement("name")]
        [DataMember]
        public string Name { get; set; }

        [XmlElement("citycode")]
        [DataMember]
        public string Citycode { get; set; }

        [XmlElement("EnName")]
        [DataMember]
        public string EnName { get; set; }

        [XmlElement("ShortName")]
        [DataMember]
        public string ShortName { get; set; }
    }
}
