using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common
{
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Reflection;
    using System.Web;

    using NPOI.HSSF.UserModel;
    using NPOI.SS.Formula.Functions;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    public class NPOIHelper<T>
    {
        private static int ExcelMaxRow = Convert.ToInt32(ConfigurationManager.AppSettings["ExcelMaxRow"]);
        /// <summary>
        /// 读取Excel[.xls](返回DataTable)
        /// </summary>
        /// <param name="path">Excel路径</param>
        /// <returns></returns>
        public static DataTable ReadExcel(string path, string extentsion)
        {
            try
            {
                DataTable dt = new DataTable();
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    IWorkbook workbook = null;
                    if (extentsion == ".xls")
                    {
                        workbook = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        workbook = new XSSFWorkbook(fs);
                    }

                    ISheet sheet = workbook.GetSheetAt(0);
                    int rfirst = sheet.FirstRowNum;
                    int rlast = sheet.LastRowNum;
                    IRow row = sheet.GetRow(rfirst);
                    int cfirst = row.FirstCellNum;
                    int clast = row.LastCellNum;
                    for (int i = cfirst; i < clast; i++)
                    {
                        if (row.GetCell(i) != null)
                            dt.Columns.Add(row.GetCell(i).StringCellValue, System.Type.GetType("System.String"));
                    }
                    row = null;
                    for (int i = rfirst + 1; i <= rlast; i++)
                    {
                        DataRow r = dt.NewRow();
                        IRow ir = sheet.GetRow(i);
                        for (int j = cfirst; j < clast; j++)
                        {
                            if (ir.GetCell(j) != null)
                            {
                                r[j] = ir.GetCell(j).ToString();
                            }
                        }
                        dt.Rows.Add(r);
                        ir = null;
                        r = null;
                    }
                    sheet = null;
                    workbook = null;
                }
                return dt;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>  
        /// 由DataSet导出Excel  
        /// </summary>  
        /// <param name="sourceTable">要导出数据的DataTable</param>     
        /// <param name="sheetName">工作表名称</param>  
        /// <returns>Excel工作表</returns>     
        private static Stream ExportDataSetToExcel(DataSet sourceDs)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();

            for (int i = 0; i < sourceDs.Tables.Count; i++)
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(sourceDs.Tables[i].TableName);
                HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
                // handling header.             
                foreach (DataColumn column in sourceDs.Tables[i].Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                // handling value.             
                int rowIndex = 1;
                foreach (DataRow row in sourceDs.Tables[i].Rows)
                {
                    HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in sourceDs.Tables[i].Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    rowIndex++;
                }
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            workbook = null;
            return ms;
        }
        /// <summary>  
        /// 由DataSet导出Excel  
        /// </summary>    
        /// <param name="sourceTable">要导出数据的DataTable</param>  
        /// <param name="fileName">指定Excel工作表名称</param>  
        /// <returns>Excel工作表</returns>     
        public static void ExportDataSetToExcel(DataSet sourceDs, string fileName)
        {
            //检查是否有Table数量超过65325  
            for (int t = 0; t < sourceDs.Tables.Count; t++)
            {
                if (sourceDs.Tables[t].Rows.Count > ExcelMaxRow)
                {
                    DataSet ds = GetdtGroup(sourceDs.Tables[t].Copy());
                    sourceDs.Tables.RemoveAt(t);
                    //将得到的ds插入 sourceDs中  
                    for (int g = 0; g < ds.Tables.Count; g++)
                    {
                        DataTable dt = ds.Tables[g].Copy();
                        sourceDs.Tables.Add(dt);
                    }
                    t--;
                }
            }

            MemoryStream ms = ExportDataSetToExcel(sourceDs) as MemoryStream;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            //HttpContext.Current.Response.End();  
            ms.Close();
            ms = null;
        }
        /// <summary>  
        /// 由DataTable导出Excel  
        /// </summary>  
        /// <param name="sourceTable">要导出数据的DataTable</param>  
        /// <returns>Excel工作表</returns>     
        private static Stream ExportDataTableToExcel(DataTable sourceTable)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(sourceTable.TableName);
            HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
            // handling header.       
            foreach (DataColumn column in sourceTable.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            // handling value.       
            int rowIndex = 1;
            foreach (DataRow row in sourceTable.Rows)
            {
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                foreach (DataColumn column in sourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }
                rowIndex++;
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }
        /// <summary>  
        /// 由DataTable导出Excel  
        /// </summary>  
        /// <param name="sourceTable">要导出数据的DataTable</param>  
        /// <param name="fileName">指定Excel工作表名称</param>  
        /// <returns>Excel工作表</returns>  
        public static void ExportDataTableToExcel(DataTable sourceTable, string fileName)
        {
            //如数据超过65325则分成多个Table导出  
            if (sourceTable.Rows.Count > ExcelMaxRow)
            {
                DataSet ds = GetdtGroup(sourceTable);
                //导出DataSet  
                ExportDataSetToExcel(ds, fileName);
            }
            else
            {
                MemoryStream ms = ExportDataTableToExcel(sourceTable) as MemoryStream;
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
                HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                //HttpContext.Current.Response.End();  
                ms.Close();
                ms = null;
            }
        }
        /// <summary>  
        /// 传入行数超过65325的Table，返回DataSet  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static DataSet GetdtGroup(DataTable dt)
        {
            string tablename = dt.TableName;

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            double n = dt.Rows.Count / Convert.ToDouble(ExcelMaxRow);

            //创建表  
            for (int i = 1; i < n; i++)
            {
                DataTable dtAdd = dt.Clone();
                dtAdd.TableName = tablename + "_" + i.ToString();
                ds.Tables.Add(dtAdd);
            }

            //分解数据  
            for (int i = 1; i < ds.Tables.Count; i++)
            {
                //新表行数达到最大 或 基表数量不足  
                while (ds.Tables[i].Rows.Count != ExcelMaxRow && ds.Tables[0].Rows.Count != ExcelMaxRow)
                {
                    ds.Tables[i].Rows.Add(ds.Tables[0].Rows[ExcelMaxRow].ItemArray);
                    ds.Tables[0].Rows.RemoveAt(ExcelMaxRow);

                }
            }

            return ds;
        }
        /// <summary>  
        /// 由DataTable导出Excel  
        /// </summary>  
        /// <param name="sourceTable">要导出数据的DataTable</param>  
        /// <param name="fileName">指定Excel工作表名称</param>  
        /// <returns>Excel工作表</returns>  
        public static void ExportDataTableToExcelModel(DataTable sourceTable, string modelpath, string modelName, string fileName, string sheetName)
        {
            int rowIndex = 2;//从第二行开始，因为前两行是模板里面的内容  
            int colIndex = 0;
            FileStream file = new FileStream(modelpath + modelName + ".xls", FileMode.Open, FileAccess.Read);//读入excel模板  
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.GetSheet("Sheet1");
            sheet1.GetRow(0).GetCell(0).SetCellValue("excelTitle");      //设置表头  
            foreach (DataRow row in sourceTable.Rows)
            {   //双循环写入sourceTable中的数据  
                rowIndex++;
                colIndex = 0;
                HSSFRow xlsrow = (HSSFRow)sheet1.CreateRow(rowIndex);
                foreach (DataColumn col in sourceTable.Columns)
                {
                    xlsrow.CreateCell(colIndex).SetCellValue(row[col.ColumnName].ToString());
                    colIndex++;
                }
            }
            sheet1.ForceFormulaRecalculation = true;
            FileStream fileS = new FileStream(modelpath + fileName + ".xls", FileMode.Create);//保存  
            hssfworkbook.Write(fileS);
            fileS.Close();
            file.Close();
        }
        /// <summary>
        /// 泛型集合类导出成excel
        /// </summary>
        /// <param name="list">泛型集合类</param>
        /// <param name="fileName">生成的excel默认文件名</param>
        /// <param name="propertyName">excel的字段ID列表（选择的列）</param>
        /// <param name="columName">excel的字段名列表（选择的列）</param>
        public static void ListToExcel(IList<T> list, string fileName, List<string> propertyName, List<string> columName)
        {
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.BinaryWrite(ListToExcel<T>(list, propertyName, columName).GetBuffer());
            HttpContext.Current.Response.End();
        }

        public static MemoryStream ListToExcel<T>(IList<T> list, List<string> propertyName, List<string> columName)
        {
            //创建流对象
            using (MemoryStream ms = new MemoryStream())
            {
                //将参数写入到一个临时集合中
                List<string> propertyNameList = new List<string>();
                if (propertyName != null)
                    propertyNameList.AddRange(propertyName);
                //床NOPI的相关对象
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                IRow headerRow = sheet.CreateRow(0);

                if (list.Count > 0)
                {
                    //通过反射得到对象的属性集合
                    PropertyInfo[] propertys = list[0].GetType().GetProperties();
                    //遍历属性集合生成excel的表头标题
                    for (int i = 0; i < propertyName.Count; i++)
                    {
                        headerRow.CreateCell(i).SetCellValue(columName[i]);
                    }

                    int rowIndex = 1;
                    //遍历集合生成excel的行集数据
                    for (int i = 0; i < list.Count; i++)
                    {
                        IRow dataRow = sheet.CreateRow(rowIndex);
                        if (propertyName.Contains("AutoNo"))
                        {//如果选择的字段有AutoNo，则自动以列index编号
                            object obj = rowIndex;
                            int cellIndex = propertyName.IndexOf("AutoNo");
                            dataRow.CreateCell(cellIndex).SetCellValue(obj.ToString());
                        }

                        for (int j = 0; j < propertys.Count(); j++)
                        {
                            if (propertyName.Contains(propertys[j].Name))
                            {
                                object obj = propertys[j].GetValue(list[i], null);
                                int cellIndex = propertyName.IndexOf(propertys[j].Name);
                                string value = "";
                                if (obj != null)
                                {
                                    value = obj.ToString();
                                }
                                dataRow.CreateCell(cellIndex).SetCellValue(value);
                            }
                        }
                        rowIndex++;
                    }
                }
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }



        #region 改造版、多sheet导出
        /// <summary>
        /// 泛型集合类导出成excel
        /// </summary>
        /// <param name="list">泛型集合类</param>
        /// <param name="fileName">生成的excel默认文件名</param>
        /// <param name="propertyName">excel的字段ID列表（选择的列）</param>
        /// <param name="columName">excel的字段名列表（选择的列）</param>
        public static void ListToExcelEX(List<IList<T>> list, string fileName, List<List<string>> propertyName, List<List<string>> columName, List<string> sheetName)
        {
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.BinaryWrite(ListToExcelEX<T>(list, propertyName, columName, sheetName).GetBuffer());
            HttpContext.Current.Response.End();
        }

        public static MemoryStream ListToExcelEX<T>(List<IList<T>> list, List<List<string>> propertyName, List<List<string>> columName, List<string> sheetName)
        {
            //创建流对象
            using (MemoryStream ms = new MemoryStream())
            {
                //创建NOPI的相关对象
                IWorkbook workbook = new HSSFWorkbook();

                for (int x = 0; x < list.Count(); x++)
                {
                    //将参数写入到一个临时集合中
                    List<string> propertyNameList = new List<string>();
                    if (propertyName != null)
                        propertyNameList.AddRange(propertyName[x]);

                    ISheet sheet = workbook.CreateSheet(sheetName[x]);
                    IRow headerRow = sheet.CreateRow(0);

                    if (list[x].Count > 0)
                    {
                        //通过反射得到对象的属性集合
                        PropertyInfo[] propertys = list[x][0].GetType().GetProperties();

                        //遍历属性集合生成excel的表头标题
                        for (int i = 0; i < propertyName[x].Count; i++)
                        {
                            headerRow.CreateCell(i).SetCellValue(columName[x][i]);
                        }

                        int rowIndex = 1;

                        //遍历集合生成excel的行集数据
                        for (int i = 0; i < list[x].Count; i++)
                        {
                            IRow dataRow = sheet.CreateRow(rowIndex);
                            if (propertyName[x].Contains("AutoNo"))
                            {//如果选择的字段有AutoNo，则自动以列index编号
                                object obj = rowIndex;
                                int cellIndex = propertyName[x][i].IndexOf("AutoNo");
                                dataRow.CreateCell(cellIndex).SetCellValue(obj.ToString());
                            }

                            for (int j = 0; j < propertys.Count(); j++)
                            {
                                if (propertyName[x].Contains(propertys[j].Name))
                                {
                                    object obj = propertys[j].GetValue(list[x][i], null);
                                    int cellIndex = propertyName[x].IndexOf(propertys[j].Name);
                                    string value = "";
                                    if (obj != null)
                                    {
                                        value = obj.ToString();
                                    }
                                    dataRow.CreateCell(cellIndex).SetCellValue(value);
                                }
                            }
                            rowIndex++;
                        }
                    }
                    workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                }
                return ms;
            }
        }
        #endregion
    }
}
