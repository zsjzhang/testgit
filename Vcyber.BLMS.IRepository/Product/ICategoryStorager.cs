using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    /// <summary>
    /// 商品类型操作
    /// </summary>
    public interface ICategoryStorager
    {
        #region ==== 公共方法 ====

        /// <summary>
        /// 添加商品类型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id">输出商品类型id</param>
        void Add(Category data, out int id);

        /// <summary>
        /// 修改商品类型信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Category data);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);
        /// <summary>
        /// 获取的商品类型 为 生日特权
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Category SelectBirthdayOne(int id);
        /// <summary>
        /// 获取商品类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Category SelectOne(int id);

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <returns></returns>
        IEnumerable<Category> SelectList();

        /// <summary>
        /// 获取商品类型信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        IEnumerable<Category> SelectList(int categoryId);

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
        /// 是否存在相同的类型名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true:存在</returns>
        bool IsName(string name);

        /// <summary>
        /// 是否存在相同的类型名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true:存在</returns>
        bool IsName(string name, int id);

        /// <summary>
        /// 商品类型下是否存在商品
        /// </summary>
        /// <param name="id">类型ID</param>
        /// <returns></returns>
        bool IsProduct(int id);

        #endregion
    }
}
