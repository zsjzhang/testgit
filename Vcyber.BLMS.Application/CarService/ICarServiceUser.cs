using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Application.CarService
{
    using Vcyber.BLMS.Entity;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.Entity.SelectCondition;

    public interface ICarServiceUser
    {
        int GetJoinIntegralByIdentity(string identity, DateTime SubmitTime, List<Car> Vins);
        int GetIntegralByIdentity(string identity, string IsPay, List<Car> Vins);
        bool IsSonataUser(string userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity">身份证号</param>
        /// <param name="service"></param>
        /// <returns></returns>
        int GetFreeServiceCount(string identity, EDMSServiceType service);
        /// <summary>
        /// 根据服务类型和身份证号码查询所有享受服务记录
        /// </summary>
        /// <param name="identity">身份证号</param>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        List<FreeServiceRecord> GetFreeServiceRecord(string identity, EDMSServiceType serviceType);

        IEnumerable<Car> SelectCarListByUserId(string userId);

        IEnumerable<Car> SelectCarListByIdentity(string identityNumber);

        CarTypeReturnIntegral GetReIntegralTypeByIdentity(string identity);
        /// <summary>
        /// 判断注册并绑定车的用户是那种类型
        /// 0： D+S 首次 100元 返 4000积分  1：  D+S 增换 返7000积分  2： 非 D+S  首 50 元 返 2000积分 3 ： 增换 返4000积分
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="createtime"></param>
        /// <returns></returns>
        CarTypeReturnIntegral GetRegisterIntegralTypeByIdentity(string identity,DateTime createtime);
        /// <summary>
        /// 获取注册并且绑定车的用户缴费可获得的积分
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="CreateTime"></param>
        /// <returns></returns>
        int GetRegisterIntegralByIdentity(string identity, DateTime CreateTime);
        /// <summary>
        /// 用户缴费返还积分
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        int GetIntegrationByBuyCarPayMoney(string identity);

        int GetIntegrationByBuyCarPayMoneyJoinMember1(string identity);
        IFCustomer GetCustomer(string identityNumber);

        bool UpdateCarInfo(string vin, string LicencePlate, string Mileage);

        Car GetCarInfoByVIN(string vin);

      

        /// <summary>
        /// 查询中间表客户信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        IEnumerable<IFCustomer> FindCustomer(CustomerCondition condition);

        /// <summary>
        /// 查询中间表客户信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="cars">客户车辆信息</param>
        /// <returns></returns>
        IEnumerable<IFCustomer> FindCustomer(CustomerCondition condition, out List<Car> cars);

        IEnumerable<IFCustomer> OldFindCustomer(CustomerCondition condition, out List<Car> cars);
        /// <summary>
        /// 删除中间表客户信息
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        bool DelCustomer(CustomerCondition condition);
        /// <summary>
        /// 根据Vin查询车辆信息
        /// </summary>
        /// <param name="vin">vin</param>
        /// <param name="customer">车主信息</param>
        /// <returns></returns>
        Car GetCarInfoByVIN(string vin, out IFCustomer customer);

        Car OldGetCarInfoByVIN(string vin, out IFCustomer customer);

        /// <summary>
        /// IFCustomer.CustId规则：JT-110222199912013115（集团-身份证号)
        /// 这是在该表中insert数据的唯一入口，所以CustId的规则一定
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IFCustomer CreateOrGetIFCustomer(IFCustomer customer);

        bool UpdateCarInfoByVIN(string vin, string custId);

        bool InsertCarInfoByVIN(string vin, string custId, string CarCategory, string Buytime);

        /// <summary>
        /// 是否存在相同的身份证号
        /// </summary>
        /// <param name="identityNumber"></param>
        /// <returns></returns>
        bool IsExistIdentity(string identityNumber);

        bool OldIsExistIdentity(string identityNumber);

        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="data"></param>
        void AddCustomer(IFCustomer data);

        /// <summary>
        /// 根据VIN查询用户的等级和卡号
        /// </summary>
        /// <param name="vin"></param>
        /// <returns></returns>
        LevelAndNo GetUserLevelByVin(string vin);

        bool AddActivityJoinRecord(string uname, string mobile, string aname);

        EMemshipLevelWX GetMemshipLevelWX(string userId);


        bool UpdateCarInfo2(string vin, string LicencePlate, string Mileage);
        
        IEnumerable<CSTestDrive> GetCSTestDrive(string userid);

        IEnumerable<CSTestDrive> GetCSTestDriveDetial(string OrderNo);

        IEnumerable<CSMaintenance> GetCSMaintenance(string userid);
        IEnumerable<CSMaintenance> GetCSMaintenanceDetial(string OrderNo);

        IEnumerable<GetVinCustTimeEF> GetVinCustTime();
        IEnumerable<CSTestDrive> GetCSTestDriveByPhone(string Phone);   

        /// <summary>
        /// 查询证件号下的车辆
        /// </summary>
        /// <param name="pid">证件号</param>
        /// <returns>车辆列表</returns>
        IEnumerable<Car> CarsByPID(string pid);
        /// <summary>
        /// 查询用户名下的车辆
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>车辆列表</returns>
        IEnumerable<Car> CarsByUserId(string userId);

        IEnumerable<Car> SelectCarListByIdentityOld(string identityNumber);

        IEnumerable<Car> SelectCarstByIdentity(string identityNumber);
    }
}
