using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Repository
{
    public class ValidateCodeStorager : IValidateCodeStorager
    {
        /// <summary>
        /// 添加验证码记录
        /// </summary>
        /// <param name="code">数据实体</param>
        /// <param name="id">返回记录ID</param>
        /// <returns>执行结果</returns>
        public bool Add(ValidateCode code)
        {
            const string sql = @"INSERT INTO validatecode(PhoneNumber,Code,DataSource) VALUES(@PhoneNumber,@Code,@DataSource)";

            return DbHelp.Execute(sql, code) > 0;
        }

        /// <summary>
        /// 查询用户的验证码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>验证码</returns>
        public ValidateCode Select(string phoneNumber)
        {
            //const string sql = @"SELECT * FROM validatecode WHERE PhoneNumber = @PhoneNumber AND validatecode.CreateTime>=@Time ORDER BY validatecode.CreateTime DESC ";
            const string sql = @"SELECT top 1 * FROM validatecode WHERE PhoneNumber = @PhoneNumber ORDER BY id DESC ";

            //return DbHelp.QueryOne<ValidateCode>(sql, new { PhoneNumber = phoneNumber,Time=DateTime.Now });
            var obj = DbHelp.QueryOne<ValidateCode>(sql, new { PhoneNumber = phoneNumber });
            return obj;
        }

        public int GetValidateCountByPhoneNumber(string phoneNumber)
        {
            string sql = "SELECT COUNT(1) FROM validatecode WHERE PhoneNumber = @PhoneNumber";
            return DbHelp.ExecuteScalar<int>(sql, new { PhoneNumber = phoneNumber });
        }

        /// <summary>
        /// 获取手机号当天的验证次数
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public int ValidateCount(string phoneNumber)
        {
            string sql = "SELECT COUNT(1) FROM validatecode WHERE PhoneNumber = @PhoneNumber AND cast(CreateTime as date)  = cast(getdate() as date)";
            return DbHelp.ExecuteScalar<int>(sql, new { PhoneNumber = phoneNumber });
        }

        /// <summary>
        /// 获取1分钟内是否发送过短信
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public int InHourValidateCount(string phoneNumber)
        {
            string sql = "select COUNT(*) from ValidateCode where PhoneNumber = @PhoneNumber and GETDATE() < Dateadd(HOUR,1,CreateTime)";
            return DbHelp.ExecuteScalar<int>(sql, new { PhoneNumber = phoneNumber });
        }

        public int InMinuteValidateCount(string phoneNumber)
        {
            string sql = "select COUNT(*) from ValidateCode where PhoneNumber = @PhoneNumber and GETDATE() < Dateadd(MINUTE,1,CreateTime)";
            return DbHelp.ExecuteScalar<int>(sql, new { PhoneNumber = phoneNumber });
        }
    }
}
