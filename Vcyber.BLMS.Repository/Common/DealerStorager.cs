using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Generated;
using Vcyber.BLMS.IRepository.Common;
using Vcyber.BLMS.Repository.Entity.Generated;
using Vcyber.BLMS.Common;
using System;

namespace Vcyber.BLMS.Repository.Common
{


    public class DealerStorager : IDealerStorager
    {
        #region ==== 原始逻辑 ====

        public IEnumerable<string> GetProvinceList()
        {
            return PocoHelper.CurrentDb().Query<string>("select distinct Province from dbo.CS_CarDealerShip where Province!='' ");
        }

        public IEnumerable<CSCarDealerShip> GetDealerShipList(string province, string city, int Istestserver, int IsDingChe, int IsWeibao)
        {
            Sql sql = new Sql("where Isdel !=1 ");
            if (!string.IsNullOrEmpty(province)) sql.Append(" and Province=@0", province);
            if (!string.IsNullOrEmpty(city)) sql.Append(" and City=@0", city);
            if (Istestserver == 1) sql.Append(" and Istestserver=1");
            if (IsDingChe == 1) sql.Append(" and IsDingChe=1");
            if (IsWeibao == 1) sql.Append(" and IsWeibao=1");
            return PocoHelper.CurrentDb().Fetch<CSCarDealerShip>(sql);
        }

        public IEnumerable<string> GetCityListByProvince(string province)
        {
            return PocoHelper.CurrentDb().Query<string>("select distinct City from dbo.CS_CarDealerShip where Province=@0", province);
        }

        public IEnumerable<CSCarDealerShip> GetDealerList(string province, string city)
        {
            return PocoHelper.CurrentDb().Fetch<CSCarDealerShip>("where Province=@0 and City=@1 and IsDel!=1", province, city);
        }


        public PetaPoco.Page<CSCarDealerShip> GetDealerListByKeyWord(string keyWord, long page, long itemsPerPage)
        {
            return PocoHelper.CurrentDb().Page<CSCarDealerShip>(page, itemsPerPage, "where name like @0 and IsDel!=1", string.Format("%{0}%", keyWord));
        }

        public CSCarDealerShip GetDealerDetailsById(string id)
        {
            return PocoHelper.CurrentDb().FirstOrDefault<CSCarDealerShip>(" where id=@0", id);
        }

        public CSCarDealerShip GetDealerByDealerId(string dealerId)
        {
            return PocoHelper.CurrentDb().FirstOrDefault<CSCarDealerShip>(" where DealerId=@0", dealerId);
        }

        public IEnumerable<CSCarDealerShip> GetAll()
        {
            return PocoHelper.CurrentDb().Query<CSCarDealerShip>("select * from CS_CarDealerShip");
        }

        public IEnumerable<CSCarDealerShip> GetPaidDealerShip(string province, string city)
        {
            Sql sql = new Sql("where AlipayAccount is not null");
            if (!string.IsNullOrEmpty(province)) sql.Append(" and Province=@0", province);
            if (!string.IsNullOrEmpty(city)) sql.Append(" and City=@0", city);
            return PocoHelper.CurrentDb().Query<CSCarDealerShip>(sql);
        }

        #endregion


        #region ==== 新逻辑 ====

        /// <summary>
        /// 查询经销商信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<CSCarDealerShip> findList(Condition condition, PageData pageData, out int totalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select COUNT(1) from CS_CarDealerShip where {0}", condition.ToWhere());
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());

            sql.Clear();

