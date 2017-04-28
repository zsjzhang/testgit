using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.ManageWeb.Models.BBS;

namespace Vcyber.BLMS.ManageWeb.Helper
{
    public class BBSHelper
    {

        private static BBSEntities db = new BBSEntities(); 

        public static List<SelectListItem> GetApprovedState()
        {
            var list = new List<SelectListItem>() {
                new SelectListItem { Text = "全部", Value = "-1", Selected = true },
                new SelectListItem { Text = "未审核", Value = "0", Selected = false },
                new SelectListItem { Text = "已审核", Value = "1", Selected = false }
            };
            return list;
        }

        public static List<SelectListItem> GetToppedState()
        {
            var list = new List<SelectListItem>() {
                new SelectListItem { Text = "全部", Value = "-1", Selected = true },
                new SelectListItem { Text = "未置顶", Value = "0", Selected = false },
                new SelectListItem { Text = "已置顶", Value = "1", Selected = false }
            };
            return list;
        }

        public static List<SelectListItem> GetHotState()
        {
            var list = new List<SelectListItem>(){
                new SelectListItem { Text = "全部", Value = "-1", Selected = true },
                new SelectListItem { Text = "未加精", Value = "0", Selected = false },
                new SelectListItem { Text = "已加精", Value = "1", Selected = false }
            };
            return list;
        }

        public static IEnumerable<SelectListItem> GetColums()
        {

            var list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "全部", Value = "0", Selected = true });


            var columns = db.BBSColumns.ToList();

            if (columns.Any())
            {
                foreach (var item in columns)
                {
                    var listItem = new SelectListItem() { Text = item.Column_Name, Value = item.Id.ToString(), Selected = false };
                    list.Add(listItem);
                }
            }

            return list;
        }
    }
}