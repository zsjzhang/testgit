using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Domain.Common
{
    using Vcyber.BLMS.Application.Common;
    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.IRepository;
    using PetaPoco;
    using Vcyber.BLMS.Entity;

    public class DealerApp : IDealer
    {
        public IEnumerable<string> GetProvinceList()
        {
            return _DbSession.DealerStorager.GetProvinceList();
        }


        public IEnumerable<CSCarDealerShip> GetDealerShipList(string province, string city, int Istestserver, int IsDingChe, int IsWeibao)
        {
            return _DbSession.DealerStorager.GetDealerShipList( province, city,Istestserver, IsDingChe, IsWeibao);
        }

        public IEnumerable<string> GetCityListByProvince(string province)
        {
            return _DbSession.DealerStorager.GetCityListByProvince(province);
        }

        public IEnumerable<CSCarDealerShip> GetDealerList(string province, string city)
        {
            return _DbSession.DealerStorager.GetDealerList(province, city);
        }

        public IEnumerable<CSCarDealerShip> GetDealerListByDistance(string province1, string city1, double long1, double lat1, string province2, string city2, double long2, double lat2, int distance)
        {
            IEnumerable<CSCarDealerShip> dealers1 = this.GetDealerList(province1, city1);
            IEnumerable<CSCarDealerShip> dealers11 = dealers1.Where(dealer => DistanceCalculator.GetDistance(
                long1, lat1, double.Parse(dealer.Position.Split(',')[0]), double.Parse(dealer.Position.Split(',')[1])) < distance);
            IEnumerable<CSCarDealerShip> dealers2 = this.GetDealerList(province2, city2);

            IEnumerable<CSCarDealerShip> dealers21 = dealers2.Where(dealer => DistanceCalculator.GetDistance(
                long1, lat1, double.Parse(dealer.Position.Split(',')[0]), double.Parse(dealer.Position.Split(',')[1])) < distance);
            return from dealer1 in dealers11
                   from dealer2 in dealers21
                   where dealer1.Id == dealer2.Id
                   select dealer2;
        }

        public IEnumerable<CSCarDealerShip> GetDealerListByDistance(
            string province,
            string city,
            double long1,
            double lat1,
            double long2,
            double lat2,
            int distance)
        {
            IEnumerable<CSCarDealerShip> dealers1 = this.GetDealerList(province, city);
            IEnumerable<CSCarDealerShip> dealers11 = dealers1.Where(dealer => dealer.Position != null && (DistanceCalculator.GetDistance(
                long1, lat1, double.Parse(dealer.Position.Split(',')[1]),
                double.Parse(dealer.Position.Split(',')[0])) < distance));
            IEnumerable<CSCarDealerShip> dealers2 = this.GetDealerList(province, city);

            IEnumerable<CSCarDealerShip> dealers21 = dealers2.Where(dealer => dealer.Position != null && (DistanceCalculator.GetDistance(
                long1, lat1, double.Parse(dealer.Position.Split(',')[1]), double.Parse(dealer.Position.Split(',')[0])) < distance));
            return from dealer1 in dealers11
                   from dealer2 in dealers21
                   where dealer1.Id == dealer2.Id
                   select dealer2;
        }

        public IEnumerable<CSCarDealerShip> GetDealerListByDistance(string province, string city, double @long, double lat, int distance)
        {
            IEnumerable<CSCarDealerShip> dealers = this.GetDealerList(province, city).Where(dealer => dealer.Position != null && DistanceCalculator.GetDistance(
                @long, lat, double.Parse(dealer.Position.Split(',')[1]), double.Parse(dealer.Position.Split(',')[0])) < distance);

            return dealers;
        }

        public Page<CSCarDealerShip> GetDealerListByKeyWord(string keyWord, long page, long itemsPerPage)
        {
            return _DbSession.DealerStorager.GetDealerListByKeyWord(keyWord, page, itemsPerPage);
        }

        public CSCarDealerShip GetDealerDetailsById(string id)
        {
            return _DbSession.DealerStorager.GetDealerDetailsById(id);
        }

        public CSCarDealerShip GetDealerByDealerId(string dealerId)
        {
            return _DbSession.DealerStorager.GetDealerByDealerId(dealerId);
        }

        public IEnumerable<CSCarDealerShip> GetPaidDealerShip(string province, string city)
        {
            return _DbSession.DealerStorager.GetPaidDealerShip(province, city);
        }

        public IEnumerable<CSCarDealerShip> GetAll()
        {
            return _DbSession.DealerStorager.GetAll();
        }

        #region ==== 新逻辑 ====

        /// <summary>
        /// 查询经销商信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<CSCarDealerShip> findList(Condition condition, PageData pageData, out int totalCount)
        {
            return _DbSession.DealerStorager.findList(condition, pageData, out totalCount);
        }

        /// <summary>
        /// 获取经销商信息
        /// </summary>
        /// <param name="dealerId">经销商标示</param>
        /// <returns></returns>
        public CSCarDealerShip findOne(string dealerId)
        {
            return _DbSession.DealerStorager.findOne(dealerId);
        }

        /// <summary>
        /// 修改经销商信息
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool update(string dealerId, CSCarDealerShip data)
        {
            return _DbSession.DealerStorager.update(dealerId, data);
        }

        /// <summary>
        /// 添加经销商信息
        /// </summary>
        /// <param name="data"></param>
        public void add(CSCarDealerShip data)
        {
            _DbSession.DealerStorager.add(data);
        }

        /// <summary>
        /// 验证数据是否重复（全称、简称等信息）
        /// </summary>
        /// <param name="dealerId">店代码</param>
        /// <param name="value">验证值</param>
        /// <returns>true:重复</returns>
        public bool validateData(string dealerId, string value)
        {
            return _DbSession.DealerStorager.validateData(dealerId, value);
        }

        #endregion


        public IFCustomerInfo GetCustomerInfoByIdentityNumber(string id)
        {
            return _DbSession.DealerStorager.GetCustomerInfoByIdentityNumber(id);
        }

        /// <summary>
        /// 车主故事--我要发帖
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="userid"></param>
        /// <param name="Content"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public bool AddDealerStory(string Title, string userid, string Content, string img)
        {
            int offsetnum = _DbSession.DealerStorager.AddDealerStory(Title, userid, Content, img);
            if (offsetnum > 0) return true;
            else return false;
        }

        /// <summary>
        /// 车主故事--删除帖子
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DelDealerStory(int id)
        {
            int offsetnum = _DbSession.DealerStorager.DelDealerStory(id);
            if (offsetnum > 0) return true;
            else return false;
        }

        /// <summary>
        /// 车主故事--修改帖子
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Contents">帖子内容</param>
        /// <returns></returns>
        public bool UpdateDealerStory(int id, string Contents, string Title, string img)
        {
            int offsetnum = _DbSession.DealerStorager.UpdateDealerStory(id, Contents, Title,img);
            if (offsetnum > 0) return true;
            else return false;
        }

        /// <summary>
        /// 车主故事--获取用户所有帖子
        /// </summary>
        /// <param name="Userid">用户ID</param>
        /// <returns></returns>
        public IEnumerable<DealerStory> SelactDealerStory(string Userid)
        {
            return _DbSession.DealerStorager.SelactDealerStory(Userid);
        }

        /// <summary>
        /// 车主故事--根据ID查找相应的帖子
        /// </summary>
        /// <param name="Userid">用户ID</param>
        /// <returns></returns>
        public DealerStory SelactDealerStoryForId(string id)
        {
            return _DbSession.DealerStorager.SelactDealerStoryForId(id);
        }
    }
}
