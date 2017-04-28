using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Vcyber.BLMS.Common.City;
using Vcyber.BLMS.WebApi.Models;
using Vcyber.BLMS.WebApi.Common;
using System.Web.Http.Description;

namespace Vcyber.BLMS.WebApi.Controllers
{
    using Vcyber.BLMS.Entity.Common;
    using System.Web;

    /// <summary>
    /// 省市级联
    /// </summary>
    public class ProvincesCityController : ApiController
    {
        #region ==== 构造函数 ====

        public ProvincesCityController() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 获取省信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(List<ProvincesV>))]
        [Route("api/provincescity/provinces")]
        public IHttpActionResult Provinces()
        {

           
            ProvincesCollectionV dataResult = new ProvincesCollectionV();
            var pros = CityService.Instance.GetProvince();
            var tempData = pros.Select<Provinces, ProvincesV>((d) => { return new ProvincesV() { Code = d.Code, Name = d.Name, ID = d.ID }; });
            dataResult.Datas = tempData != null ? tempData.ToList<ProvincesV>() : new List<ProvincesV>(1);
            return this.Ok(new ReturnObject(dataResult.Datas));
        }

        /// <summary>
        /// 获取市信息
        /// </summary>
        /// <param name="provincesCode">省Code</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(List<CityV>))]
        [Route("api/provincescity/city")]
        public IHttpActionResult City(string provincesCode)
        {
            CityCollectionV dataResult = new CityCollectionV();
            var citys = CityService.Instance.GetCity(provincesCode);

            if (citys != null && citys.Count() > 0)
            {
                var tempData = citys.Select<City, CityV>((d) => { return new CityV() { ID = d.ID, Name = d.Name, Code = d.Code }; });
                dataResult.Datas = tempData.ToList<CityV>();
            }
            return this.Ok(new ReturnObject(dataResult.Datas));
            //return this.DataResult<CityCollectionV>(dataResult, HttpStatusCode.OK);
        }

        /// <summary>
        /// 区/县信息
        /// </summary>
        /// <param name="cityCode">市Code</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(List<AreaV>))]
        [Route("api/provincescity/area")]
        public IHttpActionResult Area(string cityCode)
        {
            AreaCollectionV dataResult = new AreaCollectionV();
            var areas = CityService.Instance.GetArea(cityCode);

            if (areas != null && areas.Count() > 0)
            {
                var tempData = areas.Select<Area, AreaV>((d) => { return new AreaV() { ID = d.ID, Name = d.Name, Code = d.Code }; });
                dataResult.Datas = tempData.ToList<AreaV>();
            }
            return this.Ok(new ReturnObject(dataResult.Datas));
            //return this.DataResult<AreaCollectionV>(dataResult,HttpStatusCode.OK);
        }

        #endregion
    }
}
