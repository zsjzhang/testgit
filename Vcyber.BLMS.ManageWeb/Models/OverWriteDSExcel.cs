using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.ManageWeb.Models
{
    /// <summary>
    /// 重写经销商信息
    /// </summary>
    public class OverWriteDSExcel
    {
        #region ==== 死猪字段 ====

        //创建Excel文件的对象
        private HSSFWorkbook book = null;

        private ISheet sheet1 = null;

        private int rowNo = 1;

        #endregion

        #region ==== 构造函数 ====

        public OverWriteDSExcel()
        {
            this.book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            this.sheet1 = book.CreateSheet("Sheet" + 1);

            IRow row1 = this.sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("店代码（必填）");
            row1.CreateCell(1).SetCellValue("全称（必填）");
            row1.CreateCell(2).SetCellValue("简称（必填）");
            row1.CreateCell(3).SetCellValue("省（必填）");
            row1.CreateCell(4).SetCellValue("市（必填）");
            row1.CreateCell(5).SetCellValue("地址（必填）");
            row1.CreateCell(6).SetCellValue("销售电话（必填）");
            row1.CreateCell(7).SetCellValue("售后电话（必填）");
            row1.CreateCell(8).SetCellValue("办事处（必填）");
            row1.CreateCell(9).SetCellValue("区域（必填）");
            row1.CreateCell(10).SetCellValue("淘宝账号");
            row1.CreateCell(11).SetCellValue("支付宝账号");
            row1.CreateCell(12).SetCellValue("邮箱");
            row1.CreateCell(13).SetCellValue("纬度");
            row1.CreateCell(14).SetCellValue("经度");
            row1.CreateCell(15).SetCellValue("道路救援电话");
            row1.CreateCell(16).SetCellValue("导入结果");
            row1.CreateCell(17).SetCellValue("未成功原因");
        }

        #endregion

        #region ==== 公共属性 ====

        public HSSFWorkbook Book { get { return this.book; } }

        public int CurrentRowNo { get { return this.rowNo; } }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="jwd">经纬度</param>
        /// <param name="status"></param>
        /// <param name="errorMessage"></param>
        public void Writer(DealerShipV data, string jwd, string status, string errorMessage)
        {
            NPOI.SS.UserModel.IRow rowtemp = this.sheet1.CreateRow(this.rowNo);

            rowtemp.CreateCell(0).SetCellValue(this.ConvertString(data.DealerId));
            rowtemp.CreateCell(1).SetCellValue(this.ConvertString(data.Name));
            rowtemp.CreateCell(2).SetCellValue(this.ConvertString(data.Abbreviation));
            rowtemp.CreateCell(3).SetCellValue(this.ConvertString(data.Province));
            rowtemp.CreateCell(4).SetCellValue(this.ConvertString(data.City));
            rowtemp.CreateCell(5).SetCellValue(this.ConvertString(data.Address));
            rowtemp.CreateCell(6).SetCellValue(this.ConvertString(data.Phone));
            rowtemp.CreateCell(7).SetCellValue(this.ConvertString(data.AfterSalesPhone));
            rowtemp.CreateCell(8).SetCellValue(this.ConvertString(data.Region));
            rowtemp.CreateCell(9).SetCellValue(this.ConvertString(data.Area));
            rowtemp.CreateCell(10).SetCellValue(this.ConvertString(data.TBAccount));
            rowtemp.CreateCell(11).SetCellValue(this.ConvertString(data.AlipayAccount));
            rowtemp.CreateCell(12).SetCellValue(this.ConvertString(data.Email));
            rowtemp.CreateCell(13).SetCellValue(this.ConvertString(jwd));
            rowtemp.CreateCell(14).SetCellValue(this.ConvertString(jwd));
            rowtemp.CreateCell(15).SetCellValue(this.ConvertString(data.FreeRoadRescuePhone));
            rowtemp.CreateCell(16).SetCellValue(status);
            rowtemp.CreateCell(17).SetCellValue(errorMessage);

            this.rowNo++;
        }

        #endregion

        #region ==== 死猪方法 ===

        private string ConvertString(string value)
        {
            return string.IsNullOrEmpty(value) ? "" : value;
        }

        #endregion
    }
}