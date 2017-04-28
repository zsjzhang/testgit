using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity.SelectCondition;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 用户常用地址操作
    /// </summary>
    public class AddressStorager : IAddressStorager
    {
        #region ==== 构造函数 =====

        public AddressStorager() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加地址
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回地址Id</returns>
        public int AddAddress(Address entity)
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
            StringBuilder sql = new StringBuilder();
            sql.Append("  update address set IsDefault=0,Updatetime=@Time where address.UserID=@UserID");

            if (entity.IsDefault == true)
            {
                DbHelp.Execute(sql.ToString(), new { Time = DateTime.Now, UserID = entity.UserID });
            }

            sql.Clear();

            sql.Append("insert into address(UserID ,ReceiveName ,Phone ,Province ,City ,County ,PCC ,ZipCode ,Detail ,IsDefault ,Datastate ,Createtime ,Updatetime)");
            sql.Append("values(@UserID ,@ReceiveName ,@Phone ,@Province ,@City ,@County ,@PCC ,@ZipCode ,@Detail ,@IsDefault ,@Datastate ,@Createtime ,@Updatetime); select @@identity;");
            int id = DbHelp.ExecuteScalar<int>(sql.ToString(), entity);
            //scope.Complete();
            return id;
            //}
        }

        /// <summary>
        /// 地址是否重复
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>true:重复</returns>
        public bool IsAddresss(Address entity)
        {
            var commandText = "select count(1) from address where ReceiveName=@ReceiveName and Phone=@Phone and Province=@Province and City=@City and County=@County and PCC=@PCC and ZipCode=@ZipCode and Detail=@Detail and datastate=@datastate and userid=@userid";
            return DbHelp.ExecuteScalar<int>(commandText, entity) > 0;
        }

        /// <summary>
        /// 修改地址
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateAddress(Address entity)
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
            StringBuilder sql = new StringBuilder();
            sql.Append("  update address set IsDefault=0 where address.UserID=@UserID");

            if (entity.IsDefault == true)
            {
                DbHelp.Execute(sql.ToString(), new { UserID = entity.UserID, });
            }

            sql.Clear();

            sql.Append("update address set UserID=@UserID ,ReceiveName=@ReceiveName ,Phone=@Phone ,Province=@Province ,City=@City ,County=@County ,PCC=@PCC ,ZipCode=@ZipCode ,Detail=@Detail ,Updatetime=@UpdateTime,IsDefault=@IsDefault where ID=@ID and UserID=@UserID");
            bool result = DbHelp.Execute(sql.ToString(), entity) > 0;
            // scope.Complete();
            return result;
            //}
        }

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteAddress(int id)
        {
            return DbHelp.Execute("update address set Datastate=@Datastate,Updatetime=@Time where ID=@ID", new { Datastate = EDataStatus.Delete.ToInt32(), ID = id, Time = DateTime.Now }) > 0;
        }

        /// <summary>
        /// 设置默认地址
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SetDefaultAddress(string userId, int id)
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
            StringBuilder sql = new StringBuilder();
            sql.Append("  update address set IsDefault=0,Updatetime=@Time where address.UserID=@UserID");

            bool result1 = DbHelp.Execute(sql.ToString(), new { Time = DateTime.Now, UserID = userId }) > 0;
            bool result2 = DbHelp.Execute("update address set isdefault=1,updatetime =@Time where userid=@userid and ID=@ID and Datastate=@Datastate", new { userid = userId, ID = id, Datastate = EDataStatus.NoDelete.ToInt32(), Time = DateTime.Now }) > 0;

            if (result1 && result2)
            {
                //scope.Complete();
                return true;
            }

            return false;
            //}
        }

        /// <summary>
        /// 获取用户常用地址列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IEnumerable<Address> SelectList(string userID)
        {
            string sql = "select * from address where UserID=@UserID and address.Datastate=@Datastate";
            return DbHelp.Query<Address>(sql, new { UserID = userID, Datastate = EDataStatus.NoDelete.ToInt32() });
        }

        /// <summary>
        /// 获取地址信息
        /// </summary>
        /// <param name="id">地址Id</param>
        /// <returns></returns>
        public Address SelectOne(int id)
        {
            string sql = "select * from address where ID=@ID and Datastate=@Datastate";
            return DbHelp.QueryOne<Address>(sql, new { ID = id, Datastate =EDataStatus.NoDelete.ToInt32()});
        }

        #endregion
    }
}
