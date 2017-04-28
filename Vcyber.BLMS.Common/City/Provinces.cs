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
    [XmlRoot("Provinces")]
    public class ProvincesList
    {
        [XmlElement("Province")]
        public List<Provinces> ProvinceList { get; set; }
    }

    [DataContract]
    public class Provinces
    {
        [XmlElement("ID")]
        [DataMember]
        public int ID;

        [XmlElement("code")]
        [DataMember]
        public string Code;

        [XmlElement("letter")]
        [DataMember]
        public string Letter { get; set; }

        [XmlElement("name")]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<City> Cities { get; set; }

        [XmlElement("EnName")]
        [DataMember]
        public string EnName { get; set; }

        [XmlElement("ShortName")]
        [DataMember]
        public string ShortName { get; set; }
    }
}
