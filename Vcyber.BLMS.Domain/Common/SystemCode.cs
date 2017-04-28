using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Domain
{
    public static class SystemCode
    {

        static SystemCode()
        {
            SHCode = "4aea47a24ac3b34d014acca5d2090000";
            MZDSCCode = "402894894a9f6463014a9f657a950000";
            CLBCode = "402894894ab39d0e014ab3adc23b0001";
            CYTCode = "4aea47a24ac3b34d014accb2dec70007";
        }

        /// <summary>
        /// 售后系统
        /// </summary>
        public static  string SHCode { get; private set; }

        /// <summary>
        /// 马自达商城
        /// </summary>
        public static string MZDSCCode { get; private set; }

        /// <summary>
        /// 车联邦
        /// </summary>
        public static string CLBCode { get; private set; }

        /// <summary>
        /// 车音通
        /// </summary>
        public static string CYTCode { get; private set; }
 

    }
}
