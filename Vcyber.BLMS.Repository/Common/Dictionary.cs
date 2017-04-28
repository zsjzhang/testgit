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
    public class Dictionary : IDictionary
    {
        public IEnumerable<BLMS.Entity.Dictionary> Select(string type)
        {
            return DbHelp.Query<BLMS.Entity.Dictionary>("select * from dictionary where Type=@Type", new { Type = type });
        }

        public IEnumerable<BLMS.Entity.Dictionary> Select(string type, string code)
        {
            return DbHelp.Query<BLMS.Entity.Dictionary>("select * from dictionary where Type=@Type and Code=@Code ", new { Type = type, Code = code });
        }
    }
}
