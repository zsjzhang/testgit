using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class OptionApp : IOptionApp
    {
        List<Entity.Option> IOptionApp.GetAllOptions()
        {
            return _DbSession.OptionStorager.GetAllOptions().ToList();
        }

        List<Entity.Option> IOptionApp.GetOptionByQId(int qid)
        {
            return _DbSession.OptionStorager.GetOptionsByQId(qid).ToList();
        }
        public List<Option> GetOptionsByQIdAndOType(int qid, int oType)
        {
            return _DbSession.OptionStorager.GetOptionsByQIdAndOType(qid, oType).ToList();
        }
        public int Create(Option entity)
        {
            return _DbSession.OptionStorager.Create(entity);
        }

        public bool Edit(Option entity)
        {
            return _DbSession.OptionStorager.Edit(entity);
        }

        public bool Delete(int Id, int parentId, int sort)
        {
            bool result = false;
            if (_DbSession.OptionStorager.Delete(Id))
            {
                result = _DbSession.OptionStorager.DelBatchSort(sort, parentId);
            }
            return result;
        }

        public bool EditContent(string content, int id)
        {
            return _DbSession.OptionStorager.EditContent(content, id);
        }

        public bool EditSort(int sort, int id, int parentId, int ySort)
        {
            return _DbSession.OptionStorager.EditSort(sort, id, parentId, ySort);
        }

        public int MaxOption(int parentId)
        {
            return _DbSession.OptionStorager.MaxOption(parentId);
        }

        public bool DeleteMaxOption(int parentId)
        {
            return _DbSession.OptionStorager.DeleteMaxOption(parentId);
        }

        public bool EditType(int type, int id)
        {
            return _DbSession.OptionStorager.EditType(type, id);
        }

        public bool EditValueType(int vType, int id)
        {
            return _DbSession.OptionStorager.EditValueType(vType, id);
        }
    }
}
