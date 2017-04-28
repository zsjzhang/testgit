using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface INewsStorager
    {
        IEnumerable<News> Select(string title, int pageIndex, int pageSize, out int totalCount);

        IEnumerable<News> Select(string title, int? isDisplay, int? approveStatus, string dealer, int pageIndex, int pageSize,
            out int totalCount);

        IEnumerable<News> Select1(string title, int? isDisplay, int? approveStatus, string dealer, int pageIndex, int pageSize,
            out int totalCount,string start,string end);

        IEnumerable<News> SelectHotNews(string title, int pageIndex, int pageSize, out int totalCount);
        News GetNewsById(int id);
        int AddNews(News news);
        bool UpdateNews(News news);
        bool DeleteNews(int id, string name);
        bool ApprovedNews(int id, string userId, int status);
        bool UpdateIsDisplay(int id, int status, string operatorName);
        bool UpdateAllDisplay(int id, int priority, int dispaly, int isHot, string operatorName);
        IEnumerable<News> GetNews(int status, int pageIndex, int pageSize, out int totalCount);
    }
}
