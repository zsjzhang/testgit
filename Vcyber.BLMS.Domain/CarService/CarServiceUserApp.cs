using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Domain.CarService
{

    using Vcyber.BLMS.Application.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.IRepository;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.Entity;
    using Vcyber.BLMS.Entity.SelectCondition;

    public class CarServiceUserApp : ICarServiceUser
    {
        public bool IsSonataUser(string userId)
        {
            return _DbSession.CarServiceUserStorager.IsSonataUser(userId);
        }

        public int GetFreeServiceCount(string identity, EDMSServiceType service)
        {
            return _DbSession.CarServiceUserStorager.GetFreeServiceCount(identity, service);
        }
        public List<FreeServiceRecord> GetFreeServiceRecord(string identity, EDMSServiceType serviceType)
        {
            return _DbSession.CarServiceUserStorager.GetFreeServiceRecord(identity, serviceType);
        }

        public IEnumerable<Car> SelectCarListByUserId(string userId)
        {
            return _DbSession.CarServiceUserStorager.SelectCarListByUserId(userId);
        }

        public IEnumerable<Car> SelectCarListByIdentity(string identityNumber)
        {
            return _DbSession.CarServiceUserStorager.SelectCarListByIdentity(identityNumber);
        }

        public IEnumerable<Car> SelectCarstByIdentity(string identityNumber)
        {
            return _DbSession.CarServiceUserStorager.SelectCarstByIdentity(identityNumber);
        }

        public IEnumerable<Car> SelectCarListByIdentityOld(string identityNumber)
        {
            return _DbSession.CarServiceUserStorager.SelectCarListByIdentityOld(identityNumber);
        }

        public CarTypeReturnIntegral GetReIntegralTypeByIdentity(string identity)
        {
            var Alllist = SelectCarListByIdentity(identity);

            var afterAprilbuycar = Alllist.Where<Car>(c => c.BuyTime >= DateTime.Parse("2016-04-01") && c.BuyTime.Value.AddMonths(6).AddDays(1) > DateTime.Now).ToList();//AddDays(-90))
            var beforeApribuycar = Alllist.Where<Car>(c => c.BuyTime < DateTime.Parse("2016-04-01")).ToList();
            if (afterAprilbuycar != null && afterAprilbuycar.Count == 1 && beforeApribuycar != null && beforeApribuycar.Count > 0)
            {

                foreach (Car car in afterAprilbuycar)
                {

                    if (isDsCarType(car.CarCategory))
                    {
                        return CarTypeReturnIntegral.DSAdd;
                    }
                    else
                    {
                        return CarTypeReturnIntegral.NoDSAdd;
                    }

                }

            }

            if (afterAprilbuycar != null && afterAprilbuycar.Count == 1 && (beforeApribuycar == null || beforeApribuycar.Count == 0))
            {

                foreach (Car car in afterAprilbuycar)
                {

                    if (isDsCarType(car.CarCategory))
                    {
                        return CarTypeReturnIntegral.DS;
                    }
                    else
                    {
                        return CarTypeReturnIntegral.NoDS;
                    }

                }

            }

            if (afterAprilbuycar != null && afterAprilbuycar.Count > 1)
            {
                var ReIntegralType = CarTypeReturnIntegral.NoDSAdd;
                foreach (Car car in afterAprilbuycar)
                {

                    if (isDsCarType(car.CarCategory))
                    {
                        ReIntegralType = CarTypeReturnIntegral.DSAdd;
                        break;
                    }

                }
                return ReIntegralType;

            }

            //    if (_DbSession.DealerMembershipStorager.IsDsCarTypeByIdNumber(identity))//两次查询数据库不合理，协商一个合理的解决
            //    {
            //        return CarTypeReturnIntegral.DS;
            //    }
            //    else
            //    {
            //        return CarTypeReturnIntegral.NoDS;
            //    }
            //}
            //if (listcar != null && listcar.Count > 1)
            //{
            //    var returnvalue = CarTypeReturnIntegral.NoDSAdd;
            //    foreach (Car car in listcar)
            //    {
            //        if (_DbSession.DealerMembershipStorager.IsDsCarTypeByIdNumber(identity))
            //        {
            //            returnvalue = CarTypeReturnIntegral.DSAdd;
            //        }
            //    }
            //    return returnvalue;
            //}


            return CarTypeReturnIntegral.NoIntegral;


        }
        /// <summary>
        /// 根据身份证号判断注册并绑定车的用户是那种类型
        /// 0： D+S 首次 100元 返 4000积分  1：  D+S 增换 返7000积分  2： 非 D+S  首 50 元 返 2000积分 3 ： 增换 返4000积分
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="createtime"></param>
        /// <returns></returns>
        public CarTypeReturnIntegral GetRegisterIntegralTypeByIdentity(string identity, DateTime CreateTime)
        {
            var flag = false;
            List<Car> Vins = new List<Car>();
            //该身份证下所有的车辆
            var AllCarList = SelectCarListByIdentity(identity);

            //4月1号之后购车的数量
            var AfterBuyCar = AllCarList.Where(i => i.BuyTime >= Convert.ToDateTime("2016-04-01")).ToList();

            //4月1号之前购车的数量
            var BeforeBuyCar = AllCarList.Where(i => i.BuyTime < Convert.ToDateTime("2016-04-01")).ToList();

            //是否是90天内入会
            flag = AfterBuyCar.Count(i => i.BuyTime.Value.AddMonths(6).AddDays(1) > CreateTime) > 0;


            //4月1号之后购车并且是90天内入会符合缴费返积分
            if (AfterBuyCar.Count() > 0 && flag)
            {
                //4月1号买了一俩车
                if (AfterBuyCar.Count() == 1)
                {
                    Vins.Add(AfterBuyCar.ElementAtOrDefault(0));
                }
                else //4月1号买了多辆车
                {
                    //4月1号之后有多辆车，把满足需要加积分的车辆拿出来
                    var items = AfterBuyCar.Where(i => i.BuyTime.Value.AddMonths(6).AddDays(1) > CreateTime).OrderBy(i => i.BuyTime);
                    //4月1号之前有车
                    if (BeforeBuyCar.Count() >= 1)
                    {
                        foreach (var item in items)
                        {
                            item.Userintegral = (isDsCarType(item.CarCategory) ? "7000" : "4000");
                            Vins.Add(item);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < items.Count(); j++)
                        {
                            Vins.Add(items.ElementAtOrDefault(j));
                            if (AfterBuyCar.Count() == items.Count() && j == 0)
                            {
                                items.ElementAtOrDefault(0).Userintegral = (isDsCarType(items.ElementAtOrDefault(0).CarCategory) ? "4000" : "2000");
                                continue;
                            }

                            items.ElementAtOrDefault(j).Userintegral = (isDsCarType(items.ElementAtOrDefault(j).CarCategory) ? GetUserIntegral(CarTypeReturnIntegral.DSAdd, items.ElementAtOrDefault(j)) : GetUserIntegral(CarTypeReturnIntegral.NoDSAdd, items.ElementAtOrDefault(j))).ToString();
                        }
                    }
                }
            }
            //4.1号以后购车 并且4月1号之前就有车
            if (AfterBuyCar != null && AfterBuyCar.Count == 1 && BeforeBuyCar != null && BeforeBuyCar.Count > 0)
            {

                foreach (Car car in Vins)
                {

                    if (isDsCarType(car.CarCategory))
                    {
                        return CarTypeReturnIntegral.DSAdd;
                    }
                    else
                    {
                        return CarTypeReturnIntegral.NoDSAdd;
                    }

                }

            }
            //4.1号以后购车  购车辆等于1 并且 4月1号之前没有车
            if (AfterBuyCar != null && AfterBuyCar.Count == 1 && (BeforeBuyCar == null || BeforeBuyCar.Count == 0))
            {

                foreach (Car car in Vins)
                {

                    if (isDsCarType(car.CarCategory))
                    {
                        return CarTypeReturnIntegral.DS;
                    }
                    else
                    {
                        return CarTypeReturnIntegral.NoDS;
                    }

                }

            }
            //4.1号以后购车并且2辆或2辆以上
            if (AfterBuyCar != null && AfterBuyCar.Count > 1)
            {
                var ReIntegralType = CarTypeReturnIntegral.NoDSAdd;
                foreach (Car car in Vins)
                {

                    if (isDsCarType(car.CarCategory))
                    {
                        ReIntegralType = CarTypeReturnIntegral.DSAdd;
                        break;
                    }

                }
                return ReIntegralType;

            }

            return CarTypeReturnIntegral.NoIntegral;

        }


        /// <summary>
        /// 获取注册并且绑定车的用户缴费可获得的积分
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="CreateTime"></param>
        /// <returns>具体的积分值</returns>
        public int GetRegisterIntegralByIdentity(string identity, DateTime CreateTime/**讨论定下来申请时间就是缴费时间**/)
        {
            int Integral = 0;
            List<Car> Vins = new List<Car>();

            var flag = false;

            //该身份证下所有的车辆
            var AllCarList = SelectCarListByIdentity(identity);

            //4月1号之后购车的数量
            var AfterBuyCar = AllCarList.Where(i => i.BuyTime >= Convert.ToDateTime("2016-04-01"));

            //4月1号之前购车的数量
            var BeforeBuyCar = AllCarList.Where(i => i.BuyTime < Convert.ToDateTime("2016-04-01"));

            //是否是90天内入会
            flag = AfterBuyCar.Count(i => i.BuyTime.Value.AddMonths(6).AddDays(1) > CreateTime) > 0;


            //4月1号之后购车并且是90天内入会符合缴费返积分
            if (AfterBuyCar.Count() > 0 && flag)
            {
                //4月1号买了一俩车
                if (AfterBuyCar.Count() == 1)
                {
                    //4月1号之前有车并且4月1号购车是D+S的
                    if (BeforeBuyCar.Count() >= 1)
                    {
                        Integral = isDsCarType(AfterBuyCar.ElementAtOrDefault(0).CarCategory) ? GetUserIntegral(CarTypeReturnIntegral.DSAdd, AfterBuyCar.ElementAtOrDefault(0)) : GetUserIntegral(CarTypeReturnIntegral.NoDSAdd, AfterBuyCar.ElementAtOrDefault(0));
                    }
                    else
                    {
                        Integral = isDsCarType(AfterBuyCar.ElementAtOrDefault(0).CarCategory) ? GetUserIntegral(CarTypeReturnIntegral.DS, AfterBuyCar.ElementAtOrDefault(0)) : GetUserIntegral(CarTypeReturnIntegral.NoDS, AfterBuyCar.ElementAtOrDefault(0));
                    }
                    AfterBuyCar.ElementAtOrDefault(0).Userintegral = Integral + "";
                    Vins.Add(AfterBuyCar.ElementAtOrDefault(0));
                }
                else //4月1号买了多辆车
                {
                    //4月1号之后有多辆车，把满足需要加积分的车辆拿出来
                    var items = AfterBuyCar.Where(i => i.BuyTime.Value.AddMonths(6).AddDays(1) > CreateTime).OrderBy(i => i.BuyTime);
                    //4月1号之前有车
                    if (BeforeBuyCar.Count() >= 1)
                    {

                        foreach (var item in items)
                        {
                            item.Userintegral = (isDsCarType(item.CarCategory) ? GetUserIntegral(CarTypeReturnIntegral.DSAdd, item).ToString() : GetUserIntegral(CarTypeReturnIntegral.NoDSAdd, item).ToString());
                            Vins.Add(item);
                            Integral += isDsCarType(item.CarCategory) ? GetUserIntegral(CarTypeReturnIntegral.DSAdd, item) : GetUserIntegral(CarTypeReturnIntegral.NoDSAdd, item);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < items.Count(); j++)
                        {
                            Vins.Add(items.ElementAtOrDefault(j));
                            if (AfterBuyCar.Count() == items.Count() && j == 0)
                            {
                                items.ElementAtOrDefault(0).Userintegral = (isDsCarType(items.ElementAtOrDefault(0).CarCategory) ? GetUserIntegral(CarTypeReturnIntegral.DS, items.ElementAtOrDefault(j)).ToString() : GetUserIntegral(CarTypeReturnIntegral.NoDS, items.ElementAtOrDefault(j)).ToString());
                                Integral = isDsCarType(items.ElementAtOrDefault(0).CarCategory) ? GetUserIntegral(CarTypeReturnIntegral.DS, items.ElementAtOrDefault(j)) : GetUserIntegral(CarTypeReturnIntegral.NoDS, items.ElementAtOrDefault(j));
                                continue;
                            }

                            items.ElementAtOrDefault(j).Userintegral = (isDsCarType(items.ElementAtOrDefault(j).CarCategory) ? GetUserIntegral(CarTypeReturnIntegral.DSAdd, items.ElementAtOrDefault(j)) : GetUserIntegral(CarTypeReturnIntegral.NoDSAdd, items.ElementAtOrDefault(j))).ToString();

                            Integral += isDsCarType(items.ElementAtOrDefault(j).CarCategory) ? GetUserIntegral(CarTypeReturnIntegral.DSAdd, items.ElementAtOrDefault(j)) : GetUserIntegral(CarTypeReturnIntegral.NoDSAdd, items.ElementAtOrDefault(j));
                        }
                    }

                }

            }

            return Integral;
        }

        private int GetUserIntegral(CarTypeReturnIntegral type, Car car)
        {
            if (car.CarCategory == ConfigurationManager.AppSettings["CarCategory"].ToString() && type == CarTypeReturnIntegral.NoDS && car.BuyTime >= Convert.ToDateTime("2017-3-1") && car.BuyTime < Convert.ToDateTime("2017-5-1"))
            {
                return 4000;
            }
            if (car.CarCategory == ConfigurationManager.AppSettings["CarCategory"].ToString() && type == CarTypeReturnIntegral.NoDSAdd && car.BuyTime >= Convert.ToDateTime("2017-3-1") && car.BuyTime < Convert.ToDateTime("2017-5-1"))
            {
                return 8000;
            }

            if (car.CarCategory == ConfigurationManager.AppSettings["NewCarCategory"].ToString() && type == CarTypeReturnIntegral.NoDS && car.BuyTime >= Convert.ToDateTime("2017-4-1") && car.BuyTime < Convert.ToDateTime("2017-5-1"))
            {
                return 4000;
            }
            if (car.CarCategory == ConfigurationManager.AppSettings["NewCarCategory"].ToString() && type == CarTypeReturnIntegral.NoDSAdd && car.BuyTime >= Convert.ToDateTime("2017-4-1") && car.BuyTime < Convert.ToDateTime("2017-5-1"))
            {
                return 8000;
            }

            switch (type)
            {
                case CarTypeReturnIntegral.DS:
                case CarTypeReturnIntegral.NoDSAdd:
                    return 4000;
                case CarTypeReturnIntegral.NoDS:
                    return 2000;
                case CarTypeReturnIntegral.DSAdd:
                    return 7000;
                default:
                    return 0;
            }
        }


        private static IList<string> dsCarType = new List<string>()
        {
            "ix25",
            "ix35",
            "全新胜达",
            "全新途胜",
            "索纳塔8",
            "索纳塔9",
            "名图",
            "第九代索纳塔",
            "名驭",
            "御翔",
            "领翔",
            "改款全新胜达",
            "索纳塔9混动版",
            "索纳塔9（混合动力）",
            "索9",
            "索九",
            "索八",
            "IX25",
            "IX35"
        };


        private bool isDsCarType(string carCategory)
        {
            if (dsCarType.Contains(carCategory))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public IFCustomer GetCustomer(string identityNumber)
        {
            return _DbSession.CarServiceUserStorager.GetCustomer(identityNumber);
        }

        public bool UpdateCarInfo(string vin, string LicencePlate, string Mileage)
        {
            return _DbSession.CarServiceUserStorager.UpdateCarInfo(vin, LicencePlate, Mileage);
        }


        public Car GetCarInfoByVIN(string vin)
        {
            return _DbSession.CarServiceUserStorager.GetCarInfoByVIN(vin);
        }



        /// <summary>
        /// 查询中间表客户信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public IEnumerable<IFCustomer> FindCustomer(CustomerCondition condition)
        {
            return _DbSession.CarServiceUserStorager.FindCustomer(condition);
        }

        /// <summary>
        /// 查询中间表客户信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="cars">客户车辆信息</param>
        /// <returns></returns>
        public IEnumerable<IFCustomer> FindCustomer(CustomerCondition condition, out List<Car> cars)
        {
            cars = new List<Car>(5);
            var datas = _DbSession.CarServiceUserStorager.FindCustomer(condition);

            if (datas != null && datas.Count() > 0)
            {
                foreach (var cusItem in datas)
                {
                    var tempData = _DbSession.CarServiceUserStorager.findIfCar(cusItem.CustId);

                    if (tempData != null)
                    {
                        foreach (var temp in tempData)
                        {
                            cars.Add(temp);
                        }
                    }
                }
            }

            return datas;
        }

        public IEnumerable<IFCustomer> OldFindCustomer(CustomerCondition condition, out List<Car> cars)
        {
            cars = new List<Car>(5);
            var datas = _DbSession.CarServiceUserStorager.OldFindCustomer(condition);

            if (datas != null && datas.Count() > 0)
            {
                foreach (var cusItem in datas)
                {
                    var tempData = _DbSession.CarServiceUserStorager.OldfindIfCar(cusItem.CustId);

                    if (tempData != null)
                    {
                        foreach (var temp in tempData)
                        {
                            cars.Add(temp);
                        }
                    }
                }
            }

            return datas;
        }
        /// <summary>
        /// 删除中间表客户信息
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool DelCustomer(CustomerCondition condition)
        {
            return _DbSession.CarServiceUserStorager.DelCustomer(condition);
        }
        /// <summary>
        /// 根据Vin查询车辆信息
        /// </summary>
        /// <param name="vin">vin</param>
        /// <param name="customer">车主信息</param>
        /// <returns></returns>
        public Car GetCarInfoByVIN(string vin, out IFCustomer customer)
        {
            customer = new IFCustomer();
            var car = _DbSession.CarServiceUserStorager.GetCarInfoByVIN(vin);

            if (car != null)
            {
                customer = _DbSession.CarServiceUserStorager.findCustomer(car.CustId);
            }

            return car;
        }

        public Car OldGetCarInfoByVIN(string vin, out IFCustomer customer)
        {
            customer = new IFCustomer();
            var car = _DbSession.CarServiceUserStorager.OldGetCarInfoByVIN(vin);

            if (car != null)
            {
                customer = _DbSession.CarServiceUserStorager.OldfindCustomer(car.CustId);
            }

            return car;
        }



        public IFCustomer CreateOrGetIFCustomer(IFCustomer customer)
        {
            return _DbSession.CarServiceUserStorager.CreateOrGetIFCustomer(customer);
        }

        public bool UpdateCarInfoByVIN(string vin, string custId)
        {
            return _DbSession.CarServiceUserStorager.UpdateCarInfoByVIN(vin, custId);
        }

        public bool InsertCarInfoByVIN(string vin, string custId, string CarCategory, string Buytime)
        {
            return _DbSession.CarServiceUserStorager.InsertCarInfoByVIN(vin, custId, CarCategory, Buytime);
        }

        /// <summary>
        /// 是否存在相同的身份证号
        /// </summary>
        /// <param name="identityNumber"></param>
        /// <returns></returns>
        public bool IsExistIdentity(string identityNumber)
        {
            return _DbSession.CarServiceUserStorager.IsExistIdentity(identityNumber);
        }

        public bool OldIsExistIdentity(string identityNumber)
        {
            return _DbSession.CarServiceUserStorager.OldIsExistIdentity(identityNumber);
        }

        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="data"></param>
        public void AddCustomer(IFCustomer data)
        {
            _DbSession.CarServiceUserStorager.AddCustomer(data);
        }

        public LevelAndNo GetUserLevelByVin(string vin)
        {
            return _DbSession.CarServiceUserStorager.GetUserLevelByVin(vin);
        }

        public bool AddActivityJoinRecord(string uname, string mobile, string aname)
        {
            return _DbSession.CarServiceUserStorager.AddActivityJoinRecord(uname, mobile, aname);
        }

        public EMemshipLevelWX GetMemshipLevelWX(string userId)
        {
            return _DbSession.CarServiceUserStorager.GetMemshipLevelWX(userId);
        }

        public IEnumerable<CSTestDrive> GetCSTestDrive(string userid)
        {
            return _DbSession.CarServiceUserStorager.GetCSTestDrive(userid);
        }
        public bool UpdateCarInfo2(string vin, string LicencePlate, string Mileage)
        {
            return _DbSession.CarServiceUserStorager.UpdateCarInfo2(vin, LicencePlate, Mileage);
        }
        public IEnumerable<CSTestDrive> GetCSTestDriveDetial(string OrderNo)
        {
            return _DbSession.CarServiceUserStorager.GetCSTestDriveDetial(OrderNo);
        }
        public IEnumerable<CSMaintenance> GetCSMaintenance(string userid)
        {
            return _DbSession.CarServiceUserStorager.GetCSMaintenance(userid);

        }
        public IEnumerable<CSMaintenance> GetCSMaintenanceDetial(string OrderNo)
        {
            return _DbSession.CarServiceUserStorager.GetCSMaintenanceDetial(OrderNo);
        }

        public IEnumerable<GetVinCustTimeEF> GetVinCustTime()
        {
            return _DbSession.CarServiceUserStorager.GetVinCustTime();
        }
        public IEnumerable<CSTestDrive> GetCSTestDriveByPhone(string Phone)
        {
            return _DbSession.CarServiceUserStorager.GetCSTestDriveByPhone(Phone);
        }




        /// <summary>
        /// 用户购车缴费返积分 D+s车型和非D+S增换购返4000 非D+S首次购车返2000 D+s换购返7000
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public int GetIntegrationByBuyCarPayMoney(string identity)
        {
            var type = this.GetReIntegralTypeByIdentity(identity);
            switch (type)
            {
                case CarTypeReturnIntegral.DS:
                case CarTypeReturnIntegral.NoDSAdd:
                    return 4000;
                case CarTypeReturnIntegral.NoDS:
                    return 2000;
                case CarTypeReturnIntegral.DSAdd:
                    return 7000;
                default:
                    return 0;
            }
        }


        //【缴费获取积分】
        public int GetJoinIntegralByIdentity(string identity, DateTime CreateTime/**讨论定下来申请时间就是缴费时间**/, List<Car> Vins)
        {
            int Integral = 0;

            var flag = false;

            //该身份证下所有的车辆
            var AllCarList = SelectCarListByIdentity(identity);

            //4月1号之后购车的数量
            var AfterBuyCar = AllCarList.Where(i => i.BuyTime >= Convert.ToDateTime("2016-04-01"));

            //4月1号之前购车的数量
            var BeforeBuyCar = AllCarList.Where(i => i.BuyTime < Convert.ToDateTime("2016-04-01"));

            //是否是90天内入会
            flag = AfterBuyCar.Count(i => i.BuyTime.Value.AddMonths(6).AddDays(1) > CreateTime) > 0;


            //4月1号之后购车并且是90天内入会符合缴费返积分
            if (AfterBuyCar.Count() > 0 && flag)
            {
                //4月1号买了一俩车
                if (AfterBuyCar.Count() == 1)
                {
                    //4月1号之前有车并且4月1号购车是D+S的
                    if (BeforeBuyCar.Count() >= 1)
                    {
                        Integral = isDsCarType(AfterBuyCar.ElementAtOrDefault(0).CarCategory)
                            ? GetUserIntegral(CarTypeReturnIntegral.DSAdd, AfterBuyCar.ElementAtOrDefault(0))
                            : GetUserIntegral(CarTypeReturnIntegral.NoDSAdd, AfterBuyCar.ElementAtOrDefault(0));
                    }
                    else
                    {
                        Integral = isDsCarType(AfterBuyCar.ElementAtOrDefault(0).CarCategory)
                            ? GetUserIntegral(CarTypeReturnIntegral.DS, AfterBuyCar.ElementAtOrDefault(0))
                            : GetUserIntegral(CarTypeReturnIntegral.NoDS, AfterBuyCar.ElementAtOrDefault(0));
                    }
                    AfterBuyCar.ElementAtOrDefault(0).Userintegral = Integral + "";
                    Vins.Add(AfterBuyCar.ElementAtOrDefault(0));
                }
                else //4月1号买了多辆车
                {
                    //4月1号之后有多辆车，把满足需要加积分的车辆拿出来
                    var items = AfterBuyCar.Where(i => i.BuyTime.Value.AddMonths(6).AddDays(1) > CreateTime).OrderBy(i => i.BuyTime);
                    //4月1号之前有车
                    if (BeforeBuyCar.Count() >= 1)
                    {

                        foreach (var item in items)
                        {
                            item.Userintegral = (isDsCarType(item.CarCategory) ? GetUserIntegral(CarTypeReturnIntegral.DSAdd, item).ToString() : GetUserIntegral(CarTypeReturnIntegral.NoDSAdd, item).ToString());
                            Vins.Add(item);
                            Integral += isDsCarType(item.CarCategory)
                                ? GetUserIntegral(CarTypeReturnIntegral.DSAdd, item)
                                : GetUserIntegral(CarTypeReturnIntegral.NoDSAdd, item);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < items.Count(); j++)
                        {
                            Vins.Add(items.ElementAtOrDefault(j));
                            if (AfterBuyCar.Count() == items.Count() && j == 0)
                            {
                                items.ElementAtOrDefault(0).Userintegral =
                                    (isDsCarType(items.ElementAtOrDefault(0).CarCategory) ? GetUserIntegral(CarTypeReturnIntegral.DS, items.ElementAtOrDefault(j)).ToString() : GetUserIntegral(CarTypeReturnIntegral.NoDS, items.ElementAtOrDefault(j)).ToString());
                                Integral = isDsCarType(items.ElementAtOrDefault(0).CarCategory) ? GetUserIntegral(CarTypeReturnIntegral.DS, items.ElementAtOrDefault(j)) : GetUserIntegral(CarTypeReturnIntegral.NoDS, items.ElementAtOrDefault(j));
                                continue;
                            }

                            items.ElementAtOrDefault(j).Userintegral =
                                (isDsCarType(items.ElementAtOrDefault(j).CarCategory)
                                    ? GetUserIntegral(CarTypeReturnIntegral.DSAdd, items.ElementAtOrDefault(j))
                                    : GetUserIntegral(CarTypeReturnIntegral.NoDSAdd, items.ElementAtOrDefault(j))).ToString();

                            Integral += isDsCarType(items.ElementAtOrDefault(j).CarCategory)
                                ? GetUserIntegral(CarTypeReturnIntegral.DSAdd, items.ElementAtOrDefault(j))
                                : GetUserIntegral(CarTypeReturnIntegral.NoDSAdd, items.ElementAtOrDefault(j));
                        }
                    }
                }
            }
            return Integral;
        }

        //【入会返积分】
        //【新规则获取积分】
        public int GetIntegralByIdentity(string identity, string IsPay, List<Car> Vins)
        {
            int Integral = 0;


            var flag = false;

            //该身份证下所有的车辆
            var AllCarList = SelectCarListByIdentity(identity);

            //4月1号之后购车的数量
            var AfterBuyCar = AllCarList.Where(i => i.BuyTime >= Convert.ToDateTime("2016-04-01"));

            //4月1号之前购车的数量
            var BeforeBuyCar = AllCarList.Where(i => i.BuyTime < Convert.ToDateTime("2016-04-01"));

            //是否是6个月内入会
            flag = AfterBuyCar.Count(i => i.BuyTime.Value.AddMonths(6).AddDays(1) > DateTime.Now) > 0;


            //4月1号之后购车并且是90天内入会符合缴费返积分
            if (AfterBuyCar.Count() > 0 && flag && IsPay == "1")
            {
                //4月1号买了一俩车
                if (AfterBuyCar.Count() == 1)
                {
                    //4月1号之前有车并且4月1号购车是D+S的
                    if (BeforeBuyCar.Count() >= 1)
                    {
                        Integral = isDsCarType(AfterBuyCar.ElementAtOrDefault(0).CarCategory)
                            ? GetUserIntegral(CarTypeReturnIntegral.DSAdd, AfterBuyCar.ElementAtOrDefault(0))
                            : GetUserIntegral(CarTypeReturnIntegral.NoDSAdd, AfterBuyCar.ElementAtOrDefault(0));
                    }
                    else
                    {
                        Integral = isDsCarType(AfterBuyCar.ElementAtOrDefault(0).CarCategory)
                            ? GetUserIntegral(CarTypeReturnIntegral.DS, AfterBuyCar.ElementAtOrDefault(0))
                            : GetUserIntegral(CarTypeReturnIntegral.NoDS, AfterBuyCar.ElementAtOrDefault(0));
                    }
                    AfterBuyCar.ElementAtOrDefault(0).Userintegral = Integral + "";
                    Vins.Add(AfterBuyCar.ElementAtOrDefault(0));
                }
                else //4月1号买了多辆车
                {
                    //4月1号之后有多辆车，把满足需要加积分的车辆拿出来
                    var items = AfterBuyCar.Where(i => i.BuyTime.Value.AddMonths(6).AddDays(1) > DateTime.Now).OrderBy(i => i.BuyTime);
                    //4月1号之前有车
                    if (BeforeBuyCar.Count() >= 1)
                    {

                        foreach (var item in items)
                        {
                            item.Userintegral = (isDsCarType(item.CarCategory) ? GetUserIntegral(CarTypeReturnIntegral.DSAdd, item).ToString() : GetUserIntegral(CarTypeReturnIntegral.NoDSAdd, item).ToString());
                            Vins.Add(item);
                            Integral += isDsCarType(item.CarCategory)
                                ? GetUserIntegral(CarTypeReturnIntegral.DSAdd, item)
                                : GetUserIntegral(CarTypeReturnIntegral.NoDSAdd, item);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < items.Count(); j++)
                        {
                            Vins.Add(items.ElementAtOrDefault(j));
                            if (AfterBuyCar.Count() == items.Count() && j == 0)
                            {
                                items.ElementAtOrDefault(0).Userintegral =
                                    (isDsCarType(items.ElementAtOrDefault(0).CarCategory) ? GetUserIntegral(CarTypeReturnIntegral.DS, items.ElementAtOrDefault(j)).ToString() : GetUserIntegral(CarTypeReturnIntegral.NoDS, items.ElementAtOrDefault(j)).ToString());
                                Integral = isDsCarType(items.ElementAtOrDefault(0).CarCategory) ? GetUserIntegral(CarTypeReturnIntegral.DS, items.ElementAtOrDefault(j)) : GetUserIntegral(CarTypeReturnIntegral.NoDS, items.ElementAtOrDefault(j));
                                continue;
                            }

                            items.ElementAtOrDefault(j).Userintegral =
                                (isDsCarType(items.ElementAtOrDefault(j).CarCategory)
                                    ? GetUserIntegral(CarTypeReturnIntegral.DSAdd, items.ElementAtOrDefault(j))
                                    : GetUserIntegral(CarTypeReturnIntegral.NoDSAdd, items.ElementAtOrDefault(j))).ToString();

                            Integral += isDsCarType(items.ElementAtOrDefault(j).CarCategory)
                                ? GetUserIntegral(CarTypeReturnIntegral.DSAdd, items.ElementAtOrDefault(j))
                                : GetUserIntegral(CarTypeReturnIntegral.NoDSAdd, items.ElementAtOrDefault(j));
                        }
                    }
                }
            }
            return Integral;
        }

        public int GetIntegrationByBuyCarPayMoneyJoinMember1(string identity)
        {
            var type = this.GetReIntegralTypeByIdentity(identity);
            switch (type)
            {
                case CarTypeReturnIntegral.DS: return 100;
                case CarTypeReturnIntegral.NoDSAdd: return 50;
                case CarTypeReturnIntegral.DSAdd: return 100;
                case CarTypeReturnIntegral.NoDS:
                    return 50;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// 查询证件号下的车辆
        /// </summary>
        /// <param name="pid">证件号</param>
        /// <returns>车辆列表</returns>
        public IEnumerable<Car> CarsByPID(string pid)
        {
            return _DbSession.CarServiceUserStorager.CarsByPID(pid);
        }
        /// <summary>
        /// 查询用户名下的车辆
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>车辆列表</returns>
        public IEnumerable<Car> CarsByUserId(string userId)
        {
            return _DbSession.CarServiceUserStorager.CarsByUserId(userId);
        }
    }
}

