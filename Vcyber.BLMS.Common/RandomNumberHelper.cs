using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common
{
    public class RandomNumberHelper
    {
        public static string GetUserCustomCardCode()
        {
            var guid = Guid.NewGuid().ToString();
            var hashCode = guid.GetHashCode();
            var absHashCode = Math.Abs(guid.GetHashCode()).ToString();

            int rdmLength = 12 - absHashCode.Length;
            if (rdmLength > 4)
            {
                guid = Guid.NewGuid().ToString();
                hashCode = guid.GetHashCode();
                absHashCode = Math.Abs(guid.GetHashCode()).ToString();
            }
            if (rdmLength == 0)
            {
                return absHashCode;
            }
            else
            {
                int minRan = 1;
                int maxRan = 10;
                if (rdmLength == 4)
                {
                    minRan = 1000;
                    maxRan = 10000;
                }
                else if (rdmLength == 3)
                {
                    minRan = 100;
                    maxRan = 1000;
                }
                else if (rdmLength == 2)
                {
                    minRan = 10;
                    maxRan = 100;
                }
                Random dom = new Random(hashCode);
                int random = dom.Next(minRan, maxRan);
                return absHashCode + random.ToString();
            }
        }

    }
}
