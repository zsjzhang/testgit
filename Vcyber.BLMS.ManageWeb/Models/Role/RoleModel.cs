using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class RoleModel
    {
        public string Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空！")]
        [StringLength(50, ErrorMessage = "名称最多50个字")]
        public string Name { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDel { get; set; }

        private string _description;

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(200, ErrorMessage = "描述最多200个字")]
        public string Describe {
            get
            {
                if (_description == null)
                {
                    return string.Empty;
                }
                return _description;
            }
            set { _description = value; }
        }
    }
}