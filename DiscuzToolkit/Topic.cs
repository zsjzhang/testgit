using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Discuz.Toolkit
{
    public class Topic
    {
        [JsonPropertyAttribute("uid")]
        [XmlElement("uid", IsNullable = false)]
        public int UId;

        [JsonPropertyAttribute("title")]
        [XmlElement("title", IsNullable = false)]
        public string Title;

        [JsonPropertyAttribute("fid")]
        [XmlElement("fid", IsNullable = false)]
        public int Fid;

        [JsonPropertyAttribute("message")]
        [XmlElement("message", IsNullable = false)]
        public string Message;

        [JsonPropertyAttribute("icon_id")]
        [XmlElement("icon_id", IsNullable = true)]
        public int Iconid;

        [JsonPropertyAttribute("tags")]
        [XmlElement("tags", IsNullable = true)]
        public string Tags;
    }
}


