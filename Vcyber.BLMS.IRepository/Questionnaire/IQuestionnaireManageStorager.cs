using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IQuestionnaireManageStorager
    {
        /// <summary>
        /// 新增用户与问卷关系表数据
        /// </summary>
        /// <param name="entity">用户与问卷关系实体</param>
        /// <returns>用户与问卷关系id</returns>
        int Create(QuestionnaireManage entity);
    }
}
