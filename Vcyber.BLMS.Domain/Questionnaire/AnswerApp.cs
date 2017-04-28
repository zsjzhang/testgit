using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class AnswerApp : IAnswerApp
    {
        /// <summary>
        /// 添加答案记录列表
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool AddAnswerRang(List<Entity.Answer> entities)
        {
            int tempCount = _DbSession.AnswerStorager.AddAnswerRang(entities);
            if (tempCount > 0)
                return true;
            else
                return false;
        }


        public bool IsAnswer(string userId, int pid)
        {
            return _DbSession.AnswerStorager.IsAnswer(userId, pid);
        }
    }
}
