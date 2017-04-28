using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 商品类型操作
    /// </summary>
    public class CategoryStorager : ICategoryStorager
    {
        #region ==== 构造函数 ====

        public CategoryStorager() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加商品类型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id">输出商品类型id</param>
        public void Add(Category data, out int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("  insert into category(ParentID,Name,Datastate,Createtime,Updatetime,CardType) ");
            sql.Append("  values(@ParentID,@Name,@Datastate,@Createtime,@Updatetime,@CardType);;select @@IDENTITY ");
            id = DbHelp.ExecuteScalar<int>(sql.ToString(), data);
        }

        /// <summary>
        /// 修改商品类型信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Category data)
        {
            string sql = "update category set ParentID=@ParentId,Name=@Name,CardType=@CardType where ID=@ID";
            return DbHelp.Execute(sql, data) > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            string sql = "update category set Datastate=@Datastate where ID=@ID";
            return DbHelp.Execute(sql, new { Datastate = EDataStatus.Delete.ToInt32(), ID = id }) > 0;
        }

        /// <summary>
        /// 编辑商品时获取的商品类型为特定的类型：已删除的类型
        /// 数据库中有的 但在前台不显示的
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Category SelectBirthdayOne(int id)
        {
            string sql = "select * from category where Datastate=@Datastate and id=@id";
            return DbHelp.QueryOne<Category>(sql, new { Datastate = 1, id = id });

        }

        /// <summary>
        /// 获取商品类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Category SelectOne(int id)
        {
            string sql = "select * from category where id=@id";
            return DbHelp.QueryOne<Category>(sql, new { id = id });
            
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Category> SelectList()
        {
            string sql = "select * from category where Datastate=@Datastate and isnull(ParentID,0)=0 ";
            return DbHelp.Query<Category>(sql, new { Datastate = EDataStatus.NoDelete.ToInt32() });
        }
        public IEnumerable<Category> SelectList_ForBehind()
        {
            string sql = @"select * from category where Datastate=@Datastate and isnull(ParentID,0)=0 
              union select * from category where Datastate=1 and isnull(ParentID,0)=0 and Name='生日特权'";

            return DbHelp.Query<Category>(sql, new { Datastate = EDataStatus.NoDelete.ToInt32() });
        }
        /// <summary>
        /// 获取商品类型信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IEnumerable<Category> SelectList(int categoryId)
        {
            string sql = "select * from category where category.ParentID=@ParentID and category.Datastate=@Datastate";
            return DbHelp.Query<Category>(sql, new { ParentID = categoryId, Datastate = EDataStatus.NoDelete.ToInt32() });
        }

        /// <summary>
        /// 根据商品类型名字来获取类型ID
        /// </summary>
        /// <param name="CategoryName"></param>
        /// <returns></returns>
        public int selectIdByName(string CategoryName)
        {
            string sql = "select ID from Category where LOWER(Name)=LOWER(@CategoryName)";
            return DbHelp.ExecuteScalar<int>(sql, new { CategoryName = CategoryName });
        }
        /// <summary>
        /// 获取用户今年生日是否领取过礼品(前台生日特权在使用)
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
       public int getBirthdayCar(string userid, int CategoryID) {
           string sql = "select COUNT(1) from orderproduct op,orders o,productcategory pc where op.OrderID=o.OrderID and op.ProductID=pc.ProductID and pc.CategoryID=@CategoryID and o.UserID=@userid and YEAR(o.Createtime)=YEAR(GETDATE())";

           return DbHelp.ExecuteScalar<int>(sql, new{ CategoryID = CategoryID, userid=userid });
        }
        /// <summary>
        /// 是否存在相同的类型名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true:存在</returns>
        public bool IsName(string name)
        {
            string sql = "select COUNT(1) from category where LOWER(Name)=LOWER(@Name) and Datastate=@Datastate";
            return DbHelp.ExecuteScalar<int>(sql, new { Name = name, Datastate = EDataStatus.NoDelete.ToInt32() }) > 0;
        }

        /// <summary>
        /// 是否存在相同的类型名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true:存在</returns>
        public bool IsName(string name, int id)
        {
            string sql = "select COUNT(1) from category where LOWER(Name)=LOWER(@Name) and ID!=@ID and Datastate=@Datastate";
            return DbHelp.ExecuteScalar<int>(sql, new { Name = name, ID = id, Datastate = EDataStatus.NoDelete.ToInt32() }) > 0;
        }

        /// <summary>
        /// 商品类型下是否存在商品
        /// </summary>
        /// <param name="id">类型ID</param>
        /// <returns></returns>
        public bool IsProduct(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("  select COUNT(1) from category join productcategory on category.ID=productcategory.CategoryID");
            sql.Append("  join product on product.Id=productcategory.ProductID");
            sql.Append("  where product.Datastate=0 and category.ID=@ID");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), new { ID = id }) > 0;
        }

        #endregion
    }
}
