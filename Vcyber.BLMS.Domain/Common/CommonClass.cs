using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class CommonClass
    {
        //public static List<BusinessType> GetBusinessType(InorOutType inorOutType)
        //{

        //    List<BusinessType> businessTypes = new List<BusinessType>();

        //    switch (inorOutType)
        //    {
        //        case InorOutType.All:
        //            businessTypes.Add(BusinessType.Default);
        //            break;
        //        case InorOutType.In:
        //            businessTypes.Add(BusinessType.Distribute);
        //            businessTypes.Add(BusinessType.Drawback);
        //            break;
        //        case InorOutType.Out:
        //            businessTypes.Add(BusinessType.Expense);
        //            break;
        //        default:
        //            businessTypes.Add(BusinessType.Default);
        //            break;
        //    }

        //    return businessTypes;
        //}

        //public static List<InorOutType> GetInorOutType(BusinessType businessType)
        //{

        //    List<InorOutType> inorOutTypes = new List<InorOutType>();
        //    switch (businessType)
        //    {
        //        case BusinessType.Default:
        //            inorOutTypes.Add(InorOutType.All);
        //            break;
        //        case BusinessType.Distribute:
        //            inorOutTypes.Add(InorOutType.In);
        //            break;
        //        case BusinessType.Expense:
        //            inorOutTypes.Add(InorOutType.Out);
        //            break;
        //        case BusinessType.Drawback:
        //            inorOutTypes.Add(InorOutType.In);
        //            break;

        //        default:
        //            inorOutTypes.Add(InorOutType.All);
        //            break;
        //    }

        //    return inorOutTypes;
        //}

    }


    public class CommonDictionary : ICommonDictionary
    {
        private string _type = string.Empty;

        public CommonDictionary()
        {

        }
        //public CommonDictionary(string type)
        //{
        //    this._type = type;
        //}

        //public Dictionary<string, string> GetDictionary(string code = null)
        //{
        //    IEnumerable<Entity.Dictionary> dics = null;

        //    if (string.IsNullOrEmpty(code))
        //    {
        //        dics = _DbSession.Dictionary.Select(this._type);
        //    }
        //    else
        //    {
        //        dics = _DbSession.Dictionary.Select(this._type, code);
        //    }

        //    Dictionary<string, string> _dic = new Dictionary<string, string>();

        //    if (dics != null)
        //    {
        //        foreach (var d in dics)
        //        {
        //            if (!_dic.ContainsKey(d.Code))
        //                _dic.Add(d.Code, d.Name);
        //        }
        //    }

        //    return _dic;
        //}

        public IEnumerable<Entity.Dictionary> GetDictionary(string type, string code = null)
        {
            if (string.IsNullOrEmpty(code))
            {
                return _DbSession.Dictionary.Select(type);
            }
            else
            {
                return _DbSession.Dictionary.Select(type, code);
            }

        }

    }
}
