using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common.City
{
    [DataContract]
    public class Addresses
    {
        [DataMember]
        public Provinces Province { get; set; }

        [DataMember]
        public City City { get; set; }

        [DataMember]
        public Area Area { get; set; }
    }

    public enum AddressType
    {
        Province = 0,
        City = 1,
        Area = 2,

    }

}
