using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface ISNCardStorager
    {
        /// <summary>
        /// 根据服务码查询服务码信息
        /// </summary>
        /// <param name="snCode">服务码</param>
        /// <returns>服务码信息</returns>
        SNCard SelectSNCardByCode(string snCode);

        /// <summary>
        /// 获取用户持有的服务编码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>结果集</returns>
        IEnumerable<SNCard> SelectSNCardByUser(string userId);

        /// <summary>
        /// 获取指定数量的服务编码
        /// </summary>
        /// <param name="number">数量</param>
        /// <returns>结果集</returns>
        IEnumerable<SNCard> GetSNCard(int number);

        /// <summary>
        /// 下发服务编码
        /// </summary>
        /// <param name="userId">用户编码</param>
        /// <param name="id">服务编码ID</param>
        /// <returns>执行结果</returns>
        bool SendSNCard(string userId, int id, int sendType, int airportId, string phoneNumber, string dataSource);

        /// <summary>
        /// 使用服务码
        /// </summary>
        /// <param name="snCode">服务码</param>
        /// <returns>执行结果</returns>
        bool UseSNCard(ReadCheckinDataItemDetail record);

        /// <summary>
        /// 添加使用记录
        /// </summary>
        /// <param name="record">机场核销数据</param>
        /// <returns>保存结果</returns>
        bool AddUsedRecord(ReadCheckinDataItemDetail record, string batchNo);

        IEnumerable<SNCard> SelectSNCard(string phoneNumber, int status, string iscallcenter, string start, string end, int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 卡券统计
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="status"></param>
        /// <param name="noSend"></param>
        IEnumerable<SNCard> SelectSNCardCount(string phoneNumber, int status, out int noSend);

        /// <summary>
        /// 增加卡券
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        bool AddSNCard(SNCard card);

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
        IEnumerable<string> SelectProvince();

        IEnumerable<string> SelectCityByProvince(string province);

        /// <summary>
        /// 查询所有机场列表
        /// </summary>
        /// <returns>机场列表</returns>
        IEnumerable<Airport> SelectAirportList();

        /// <summary>
        /// 根据省、市查询机场
        /// </summary>
        /// <param name="province">省</param>
        /// <param name="city">市</param>
        /// <returns></returns>
        IEnumerable<Airport> SelectAirportList(string province, string city);

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
        /// <returns></returns>
        IEnumerable<Airport> AllAirportRoomList();

        /// <summary>
        /// 查询机场详细信息
        /// </summary>
        /// <param name="id">机场ID</param>
        /// <returns>机场信息</returns>
        Airport SelectAirport(int id);

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
        /// <param name="phone"></param>
        /// <returns></returns>
        IEnumerable<SNCard> AirportServiceList(string phone);

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
