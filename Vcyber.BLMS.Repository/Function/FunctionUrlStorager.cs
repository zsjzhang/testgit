using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 功能访问Url操作
    /// </summary>
    public class FunctionUrlStorager : IFunctionUrlStorager
    {
        #region ==== 构造函数 ====

        public FunctionUrlStorager()
        { }

        #endregion

        #region ==== 构造方法 ====

        /// <summary>
        /// 添加访问Url信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="keyId">输出Id</param>
        /// <returns>true:成功</returns>
        public bool Add(FunctionUrl data, out int keyId)
        {
            string sql = "INSERT INTO functionurl(FunId,Action,Controller,Describe,IsDel) VALUES(@FunId,@Action,@Controller,@Describe,@IsDel);SELECT @@identity;";
            keyId = DbHelp.ExecuteScalar<int>(sql, new { @FunId = data.FunId, @Action = data.Action,@Controller=data.Controller, @Describe = data.Describe, @IsDel = data.IsDel });
            return keyId > 0;
        }

        /// <summary>
        /// 删除Url
        /// </summary>
        /// <param name="keyId">Url KeyId</param>
        /// <returns></returns>
        public bool Delete(int keyId)
        {
            string sql="UPDATE functionurl SET IsDel=@IsDel WHERE Id=@Id";
            return DbHelp.Execute(sql, new { @IsDel = (int)EDataStatus.Delete, @Id = keyId }) > 0;
        }

        /// <summary>
        /// 修改Url访问
        /// </summary>
        /// <param name="keyId">Url KeyId</param>
        /// <param name="url">访问地址</param>
        /// <param name="controller"></param>
        /// <param name="describe">描述</param>
        /// <param name="action"></param>
        /// <param name="routeSelection"></param>
        /// <returns>ture:成功</returns>
        /// (int keyId, string action, string controller, string describe)
        public bool Update(int keyId, string action, string controller, string describe, string routeSelection)
        {
            string sql = "UPDATE functionurl SET Action=@Action,Controller=@Controller,Describe=@Describe WHERE FunId=@Id";
            return DbHelp.Execute(sql, new { @Action = action, @Controller = controller, @Describe = describe, @Id = keyId }) > 0;
        }

        /// <summary>
        /// 获取某个功能可以访问的Url
        /// </summary>
        /// <param name="funId"></param>
        /// <returns></returns>
        public IEnumerable<FunctionUrl> SelectUrl(int funId)
        {
            string sql = "SELECT * FROM functionurl WHERE FunId=@FunId  AND IsDel=@IsDel";
            return DbHelp.Query<FunctionUrl>(sql, new { @FunId = funId, @IsDel = (int)EDataStatus.NoDelete });
        }

        #endregion
    }
}
