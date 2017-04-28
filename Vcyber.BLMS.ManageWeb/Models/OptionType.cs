using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class OptionType
    {
       
            public string id { get; set; }

            public string value { get; set; }
            public OptionType()
            {
            }
            public OptionType(string id, string value)
            {
                this.id = id;
                this.value = value;
            }
        
    }
}