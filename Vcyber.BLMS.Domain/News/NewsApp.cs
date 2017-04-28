using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
   public class NewsApp:INewsApp
    {
       /// <summary>
       /// 分页获取所有新闻
       /// </summary>
       /// <param name="title"></param>
       /// <param name="pageIndex"></param>
       /// <param name="pageSize"></param>
       /// <param name="totalCount"></param>
       /// <returns></returns>
       public IEnumerable<News> Select(string title, int pageIndex, int pageSize, out int totalCount)
       {
           return _DbSession.NewsStorager.Select(title, pageIndex, pageSize, out totalCount);
       }

       public IEnumerable<News> Select(string title, int? isDisplay, int? approveStatus, string dealer, int pageIndex, int pageSize,
           out int totalCount)
       {
           return _DbSession.NewsStorager.Select(title, isDisplay, approveStatus, dealer,pageIndex, pageSize, out totalCount);
       
       }
       public IEnumerable<News> Select1(string title, int? isDisplay, int? approveStatus, string dealer, int pageIndex, int pageSize,
         out int totalCount,string start,string end)
       {
           return _DbSession.NewsStorager.Select1(title, isDisplay, approveStatus, dealer, pageIndex, pageSize, out totalCount,start,end);

       }

       /// <summary>
       /// 分页获取所有热点新闻
       /// </summary>
       /// <param name="title"></param>
       /// <param name="pageIndex"></param>
       /// <param name="pageSize"></param>
       /// <param name="totalCount"></param>
       /// <returns></returns>
       public IEnumerable<News> SelectHotNews(string title, int pageIndex, int pageSize, out int totalCount)
       {
           return _DbSession.NewsStorager.SelectHotNews(title, pageIndex, pageSize, out totalCount);
       }

       public News GetNewsById(int id)
       {
           return _DbSession.NewsStorager.GetNewsById(id);
       }
       public int AddNews(News news)
       {
           return _DbSession.NewsStorager.AddNews(news);
       }
       public bool UpdateNews(News news)
       {
           return _DbSession.NewsStorager.UpdateNews(news);
       }
       public bool DeleteNews(int id,string name)
       {
           return _DbSession.NewsStorager.DeleteNews(id,name);
       }
     
       public bool ApprovedNews(int id, string userId, string userName, int status, string memo)
       {
           //using (TransactionScope scope = new TransactionScope())
           //{
           var approve = new ApproveRecord();
           approve.ItemId = id;
           approve.IsApproval = status;
           approve.ItemType = ((int)EApproveType.News).ToString();
           approve.ApprovalMemo = memo;
           approve.OperatorId = userId;
           approve.OperatorName = userName;
           _AppContext.ApproveRecordApp.UpdateStatus(approve);
           return _DbSession.NewsStorager.ApprovedNews(id, userId, status);
           //}
       }

       public bool UpdateIsDisplay(int id, int status, string operatorName)
       {
           return _DbSession.NewsStorager.UpdateIsDisplay(id, status, operatorName);
       }
       public bool UpdateAllDisplay(int id, int priority, int dispaly, int isHot, string operatorName)
       {
           return _DbSession.NewsStorager.UpdateAllDisplay(id, priority,dispaly, isHot, operatorName);
       }
       public IEnumerable<News> GetNews(int status, int pageIndex, int pageSize, out int totalCount)
       {
           return _DbSession.NewsStorager.GetNews(status, pageIndex, pageSize, out totalCount);
      
       }
    }
}
