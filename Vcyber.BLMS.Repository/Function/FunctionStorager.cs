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
    /// 功能操作
    /// </summary>
    public class FunctionStorager : IFunctionStorager
    {
        #region ==== 构造函数 ====

        public FunctionStorager()
        { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 根据功能Id，获取信息
        /// </summary>
        /// <param name="funcId"></param>
        /// <returns></returns>
        public Function SelectOne(int funcId)
        {
            string sql = "SELECT Id,Name,ParentId,FType,Describe,RouteSelection FROM [function] where Id=@Id And IsDel=@IsDel order by [function].Rate";
            return DbHelp.QueryOne<Function>(sql, new { @Id = funcId, @IsDel = (int)EDataStatus.NoDelete });
        }


        /// <summary>
        /// 根据Url，获取单个功能信息
        /// </summary>
        /// <param name="funcUrl">功能Url</param>
        /// <returns></returns>
        public Function SelectOne(string funcUrl)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT [function].*");
            sql.Append(" FROM [function] INNER JOIN functionurl ON [function].Id=functionurl.FunId");
            sql.Append(" WHERE LOWER(functionurl.Url)=@Url AND [function].IsDel=0 AND functionurl.IsDel=0 order by [function].Rate");
            return DbHelp.QueryOne<Function>(sql.ToString(), new { @Url = funcUrl });
        }

        /// <summary>
        /// 根据action. controller，获取单个功能信息
        /// </summary>
        /// <param name="action">功能Url</param>
        /// <param name="controller">功能Url</param>/// <returns></returns>
        public Function SelectOne(string action, string controller)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT [function].*");
            sql.Append(" FROM [function] INNER JOIN functionurl ON [function].Id=functionurl.FunId");
            sql.Append(" WHERE LOWER(functionurl.action)=@Action And (functionurl.controller)=@Controller AND [function].IsDel=0 AND functionurl.IsDel=0 order by [function].Rate");
            return DbHelp.QueryOne<Function>(sql.ToString(), new { @Action = action, @Controller = controller });
        }

        /// <summary>
        /// 添加功能信息
        /// </summary>
        /// <param name="data">功能信息</param>
        /// <param name="type">功能类型</param>
        /// <param name="keyId">输出Id</param>
        /// <returns>true：成功</returns>
        public bool Add(Function data, EFunctionType type, out int keyId)
        {
            string sql = "INSERT INTO [function](Name,ParentId,FType,Describe,IsDel,RouteSelection) VALUES(@Name,@ParentId,@FType,@Describe,@IsDel,@RouteSelection);SELECT @@identity;";
            keyId = DbHelp.ExecuteScalar<int>(sql, new { @Name = data.Name, @ParentId = data.ParentId, @FType = type, @Describe = data.Describe, @IsDel = data.IsDel, @RouteSelection=data.RouteSelection});
            return keyId > 0;
        }

        /// <summary>
        /// 删除某个功能
        /// </summary>
        /// <param name="keyId">功能KeyId</param>
        /// <returns>true:成功</returns>
        public bool Delete(int keyId)
        {
            string sql = "UPDATE [function] SET IsDel=@IsDel WHERE Id=@Id";
            return DbHelp.Execute(sql, new { @IsDel = (int)EDataStatus.Delete, @Id = keyId }) > 0;
        }

        /// <summary>
        /// 获取顶级模块
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Function> RootFunc()
        {
            string sql = "SELECT Id,Name,ParentId,FType,Describe,RouteSelection FROM [function] WHERE FType=@FType AND ISNULL(ParentId,0)=0 AND IsDel=@IsDel";
            return DbHelp.Query<Function>(sql, new { @FType = EFunctionType.Fun, @IsDel = (int)EDataStatus.NoDelete });
        }

        /// <summary>
        /// 获取用户顶级功能列表
        /// </summary>
        /// <param name="manageId"></param>
        /// <returns></returns>
        public IEnumerable<Function> RootFunc(string manageId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT tempTable.* FROM(");
            sql.Append("  SELECT [function].*,functionurl.[Action] as [Action], functionurl.Controller as Controller");
            sql.Append("  FROM users INNER JOIN userroles ON users.Id=userroles.UserId INNER JOIN roles ON ");
            sql.Append("  roles.Id=userroles.RoleId INNER JOIN rolefunctions ON roles.Id=rolefunctions.RoleId INNER JOIN [function]");
            sql.Append("  ON rolefunctions.FunId=[function].Id  left JOIN functionurl ON [function].Id=functionurl.FunId");
            sql.Append("  WHERE roles.IsDelete=0 AND [function].IsDel=0 AND ISNULL(functionurl.IsDel,0)=0 AND ISNULL([function].ParentId,0)=0 AND users.Id=@managerId) as tempTable ");

            return DbHelp.Query<Function>(sql.ToString(), new { @managerId = manageId });
        }

        public IEnumerable<Function> AllFunc(string manageId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT distinct tempTable.* FROM(");
            sql.Append("  SELECT [function].*,functionurl.[Action] as [Action], functionurl.Controller as Controller");
            sql.Append("  FROM users INNER JOIN userroles ON users.Id=userroles.UserId INNER JOIN roles ON ");
            sql.Append("  roles.Id=userroles.RoleId INNER JOIN rolefunctions ON roles.Id=rolefunctions.RoleId INNER JOIN [function]");
            sql.Append("  ON rolefunctions.FunId=[function].Id  inner JOIN functionurl ON [function].Id=functionurl.FunId");
            sql.Append("  WHERE roles.IsDelete=0 AND [function].IsDel=0 AND ISNULL(functionurl.IsDel,0)=0 AND users.Id=@managerId) as tempTable  order by routeselection ");

            return DbHelp.Query<Function>(sql.ToString(), new { @managerId = manageId });
        }
        /// <summary>
        /// 获取用户孩子功能列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="managerId"></param>
        /// <returns></returns>
        public IEnumerable<Function> ChildFunc(int parentId, string managerId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT tempTable.* FROM(");
            sql.Append("   SELECT [function].*,functionurl.[Action] as [Action], functionurl.Controller as Controller");
            sql.Append("  FROM users INNER JOIN userroles ON users.Id=userroles.UserId INNER JOIN roles ON ");
            sql.Append("  roles.Id=userroles.RoleId INNER JOIN rolefunctions ON roles.Id=rolefunctions.RoleId INNER JOIN [function]");
            sql.Append("  ON rolefunctions.FunId=[function].Id  INNER JOIN functionurl ON [function].Id=functionurl.FunId");
            sql.Append("  WHERE roles.IsDelete=0 AND [function].IsDel=0 AND functionurl.IsDel=0 AND [function].ParentId=@ParentId AND users.Id=@usersId) as tempTable ");

            return DbHelp.Query<Function>(sql.ToString(), new { @ParentId = parentId, @usersId = managerId });
        }

        /// <summary>
        /// 获取孩子功能
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns></returns>
        public IEnumerable<Function> ChildFunc(int parentId)
        {
            // string sql = "SELECT Id,Name,ParentId,FType,Describe FROM function WHERE ParentId=@ParentId AND IsDel=@IsDel";
            string sql =
                "SELECT f.Id,f.Name,f.ParentId,FType,f.Describe,fu.[Action],fu.Controller ,f.Rate, f.RouteSelection FROM [function]  f left join functionurl fu on f.Id = fu.funid WHERE f.ParentId=@ParentId AND f.IsDel=@IsDel";

            return DbHelp.Query<Function>(sql, new { @ParentId = parentId, @IsDel = (int)EDataStatus.NoDelete });
        }

        public Function ParnetFun(int childId)
        {
            string sql = "SELECT distinct(ff.Id) as funId,ff.*,fu.[Action],fu.Controller,f.RouteSelection FROM [function] f inner join [function] ff on ff.id= f.ParentId inner join functionurl fu on fu.FunId = ff.Id WHERE f.Id=@childId AND f.IsDel=@IsDel";
            return DbHelp.QueryOne<Function>(sql, new { @childId = childId, @IsDel = EDataStatus.NoDelete });
        }

        public IEnumerable<Function> AllParnetFun()
        {
            string sql = "SELECT distinct(ff.Id) as funId,ff.*,fu.[Action],fu.Controller,f.RouteSelection,f.id as childid FROM [function] f inner join [function] ff on ff.id= f.ParentId inner join functionurl fu on fu.FunId = ff.Id WHERE f.IsDel=@IsDel order by ff.RouteSelection ";
            return DbHelp.Query<Function>(sql, new { @IsDel = EDataStatus.NoDelete });
        }

        /// <summary>
        /// 修改功能信息
        /// </summary>
        /// <param name="keyId">功能KeyId</param>
        /// <param name="name">功能名称</param>
        /// <param name="describe">描述</param>
        /// <param name="routeSelection"></param>
        /// <returns>true:成功</returns>
        public bool Update(int keyId, string name, string describe, string routeSelection)
        {
            string sql = "UPDATE [function] SET Name=@Name,Describe=@Describe,RouteSelection=@RouteSelection WHERE Id=@Id";
            return DbHelp.Execute(sql, new { @Name = name, @Describe = describe, @Id = keyId, @RouteSelection = routeSelection }) > 0;
        }

        /// <summary>
        /// 验证某个模块中是否存在某个功能名称
        /// </summary>
        /// <param name="name">功能名称</param>
        /// <param name="parentId">功能父Id</param>
        /// <returns></returns>
        public bool IsFunName(string name, int parentId)
        {
            name = name.Replace(" ", "");
            string sql = "SELECT COUNT(*) FROM [function] WHERE NAME=@Name AND ParentId=@ParentId AND IsDel=@IsDel";
            return DbHelp.ExecuteScalar<int>(sql, new { @Name = name, @ParentId = parentId, @IsDel = (int)EDataStatus.NoDelete }) > 0;
        }

        #region role and function method

        public IEnumerable<Function> GetRoleFuncs(string roleId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT f.Id,f.Name,f.ParentId,f.FType,f.Describe ");
            sql.Append("FROM roles r INNER JOIN rolefunctions rr ON r.Id=rr.RoleId INNER JOIN [function] f ON rr.FunId=f.Id ");
            sql.Append("WHERE r.IsDelete=0 AND f.IsDel=0 AND r.Id=@RoleId");

            return DbHelp.Query<Function>(sql.ToString(), new { @RoleId = roleId });
        }
        public IEnumerable<AuthorityDescriptor> GeFuncsByRoleName(List<string> roleNames)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT DISTINCT url.[Action], url.Controller ");
            sql.Append("FROM roles r INNER JOIN rolefunctions rr ON r.Id=rr.RoleId INNER JOIN [function] f ON rr.FunId=f.Id ");
            sql.Append("INNER JOIN functionurl url ON f.Id=url.FunId ");
            sql.AppendFormat("WHERE r.IsDelete=0 AND f.IsDel=0 AND url.IsDel=0 AND r.Name in('{0}')", string.Join("','", roleNames));

            return DbHelp.Query<AuthorityDescriptor>(sql.ToString());
        }
        public bool AddFunc(int funId, string roleId)
        {
            string sql = "INSERT INTO rolefunctions(FunId,RoleId) VALUES(@FunId,@RoleId)";
           return DbHelp.Execute(sql, new { @FunId = funId, @RoleId = roleId })>0;
        }

        public void DeleteFunc(string roleId)
        {
            string sql = "delete from rolefunctions where RoleId=@RoleId";
            DbHelp.Execute(sql, new {@RoleId = roleId});
        }
        public bool DelRoleFunc(int functionId)
        {
            string sql = "delete from rolefunctions where FunId=@FunId";
            return DbHelp.Execute(sql, new { @FunId = functionId }) > 0;
        }

        #endregion

        #endregion


        public IEnumerable<Function> GetTopMenus(List<string> roles)
        {
            if (roles == null || roles.Count <= 0)
                return new List<Function>();
            var sql = new StringBuilder("select distinct fun.id, fun.name,fun.routeselection,furl.[action], furl.controller, fun.rate from roles r "+
                " inner join rolefunctions rf on r.id=rf.roleid inner join [function] fun on fun.id=rf.funid "+
                " inner join functionurl furl on fun.id=furl.funid where fun.parentid=0 and r.name in ('{0}') order by fun.rate asc ");
            string whereExp = string.Empty;
            roles.ForEach(e => 
            {
                whereExp += e + ",";
            });
            whereExp = whereExp.Substring(0, whereExp.Length - 1);

            return DbHelp.Query<Function>(string.Format(sql.ToString(), whereExp));
        }
    }
}
