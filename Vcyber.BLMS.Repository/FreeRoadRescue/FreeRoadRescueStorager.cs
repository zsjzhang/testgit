using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Repository
{
    public class FreeRoadRescueStorager : IFreeRoadRescueStorager
    {

        public bool AddFreeRoadRescue(BLMS.Entity.CS_FreeRoadRescue entity)
        {
            string sql = @"INSERT INTO CS_FreeRoadRescue(PhoneNumber,Address,Position,IsFinish,CreateTime) 
                            VALUES(@PhoneNumber,@Address,@Position,'N',GETDATE());";

            return DbHelp.Execute(sql, entity) > 0;
        }

        public bool FinishFreeRoadRescue(int id)
        {
            string sql = "UPDATE CS_FreeRoadRescue SET IsFinish = 'Y',UpdateTime = GETDATE() WHERE Id = @Id";

            return DbHelp.Execute(sql, new { @Id = id }) > 0;
        }

        public IEnumerable<CS_FreeRoadRescue> SelectFreeRoadRescueList(string start, string end, string phoneNumber, string status)
        {
            var sql = @"SELECT Id,PhoneNumber,Address,Position,IsFinish,CreateTime,UpdateTime 
                            FROM CS_FreeRoadRescue WHERE 1=1 ";

            if (!string.IsNullOrEmpty(start))
            {
                sql += "AND CreateTime >= @Start ";
            }

            if (!string.IsNullOrEmpty(end))
            {
                sql += "AND CreateTIme <= @End ";
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                sql += "AND PhoneNumber = @PhoneNumber ";
            }

            if (!string.IsNullOrEmpty(status))
            {
                sql += "AND IsFinish = @Status";
            }

            sql += " ORDER BY CREATETIME DESC";

            return DbHelp.Query<CS_FreeRoadRescue>(sql, new { @Start = start, @End = end, @PhoneNumber = phoneNumber, @Status = status });
        }
    }
}
