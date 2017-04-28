using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class MaintCarPackage
    {
        public int Id { get; set; }

        public string CarType { get; set; }

        public string PackageType { get; set; }

        public string KM { get; set; }

        public string ItemName { get; set; }
    }
}
