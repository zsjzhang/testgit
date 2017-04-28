using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.IRepository.Common
{
    public interface IIdGeneratorStorager
    {
        void GetId(string category, int count, out int minValue, out int maxValue);
    }
}
