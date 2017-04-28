using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IActivitiesApp
    {
        IEnumerable<Activities> Select(string dealer, int? status, int pageIndex, int pageSize, out int totalCount);

        IEnumerable<Activities> SelectHotActivitieses(string dealer, int? status, int pageIndex, int pageSize,
            out int totalCount);

        IEnumerable<Activities> SelectStatus(string dealer, int? status, int? approveStatus, int? display, int? isHot,
            int pageIndex, int pageSize, out int totalCount);

        // IEnumerable<Activities> SelectInProgress(string dealer, int pageIndex, int pageSize, out int totalCount);
        Activities GetActivitiesById(int id);
        int AddActivities(Activities activities);
        bool UpdateActivities(Activities activities);
        bool DeleteActivities(int id, string name);
        bool ApprovedActivities(int id, string userId, string userName, int status, string memo);
        IEnumerable<Activities> GetActivities(int status, int pageIndex, int pageSize, out int totalCount);
        bool UpdateIsDisplay(int id, int status, string operatorName);
        bool UpdateAllDisplay(int id, int priority, int isDisplay, int isHot, string operatorName);
        /// <summary>
        /// 核销卡券
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        ReturnResult ServiceCardConsum(long cardId);
    }
}
