using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IAnswerApp
    {
        bool AddAnswerRang(List<Answer> entities);

        bool IsAnswer(string memberId, int pid);
    }
}
