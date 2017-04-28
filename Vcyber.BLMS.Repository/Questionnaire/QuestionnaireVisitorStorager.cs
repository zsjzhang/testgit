using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Repository
{
    public class QuestionnaireVisitorStorager : IQuestionnaireVisitorStorager
    {
        public int AddQuestionnaireVisitor(QuestionnaireVisitor entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO CS_QuestionnaireVisitor(VName,Sex,PhoneNumber,Provency,City,Area,MailAddress,QuestionnaireId,Age,Education,CarModels,CreateTime,IsMember,Email,VSource) VALUES(@VName,@Sex,@PhoneNumber,@Provency,@City,@Area,@MailAddress,@QuestionnaireId,@Age,@Education,@CarModels,@CreateTime,@IsMember,@Email,@VSource);SELECT @@identity");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), entity);
        }
    }
}
