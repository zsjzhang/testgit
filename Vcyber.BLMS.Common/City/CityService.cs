using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;
using System.Web;

namespace Vcyber.BLMS.Common.City
{
    /// <summary>
    /// 省市级联
    /// </summary>
    public class CityService
    {
        #region ==== 私有字段 ====

        private static readonly CityService instance = new CityService();

        private ProvincesList provinecs = null;

        private Cities citys = null;

        private Areas areas = null;

        #endregion

        #region ==== 构造函数 ====

        private CityService()
        {
            this.provinecs = this.DeserializeXml<ProvincesList>(HttpContext.Current.Server.MapPath("~") + "/bin/City/Province.xml");
            this.citys = this.DeserializeXml<Cities>(HttpContext.Current.Server.MapPath("~") + "/bin/City/city.xml");
            this.areas = this.DeserializeXml<Areas>(HttpContext.Current.Server.MapPath("~") + "/bin/City/area.xml");
        }

        #endregion

        #region ==== 公共属性 ====

        public static CityService Instance
        {
            get { return instance; }
        }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 获取全部省信息
        /// </summary>
        /// <returns></returns>
        public List<Provinces> GetProvince()
        {
            List<Provinces> dataResult = new List<Provinces>();

            foreach (var item in this.provinecs.ProvinceList)
            {
                Provinces data = new Provinces() { Code = item.Code, EnName = item.EnName, ID = item.ID, Letter = item.Letter, Name = item.Name, ShortName = item.ShortName };
                dataResult.Add(data);
            }

            return dataResult;
        }

        /// <summary>
        /// 根据省code获取市
        /// </summary>
        /// <param name="provinceCode"></param>
        /// <returns></returns>
        public List<City> GetCity(string provinceCode)
        {
            var citys = this.citys.CityList.Where<City>((d) => { return d.ProvinceCode.Equals(provinceCode); });
            return citys != null && citys.Count() > 0 ? citys.ToList<City>() : new List<City>();
        }

        public List<City> GetCityName(string provinceName)
        {
            var provincesDatas = this.provinecs.ProvinceList.Where<Provinces>((p) => { return p.Name.Equals(provinceName); });

            if (provincesDatas != null && provincesDatas.Count() > 0)
            {
                return this.GetCity(provincesDatas.ToList<Provinces>()[0].Code);
            }

            return new List<City>(1);
        }

        /// <summary>
        ///根据市code获取区县
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        public List<Area> GetArea(string cityCode)
        {
            var areas = this.areas.AreaList.Where<Area>((d) => { return d.Citycode.Equals(cityCode); });
            return areas != null && areas.Count() > 0 ? areas.ToList<Area>() : new List<Area>();
        }

        #endregion

        #region ==== 私有方法 ====

        private T DeserializeXml<T>(string filePath) where T : class
        {
            try
            {
                FileStream fileSteam = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                XmlSerializer xml = new XmlSerializer(typeof(T));
                T data = xml.Deserialize(fileSteam) as T;
                fileSteam.Close();
                return data;
            }
            catch (Exception ex)
            {
                LogService.Instance.Error(ex.Message, ex);
            }

            return null;
        }

        #endregion
    }
}
