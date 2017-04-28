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
    public class ServiceCardBatchConsumeStorager : IServiceCardBatchConsumeStorager
    {
        public IEnumerable<ServiceCardBatchConsume> SelectServiceCardBatchConsumeList()
        {
            string sql = "SELECT * FROM ServiceCardBatchConsume";

            return DbHelp.Query<ServiceCardBatchConsume>(sql);
        }

        public IEnumerable<ServiceCardBatchConsume> SelectServiceCardBatchConsumeList(string serviceBatchId)
        {
            string sql = "SELECT * FROM ServiceCardBatchConsume WHERE ServiceBatchId = @ServiceBatchId";

            return DbHelp.Query<ServiceCardBatchConsume>(sql, new { @ServiceBatchId = serviceBatchId });
        }

        public bool AddServiceCardBatchConsume(ServiceCardBatchConsume item)
        {
            string sql = "INSERT INTO ServiceCardBatchConsume(ServiceBatchId,ConsumeScopeId) VALUES(@ServiceBatchId,@ConsumeScopeId)";

            return DbHelp.Execute(sql, item) > 0;
        }

        public bool DeleteServiceCardBatchConsume(string id)
        {
            string sql = "Delete FROM ServiceCardBatchConsume WHERE Id = @Id";

            return DbHelp.Execute(sql, new { @Id = id }) > 0;
        }

        public bool DeleteServiceCardBatchConsumeByBatchId(string batchId)
        {
            string sql = "Delete FROM ServiceCardBatchConsume WHERE ServiceBatchId = @ServiceBatchId";

            return DbHelp.Execute(sql, new { @ServiceBatchId = batchId }) > 0;
        }
    }
}
