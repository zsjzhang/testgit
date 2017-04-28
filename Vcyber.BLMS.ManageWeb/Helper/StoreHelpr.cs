using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.ManageWeb.Helper
{
    public class StoreHelpr
    {

        #region ==== 商品 ====

        public static List<SelectListItem> GetVirtualType()
        {
            List<SelectListItem> list = new List<SelectListItem>() { 
                new SelectListItem { Text = "实体", Value = "0", Selected = true },
                new SelectListItem { Text = "虚拟", Value = "1", Selected = false },
                new SelectListItem { Text = "服务", Value = "2", Selected = false }
            };
            return list;
        }

        public static string GetVirtualName(int? type)
        {
            string name;
            switch (type)
            {
                case 0:
                    name = "实体"; break;
                case 1:
                    name = "虚拟"; break;
                case 2:
                    name = "服务"; break;
                default:
                    name = "";
                    break;
            }
            return name;
        }

        public static List<SelectListItem> GetProductCategory()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "全部", Value = "0", Selected = true });

            var categorys = _AppContext.CategoryApp.GetList();

            if (categorys != null && categorys.Count() > 0)
            {
                foreach (var item in categorys)
                {
                    SelectListItem listItem = new SelectListItem() { Text = item.Name, Value = item.ID.ToString(), Selected = false };
                    list.Add(listItem);
                }
            }

            return list;
        }

        public static List<SelectListItem> GetRecommend()
        {
            List<SelectListItem> list = new List<SelectListItem>() {
                new SelectListItem { Text = "全部", Value = "0", Selected = true },
                new SelectListItem { Text = "新品", Value =EProductRecommend.XP.ToInt32().ToString(), Selected = false },
                new SelectListItem { Text = "热销", Value = EProductRecommend.RX.ToInt32().ToString(), Selected = false }
            };
            return list;
        }

        public static List<SelectListItem> GetProductState()
        {
            List<SelectListItem> list = new List<SelectListItem>() {
                new SelectListItem { Text = "全部", Value = "-1", Selected = true },
                new SelectListItem { Text = "下架", Value = "0", Selected = false },
                new SelectListItem { Text = "上架", Value = "1", Selected = false }
            };
            return list;
        }

        public static string GetProductStateName(int? state)
        {
            string name;
            switch (state)
            {
                case 0:
                    name = "下架"; break;
                case 1:
                    name = "上架"; break;
                default:
                    name = "";
                    break;
            }
            return name;
        }

        public static string GetOrderStateName(int state)
        {
            string name;
            switch (state)
            {
                case 1:
                    name = "待付款"; break;
                case 2:
                    name = "已取消"; break;
                case 3:
                    name = "待发货"; break;
                case 4:
                    name = "已发货"; break;
                case 5:
                    name = "已完成"; break;
                case 6:
                    name = "待审核"; break;
                case 7:
                    name = "退货中"; break;
                case 8:
                    name = "已退货"; break;
                default:
                    name = "";
                    break;
            }
            return name;
        }

        public static List<SelectListItem> GetOrderType()
        {
            List<SelectListItem> list = new List<SelectListItem>() { 
                new SelectListItem { Text = "", Value = "0", Selected = true },
                new SelectListItem { Text = "金钱", Value = "1", Selected = false },
                new SelectListItem { Text = "积分", Value = "2", Selected = false },
                new SelectListItem { Text = "奖品", Value = "3", Selected = false }
            };
            return list;
        }

        public static string GetOrderTypeName(int state)
        {
            string name;
            switch (state)
            {
                case 1:
                    name = "金钱订单"; break;
                case 2:
                    name = "积分订单"; break;
                case 3:
                    name = "奖品订单"; break;
                default:
                    name = "";
                    break;
            }
            return name;
        }

        public static List<SelectListItem> GetShippingType()
        {
            List<SelectListItem> list = new List<SelectListItem>() { 
                new SelectListItem { Text = "EMS", Value = "1", Selected = true },
                new SelectListItem { Text = "顺丰", Value = "2", Selected = false },
                new SelectListItem { Text = "申通", Value = "3", Selected = false },
                new SelectListItem { Text = "圆通", Value = "4", Selected = false },
                new SelectListItem { Text = "中通", Value = "5", Selected = false },
                new SelectListItem { Text = "汇通", Value = "6", Selected = false },
                new SelectListItem { Text = "韵达", Value = "7", Selected = false },
                new SelectListItem { Text = "天天快递", Value = "8", Selected = false },
                new SelectListItem { Text = "上门自提", Value = "9", Selected = false }
            };
            return list;
        }

        public static string GetShippingName(int? type)
        {
            string name;
            switch (type)
            {
                case 1:
                    name = "EMS"; break;
                case 2:
                    name = "顺丰"; break;
                case 3:
                    name = "申通"; break;
                case 4:
                    name = "圆通"; break;
                case 5:
                    name = "中通"; break;
                case 6:
                    name = "汇通"; break;
                case 7:
                    name = "韵达"; break;
                case 8:
                    name = "天天快递"; break;
                case 9:
                    name = "上门自提"; break;
                default:
                    name = "";
                    break;
            }
            return name;
        }


        public static List<SelectListItem> GetBackType()
        {
            List<SelectListItem> list = new List<SelectListItem>(){
                new SelectListItem { Text = "", Value = "0", Selected = true },
                new SelectListItem { Text = "退货", Value = "1", Selected = false },
                new SelectListItem { Text = "换货", Value = "2", Selected = false },
                new SelectListItem { Text = "报修", Value = "3", Selected = false }
            };
            return list;
        }

        public static string GetBackTypeName(int? type)
        {
            string name;
            switch (type)
            {
                case 1:
                    name = "退货"; break;
                case 2:
                    name = "换货"; break;
                case 3:
                    name = "报修"; break;
                default:
                    name = "";
                    break;
            }
            return name;
        }

        //public static List<SelectListItem> OrderTradeType()
        //{
        //    List<SelectListItem> list = new List<SelectListItem>() { 
            
        //    new SelectListItem{Text="全部",Value="-1",Selected=true},
        //    new SelectListItem{Text="待发货",Value="17",Selected=false},
        //     new SelectListItem{Text="已发货",Value="6",Selected=false},
        //     new SelectListItem{Text="待付款",Value="1",Selected=false},
        //     new SelectListItem{Text="交易完成",Value="2",Selected=false},
        //     new SelectListItem{Text="部分退款",Value="3",Selected=false},
        //     new SelectListItem{Text="全部退款",Value="4",Selected=false},
        //      new SelectListItem{Text="交易取消",Value="5",Selected=false}
        //    };

        //    return list;
        //}



        public static List<SelectListItem> GetOrderTrade()
        {
            List<SelectListItem> list = new List<SelectListItem>() { 
            
            new SelectListItem{Text="全部",Value="-1",Selected=true},
            new SelectListItem{Text="待发货",Value="17",Selected=false},
             new SelectListItem{Text="已发货",Value="6",Selected=false},
             new SelectListItem{Text="待付款",Value="1",Selected=false},
             new SelectListItem{Text="交易完成",Value="2",Selected=false},
             new SelectListItem{Text="部分退款",Value="3",Selected=false},
             new SelectListItem{Text="全部退款",Value="4",Selected=false},
              new SelectListItem{Text="交易取消",Value="5",Selected=false}
            };

            return list;
        }

        public static List<SelectListItem> GetBackReason()
        {
            List<SelectListItem> list = new List<SelectListItem>(){
                new SelectListItem { Text = "", Value = "0", Selected = true },
                new SelectListItem { Text = "质量问题", Value = "1", Selected = false },
                new SelectListItem { Text = "非质量问题", Value = "2", Selected = false }
            };
            return list;
        }

        public static string GetBackReasonName(int? type)
        {
            string name;
            switch (type)
            {
                case 1:
                    name = "质量问题"; break;
                case 2:
                    name = "非质量问题"; break;
                default:
                    name = "";
                    break;
            }
            return name;
        }

        public static List<SelectListItem> GetBackState()
        {
            List<SelectListItem> list = new List<SelectListItem>(){
                new SelectListItem { Text = "", Value = "0", Selected = true },
                new SelectListItem { Text = "审核中", Value = "1", Selected = false },
                new SelectListItem { Text = "审核不通过", Value = "2", Selected = false },
                new SelectListItem { Text = "退货中", Value = "3", Selected = false },
                new SelectListItem { Text = "已退货", Value = "4", Selected = false },
                 new SelectListItem { Text = "重新发货", Value = "5", Selected = false }
            };
            return list;
        }

        public static string GetBackStateName(int? state)
        {
            string name;
            switch (state)
            {
                case 1:
                    name = "审核中"; break;
                case 2:
                    name = "审核不通过"; break;
                case 3:
                    name = "退货中"; break;
                case 4:
                    name = "已退货"; break;
                case 5:
                    name = "重新发货"; break;
                default:
                    name = "";
                    break;
            }
            return name;
        }

        #endregion

        #region ==== 会员 ====

        /// <summary>
        /// 获取会员级别
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetMLevel()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "全部", Value = "0", Selected = true });

            SelectListItem listItem1 = new SelectListItem() { Text = "一星", Value = "1", Selected = false };
            list.Add(listItem1);

            SelectListItem listItem2 = new SelectListItem() { Text = "二星", Value = "2", Selected = false };
            list.Add(listItem2);

            SelectListItem listItem3 = new SelectListItem() { Text = "三星", Value = "3", Selected = false };
            list.Add(listItem3);

            SelectListItem listItem9 = new SelectListItem() { Text = "银卡会员", Value = "9", Selected = false };
            list.Add(listItem9);
            return list;
        }

        public static List<SelectListItem> GetDealerList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "全部", Value = "0", Selected = true });

            var datas = _AppContext.DealerApp.GetAll();

            foreach (var item in datas)
            {
                SelectListItem value = new SelectListItem() { Text = item.Name, Value = item.DealerId, Selected = false };
                list.Add(value);
            }

            return list;
        }

        public static List<SelectListItem> GetCarCategoryList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "全部", Value = "0", Selected = true });

            var datas = _AppContext.BaseCarApp.GetNamelist();

            foreach (var item in datas)
            {
                SelectListItem value = new SelectListItem() { Text = item, Value = item, Selected = false };
                list.Add(value);
            }

            return list;
        }


        public static List<SelectListItem> GetYKStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "全部", Value = "0", Selected = true });

            SelectListItem listItem1 = new SelectListItem() { Text = "已提交", Value = "1", Selected = false };
            list.Add(listItem1);

            SelectListItem listItem2 = new SelectListItem() { Text = "审批中", Value = "2", Selected = false };
            list.Add(listItem2);

            SelectListItem listItem3 = new SelectListItem() { Text = "审批通过", Value = "3", Selected = false };
            list.Add(listItem3);

            SelectListItem listItem9 = new SelectListItem() { Text = "审批拒绝", Value = "4", Selected = false };
            list.Add(listItem9);
            return list;
        }

        public static List<SelectListItem> GetInputMode()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "全部", Value = "0", Selected = true });

            SelectListItem listItem1 = new SelectListItem() { Text = "网站", Value = "1", Selected = false };
            list.Add(listItem1);

            SelectListItem listItem2 = new SelectListItem() { Text = "APP", Value = "2", Selected = false };
            list.Add(listItem2);

            SelectListItem listItem3 = new SelectListItem() { Text = "微信", Value = "3", Selected = false };
            list.Add(listItem3);

            SelectListItem listItem4 = new SelectListItem() { Text = "线下", Value = "4", Selected = false };
            list.Add(listItem4);
            return list;
        }

        #endregion

        #region ==== 积分 ====

        public static List<SelectListItem> GetOrderMode()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem() { Text = "全部", Value = "-1", Selected = true });

            foreach (var item in Enum.GetValues(typeof(EOrderMode)))
            {
                EOrderMode tempType = (EOrderMode)item;
                SelectListItem listItem1 = new SelectListItem() { Text = tempType.GetDiscribe(), Value = tempType.ToInt32().ToString(), Selected = false };
                list.Add(listItem1);
            }

            return list;
        }

        public static List<SelectListItem> GetIntegralSource()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "全部", Value = "-1", Selected = true });

            foreach (var item in Enum.GetValues(typeof(EIRuleType)))
            {
                EIRuleType tempType = (EIRuleType)item;
                SelectListItem listItem1 = new SelectListItem() { Text = tempType.GetDiscribe(), Value = tempType.ToInt32().ToString(), Selected = false };
                list.Add(listItem1);
            }

            return list;
        }

        public static List<SelectListItem> GetRegionList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "全部", Value = "", Selected = true });

            IEnumerable<string> result = DbHelp.Query<string>("select distinct(region) region from CS_CarDealerShip").Where(x => x != null);

            foreach (var item in result)
            {
                SelectListItem listItem1 = new SelectListItem() { Text = item, Value = item.ToString(), Selected = false };
                list.Add(listItem1);
            }

            return list;
        }

        public static List<SelectListItem> GetAreaList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "全部", Value = "", Selected = true });

            IEnumerable<string> result = DbHelp.Query<string>("select distinct(area) area from CS_CarDealerShip").Where(x => x != null);

            foreach (var item in result)
            {
                SelectListItem listItem1 = new SelectListItem() { Text = item, Value = item.ToString(), Selected = false };
                list.Add(listItem1);
            }

            return list;
        }

        #endregion

        #region ==== 服务卡 ====

        /// <summary>
        /// 查询服务卡类型名称
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public static string FindCardTypeName(string typeCode)
        {
            var data = _AppContext.ServiceCardBatchApp.CardTypeOne(typeCode);

            return data != null ? data.TypeName : string.Empty;
        }

        public static List<SelectListItem> CardTypeAll()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var datas = _AppContext.ServiceCardBatchApp.CardTypeAll();

            foreach (var item in datas)
            {
                SelectListItem listItem1 = new SelectListItem() { Text = item.TypeName, Value = item.TypeCode, Selected = false };
                list.Add(listItem1);
            }

            return list;
        }

        public static List<SelectListItem> GetServiceCardStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "全部", Value = "-1", Selected = true });

            foreach (var item in Enum.GetValues(typeof(EServiceCardStatus)))
            {
                EServiceCardStatus tempType = (EServiceCardStatus)item;
                SelectListItem listItem1 = new SelectListItem() { Text = tempType.GetDiscribe(), Value = tempType.ToInt32().ToString(), Selected = false };
                list.Add(listItem1);
            }

            return list;
        }

        #endregion
    }
}