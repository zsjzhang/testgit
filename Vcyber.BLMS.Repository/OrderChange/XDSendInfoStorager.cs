using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class XDSendInfoStorager : IXDSendInfoStorager
    {
        public XDSendInfoStorager() { }
        public bool AddXDSendInfo(XDSendInfo xDSendInfo)
        {
            string sql = "INSERT INTO [XD_SendInfo] ([SendProvince], [SendCity], [SendDistrinct], [SendAddress], [UserId], [UserMobile], [IsValid], [CreaterId], [CreaterName], [CreaterTime], [UpdaterId], [UpdaterName], [UpdaterTime], [UserName],[ActivityId],[SendSource],[LotteryId],[LotteryRecordId]) VALUES (@SendProvince, @SendCity, @SendDistrinct, @SendAddress, @UserId, @UserMobile, @IsValid, @CreaterId, @CreaterName, @CreaterTime, @UpdaterId, @UpdaterName, @UpdaterTime, @UserName, @ActivityId , @SendSource,@LotteryId,@LotteryRecordId);";
            return DbHelp.Execute(sql, xDSendInfo) > 0;
        }
    }
}
