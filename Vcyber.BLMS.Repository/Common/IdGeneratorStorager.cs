using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Repository.Common
{
    using System.Data;
    using System.Data.SqlClient;

    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.IRepository.Common;

    public class IdGeneratorStorager:IIdGeneratorStorager
    {        
        public void GetId(string category, int count, out int minValue, out int maxValue)
        {
            using (var db = BMDb.GetInstance())
            {
                IDbDataParameter pCategory = new SqlParameter("@category", SqlDbType.Char);
                IDbDataParameter pCount = new SqlParameter("@count", SqlDbType.Int);
                IDbDataParameter pMinValue = new SqlParameter("@minValue", SqlDbType.Int);
                IDbDataParameter pMaxValue = new SqlParameter("@maxValue", SqlDbType.Int);
                pMinValue.Direction = ParameterDirection.Output;
                pMinValue.Value = 0;
                pMaxValue.Direction = ParameterDirection.Output;
                pMaxValue.Value = 0;

                db.Execute(";EXEC dbo.GenerateSeqNo @0, @1, @2 output, @3 output", category, count, pMinValue, pMaxValue);

                minValue = int.Parse(pMinValue.Value.ToString());
                maxValue = int.Parse(pMaxValue.Value.ToString());
            }
        }
    }
}
