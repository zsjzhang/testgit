using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace Vcyber.BLMS.IRepository.Common
{
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.Entity;

    public interface IDealerStorager
    {
        IEnumerable<string> GetProvinceList();

        IEnumerable<CSCarDealerShip> GetDealerShipList(string province, string city, int Istestserver, int IsDingChe, int IsWeibao);

        IEnumerable<string> GetCityListByProvince(string province);

        IEnumerable<CSCarDealerShip> GetDealerList(string province, string city);

        Page<CSCarDealerShip> GetDealerListByKeyWord(string keyWord, long page, long itemsPerPage);

        CSCarDealerShip GetDealerDetailsById(string id);

        CSCarDealerShip GetDealerByDealerId(string dealerId);

        IEnumerable<CSCarDealerShip> GetAll();

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
        int AddDealerStory(string Title, string userid, string Content, string img);

        /// <summary>
        /// 车主故事--删除帖子
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DelDealerStory(int id);

        /// <summary>
        /// 车主故事--修改帖子
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Contents">帖子内容</param>
        /// <returns></returns>
        int UpdateDealerStory(int id, string Contents, string Title, string img);

        /// <summary>
        /// 车主故事--获取用户全部帖子
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        IEnumerable<DealerStory> SelactDealerStory(string Userid);

        /// <summary>
        /// 车主故事--根据ID查找相应的帖子
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        DealerStory SelactDealerStoryForId(string id);
    }
}