            sql.AppendFormat(" select top {0} * from CS_CarDealerShip where {1}", pageData.Size, condition.ToWhere());
            sql.Append(" and Id not in(");
            sql.AppendFormat(" select top {0} id from CS_CarDealerShip where {1})", pageData.Index, condition.ToWhere());
            return DbHelp.Query<CSCarDealerShip>(sql.ToString());
        }

        /// <summary>
        /// 获取经销商信息
        /// </summary>
        /// <param name="dealerId">经销商标示</param>
        /// <returns></returns>
        public CSCarDealerShip findOne(string dealerId)
        {
            string sql = "select * from CS_CarDealerShip where DealerId=@DealerId";
            return DbHelp.QueryOne<CSCarDealerShip>(sql, new { DealerId = dealerId });
        }

        /// <summary>
        /// 修改经销商信息
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool update(string dealerId, CSCarDealerShip data)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" update CS_CarDealerShip set Position=@Position,Province=@Province,City=@City,");
            sql.Append(" Name=@Name,Address=@address,WorkName=@WorkName,BU=@BU,Phone=@Phone,TBAccount=@TBAccount,");
            sql.Append(" AlipayAccount=@AlipayAccount,Email=@Email,WebSite=@WebSite,Abbreviation=@Abbreviation,");
            sql.Append(" IsDel=@IsDel,Istestserver=@Istestserver,IsDingChe=@IsDingChe,IsWeibao=@IsWeibao, ");
            sql.Append(" AfterSalesPhone=@AfterSalesPhone,Area=@Area,Region=@Region,FreeRoadRescuePhone=@FreeRoadRescuePhone ");
            sql.Append(" where DealerId=@DealerId");
            data.DealerId = dealerId;
            return DbHelp.Execute(sql.ToString(), data) > 0;
        }

        /// <summary>
        /// 添加经销商信息
        /// </summary>
        /// <param name="data"></param>
        public void add(CSCarDealerShip data)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into CS_CarDealerShip(Id,Position,Province,City,Name,Address,WorkName,BU,Phone,TBAccount,");
            sql.Append(" AlipayAccount,Email,WebSite,Abbreviation,AfterSalesPhone,DealerId,Area,Region,FreeRoadRescuePhone,IsDel,Istestserver,IsDingChe,IsWeibao)");
            sql.Append(" values(@Id,@Position,@Province,@City,@Name,@Address,@WorkName,@BU,@Phone,@TBAccount,");
            sql.Append(" @AlipayAccount,@Email,@WebSite,@Abbreviation,@AfterSalesPhone,@DealerId,@Area,@Region,@FreeRoadRescuePhone,@IsDel,@Istestserver,@IsDingChe,@IsWeibao )");
            DbHelp.Execute(sql.ToString(), data);
        }

        /// <summary>
        /// 验证数据是否重复（全称、简称等信息）
        /// </summary>
        /// <param name="dealerId">店代码</param>
        /// <param name="value">验证值</param>
        /// <returns>true:重复</returns>
        public bool validateData(string dealerId, string value)
        {
            string sql = "select COUNT(1) from CS_CarDealerShip where DealerId!=@DealerId and (Name=@Value or Abbreviation=@Value)";
            return DbHelp.ExecuteScalar<int>(sql, new { DealerId = dealerId, Value = value }) > 0;
        }

        #endregion

        /// <summary>
        /// 根据身份证号码获取客户信息
        /// </summary>
        /// <param name="id">客户身份证号码</param>
        /// <returns></returns>
        public IFCustomerInfo GetCustomerInfoByIdentityNumber(string id)
        {
            string sql = "SELECT CustId , CustName , CustMobile , IdentityNumber , Gender , Email , Address , City , Member , Agree , AccntType FROM IF_Customer  where IdentityNumber=@IdentityNumber";
            return DbHelp.QueryOne<IFCustomerInfo>(sql, new { IdentityNumber = id });
        }

        /// <summary>
        /// 车主故事--我要发帖
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="userid"></param>
        /// <param name="Content"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public int AddDealerStory(string Title, string userid, string Content, string img)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into DealerStory(UserId,Title,Contents,[Image],IsDel,CreateTime,UpdateTime)");
            sql.Append("values(@UserId,@Title,@Contents,@Image,@IsDel,@CreateTime,@UpdateTime)");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), new
            {
                @UserId = userid,
                @Title = Title,
                @Contents = Content,
                @Image = img,
                @CreateTime = System.DateTime.Now,
                @UpdateTime = System.DateTime.Now,
                @IsDel = "0"
            }
                );

        }

        /// <summary>
        /// 车主故事--删除帖子
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DelDealerStory(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update DealerStory set IsDel = 1 where id = @id");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), new
            {
                @id = id
            }
            );
        }

        /// <summary>
        /// 车主故事--修改帖子
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Contents">帖子内容</param>
        /// <returns></returns>
        public int UpdateDealerStory(int id, string Contents, string Title, string img)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update DealerStory set Contents = @Contents,Title = @Title,Image = @Image,UpdateTime = @UpdateTime where id = @id");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), new
            {
                @id = id,
                @Contents = Contents,
                @Title = Title,
                @Image = img,
                @UpdateTime = DateTime.Now
            }
            );
        }

        /// <summary>
        /// 车主故事--获取用户所有帖子
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public IEnumerable<DealerStory> SelactDealerStory(string Userid)
        {
            string sql = @" select * from DealerStory WHERE userid =@Userid and IsDel = 0 order by UpdateTime desc";
            return DbHelp.Query<DealerStory>(sql, new
            {
                @Userid = Userid
            });
        }

        /// <summary>
        /// 车主故事--根据帖子ID查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DealerStory SelactDealerStoryForId(string id)
        {
            string sql = @" select * from DealerStory WHERE id =@id and IsDel = 0";
            return DbHelp.QueryOne<DealerStory>(sql, new
            {
                @id = id
            });
        }
    }
}
