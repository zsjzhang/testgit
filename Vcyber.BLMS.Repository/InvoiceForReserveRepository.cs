using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Repository.Entity.Generated;

namespace Vcyber.BLMS.Repository
{
    public class InvoiceForReserveRepository : IInvoiceForReserveRepository
    {
        public int Insert(InvoiceForReserve entity)
        {
            var commandText = @"insert into InvoiceForReserve(MembershipId,ImageUrl,ServiceType,CreateTime) 
                                values(@MembershipId,@ImageUrl,@ServiceType,@CreateTime); select @@identity;";
            int id = DbHelp.ExecuteScalar<int>(commandText, entity);
            return id;
        }
        /// <summary>
        /// 前台用户上传凭证
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertUserProofRecord(InvoiceForReserve entity)
        {
            var commandText = @"insert into UserProofRecord(MembershipId,ImageProofFront,ImageProofVerso,ImageProofByHand,ApproveStatus,CreateTime,UpdateTime,IsDelete) 
                                values(@MembershipId,@ImageProofFront,@ImageProofVerso,@ImageProofByHand,@ApproveStatus,@CreateTime,@UpdateTime,@IsDelete); select @@identity;";
            int id = DbHelp.ExecuteScalar<int>(commandText, entity);
            return id;

        }

        public bool UpdateUserProofRecord(InvoiceForReserve entity)
        {
            var commandText = @"update UserProofRecord set ApproveStatus=0,IsDelete=0,UpdateTime=getdate(),ImageProofFront=@ImageProofFront,ImageProofVerso=@ImageProofVerso,ImageProofByHand=@ImageProofByHand  where MembershipId=@MembershipId";
            return DbHelp.Execute(commandText, entity) > 0;
        }
        /// <summary>
        /// 更改用户审核状态为通过
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool UpdateProofStatus(string id,string status)
        {
            string sql = "Update UserProofRecord set IsDelete=0, ApproveStatus =@Status,UpdateTime=getdate() where MembershipId =@Id ";
            return DbHelp.Execute(sql, new {@Status=status, @Id = id }) > 0;

        }
        /// <summary>
        /// 更改用户审标记为删除
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool DeleteProofInfo(string Ids)
        {
            string sql = "Update UserProofRecord set IsDelete =1,UpdateTime=getdate() where Id in(@Ids) ";
            return DbHelp.Execute(sql, new { @Ids = Ids }) > 0;
        }
        /// <summary>
        /// 通过UserID获取所需数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InvoiceForReserve GetProofInfoById(string id)
        {
            string sql = "Select * From UserProofRecord WHERE MembershipId=@Id ";
            return DbHelp.QueryOne<InvoiceForReserve>(sql, new { @Id = id });
        }

        /// <summary>
        /// 根据查询条件获得列表数据
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="mlevel"></param>
        /// <param name="paperwork"></param>
        /// <param name="identitynumber"></param>
        /// <param name="status"></param>
        /// <param name="createTime"></param>
        /// <param name="end"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<InvoiceForReserve> GetSearch(string phone, string mlevel, string paperwork, string identitynumber, string status, string createTime, string end, int startrow, int endrow, out int totalCount)
        {
            string where = "";
            DateTime endTime = DateTime.MaxValue;

            if (!string.IsNullOrWhiteSpace(phone))
            {
                where += " and m.PhoneNumber=@phone ";
            }
            if (!string.IsNullOrWhiteSpace(mlevel))
            {
                if(mlevel!="-1")
                {
                    where += " and m.MLevel=@mlevel  ";
                }
            }
            if (!string.IsNullOrWhiteSpace(paperwork))
            {
                where += " and p.PaperWork=@paperwork ";
            }
            if (!string.IsNullOrWhiteSpace(identitynumber))
            {
                where += " and m.IdentityNumber=@identitynumber ";
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                where += " and u.ApproveStatus=@status ";
            }

            if (!string.IsNullOrWhiteSpace(createTime))
            {
                 
                where += " and u.CreateTime>=@start ";
            }

            if (!string.IsNullOrWhiteSpace(end))
            {
                endTime=DateTime.Parse(end).AddDays(1);
                where += " and u.CreateTime<@end ";
            }
           
            var parameters = new Dictionary<string, object>
            {
                {"@phone", phone},
                {"@mlevel", mlevel},
                {"@paperwork", paperwork},
                {"@identitynumber", identitynumber},
                {"@status", status},
                {"@start", createTime},
                {"@end",endTime},
            };

            StringBuilder selectSQL = new StringBuilder();
            selectSQL.Append("select count(1) from Membership m left join Membership_Schedule p on m.id=p.MembershipId join UserProofRecord u on u.MembershipId=m.id ");
            selectSQL.AppendFormat("where u.IsDelete=0 and 1=1 {0}", where);


            totalCount = DbHelp.ExecuteScalar<int>(selectSQL.ToString(), parameters);
            //totalCount = DbHelp.ExecuteScalar<int>(selectSQL.ToString(),new
            //{
            //    @phone = phone,
            //    @mlevel = mlevel,
            //    @paperwork = paperwork,
            //    @identitynumber = identitynumber,
            //    @status = status,
            //    @start =DateTime.Parse(createTime),
            //    @end = DateTime.Parse(end)
            //});

            selectSQL.Clear();

            selectSQL.AppendFormat(
                @"select * from (select  ROW_NUMBER() over(order by u.createtime desc) as rownum,m.RealName,m.PhoneNumber,u.MembershipId,m.UserName,
case p.PaperWork when  1 then '身份证' when 2 then '护照' when 3 then '军官证' end PaperWork,m.IdentityNumber,u.CreateTime,m.MLevel,
u.Id,u.ImageProofFront,u.ImageProofVerso,u.ImageProofByHand,u.ApproveStatus from Membership m left join Membership_Schedule p
 on m.id=p.MembershipId join UserProofRecord u on u.MembershipId=m.id where u.IsDelete=0 and 1=1 {0}) t
 where  t.rownum>={1} and t.rownum<={2}",where,startrow,endrow);
            
            //selectSQL.AppendFormat("and 1=1 {0}", where);
            //selectSQL.Append(@"select ROW_NUMBER() over(order by u.createtime desc) as rownum,m.PhoneNumber,m.UserName,
            //case p.PaperWork when  1 then '身份证' 
            //when 2 then '护照'
            //when 3 then '军官证' end PaperWork,m.IdentityNumber,
            //u.CreateTime,m.MLevel,u.ApproveStatus 
            //from Membership m
            //join Membership_Schedule p
            //on m.id=p.MembershipId
            //join UserProofRecord u
            //on u.MembershipId=m.id
            //");
            // sql += " select * from ";
            return DbHelp.Query<InvoiceForReserve>(selectSQL.ToString(), parameters); 
        }
    }
}
