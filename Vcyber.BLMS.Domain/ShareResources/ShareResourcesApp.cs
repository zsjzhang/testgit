using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class ShareResourcesApp : IShareResourcesApp
    {
        public int AddShareRes(Entity.ShareResources shareRes)
        {
            return _DbSession.ShareResourcesStorager.AddShareRes(shareRes);
        }

        public int UpdateShareRes(Entity.ShareResources shareRes)
        {
            return _DbSession.ShareResourcesStorager.UpdateShareRes(shareRes);
        }

        public int DelShareResByID(int id)
        {
            return _DbSession.ShareResourcesStorager.DelShareResByID(id);
        }

        public Entity.ShareResources GetShareResById(int id)
        {
            return _DbSession.ShareResourcesStorager.GetShareResById(id);
        }

        public Entity.ShareResources GetNewShareRes()
        {
            return _DbSession.ShareResourcesStorager.GetNewShareRes();
        }

        public IEnumerable<Entity.ShareResources> GetShareRes(object queryObj, int fileType, string category, int pageIndex, int pageSize, out int totalCount)
        {
            return _DbSession.ShareResourcesStorager.GetShareRes(queryObj, fileType, category, pageIndex, pageSize, out totalCount);
        }

        public IEnumerable<Entity.ShareResources> GetShareRes(object queryObj)
        {
            return _DbSession.ShareResourcesStorager.GetShareRes(queryObj);
        }
    }
}
