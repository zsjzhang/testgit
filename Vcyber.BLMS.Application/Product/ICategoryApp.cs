using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    /// <summary>
    /// 商品类型业务
    /// </summary>
    public interface ICategoryApp
    {
        #region ==== 公共方法 ====

        /// <summary>
        /// 添加商品类型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        void Add(Category data, out int id);

        bool Update(Category data);

        bool Delete(int id);

        /// <summary>
        /// 获取商品类型信息列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Category> GetList();


        /// <summary>
        /// 根据商品类型名字来获取类型ID
        /// </summary>
        /// <param name="CategoryName"></param>
        /// <returns></returns>
        int selectIdByName(string CategoryName);

        /// <summary>
        /// 获取用户今年生日是否领取过礼品(前台生日特权在使用)
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        int getBirthdayCar(string userid, int CategoryID);

        /// <summary>
        /// 获取商品类型信息列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Category> GetList(int category);

        #endregion
    }
}
