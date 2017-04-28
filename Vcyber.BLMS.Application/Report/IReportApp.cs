using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IReportApp
    {
        DataTable GetReport(DateTime startTime, DateTime endTime, string tableName);
        /// <summary>
        /// 报表管理--按车型、区域统计会员入会数量
        /// </summary>
        /// <param name="CreateTimeStart">入会开始时间</param>
        /// <param name="CreateTimeEnd">入会结束时间</param>
        /// <param name="AuthenticationTimeStart">入会认证开始时间</param>
        /// <param name="AuthenticationTimeEnd">入会认证结束时间</param>
        /// <param name="BuyTimeStart">购车开始时间</param>
        /// <param name="BuyTimeEnd">购车结束时间</param>
        /// <param name="CarCategory">车型</param>
        /// <param name="tableName">存储过程名称</param>
        /// <returns></returns>
        DataTable GetReport(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, DateTime? AuthenticationTimeStart, DateTime? AuthenticationTimeEnd, DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory, string AccntType, string tableName);

        DataTable GetReport(DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory, string tableName);
        /// <summary>
        /// 按照车型获取完成认证的会员数量
        /// </summary>
        /// <param name="AuthenticationTimeStart">认证开始时间</param>
        /// <param name="AuthenticationTimeEnd">认证结束时间</param>
        /// <param name="CarCategory">车型</param>
        /// <returns></returns>
        DataTable GetAuthenticationSourceByCarCategory(DateTime? AuthenticationTimeStart, DateTime? AuthenticationTimeEnd,  DateTime? BuyTimeStart, DateTime? BuyTimeEnd,string CarCategory, string tableName);


        DataTable GetUserCarIntegralRecordValueSum( DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory, string tableName);
        


        DataTable GetCommonReport(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType, string tableName);

        DataTable GetCommonReport(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType, string CarCategory, string tableName);
        /// <summary>
        /// 按年月日统计[活动]的入会人数
        /// </summary>
        /// <param name="CreateTimeStart">参加活动的开始时间</param>
        /// <param name="CreateTimeEnd">参加活动的结束时间</param>
        /// <param name="TimeType">查询维度，年月日</param>
        /// <param name="ActivityId">活动ID</param>
        /// <param name="tableName">存储过程名称</param>
        /// <returns></returns>
        DataTable GetMembershipCountByTimeAndActivity(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType, string ActivityId, string tableName);
        /// <summary>
        /// 查询活动中奖明细
        /// </summary>
        /// <param name="CreateTimeStart">中奖开始时间</param>
        /// <param name="CreateTimeEnd">中奖结束时间</param>
        /// <param name="ActivityId">活动ID</param>
        /// <param name="tableName">存储过程名</param>
        /// <returns></returns>
        DataTable GetWinningInfoDetailsByActivity(DateTime? CreateTimeStart, DateTime? CreateTimeEnd,  string ActivityId, string tableName);

        /// <summary>
        /// 会员创建渠道管理
        /// </summary>
        /// <param name="qType">渠道归属状态, 0=未分配，1=已分配，2=所有</param>
        /// <param name="CreatedPerson">渠道标识</param>
        /// <param name="tableName">存储过程名</param>
        /// <returns></returns>
        DataTable GetCreatedPerson(string qType, string CreatedPerson, string tableName);

        int SaveCreatedType(string qType, string CreatedPerson, string tableName);
        

        DataTable GetReport(int activityId, string tableName);
        DataTable GetReport(string tableName);

        DataTable GetUserintegralReport(string startTime, string endTime);

        DataTable GetMemberResourceReport(string startTime, string endTime);

        DataTable GetMemberDearBuyCarReport(string startTime, string endTime, string tableName);
        //购车入会率
        DataTable GetMemberJoinDate(DateTime startTime, DateTime endTime, DateTime buyStartTime, DateTime buyEndTime, string region, string tableName);
        
        //活动卡券已领取未核销
        DataTable AcitivityCard_NoCancel(string  startTime, string  endTime, string tableName, string AcitivityName);
       //获取活动列表
        IEnumerable<SCServiceCardType> GetScServiceActivitName();
        
         //会员积分消费报表
        /// <summary>
        /// 查询会员信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<MemberReportInfo> FindMemberList(MemberReportCondition condition, PageData pageData, out int totalCount);

        /// <summary>
        /// 查询会员消耗积分报表信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<IntegralOutReportInfo> FindIntegralOutList(IntegralOutReportCondition condition, PageData pageData, out int totalCount);

        /// <summary>
        /// 查询会员获取的积分信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<IntegralInputReportInfo> FindIntegralInputList(IntegralInputReportCondition condition, PageData pageData, out int totalCount);

        /// <summary>
        /// 获取会员积分信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<IntegralReportInfo> FindIntegralList(IntegralReportCondition condition, PageData pageData, out int totalCount);

       DataTable GetEquity(string startTime, string endTime);

        //发送卡券和核销数据
       DataTable CardReport(string startTime, string endTime);

        //获取周报数据
       DataTable GetWeekReport(string startTime, string endTime);

        //会员积分消费报表
        DataTable GetPointCostReport(string starTime,string endTime);

       IEnumerable<ServiceModel> ServiceUse(string Createtime, string RealName, string phoneNumber, string DataSource, string AirportName, string DealerId, string Status, string OrderType, int pageIndex, int pageSize, out int totalCount);


       IEnumerable<IntegralCountReportInfo> FindIntegralCountList(IntegralCountReportCondition condition);
       bool validateData(string dealerId, string startTime, string endTime);
       IEnumerable<IntegralCountReportInfo> FindDealerCountList(IntegralCountReportCondition condition);
       void DealerSettlement(string dealerId, string startDate, string endDate, int SettlementState);
       IEnumerable<QuestionnaireVisitor> FindQuestionnaireVisitorList(QuestionnaireVisitorCondition condition, PageData pageData, out int totalCount);

       QuestionnarieDayReportInfo FindQuestionnaireDayInfo(string day,string endDay,int qId);

       List<AnswerReportInfo> FindAnswerList(int questionnarieId);

       List<AnswerReportInfo> FindAnswerListCs(int questionnarieId);

       QuestionnarieDayReportInfoCS FindQuestionnaireDayInfoCs(string day, string endDay, int qId);

       IEnumerable<AnswerReport> FindAnswerListCSNew(int questionnarieId);

       IEnumerable<string> FindAnswerListMember(int questionnarieId);
    }
}
