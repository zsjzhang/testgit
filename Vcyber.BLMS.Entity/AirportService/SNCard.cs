using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity
{
    public class SNCard
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 服务编码
        /// </summary>
        public string SNCode
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status
        {
            get;
            set;
        }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 下发时间
        /// </summary>
        public DateTime? SendTime
        {
            get;
            set;
        }

        /// <summary>
        /// 下发时间
        /// </summary>
        public string SendTime_Str
        {
            get 
            { 
            return  string.IsNullOrEmpty(SendTime.ToString())?"":((DateTime)SendTime).ToString("yyyy-MM-dd hh:mm:ss");
            }
           private set{ SendTime_Str=value; }
        }

        /// <summary>
        /// 下发方式
        /// </summary>
        public int SendType
        {
            get;
            set;
        }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? UseTime
        {
            get;
            set;
        }

        public string UseTime_Str
        {
            get
            {
                return string.IsNullOrEmpty(UseTime.ToString()) ? "" : ((DateTime)UseTime).ToString("yyyy-MM-dd hh:mm:ss");
            }
            private set
            {
                SendTime_Str = value;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? ValidateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 用户
        /// </summary>
        public string PhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 接收手机号
        /// </summary>
        public string RecivePhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName
        {
            get
            {
                switch (Status)
                {
                    case 1: return EnumExtension.GetDiscribe<ESNCardStatus>(ESNCardStatus.Created);
                    case 2: return EnumExtension.GetDiscribe<ESNCardStatus>(ESNCardStatus.Send);
                    default: return EnumExtension.GetDiscribe<ESNCardStatus>(ESNCardStatus.Used);
                }
            }
        }

        public string SendTypeName
        {
            get
            {
                switch (SendType)
                {
                    case 1: return EnumExtension.GetDiscribe<ESendType>(ESendType.WebSite);
                    case 2: return EnumExtension.GetDiscribe<ESendType>(ESendType.App);
                    case 3: return EnumExtension.GetDiscribe<ESendType>(ESendType.System);
                    default: return EnumExtension.GetDiscribe<ESendType>(ESendType.Trade);
                }
            }
        }

        /// <summary>
        /// 机场ID
        /// </summary>
        public int AirportId
        {
            get;
            set;
        }

        /// <summary>
        /// 机场名称
        /// </summary>
        public string AirportName
        {
            get;
            set;
        }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IdentityNumber
        {
            get;
            set;
        }
        public string RealName
        {
            get;
            set;
        }
        public string UseAdd
        {
            get;
            set;
        }

        public string IsCallCenter
        {
            get;
            set;
        }

        public string DataSource
        {
            get;
            set;
        }
    }
}
