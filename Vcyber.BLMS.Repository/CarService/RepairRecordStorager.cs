using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    using PetaPoco;

    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.Repository.Entity.Generated;

    public class RepairRecordStorager : IRepairRecordStorager
    {

        public IEnumerable<RepairRecord> GetRepirRecordList(string identityNumber, string vin, DateTime? repairStartTime, DateTime? repairEndTime, string serviceType, int? start, int? count, string dealerid, out int totalCount)
        {
            var appendStr = new StringBuilder();
            if (!string.IsNullOrEmpty(identityNumber))
            {
                appendStr.Append(" and IdentityNumber= @IdentityNumber ");
            }

            //if (string.IsNullOrEmpty(serviceType))
            //{
            //    appendStr.Append(" and ServiceType in('");
            //    List<string> list = new List<string>();
            //    foreach (var obj in Enum.GetValues(typeof(EDMSServiceType)))
            //    {
            //        list.Add(((EDMSServiceType)obj).DisplayName());
            //    }
            //    string result = string.Join("','", list);
            //    appendStr.Append(result + "')");
            //}

            if (!string.IsNullOrEmpty(serviceType))
            {
                appendStr.Append(" and ServiceType=@ServiceType ");
            }

            if (!string.IsNullOrEmpty(vin))
            {
                appendStr.Append(" and vincode=@vin ");
            }

            if (repairStartTime != null)
            {
                appendStr.Append(" and RepairTime>=@repairStartTime ");
            }

            if (repairEndTime != null)
            {
                appendStr.Append(" and RepairTime<=@repairEndTime ");
            }

            if (!string.IsNullOrEmpty(dealerid))
            {
                appendStr.Append(" and DealerId=@dealerid ");
            }

            var totalSql = "select count(1) from IF_RepairReport where 1=1 {0}";

            var sql = new StringBuilder();
            sql.Append("select * from (select row_number() over (order by id) as row_num,* from IF_RepairReport  where 1=1 {0}) as nt where nt.row_num between @PageIndex and @PageEnd");

            var parameters = new Dictionary<string, object>
            {
                {"@ServiceType", serviceType},
                {"@IdentityNumber", identityNumber},
                {"@vin",vin},
                {"@repairStartTime",repairStartTime},
                {"@repairEndTime",repairEndTime},
                {"@PageIndex", start + 1},
                {"@PageEnd", count +start},
                {"@dealerid",dealerid}
            };
            int.TryParse(DbHelp.ExecuteScalar<int>(string.Format(totalSql.ToString(), appendStr.ToString()), parameters).ToString(), out totalCount);
            return DbHelp.Query<RepairRecord>(string.Format(sql.ToString(), appendStr), parameters);
        }

        public Page<IFRepairReport> QueryServiceOrders(string identityNumber, EDMSServiceType4Q type, long page = 1, long itemsPerPage = 10000)
        {
            Sql sql = new Sql("where IdentityNumber=@0", identityNumber);
            if (type != EDMSServiceType4Q.All) sql.Append(" and ServiceType=@0", type.DisplayName());
            else
            {
                List<string> types = new List<string>();
                foreach (var item in Enum.GetValues(typeof(EDMSServiceType4Q)))
                {
                    types.Add(((EDMSServiceType4Q)item).DisplayName());
                }
                sql.Append(" and ServiceType in (@types)", new { types = types.ToArray() });

            }
            sql.Append(" order by RepairTime desc");
            return PocoHelper.CurrentDb().Page<IFRepairReport>(page, itemsPerPage, sql);

        }
    }
}
