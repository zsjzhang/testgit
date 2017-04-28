using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class MenuViewModel
    {
        public int id { get; set; }
        public string value { get; set; }
        public bool isDefault { get; set; }

        public List<MenuItemViewModel> Menus;
    }

    public class MenuItemViewModel
    {
        public int id { get; set; }

        public string value { get; set; }

        public string href { get; set; }

        public bool isDefault { get; set; }
    }
}