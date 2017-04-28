using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 分页数据结构
    /// </summary>
    public class PageData
    {
        #region ==== public constructor ====

        public PageData() { }

        #endregion

        #region ==== private field ====

        private int pageIndex = 0;

        private int pageSize = 0;

        #endregion

        #region ==== public property ====

        public int Index
        {
            get
            {
                return this.pageIndex < 1 ? 0: (this.pageIndex - 1) * this.pageSize;
            }
            set
            {
                this.pageIndex = value <= 0 ? 0 : value;
            }

        }

        public int Size
        {
            get
            {
                return this.pageSize <=0 ? 10 : this.pageSize;
            }

            set
            {
                this.pageSize = value < 0 ? 10 : value;
            }

        }

        #endregion
    }
}
