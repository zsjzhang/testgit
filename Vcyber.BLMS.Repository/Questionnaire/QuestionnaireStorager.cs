using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class QuestionnaireStorager : IQuestionnaireStorager
    {
        public bool UpdateMemberShipEmail(string memberId, string emailVal)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("update dbo.MemberShip set Email='{0}' where Id='{1}'", emailVal, memberId);
            return DbHelp.Execute(sql.ToString()) > 0;
        }
        public IEnumerable<Questionnaire> GetQuestionnaireList(int qType)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from dbo.CS_Questionnaire where State>1  and QType={0}", qType);
            return DbHelp.Query<Questionnaire>(sql.ToString());
        }

        public Questionnaire GetCurrentQuestionnatire(int qType)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from CS_Questionnaire where '{0}' >= BeginTime and '{0}'<= EndTime and State=2 and QType={1}", DateTime.Now, qType);
            return DbHelp.QueryOne<Questionnaire>(sql.ToString());
        }


        public Questionnaire GetQuestionnatireById(int qid, int qType)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from CS_Questionnaire where Id={0} and State>1 and QType={1}", qid, qType);
            return DbHelp.QueryOne<Questionnaire>(sql.ToString());
        }

        public Questionnaire GetLastQuestionnatire(int qType)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select top(1)* from CS_Questionnaire where State>1  and QType={1} order by EndTime desc", DateTime.Now, qType);
            return DbHelp.QueryOne<Questionnaire>(sql.ToString());
        }

        public IEnumerable<Questionnaire> selectQuestionnaire(EQuestionnaireType qType, string qName, string order, int pageIndex, int pageSize, out int total)
        {
            qName = "%" + qName + "%";
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select count(*) from CS_Questionnaire where State <> 0 and QType = @QType and Theme like @Theme");
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { Theme = qName, QType = Convert.ToInt32(qType) });
            sql.Clear();

            sql.AppendFormat("select top {0} * from CS_Questionnaire", pageSize);
            sql.AppendLine(" where State <> 0 and QType = @QType and Theme like @Theme and CS_Questionnaire.Id not in (");
            sql.AppendFormat(" select top {0} CS_Questionnaire.Id from CS_Questionnaire where CS_Questionnaire.State <> 0 and CS_Questionnaire.QType = @QType and CS_Questionnaire.Theme like @Theme order by CS_Questionnaire.Id desc", (pageIndex - 1) * pageSize);
            sql.AppendLine(" )");
            sql.AppendLine(" order by CS_Questionnaire.Id desc");
            return DbHelp.Query<Questionnaire>(sql.ToString(), new { Theme = qName, QType = Convert.ToInt32(qType) });
            //string sql = string.Empty;
            //qName = "%" + qName + "%";
            //sql = @"select Id,Theme,AlternateTheme,Category,BeginTime,EndTime,CreateTime from CS_Questionnaire where State <> 0 and QType = @QType and Theme like @Theme order by Id " + order;
            //total = DbHelp.ExecuteScalar<int>("select count(*) from CS_Questionnaire where State <> 0 and QType = @QType and Theme like @Theme", new { Theme = qName, QType = Convert.ToInt32(qType) });
            //return DbHelp.Query<Questionnaire>(sql, new { Theme = qName, QType = Convert.ToInt32(qType) });
        }


        public int SSICreate(SSIQuestion entity)
        {


            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into QuestionNaire  values (@section_name,@section_tel,@section_car,@section_color,@Q1,@N1,@Q2,@N2,@Q3,@N3,@Q4,@N4,@Q5,@N5,@Q6,@N6,@Q7,@N7,@Q8,@N8,@Q9,@N9,@Q10,@N10,getdate());");
            return DbHelp.Execute(sql.ToString(),entity);
        }

        /// <summary>
        /// 新增问卷
        /// </summary>
        /// <param name="entity">问卷实体</param>
        /// <returns></returns>
        public int Create(Questionnaire entity)
        {
            entity.CreateTime = DateTime.Now;

            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into CS_Questionnaire (Theme,AlternateTheme,Category,BeginTime,EndTime,PeriodicalCount,BlueBeanCount,State,CreateTime,Image,Remarks,SyImage,LbRemarks,QType)");
            sql.Append(" values (@Theme,@AlternateTheme,@Category,@BeginTime,@EndTime,@PeriodicalCount,@BlueBeanCount,@State,@CreateTime,@Image,@Remarks,@SyImage,@LbRemarks,@QType);select @@identity;");

            return DbHelp.ExecuteScalar<int>(sql.ToString(), entity);

        }

        public bool CreateSelect(DateTime beginTime, int id, int qType)
        {
            string sqlStr = "select Id from CS_Questionnaire where EndTime >= @BeginTime and State = 2 and Id <> @Id and QType = @QType";
            IEnumerable<Questionnaire> qList = DbHelp.Query<Questionnaire>(sqlStr, new { BeginTime = beginTime, Id = id, QType = qType });
            return qList.Count() > 0;
        }

        // <summary>
        /// 修改问卷
        /// </summary>
        /// <param name="entity">问卷实体</param>
        /// <returns></returns>
        public bool Edit(Questionnaire entity)
        {
            string sql = "update CS_Questionnaire set Theme=@Theme,AlternateTheme=@AlternateTheme,Category=@Category,BeginTime=@BeginTime,EndTime=@EndTime,PeriodicalCount=@PeriodicalCount,BlueBeanCount=@BlueBeanCount,State=@State,CreateTime=@CreateTime,Image=@Image,Remarks=@Remarks,SyImage=@SyImage where Id=@Id";
            bool result = DbHelp.Execute(sql, entity) > 0;
            return result;
        }

        /// <summary>
        /// 删除问卷（不删除数据，只修改状态）
        /// </summary>
        /// <param name="id">删除数据的ID</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            return DbHelp.Execute("update CS_Questionnaire set State=@State where Id=@Id", new { State = 0, Id = id }) > 0;
        }

        public Questionnaire SelectSingle(int id)
        {
            string sql = "select * from CS_Questionnaire where Id = @Id";
            Questionnaire tempData = DbHelp.QueryOne<Questionnaire>(sql, new { Id = id });
            return tempData;
        }


        public int GetCurrentQuestionnaireState(int qType)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select ID from CS_Questionnaire where '{0}' between BeginTime and EndTime and QType={1} and State=2", DateTime.Now, qType);
            return DbHelp.ExecuteScalar<int>(sql.ToString());
        }

        public bool UpdateState(int id, int state)
        {
            return DbHelp.Execute("update CS_Questionnaire set State=@State where Id=@Id", new { State = state, Id = id }) > 0;
        }


        public bool IsCSManager(string userId, string roleName)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select COUNT(r.Id) from Roles r");
            sql.AppendLine(" where r.Id = (select ur.RoleId from UserRoles ur where ur.UserId = @UserId)");
            sql.AppendLine(" and IsDelete = 0");
            sql.AppendLine(" and r.Name = @Name");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), new { UserId = userId, Name = roleName }) > 0;

        }

    }
}
