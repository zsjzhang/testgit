using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Vcyber.BLMS.ManageWeb.Models
{
    /// <summary>
    /// 重写客户信息
    /// </summary>
    public class OverWriteExcel
    {
        #region ==== 死猪字段 ====

        //创建Excel文件的对象
        private HSSFWorkbook book = null;

        private ISheet sheet1 = null;

        private int rowNo = 1;

        #endregion

        #region ==== 构造函数 ====

        public OverWriteExcel()
        {
            this.book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            this.sheet1 = book.CreateSheet("Sheet" + 1);

            IRow row1 = this.sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("客户姓名（必填）");
            row1.CreateCell(1).SetCellValue("手机号（必填）");
            row1.CreateCell(2).SetCellValue("证件号（必填）");
            row1.CreateCell(3).SetCellValue("性别（男/女）（必填）");
            row1.CreateCell(4).SetCellValue("邮箱");
            row1.CreateCell(5).SetCellValue("所在城市（必填）");
            row1.CreateCell(6).SetCellValue("地址（必填）");
            row1.CreateCell(7).SetCellValue("VIN（必填）");
            row1.CreateCell(8).SetCellValue("导入结果");
            row1.CreateCell(9).SetCellValue("未成功原因");
        }

        #endregion

        #region ==== 公共属性 ====

        public HSSFWorkbook Book { get { return this.book; } }

        public int CurrentRowNo { get { return this.rowNo; } }

        #endregion

        #region ==== 公共方法 ====

        public void Writer(IFCustomerV data, string status, string errorMessage)
        {
            NPOI.SS.UserModel.IRow rowtemp = this.sheet1.CreateRow(this.rowNo);

            rowtemp.CreateCell(0).SetCellValue(this.ConvertString(data.CustName));
            rowtemp.CreateCell(1).SetCellValue(this.ConvertString(data.CustMobile));
            rowtemp.CreateCell(2).SetCellValue(this.ConvertString(data.IdentityNumber));
            rowtemp.CreateCell(3).SetCellValue(this.ConvertString(data.Gender));
            rowtemp.CreateCell(4).SetCellValue(this.ConvertString(data.Email));
            rowtemp.CreateCell(5).SetCellValue(this.ConvertString(data.City));
            rowtemp.CreateCell(6).SetCellValue(this.ConvertString(data.Address));
            rowtemp.CreateCell(7).SetCellValue(this.ConvertString(data.VIN));
            rowtemp.CreateCell(8).SetCellValue(status);
            rowtemp.CreateCell(9).SetCellValue(errorMessage);

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