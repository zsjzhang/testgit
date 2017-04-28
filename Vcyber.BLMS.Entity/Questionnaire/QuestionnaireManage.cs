using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class QuestionnaireManage
    {
        public QuestionnaireManage() { }

        public int Id { get; set; }

        public string UserId { get; set; }

        public bool IsFirst { get; set; }

        public int State { get; set; }

        public int ParentId { get; set; }
    }
}
