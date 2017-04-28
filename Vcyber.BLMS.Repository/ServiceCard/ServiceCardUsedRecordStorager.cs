using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class ServiceCardUsedRecordStorager : IServiceCardUsedRecordStorager
    {
        public IEnumerable<ServiceCardUsedRecord> SelectServiceCardUsedRecordList()
        {
            string sql = "SELECT * FROM ServiceCardUsedRecord";

            return DbHelp.Query<ServiceCardUsedRecord>(sql);
        }

        public bool AddServiceCardUsedRecord(ServiceCardUsedRecord record)
        {
            string sql = "INSERT INTO ServiceCardUsedRecord(CardNo,UseTime,DealerId,ConsumeId,CreateTime,ConsumeType) VALUES(@CardNo,@UseTime,@DealerId,@ConsumeId,GETDATE(),@ConsumeType)";

            return DbHelp.Execute(sql, new { @CardNo = record.CardNo, @UseTime = record.UseTime, @DealerId = record.DealerId, @ConsumeId = record.ConsumeId }) > 0;
        }
    }
}
