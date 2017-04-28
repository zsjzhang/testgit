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
    public class FreeRoadRescueApp : IFreeRoadRescueApp
    {
        public bool AddFreeRoadRescue(CS_FreeRoadRescue entity)
        {
            return _DbSession.FreeRoadRescueStorager.AddFreeRoadRescue(entity);
        }

        public bool FinishFreeRoadRescue(int id)
        {
            return _DbSession.FreeRoadRescueStorager.FinishFreeRoadRescue(id);
        }

        public IEnumerable<CS_FreeRoadRescue> SelectFreeRoadRescueList(string start, string end, string phoneNumber, string status)
        {
            return _DbSession.FreeRoadRescueStorager.SelectFreeRoadRescueList(start, end, phoneNumber, status);
        }
    }
}
