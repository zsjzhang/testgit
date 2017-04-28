using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.ManageWeb.Models
{
    /// <summary>
    /// 服务卡导出
    /// </summary>
    public class ServiceCardExport
    {
       #region ==== 私有字段 ====

        //创建Excel文件的对象
        private HSSFWorkbook book = null;

        private ISheet sheet1 = null;

        private int rowNo = 1;

        #endregion

        #region ==== 构造函数 ====

        public ServiceCardExport()
        {
            this.book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            this.sheet1 = book.CreateSheet("Sheet" + 1);

            IRow row1 = this.sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("批次号");
            row1.CreateCell(1).SetCellValue("卡卷名称");
            row1.CreateCell(2).SetCellValue("卡卷编号");
            row1.CreateCell(3).SetCellValue("卡卷面值");
            row1.CreateCell(4).SetCellValue("卡卷状态");
            row1.CreateCell(5).SetCellValue("使用时间");
            row1.CreateCell(6).SetCellValue("生成时间");
            row1.CreateCell(7).SetCellValue("下发时间");
            row1.CreateCell(8).SetCellValue("是否过期");
        }

        #endregion

        #region ==== 公共属性 ====

        public HSSFWorkbook Book { get { return this.book; } }

        public int CurrentRowNo { get { return this.rowNo; } }

        #endregion

        #region ==== 公共方法 ====

        public void Writer(ServiceCard data)
        {
            NPOI.SS.UserModel.IRow rowtemp = this.sheet1.CreateRow(this.rowNo);

            rowtemp.CreateCell(0).SetCellValue(this.ConvertString(data.BatchNo));
            rowtemp.CreateCell(1).SetCellValue(this.ConvertString(data.BatchName));
            rowtemp.CreateCell(2).SetCellValue(this.ConvertString(data.CardNo));
            rowtemp.CreateCell(3).SetCellValue(data.BatchPrice.ToString());
            rowtemp.CreateCell(4).SetCellValue(((EServiceCardStatus)data.Status).GetDiscribe());
            rowtemp.CreateCell(5).SetCellValue(data.UseTime.ToString());
            rowtemp.CreateCell(6).SetCellValue(data.CreateTime.ToString());
            rowtemp.CreateCell(7).SetCellValue(data.SendTime.ToString());
            rowtemp.CreateCell(8).SetCellValue(data.IsOverdue);

            this.rowNo++;
        }

        #endregion

        #region ==== 私有方法 ===

        private string ConvertString(string value)
        {
            return string.IsNullOrEmpty(value) ? "" : value;
        }

        #endregion
    }
}