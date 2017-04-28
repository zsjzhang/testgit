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
    /// <summary>
    /// 商品类型业务
    /// </summary>
    public class CategoryApp : ICategoryApp
    {
        #region ==== 构造函数 ====

        public CategoryApp() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加商品类型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        public void Add(Category data,out int id)
        {
            if (_DbSession.CategoryStorager.IsName(data.Name))
            {
                throw new RepeatException("商品类型名称重复。");
            }

            _DbSession.CategoryStorager.Add(data, out id);
        }

        public bool Update(Category data)
        {
            if (_DbSession.CategoryStorager.IsName(data.Name,data.ID))
            {
                throw new RepeatException("商品类型名称重复。");
            }

            return _DbSession.CategoryStorager.Update(data);
        }

        public bool Delete(int id)
        {
            if (_DbSession.CategoryStorager.IsProduct(id))
            {
                throw new RepeatException("商品类型中下存在商品！");
            }

            var childs = _DbSession.CategoryStorager.SelectList(id);

            if (childs!=null&&childs.Count()>0)
            {
                foreach (var item in childs)
                {
                    if (_DbSession.CategoryStorager.IsProduct(item.ID))
                    {
                        throw new RepeatException("商品类型中下存在商品！");
                    }
                }
            }

            return _DbSession.CategoryStorager.Delete(id);
        }

        /// <summary>
        /// 获取商品类型信息列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Category> GetList()
        {
            var categorys= _DbSession.CategoryStorager.SelectList();

            if (categorys!=null)
            {
                foreach (var item in categorys)
                {
                    var childs=_DbSession.CategoryStorager.SelectList(item.ID);
                    item.Childs = childs != null ? childs.ToList<Category>() : new List<Category>(1);
                    item.IsChild = childs != null && childs.Count() > 0 ? true : false;
                }
            }

            return categorys;
        }

        /// <summary>
        /// 根据商品类型名字来获取类型ID
        /// </summary>
        /// <param name="CategoryName"></param>
        /// <returns></returns>
        public int selectIdByName(string CategoryName) {
            return _DbSession.CategoryStorager.selectIdByName(CategoryName);
        }

        /// <summary>
        /// 获取用户今年生日是否领取过礼品(前台生日特权在使用)
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        public int getBirthdayCar(string userid, int CategoryID) {
            return _DbSession.CategoryStorager.getBirthdayCar(userid, CategoryID);
        }

        /// <summary>
        /// 获取商品类型信息列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Category> GetList(int category)
        {
            return _DbSession.CategoryStorager.SelectList(category);
        }

        #endregion
    }
}
