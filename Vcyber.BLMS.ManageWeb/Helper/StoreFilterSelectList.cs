using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Repository;

namespace Vcyber.BLMS.ManageWeb.Helper
{
    public class StoreFilterSelectList
    {
        public static SelectList GetGoodsFilterRangList()
        {
            var list = new List<ListItem>
            {
                new ListItem() {Index = 0, Name = "全部"},
                new ListItem() {Index = 1, Name = "0"},
                new ListItem() {Index = 2, Name = "0-10"},
                new ListItem() {Index = 3, Name = "10-50"},
                new ListItem() {Index = 4, Name = ">50"}
            };
            return new SelectList(list,"Index","Name");
        }

        public static SelectList GetProductIdentity()
        {
            List<ListItem> list = new List<ListItem>() { 
            new ListItem(){Index=0,Name="全部"},
           //  new ListItem(){Index=1,Name="注册会员"},
              new ListItem(){Index=10,Name="普卡会员"},
               new ListItem(){Index=11,Name="银卡会员"},
                new ListItem(){Index=12,Name="金卡会员"},
                new ListItem(){Index=13,Name="金卡和银卡会员"}
            };

            return new SelectList(list, "Index", "Name");
        }

        public static SelectList GetProductCategory()
        {
            List<ListItem> list = new List<ListItem>();

            ListItem listItem1 = new ListItem() { Name = "请选择", Index = -1 };
            list.Add(listItem1);

            var categorys = new CategoryStorager().SelectList_ForBehind(); //_AppContext.CategoryApp.GetList();

            if (categorys != null && categorys.Count() > 0)
            {
                foreach (var item in categorys)
                {
                    ListItem listItem = new ListItem() { Name = item.Name, Index = item.ID };
                    list.Add(listItem);
                }
            }

            return new SelectList(list, "Index", "Name");
        }

        public static SelectList GetProductCategory(int parentId)
        {
            List<ListItem> list = new List<ListItem>();

            ListItem listItem1= new ListItem() { Name ="请选择", Index = -1 };
            list.Add(listItem1);

            var categorys = _AppContext.CategoryApp.GetList(parentId==0?-1:parentId);

            if (categorys != null && categorys.Count() > 0)
            {
                foreach (var item in categorys)
                {
                    ListItem listItem = new ListItem() { Name = item.Name, Index = item.ID };
                    list.Add(listItem);
                }
            }

            return new SelectList(list, "Index", "Name");
        }

        public static SelectList GetProductStatusList()
        {
            var list = new List<ListItem>
            {
                new ListItem() {Index = 0, Name = "下架"},
                new ListItem() {Index = 1, Name = "上架"}
            };
            return new SelectList(list, "Index", "Name");
        }

        public static SelectList GetProductRecommendList()
        {
            var list = new List<ListItem>
            {
                 new ListItem() {Index = EProductRecommend.BT.ToInt32(), Name = "普通"},
                new ListItem() {Index = EProductRecommend.RX.ToInt32(), Name = "热销"},
                new ListItem() {Index =  EProductRecommend.XP.ToInt32(), Name = "新品"}
            };
            return new SelectList(list, "Index", "Name");
        }

        public static SelectList GetVirtualGoodsTypeList()
        {
            var list = new List<ListItem>
            {
                new ListItem() {Index = 0, Name = "电话卡"},
                new ListItem() {Index = 1, Name = "Q币卡"}
            };
            return new SelectList(list, "Index", "Name");
        }

        public static IEnumerable<ProductColor> GetAllProductColorsList()
        {

            var colorlist = _AppContext.ProductApp.GetAllProductColors();
            return colorlist;

        }

        public static IEnumerable<ProductType> GetAllProductTypesList()
        {

            
            var Typelist = _AppContext.ProductApp.GetAllProductTypes();
            return Typelist;

        }  
    }

    public class ListItem
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}