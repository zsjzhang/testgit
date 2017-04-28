using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace Vcyber.BLMS.Application.Common
{
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.Entity;

    public interface IDealer
    {
        IEnumerable<CSCarDealerShip> GetAll();

        IEnumerable<string> GetProvinceList();
        IEnumerable<CSCarDealerShip> GetDealerShipList(string province, string city, int Istestserver, int IsDingChe, int IsWeibao);

        IEnumerable<string> GetCityListByProvince(string province);

        IEnumerable<CSCarDealerShip> GetDealerList(string province, string city);

        /// <summary>
        /// 获取符合条件的经销商
        /// </summary>
        /// <param name="province1">第一个地点所在省份</param>
        /// <param name="city1">第一个地点所在城市</param>
        /// <param name="long1">第一个地点经度</param>
        /// <param name="lat1">第一个地点纬度</param>
        /// <param name="province2">第二个地点所在省份</param>
        /// <param name="city2">第二个地点所在城市</param>
        /// <param name="long2">第二个地点经</param>
        /// <param name="lat2">第二个地点纬度</param>
        /// <param name="distance">搜索半径(米)</param>
        /// <returns>经销商列表</returns>
        [Obsolete]
        IEnumerable<CSCarDealerShip> GetDealerListByDistance(string province1, string city1, double long1, double lat1, string province2, string city2, double long2, double lat2, int distance);

        /// <summary>
        /// 根据两个地点获取符合条件的经销商
        /// </summary>
        /// <param name="province">经销商所在省份</param>
        /// <param name="city">经销商所在城市</param>
        /// <param name="long1">第一个地点经度</param>
        /// <param name="lat1">第一个地点纬度</param>
        /// <param name="long2">第二个地点经</param>
        /// <param name="lat2">第二个地点纬度</param>
        /// <param name="distance">搜索半径(米)</param>
        /// <returns>经销商列表</returns>
        IEnumerable<CSCarDealerShip> GetDealerListByDistance(string province, string city, double long1, double lat1, double long2, double lat2, int distance);


        /// <summary>
        /// 根据一个地点获取符合条件的经销商
        /// </summary>
        /// <param name="province">经销商所在省份</param>
        /// <param name="city">经销商所在城市</param>
        /// <param name="long">地点经度</param>
        /// <param name="lat">地点纬度</param>
        /// <param name="distance">搜索半径(米)</param>
        /// <returns>经销商列表</returns>
        IEnumerable<CSCarDealerShip> GetDealerListByDistance(string province, string city, double @long, double lat, int distance);

        Page<CSCarDealerShip> GetDealerListByKeyWord(string keyWord, long page, long itemsPerPage);

        CSCarDealerShip GetDealerDetailsById(string id);

        CSCarDealerShip GetDealerByDealerId(string dealerId);

        IEnumerable<CSCarDealerShip> GetPaidDealerShip(string province, string city);

        #region ==== 新逻辑 ====

        /// <summary>
        /// 查询经销商信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<CSCarDealerShip> findList(Condition condition, PageData pageData, out int totalCount);

        /// <summary>
        /// 获取经销商信息
        /// </summary>
        /// <param name="dealerId">经销商标示</param>
        /// <returns></returns>
        CSCarDealerShip findOne(string dealerId);

        /// <summary>
        /// 修改经销商信息
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        bool update(string dealerId, CSCarDealerShip data);

        /// <summary>
        /// 添加经销商信息
        /// </summary>
        /// <param name="data"></param>
        void add(CSCarDealerShip data);

        /// <summary>
        /// 验证数据是否重复（全称、简称等信息）
        /// </summary>
        /// <param name="dealerId">店代码</param>
        /// <param name="value">验证值</param>
        /// <returns>true:重复</returns>
        bool validateData(string dealerId, string value);

        #endregion



        #region  IF_Customer客户信息
        /// <summary>
        /// 根据身份证号码获取客户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFCustomerInfo GetCustomerInfoByIdentityNumber(string id);
        #endregion 

        /// <summary>
        /// 车主故事--我要发帖
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="userid"></param>
        /// <param name="Content"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        bool AddDealerStory(string Title, string userid, string Content, string img);

        /// <summary>
        /// 车主故事--删除帖子
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DelDealerStory(int id);

        /// <summary>
        /// 车主故事--修改帖子
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Contents">帖子内容</param>
        /// <returns></returns>
        bool UpdateDealerStory(int id,string Contents, string Title, string img);

        /// <summary>
        /// 车主故事--获取用户全部帖子
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        IEnumerable<DealerStory> SelactDealerStory(string Userid);

        /// <summary>
        /// 车主故事--根据ID查找相应的帖子
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DealerStory SelactDealerStoryForId(string id);
    }
}
