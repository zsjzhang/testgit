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
    /// 用户审核操作
    /// </summary>
    public class UserVerifyStorager : IUserVerifyStorager
    {
        #region ==== 构造函数 ====

        public UserVerifyStorager() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加验证信息
        /// </summary>
        /// <param name="data"></param>
        public void Add(UserVerify data)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" INSERT INTO userverify( UserId, Number, RealName, State, CreateTime,Img,ValidTime,VerifyType,UpdateTime)");
            sql.Append(" VALUES(@UserId,@Number,@RealName,@State,@CreateTime,@Img,@ValidTime,@VerifyType,@UpdateTime);");
            DbHelp.Execute(sql.ToString(), data);
        }

        /// <summary>
        /// 修改审核状态
        /// </summary>
        /// <param name="id">审核信息Id</param>
        /// <param name="sate"></param>
        /// <returns></returns>
        public bool UpdateState(int id,EVerifyState sate)
        {
            string sql = "UPDATE userverify SET State=@State,UPDATE=@Update WHERE userverify.Id=@Id";
            return DbHelp.Execute(sql, new { State = (int)sate, Update = DateTime.Now, Id = id }) > 0;
        }

        /// <summary>
        /// 修改审核信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(UserVerify data)
        {
            string sql = "UPDATE userverify SET  Number=@Number,RealName=@RealName,State=@State,Img=@Img,ValidTime=@ValidTime,VerifyType=@VerifyType,UpdateTime=@UpdateTime WHERE userverify.Id=@Id";
            return DbHelp.Execute(sql, data) > 0;
        }

        /// <summary>
        /// 获取用户审核信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public UserVerify SelectOne(int userId)
        {
            string sql = "SELECT * FROM userverify WHERE userverify.UserId=@UserId ORDER BY userverify.UpdateTime DESC";
            return DbHelp.QueryOne<UserVerify>(sql, new { UserId=userId });
        }

        #endregion
    }
}
