using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class ReportApp : IReportApp
    {
        public DataTable GetReport(DateTime startTime, DateTime endTime, string tableName)
        {
            return _DbSession.ReportStorager.GetReport(startTime, endTime, tableName);
        }
        public DataTable GetReport(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, DateTime? AuthenticationTimeStart, DateTime? AuthenticationTimeEnd, DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory, string AccntType, string tableName)
        {
            return _DbSession.ReportStorager.GetReport(CreateTimeStart, CreateTimeEnd, AuthenticationTimeStart, AuthenticationTimeEnd, BuyTimeStart, BuyTimeEnd, CarCategory, AccntType, tableName);
        }

        public DataTable GetReport(DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory, string tableName)
        {
            return _DbSession.ReportStorager.GetReport(BuyTimeStart, BuyTimeEnd, CarCategory, tableName);
        }

        public DataTable GetAuthenticationSourceByCarCategory(DateTime? AuthenticationTimeStart, DateTime? AuthenticationTimeEnd, DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory, string tableName)
        {
            return _DbSession.ReportStorager.GetAuthenticationSourceByCarCategory(AuthenticationTimeStart, AuthenticationTimeEnd,  BuyTimeStart, BuyTimeEnd,CarCategory, tableName);
        }
        /// <summary>
        /// 车型积分下发统计
        /// </summary>
        /// <param name="BuyTimeStart"></param>
        /// <param name="BuyTimeEnd"></param>
        /// <param name="CarCategory"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetUserCarIntegralRecordValueSum(DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory, string tableName)
        {
            return _DbSession.ReportStorager.GetUserCarIntegralRecordValueSum(BuyTimeStart, BuyTimeEnd, CarCategory, tableName);
        }

        public DataTable GetCommonReport(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType, string tableName)
        {
            return _DbSession.ReportStorager.GetCommonReport(CreateTimeStart, CreateTimeEnd, TimeType, tableName);
        }

        public DataTable GetCommonReport(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType, string CarCategory, string tableName)
        {
            return _DbSession.ReportStorager.GetCommonReport(CreateTimeStart, CreateTimeEnd, TimeType, CarCategory, tableName);
        }
        /// <summary>
        /// 按年月日统计[活动]的入会人数
        /// </summary>
        /// <param name="CreateTimeStart">参加活动的开始时间</param>
        /// <param name="CreateTimeEnd">参加活动的结束时间</param>
        /// <param name="TimeType">查询维度，年月日</param>
        /// <param name="ActivityId">活动ID</param>
        /// <param name="tableName">存储过程名称</param>
        /// <returns></returns>
        public DataTable GetMembershipCountByTimeAndActivity(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType, string ActivityId, string tableName)
        {
            return _DbSession.ReportStorager.GetMembershipCountByTimeAndActivity(CreateTimeStart, CreateTimeEnd, TimeType, ActivityId, tableName);
        }

        public DataTable GetWinningInfoDetailsByActivity(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string ActivityId, string tableName)
        {
            return _DbSession.ReportStorager.GetWinningInfoDetailsByActivity(CreateTimeStart, CreateTimeEnd, ActivityId, tableName);
        }

        public DataTable GetCreatedPerson(string qType, string CreatedPerson, string tableName)
        {
            return _DbSession.ReportStorager.GetCreatedPerson(qType, CreatedPerson,  tableName);
        }

        public int SaveCreatedType(string qType, string CreatedPerson, string tableName)
        {
            return _DbSession.ReportStorager.SaveCreatedType(qType, CreatedPerson, tableName);
        }

        //积分下发报表
        public DataTable GetUserintegralReport(string startTime, string endTime)
        {
            return _DbSession.ReportStorager.GetUserintegralReport(startTime, endTime);
        }
        //
        public DataTable GetMemberResourceReport(string startTime, string endTime)
        {
            return _DbSession.ReportStorager.GetMemberResourceReport(startTime, endTime);
        }
        public DataTable GetReport(int activityId, string tableName)
        {
            return _DbSession.ReportStorager.GetReport(activityId, tableName);
        }
        public DataTable GetEquity(string startTime, string endTime)
        {
            return _DbSession.ReportStorager.GetEquity(startTime, endTime);
        }

        public DataTable GetWeekReport(string startTime, string endTime)
        {
            return _DbSession.ReportStorager.GetWeekReport(startTime, endTime);
        }

        public DataTable CardReport(string name,string type)
        {
            return _DbSession.ReportStorager.CardReport(name,type);
        }

        //会员积分消费报表
        public DataTable GetPointCostReport(string startTime, string endTime)
        {
            return _DbSession.ReportStorager.GetPointCostReport(startTime,endTime);
        }
        //活动卡券已领取未核销
        public DataTable AcitivityCard_NoCancel(string  startTime, string  endTime, string tableName, string AcitivityName)
        {
            return _DbSession.ReportStorager.AcitivityCard_NoCancel(startTime, endTime, tableName, AcitivityName);
        }
        public DataTable GetMemberDearBuyCarReport(string startTime, string endTime, string tableName)
        {
            return _DbSession.ReportStorager.GetMemberDearBuyCarReport(startTime, endTime, tableName);
        }
        public DataTable GetMemberJoinDate(DateTime startTime, DateTime endTime, DateTime buyStartTime, DateTime buyEndTime, string region, string tableName)
        {
            return _DbSession.ReportStorager.GetMemberJoinDate(startTime, endTime, buyStartTime, buyEndTime, region, tableName);
        }
        public IEnumerable<ServiceModel> ServiceUse(string Createtime, string RealName, string phoneNumber, string DataSource, string AirportName, string DealerId, string Status, string OrderType, int pageIndex, int pageSize, out int totalCount)
        {
            return _DbSession.ReportStorager.ServiceUse(Createtime, RealName, phoneNumber, DataSource, AirportName, DealerId, Status, OrderType, pageIndex, pageSize, out totalCount);
        }


        /// <summary>
        /// 查询会员信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<MemberReportInfo> FindMemberList(MemberReportCondition condition, PageData pageData, out int totalCount)
        {
            return _DbSession.ReportStorager.FindMemberList(condition, pageData, out totalCount);
        }

        /// <summary>
        /// 查询会员消耗积分报表信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<IntegralOutReportInfo> FindIntegralOutList(IntegralOutReportCondition condition, PageData pageData, out int totalCount)
        {
            return _DbSession.ReportStorager.FindIntegralOutList(condition, pageData, out totalCount);
        }

        /// <summary>
        /// 查询会员获取的积分信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<IntegralInputReportInfo> FindIntegralInputList(IntegralInputReportCondition condition, PageData pageData, out int totalCount)
        {
            return _DbSession.ReportStorager.FindIntegralInputList(condition, pageData, out totalCount);
        }

        /// <summary>
        /// 获取会员积分信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<IntegralReportInfo> FindIntegralList(IntegralReportCondition condition, PageData pageData, out int totalCount)
        {
            return _DbSession.ReportStorager.FindIntegralList(condition, pageData, out totalCount);
        }


        /// <summary>
        /// 管理员获取积分结算报表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<IntegralCountReportInfo> FindIntegralCountList(IntegralCountReportCondition condition)
        {
            return _DbSession.ReportStorager.FindIntegralCountList(condition);
        }
        /// <summary>
        /// 验证 SettleDealerPoint 中是否有重复信息
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public bool validateData(string dealerId, string startTime, string endTime)
        {
            return _DbSession.ReportStorager.validateData(dealerId, startTime,endTime);
        }
        /// <summary>
        /// 去SettleDealerPoint查积分结算核对报表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<IntegralCountReportInfo> FindDealerCountList(IntegralCountReportCondition condition)
        {
            return _DbSession.ReportStorager.FindDealerCountList(condition);
        }
        /// <summary>
        /// 经销商特约店积分结算 确认 或 申请复核 操作
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="SettlementState"></param>
        /// <param name="consumeType"></param>
        public void DealerSettlement(string dealerId, string startDate, string endDate, int SettlementState)
        {
            _DbSession.ReportStorager.UpdateDealerSettlementSeate(dealerId, startDate, endDate, SettlementState);
        }

        public IEnumerable<QuestionnaireVisitor> FindQuestionnaireVisitorList(QuestionnaireVisitorCondition condition, PageData pageData, out int totalCount)
        {
            return _DbSession.ReportStorager.FindQuestionnaireVisitorList(condition, pageData, out totalCount);
        }

        public QuestionnarieDayReportInfo FindQuestionnaireDayInfo(string day, string endDay, int qId)
        {
            DateTime dayTime = Convert.ToDateTime(day);
            DateTime endDayTime = Convert.ToDateTime(endDay);
            QuestionnarieDayReportInfo qdInfo = new QuestionnarieDayReportInfo();
            qdInfo.SjCount = _DbSession.ReportStorager.GetSjCount(dayTime, endDayTime, qId, false);
            qdInfo.PtCount = _DbSession.ReportStorager.GetPtCount(dayTime, endDayTime, qId, false);
            qdInfo.FcCount = _DbSession.ReportStorager.GetFcCount(dayTime, endDayTime, qId);
            qdInfo.VisitorCount = _DbSession.ReportStorager.GetTotalCount(dayTime, endDayTime, qId) - qdInfo.SjCount - qdInfo.FcCount - qdInfo.PtCount;
            return qdInfo;
        }

        public QuestionnarieDayReportInfoCS FindQuestionnaireDayInfoCs(string day, string endDay, int qId)
        {
            DateTime dayTime = Convert.ToDateTime(day);
            DateTime endDayTime = Convert.ToDateTime(endDay);
            QuestionnarieDayReportInfoCS qdInfo = new QuestionnarieDayReportInfoCS();
            qdInfo.SjCount = _DbSession.ReportStorager.GetSjCount(dayTime, endDayTime, qId, false);
            qdInfo.PtCount = _DbSession.ReportStorager.GetPtCount(dayTime, endDayTime, qId, false);
            qdInfo.NewSjCount = _DbSession.ReportStorager.GetSjCount(dayTime, endDayTime, qId, true);
            qdInfo.NewPtCount = _DbSession.ReportStorager.GetPtCount(dayTime, endDayTime, qId, true);
            return qdInfo;
        }


        public List<AnswerReportInfo> FindAnswerList(int questionnarieId)
        {
            List<AnswerReportInfo> result = new List<AnswerReportInfo>();
            result.AddRange(_DbSession.ReportStorager.FindAnswerList(questionnarieId));
            result.AddRange(_DbSession.ReportStorager.FindAnswerJzList(questionnarieId));
            IEnumerable<AnswerReportInfo> query = null;
            //result.Sort();
            query = from item in result orderby item.Sort select item;
            return query.ToList();
        }

        public List<AnswerReportInfo> FindAnswerListCs(int questionnarieId)
        {
            List<AnswerReportInfo> result = new List<AnswerReportInfo>();
            result.AddRange(_DbSession.ReportStorager.FindAnswerListCs(questionnarieId));
            result.AddRange(_DbSession.ReportStorager.FindAnswerJzListCs(questionnarieId));
            IEnumerable<AnswerReportInfo> query = null;
            //result.Sort();
            query = from item in result orderby item.Sort select item;
            return query.ToList();
        }


        public DataTable GetReport(string tableName)
        {
            return _DbSession.ReportStorager.GetReport(tableName);
        }

        public IEnumerable<AnswerReport> FindAnswerListCSNew(int questionnarieId)
        {
            return _DbSession.ReportStorager.FindAnswerListCSNew(questionnarieId);
        }

        public IEnumerable<string> FindAnswerListMember(int questionnarieId)
        {
            return _DbSession.ReportStorager.FindAnswerListMember(questionnarieId);
        }
        public IEnumerable<SCServiceCardType> GetScServiceActivitName()
        {
            return _DbSession.ReportStorager.GetScServiceActivitName();
        }
    }
}
