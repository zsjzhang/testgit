using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 创建会员编号业务
    /// </summary>
    public class MemberNumberApp : IMemberNumberApp
    {
        /// <summary>
        /// 获取会员编号
        /// </summary>
        /// <param name="carType">车系类型</param>
        /// <returns></returns>
        public string GetNumber(string carType)
        {
            string id = _DbSession.MemberNumberRepository.Insert().ToString();
            DateTime currentTime = DateTime.Now;
            string year = currentTime.Year.ToString().Substring(2);
            string month = currentTime.Month >= 10 ? currentTime.Month.ToString() : "0" + currentTime.Month;

            StringBuilder number = new StringBuilder();

            for (int i = 1; i <= 6 - id.Length; i++)
            {
                number.Append("0");
            }

            number.Append(id);
            return string.Format("{0}{1}{2}{3}", year, month, carType, number.ToString());
        }
    }
}
