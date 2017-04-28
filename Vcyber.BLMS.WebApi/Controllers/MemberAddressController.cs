using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Application;

using System.Web.Http.Description;
using Vcyber.BLMS.WebApi.Common;
using Vcyber.BLMS.WebApi.Models;

namespace Vcyber.BLMS.WebApi.Controllers
{
    using Vcyber.BLMS.Entity.Common;
    using Vcyber.BLMS.WebApi.Filter;

    /// <summary>
    /// 会员常用地址
    /// </summary>
      [IOVAuthorize]
    public class MemberAddressController : ApiController
    {
        #region ==== 构造函数 ====

        public MemberAddressController() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 获取用户常用地址
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(List<AddressV>))]
        [Route("api/memberaddress/all")]
        public IHttpActionResult All(string userId)
        {
            AddressCollectionV dataResult = new AddressCollectionV();
            var address = _AppContext.AddressApp.GetList(userId);

            if (address != null && address.Count() > 0)
            {
                var tempData = address.Select(d => { return new AddressV() { CityID = d.City, CountyID = d.County, Detail = d.Detail, ID = d.ID, IsDefault = d.IsDefault, PCC = d.PCC, Phone = d.Phone, ProvinceID = d.Province, ReceiveName = d.ReceiveName, UserID = d.UserID, ZipCode = d.ZipCode }; });
                dataResult.Datas = tempData.ToList();
            }
            return this.Ok(new ReturnObject(dataResult.Datas));
            //return this.DataResult<AddressCollectionV>(dataResult, HttpStatusCode.OK);
        }

        /// <summary>
        /// 添加会员地址
        /// </summary>
        /// <param name="data">地址信息</param>
        /// <returns>地址记录Id</returns>
        [HttpPost]
        [Route("api/memberaddress/add")]
        [ResponseType(typeof(int))]
        public IHttpActionResult Add(AddressV data)
        {
            if (data == null || !data.ValidateData())
            {
                return this.Ok(new ReturnObject("00"));
            }

            Address address = new Address() { ZipCode = data.ZipCode, UserID = data.UserID, ReceiveName = data.ReceiveName, Province = data.ProvinceID, Phone = data.Phone, PCC = data.PCC, IsDefault = data.IsDefault, ID = data.ID, Detail = data.Detail, County = data.CountyID, City = data.CityID, CreateTime = DateTime.Now, UpdateTime = DateTime.Now, Datastate = 0 };

            try
            {

                return this.Ok(new ReturnObject("200", _AppContext.AddressApp.Add(address)));
            }
            catch (RepeatException repeat)
            {
                return this.Ok(new ReturnObject("91"));
            }
        }

        /// <summary>
        /// 设置默认地址
        /// </summary>
        /// <param name="addressId">地址Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>添加结果</returns>
        [HttpGet]
        [Route("api/memberaddress/setdefault")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult SetDefault(int addressId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return this.DataResult("00");
            }

            bool reslt = _AppContext.AddressApp.SetDefault(addressId, userId);
            string status = reslt ? "200" : "00";
            return this.Ok(new ReturnObject(status, reslt));
        }

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="addressId">地址Id</param>
        /// <returns>99:成功；00失败</returns>
        [HttpGet]
        [Route("api/memberaddress/delete")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult Delete(int addressId)
        {
            bool result = _AppContext.AddressApp.Delete(addressId);
            return this.Ok(new ReturnObject(result?"99":"00"));
        }

        /// <summary>
        /// 获取地址信息
        /// </summary>
        /// <param name="addressId">地址Id</param>
        /// <returns>200:存在；404：不存在</returns>
        [HttpGet]
        [Route("api/memberaddress/detali")]
        [ResponseType(typeof(AddressV))]
        public IHttpActionResult Detali(int addressId)
        {
            var d = _AppContext.AddressApp.GetOne(addressId);
            AddressV dataResult = null;

            if (d != null)
            {
                dataResult = new AddressV() { CityID = d.City, CountyID = d.County, Detail = d.Detail, ID = d.ID, IsDefault = d.IsDefault, PCC = d.PCC, Phone = d.Phone, ProvinceID = d.Province, ReceiveName = d.ReceiveName, UserID = d.UserID, ZipCode = d.ZipCode };
            }

            return this.Ok(new ReturnObject(d != null ? "200" : "404", dataResult));
        }

        #endregion
    }
}
