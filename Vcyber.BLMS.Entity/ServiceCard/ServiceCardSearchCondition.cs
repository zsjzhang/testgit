using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class ServiceCardSearchCondition
    {
        public string BatchNo { get; set; }

        public string CardNo { get; set; }

        public int Status { get; set; }

        public string ToWhere()
        {
            var sb = new StringBuilder(" 1 = 1 ");

            if (!string.IsNullOrEmpty(BatchNo))
            {
                sb.Append(" AND BatchNo LIKE '%" + BatchNo + "%'");
            }

            if (!string.IsNullOrEmpty(CardNo))
            {
                sb.Append(" AND CardNo LIKE '%" + CardNo + "%'");
            }

            if (Status > 0)
            {
                sb.Append(" AND Status = " + Status.ToString());
            }

            return sb.ToString();
        }
    }
}
