using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IActivitiesStorager
    {
        IEnumerable<Activities> Select(int? approveStatus, int? displayStatus, int? isHot, string dealer, int pageIndex, int pageSize, out int totalCount);
        IEnumerable<Activities> SelectNotStart(int? approveStatus, int? displayStatus, int? isHot, string dealer, int pageIndex, int pageSize, out int totalCount);
        IEnumerable<Activities> SelectFinished(int? approveStatus, int? displayStatus, int? isHot, string dealer, int pageIndex, int pageSize, out int totalCount);
        IEnumerable<Activities> SelectInProgress(int? approveStatus, int? displayStatus, int? isHot, string dealer, int pageIndex, int pageSize, out int totalCount);
        Activities GetActivitiesById(int id);
        int AddActivities(Activities activities);
        bool UpdateActivities(Activities activities);
        bool DeleteActivities(int id,string name);
        bool ApprovedActivities(int id,string userId, int status);
        IEnumerable<Activities> GetActivities(int status, int pageIndex, int pageSize, out int totalCount);
        bool UpdateIsDisplay(int id, int status, string operatorName);
        bool UpdateAllDisplay(int id, int priority,int isDisplay,int isHot, string operatorName);
        /// <summary>
        /// 验证卡券的有效性
        /// </summary>
        /// <param name="cardType"></param>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        AfterSaleServiceWXModel CheckCusomCard(string cardType, string cardNo);

        /// <summary>
        /// 核销卡券
        /// </summary>
        /// <param name="cardType"></param>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        AfterSaleServiceWXModel CancelCusomCard(long cardId);
    }
}
