using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IMaintCarOilApp
    {
        IEnumerable<MaintCarOil> GetMaintCarOilList(string carType);
    }
}
