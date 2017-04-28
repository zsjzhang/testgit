using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Application
{
    public interface IMemberNumberApp
    {
        /// <summary>
        /// 获取会员编号
        /// </summary>
        /// <param name="carType"></param>
        /// <returns></returns>
        string GetNumber(string carType);
    }
}
