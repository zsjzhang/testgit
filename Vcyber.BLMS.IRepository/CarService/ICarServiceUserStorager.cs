using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.IRepository.CarService
{
    using Vcyber.BLMS.Entity;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.Entity.SelectCondition;

    public interface ICarServiceUserStorager
    {
        bool IsSonataUser(string userId);

        bool IsTlcUser(string identityNumber);

        int GetFreeServiceCount(string identity, EDMSServiceType service);
        List<FreeServiceRecord> GetFreeServiceRecord(string identity, EDMSServiceType serviceType);

        IEnumerable<Car> SelectCarListByUserId(string identityNumber);

        IEnumerable<Car> SelectCarListByIdentity(string identityNumber);

        IEnumerable<Car> SelectCarstByIdentity(string identityNumber);

        IFCustomer GetCustomer(string identityNumber);

        bool UpdateCarInfo(string vin, string LicencePlate, string Mileage);

        bool InsertCarInfoByVIN(string vin, string custId, string CarCategory, string Buytime);

        Car GetCarInfoByVIN(string vin);

        /// <summary>
        /// 查询中间表客户信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        IEnumerable<IFCustomer> FindCustomer(CustomerCondition condition);

        IEnumerable<IFCustomer> OldFindCustomer(CustomerCondition condition);
        /// <summary>
        /// 删除中间表客户信息
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        bool DelCustomer(CustomerCondition condition);

        bool OldIsExistIdentity(string identityNumber);

        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <param name="cusId"></param>
        /// <returns></returns>
        IFCustomer findCustomer(string cusId);

        /// <summary>
        /// 查询客户车辆信息
        /// </summary>
        /// <param name="cusId"></param>
        /// <returns></returns>
        IEnumerable<Car> findIfCar(string cusId);

        IEnumerable<Car> OldfindIfCar(string cusId);

        /// <summary>
        /// IFCustomer.CustId规则：JT-110222199912013115（集团-身份证号)
        /// 这是在该表中insert数据的唯一入口，所以CustId的规则一定
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IFCustomer CreateOrGetIFCustomer(IFCustomer customer);

        bool UpdateCarInfoByVIN(string vin, string custId);

        /// <summary>
        /// 是否存在相同的身份证号
        /// </summary>
        /// <param name="identityNumber"></param>
        /// <returns></returns>
        bool IsExistIdentity(string identityNumber);

        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="data"></param>
        void AddCustomer(IFCustomer data);

        LevelAndNo GetUserLevelByVin(string vin);

        bool AddActivityJoinRecord(string uname, string mobile, string aname);

        bool UpdateCarInfo2(string vin, string LicencePlate, string Mileage);

        EMemshipLevelWX GetMemshipLevelWX(string userId);

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

        Car OldGetCarInfoByVIN(string vin);

        IFCustomer OldfindCustomer(string cusId);

        IEnumerable<Car> SelectCarListByIdentityOld(string identityNumber);
    }
}
