using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IAirportServiceApp
    {
        /// <summary>
        /// 核销机场数据
        /// </summary>
        /// <returns>返回处理结果</returns>
        ReturnResult ReadCheckinData();

        /// <summary>
        /// 根据批次确认核销数据
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回处理结果</returns>
        ReturnResult ConfirmData(string batchNo);

        /// <summary>
        /// 领取免费机场服务码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="phoneNumber">领取的手机号</param>
        /// <param name="number">领取的张数</param>
        /// <param name="sendType">下发方式</param>
        /// <param name="airportId">机场编号</param>
        /// <returns>返回处理结果</returns>
        ReturnResult GetFreeServiceCard(string userId, string phoneNumber, int number, int sendType, int airportId);

        /// <summary>
        /// 积分兑换机场服务码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="phoneNumber">领取的手机号</param>
        /// <param name="number">领取的张数</param>
        /// <param name="airportId">机场编号</param>
        /// <returns></returns>
        ReturnResult TradeServiceCardByIntegral(string userId, string phoneNumber, int number, int airportId);

        /// <summary>
        /// 领取机场服务码(优先领取免费次数，不够用积分兑换)
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="phoneNumber">领取的手机号</param>
        /// <param name="number">领取的张数</param>
        /// <param name="sendType">下发方式</param>
        /// <param name="airportId">机场编号</param>
        /// <returns>返回处理结果</returns>
        ReturnResult GetServiceCardAuto(string userId, string phoneNumber, int number, int sendType, int airportId);


        /// <summary>
        /// 预约机场服务
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="phoneNumber">验证码接收手机号</param>
        /// <param name="num1">免费次数</param>
        /// <param name="num2">兑换次数</param>
        /// <param name="airportId">机场编号</param>
        /// <returns></returns>
        ReturnResult GetServiceCard(string userId, string phoneNumber, int num1, int num2, int airportId, string isCallCenter, string dataSource);



        /// <summary>
        /// 分页查询服务码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<SNCard> SelectSNCard(string phoneNumber, int status, string iscallcenter, string start, string end, int pageIndex, int pageSize, out int totalCount);


        /// <summary>
        /// 卡券统计
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="status"></param>
        /// <param name="noSend"></param>
        IEnumerable<SNCard> SelectSNCardCount(string phoneNumber, int status, out int noSend);

        /// <summary>
        /// 获取用户持有的服务编码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>结果集</returns>
        IEnumerable<SNCard> SelectSNCardByUser(string userId);

        /// <summary>
        /// 获取用户可预约的机场服务次数
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>返回可预约的次数</returns>
        int GetSNCardNumber(string userId);

        /// <summary>
        /// 导入卡券
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        ReturnResult ImportSNCard(string path);

        /// <summary>
        /// 发放卡券
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="phonenumber"></param>
        /// <param name="code">
        /// <param name="sendType"></param>
        /// <returns></returns>
        ReturnResult SendCard(string userid, int id, string code, string phonenumber, int sendType, int airportId, string dataSource);

        /// <summary>
        /// 重新发送服务码短信
        /// </summary>
        /// <param name="code">服务码编号</param>
        /// <returns>发送结果</returns>
        ReturnResult SendCardSMS(string code);
        

        /// <summary>
        /// 查询服务券详细信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        SNCard SelectCard(string code);


        /// <summary>
        /// 查询服务券的使用信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        SNUsedRecord SelectCardUsedRecord(string code);

        /// <summary>
        /// 返回所有机场所在省
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> SelectAirportProvince();

        IEnumerable<string> SelectCityByProvince(string province);

        /// <summary>
        /// 查询所有机场列表
        /// </summary>
        /// <returns>机场列表</returns>
        IEnumerable<Airport> SelectAirportList();

        IEnumerable<Airport> SelectAirportRoomList(string province, string city, string airport);

        /// <summary>
        /// 根据机场名称查询机场候机室列表
        /// </summary>
        /// <param name="airportName">机场名称</param>
        /// <returns>机场候机室列表</returns>
        IEnumerable<Airport> SelectAirportRoomList(string airportName);

        /// <summary>
        /// 获取所有机场候车室
        /// </summary>
        /// <param name="airportName"></param>
        /// <returns></returns>
        IEnumerable<Airport> AllAirportRoomList();

        /// <summary>
        /// 根据省、市查询机场
        /// </summary>
        /// <param name="province">省</param>
        /// <param name="city">市</param>
        /// <returns></returns>
        IEnumerable<Airport> SelectAirportList(string province, string city);

        /// <summary>
        /// 用户轮询
        /// </summary>
        /// <param name="identityNumber">身份证号</param>
        /// <returns></returns>
        bool MembershipMonitor(string identityNumber);

        /// <summary>
        /// 用户轮询积分
        /// </summary>
        /// <param name="identityNumber">身份证号</param>
        /// <returns></returns>
        bool MembershipMonitorIntegral(string identityNumber);

        /// <summary>
        /// 预约候机
        /// </summary>
        /// <param name="airportName"></param>
        /// <returns></returns>
        IEnumerable<SNCard> AirportServiceList(string phone);

        /// <summary>
        /// 活动抽奖，提供服务码
        /// </summary>
        /// <param name="phoneNumber">用户手机号</param>
        /// <param name="userId">用户ID</param>
        /// <returns>服务码</returns>
        string SendCardByActivity(string phoneNumber,string userId);

        bool AddAirport(Airport airport);

        bool UpdateAirport(Airport airport);

        bool DeleteAirport(int Id);

        /// <summary>
        /// 获取单个候机服务券详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SNCard GetCardByUserDetails(string userid, string id);
    }
}
