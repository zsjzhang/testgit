using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.Pay.Entity 
{
    public static class SystemCode
    {

        static SystemCode()
        {
            SHCode = "SH001";
            MZDSCCode = "MZDSC001";
            YJCode = "YJ001";
            DJCode = "DJ001";
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
        /// 语驾系统
        /// </summary>
        public static string YJCode { get; private set; }

        /// <summary>
        /// 代驾系统
        /// </summary>
        public static string DJCode { get; private set; }
 

    }
}
