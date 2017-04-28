using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class ReturnResult
    {
        /// <summary>
        /// 执行是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 错误数据
        /// </summary>
        public object Errors { get; set; }

        /// <summary>
        /// 备用数据
        /// </summary>
        public object Data { get; set; }
    }
}
