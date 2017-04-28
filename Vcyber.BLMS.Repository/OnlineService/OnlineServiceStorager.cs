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
    public class OnlineServiceStorager : IOnlineServiceStorager
    {
        public int AddOnLineService(OnlineServiceRecord service)
        {
            //INSERT INTO BLMS].dbo].OnlineServiceRecord] (Id, UserName, Phone, ServiceProviderId, ServiceProviderName, QuestionType, DataSource, IsDel, CreateTime, Remark, Public1, Public2) VALUES ('2', N'aaa', '12345678910', NULL, NULL, NULL, NULL, '0', '2017-01-17 13:41:29.723', NULL, NULL, NULL);
            string sql = "Insert into OnlineServiceRecord(UserName, Phone, ServiceProviderId, ServiceProviderName, QuestionType, DataSource, IsDel, CreateTime, Remark) values(@UserName, @Phone, @ServiceProviderId, @ServiceProviderName, @QuestionType, @DataSource, @IsDel, @CreateTime, @Remark);SELECT @@identity";
            return DbHelp.Execute(sql, new { @UserName=service.UserName, @Phone=service.Phone, @ServiceProviderId=service.ServiceProviderId, @ServiceProviderName=service.ServiceProviderName, @QuestionType=service.QuestionType, @DataSource=service.DataSource, @IsDel=0, @CreateTime=DateTime.Now, @Remark=service.Remark });
        }
    }
}
