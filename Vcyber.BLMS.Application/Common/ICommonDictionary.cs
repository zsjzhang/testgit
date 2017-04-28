using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Application
{
    public interface ICommonDictionary
    {
        IEnumerable<Entity.Dictionary> GetDictionary(string type, string code = null);
    }
}
