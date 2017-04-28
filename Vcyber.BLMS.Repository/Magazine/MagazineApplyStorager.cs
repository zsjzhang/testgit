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
    /// 资料申请操作
    /// </summary>
    public class MagazineApplyStorager : IMagazineApplyStorager
    {
        #region ==== 构造函数 ====

        public MagazineApplyStorager() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="data">申请表数据</param>
        public void Add(MagazineApply data, out string id)
        {
            id = Guid.NewGuid().ToString();
            data.Id = id;

            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into MagazineApply(Id,MagazineId,UserId,MagazineTitle,ReceiveName,Phone,ZipCode,ProvinceCode,");
            sql.Append(" CityCode,CountyCode,PCC,Detail,[Status],ExpressNumber,CreateTime,UpdateTime)");
            sql.Append(" values(@Id,@MagazineId,@UserId,@MagazineTitle,@ReceiveName,@Phone,@ZipCode,@ProvinceCode,");
            sql.Append(" @CityCode,@CountyCode,@PCC,@Detail,@Status,@ExpressNumber,@CreateTime,@UpdateTime)");
            DbHelp.Execute(sql.ToString(), data);
        }

        /// <summary>
        /// 修改申请表状态
        /// </summary>
        /// <param name="id">申请表Id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public bool UpdateStatus(string id, EMApplyStatus status)
        {
            string sql = "update MagazineApply set [Status]=@Status,UpdateTime=@UpdateTime where Id=@Id";
            return DbHelp.Execute(sql, new { Status = status.ToInt32(), UpdateTime = DateTime.Now, Id = id }) > 0;
        }

        /// <summary>
        /// 发送资料
        /// </summary>
        /// <param name="id">申请表Id</param>
        /// <param name="expressNumber">快递单号</param>
        /// <returns></returns>
        public bool SendMagazine(string id, string expressNumber)
        {
            string sql = "update MagazineApply set [Status]=@Status,UpdateTime=@UpdateTime,ExpressNumber=@ExpressNumber where Id=@Id";
            return DbHelp.Execute(sql, new { Status = EMApplyStatus.YJYJ.ToInt32(), UpdateTime = DateTime.Now, Id = id, ExpressNumber = expressNumber }) > 0;
        }

        /// <summary>
        /// 获取单个申请表信息
        /// </summary>
        /// <param name="id">申请表Id</param>
        /// <returns></returns>
        public MagazineApply SelectOne(string id)
        {
            string sql = "select * from MagazineApply where Id=@Id";
            return DbHelp.QueryOne<MagazineApply>(sql, new { Id = id });
        }

        /// <summary>
        /// 分页获取申请信息
        /// </summary>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<MagazineApply> SelectList(PageData pageData, out int totalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select count(1) from MagazineApply");
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());

            sql.Clear();

            sql.AppendFormat("  select top {0} * from MagazineApply where MagazineApply.Id not in(", pageData.Size);
            sql.AppendFormat(" select top {0} Id from MagazineApply order by MagazineApply.Status,MagazineApply.CreateTime)", pageData.Index);
            sql.Append(" order by MagazineApply.Status,MagazineApply.CreateTime");
            return DbHelp.Query<MagazineApply>(sql.ToString());
        }


        #endregion

        /// <summary>
        /// 获取纸质杂志申请记录
        /// </summary>
        /// <param name="status">记录状态</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="title">标题</param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<MagazineApply> GetMagazineApplyListByFilter(int? status, int? year, int? month, int? day, string title, string phoneNum, int start, int count, out int total)
        {
            try
            {
                var filter = new StringBuilder();
                if (status == null)
                    filter.Append(" and [Status] = 0 ");
                else
                    filter.AppendFormat(" and [Status] = {0} ", status);

                if (year != null && year > 0)
                    filter.AppendFormat(" and Datepart(YYYY,CreateTime) = {0} ", year);
                if (month != null && month > 0)
                    filter.AppendFormat(" and Datepart(MM,CreateTime) = {0} ", month);
                if (day != null && day > 0)
                    filter.AppendFormat(" and Datepart(DD,CreateTime) = {0} ", day);
                if (!string.IsNullOrEmpty(phoneNum))
                    filter.AppendFormat(" and phone= '{0}' ", phoneNum);
                var sql = new StringBuilder();
                sql.AppendFormat("SELECT top {0} * FROM MagazineApply WHERE MagazineTitle LIKE '%{1}%' ", count, title);
                sql.Append(filter.ToString());
                sql.AppendFormat(" and id not in (select top {0} id from MagazineApply WHERE MagazineTitle LIKE '%{1}%' ", start, title);
                sql.Append(filter.ToString());
                sql.Append(" ORDER BY CreateTime desc) ORDER BY CreateTime desc");

                var totalSql = new StringBuilder();
                totalSql.AppendFormat("Select count(1) from MagazineApply WHERE MagazineTitle like '%{0}%' ", title);
                totalSql.Append(filter.ToString());

                total = DbHelp.ExecuteScalar<int>(totalSql.ToString());
                return DbHelp.Query<MagazineApply>(sql.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 导出纸质杂志申请记录到Excel
        /// </summary>
        /// <param name="status">记录状态</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="title">标题</param>
        /// <returns></returns>
        public IEnumerable<MagazineApply> ExportMagazineApplyListByFilter(int? status, int? year, int? month, int? day, string title)
        {
            try
            {
                var filter = new StringBuilder();
                if (status == null)
                    filter.Append(" and [Status] = 0 ");
                else
                    filter.AppendFormat(" and [Status] = {0} ", status);

                if (year != null && year > 0)
                    filter.AppendFormat(" and Datepart(YYYY,CreateTime) = {0} ", year);
                if (month != null && month > 0)
                    filter.AppendFormat(" and Datepart(MM,CreateTime) = {0} ", month);
                if (day != null && day > 0)
                    filter.AppendFormat(" and Datepart(DD,CreateTime) = {0} ", day);
                var sql = new StringBuilder();
                sql.AppendFormat("SELECT * FROM MagazineApply WHERE MagazineTitle LIKE '%{0}%' ", title);
                sql.Append(filter.ToString());
                sql.Append(" ORDER BY CreateTime desc");

                return DbHelp.Query<MagazineApply>(sql.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
