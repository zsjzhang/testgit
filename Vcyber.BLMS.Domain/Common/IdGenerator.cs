using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Domain.Common
{
    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.IRepository;
    public class IdGenerator
    {

        /// <summary>
        /// 生成所需的id序列
        /// </summary>
        /// <param name="type">id类型</param>
        /// <returns>序列号</returns>
        public static string GetId(SequenceCategory type)
        {
            int minValue, maxValue;
            _DbSession.IdGeneratorStorager.GetId(type.ToString().ToLower(), 1, out minValue, out maxValue);
            switch (type)
            {
                case SequenceCategory.DC:
                case SequenceCategory.GH:
                case SequenceCategory.HH:
                case SequenceCategory.MJ:
                case SequenceCategory.SJ:
                case SequenceCategory.WB:
                case SequenceCategory.SN:
                case SequenceCategory.JF:                
                case SequenceCategory.XF:
                    {
                        return type + DateTime.Today.ToString("yyyyMMdd") + minValue.ToString("0000");
                    }
                case SequenceCategory.BH:
                    {
                        return type + DateTime.Today.ToString("yyMMdd") + minValue.ToString("000000");
                    }
                default:
                    return minValue.ToString();
            }

        }
    }
}
