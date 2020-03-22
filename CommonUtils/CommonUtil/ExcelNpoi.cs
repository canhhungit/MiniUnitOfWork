using System;
using System.Collections.Generic;
using System.Text;

namespace CommonUtil
{
    public static class ExcelNpoi
    {
        /// <summary>
        /// Tao Style cho o excel
        /// </summary>
        /// <param name="wk"></param>
        /// <param name="Alignment">Can theo chieu doc</param>
        /// <param name="border"></param>
        /// <param name="FillBackgroundColor">Mau nen</param>
        /// <param name="FillForegroundColor">Ma khi chon</param>
        /// <param name="FillPattern">kieu to mau nen</param>
        /// <param name="font">kieu chu</param>
        /// <param name="IsHidden">o an</param>
        /// <param name="IsLocked"> o khoa</param>
        /// <param name="VerticalAlignment">can theo chieu ngang</param>
        /// <param name="WrapText">xuong dong</param>
        /// <param name="Rotation">Do nghieng cua chu - góc</param>
        /// <returns></returns>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static ICellStyle CreateCellType(this IWorkbook wk, HorizontalAlignment Alignment, BorderStyle border,
            short FillBackgroundColor, short FillForegroundColor, FillPattern fillPattern, IFont font, bool IsHidden,
            bool IsLocked, VerticalAlignment VerticalAlignment, bool WrapText, short Rotation)
        {
            ICellStyle result = wk.CreateCellStyle();
            result.Alignment = Alignment;
            result.BorderBottom = result.BorderLeft = result.BorderRight = result.BorderTop = border;
            result.FillBackgroundColor = FillBackgroundColor;
            result.FillForegroundColor = FillForegroundColor;
            result.FillPattern = fillPattern;
            result.IsHidden = IsHidden;
            result.IsLocked = IsLocked;
            result.SetFont(font);
            result.VerticalAlignment = VerticalAlignment;
            result.WrapText = WrapText;
            result.Rotation = Rotation;
            return result;
        }

        /// <summary>
        /// Tao font
        /// </summary>
        /// <param name="wk">WorkBook</param>
        /// <param name="Boldweight">Độ đậm - FontBoldWeight</param>
        /// <param name="color">Màu chữ - FontColor</param>
        /// <param name="fontsize">cỡ chữ</param>
        /// <param name="FontName">kiểu chữ</param>
        /// <param name="IsItalic">in nghiêng</param>
        /// <param name="Underline">ngạch chân - FontUnderlineType</param>
        /// <param name="TypeOffset">Vi tri - FontSuperScript</param>
        /// <returns></returns>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static IFont CreateFont(this IWorkbook wk, short Boldweight, short color, short fontsize, string FontName, bool IsItalic,
            FontUnderlineType Underline, FontSuperScript TypeOffset)
        {
            IFont result = wk.CreateFont();
            result.Boldweight = Boldweight;
            result.Color = color;
            result.FontHeightInPoints = fontsize;
            result.FontName = FontName;
            result.IsItalic = IsItalic;
            result.Underline = Underline;
            result.TypeOffset = TypeOffset;
            return result;
        }

        /// <summary>
        /// Tao Merged va tao hoac lay row tuong uong
        /// Sao khi goi ham nay thi chi dung ham ISheet.GetRow hoac  ExcelNpoi.GetCreateRow voi nhung row trong vung
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="cellType"></param>
        /// <param name="rowFrom"></param>
        /// <param name="colFrom"></param>
        /// <param name="rowTo"></param>
        /// <param name="colTo"></param>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static void AddMergedRegion(this ISheet sheet, ICellStyle cellType, int rowFrom, int colFrom, int rowTo, int colTo)
        {
            IRow row = GetCreateRow(sheet, rowFrom);
            sheet.AddMergedRegion(new CellRangeAddress(rowFrom, rowTo, colFrom, colTo));
            ICell hcell = row.GetCell(colFrom);
            if (hcell != null)
                cellType = hcell.CellStyle;
            int xrow = rowTo - rowFrom;
            switch (xrow)
            {
                case 0:
                    for (int i = colFrom + 1; i <= colTo; i++)
                    {
                        row.CreateCell(i).CellStyle = cellType;
                    }
                    break;

                case 1:
                    IRow rowT = GetCreateRow(sheet, rowTo);
                    for (int i = colFrom + 1; i <= colTo; i++)
                    {
                        row.CreateCell(i).CellStyle = cellType;
                        rowT.CreateCell(i).CellStyle = cellType;
                    }
                    rowT.CreateCell(colFrom).CellStyle = cellType;
                    break;

                default:
                    IRow rowT2 = GetCreateRow(sheet, rowTo);
                    for (int i = colFrom + 1; i <= colTo; i++)
                    {
                        row.CreateCell(i).CellStyle = cellType;
                        rowT2.CreateCell(i).CellStyle = cellType;
                    }
                    rowT2.CreateCell(colFrom).CellStyle = cellType;
                    if (colFrom != colTo)
                    {
                        for (int j = rowFrom + 1; j < rowTo; j++)
                        {
                            row = GetCreateRow(sheet, j);
                            row.CreateCell(colFrom).CellStyle = cellType;
                            row.CreateCell(colTo).CellStyle = cellType;
                        }
                    }
                    else
                    {
                        for (int j = rowFrom + 1; j < rowTo; j++)
                        {
                            row = GetCreateRow(sheet, j);
                            row.CreateCell(colFrom).CellStyle = cellType;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Lay row o vi tri hien tai neu khong co thi tao moi
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static IRow GetCreateRow(this ISheet sheet, int rowIndex)
        {
            IRow hsRow = sheet.GetRow(rowIndex);
            if (hsRow == null)
                return sheet.CreateRow(rowIndex);
            else
                return hsRow;
        }

        /// <summary>
        /// Hàm vẽ một line trên file excel
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="col1">Cột chứa điểm đầu của line</param>
        /// <param name="row1">Dòng chứa điểm đầu của line</param>
        /// <param name="x1">Tọa độ X điểm đầu của line (Tọa độ x trong 1 cell có giá trị từ 0 - 1023)</param>
        /// <param name="y1">Tọa độ Y điểm đầu của line (Tọa độ Y trong 1 cell có giá trị từ 0 - 255)</param>
        /// <param name="col2">Cột chứa điểm cuối của line</param>
        /// <param name="row2">Dòng chứa điểm cuối của line</param>
        /// <param name="x2">Tọa độ X điểm cuối của line</param>
        /// <param name="y2">Tọa độ Y điểm cuối của line</param>
        /// <modified>
        /// Author      Date        comment
        /// TuanVM      15/07/2012  Tạo mới
        /// </modified>
        public static void DrawLine(this ISheet sheet, short col1, int row1, int x1, int y1, short col2, int row2, int x2, int y2)
        {
            HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
            HSSFClientAnchor a1 = new HSSFClientAnchor();
            a1.SetAnchor(col1, row1, x1, y1, col2, row2, x2, y2);
            HSSFSimpleShape shape1 = patriarch.CreateSimpleShape(a1);
            shape1.ShapeType = (HSSFSimpleShape.OBJECT_TYPE_LINE);
        }

        /// <summary>
        /// Hàm vẽ một line căn vào giữa của 2 cột trên sheet
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="col1">Cột chứa điểm bắt đầu của line</param>
        /// <param name="row">Dòng chứa điểm bắt đầu của line</param>
        /// <param name="x">Tọa độ x của điểm bắt đầu (Tọa độ x trong 1 cell có giá trị từ 0 - 1023)</param>
        /// <param name="y">Tọa độ y của điểm bắt đầu (Tọa độ Y trong 1 cell có giá trị từ 0 - 255)</param>
        /// <param name="col2"></param>
        /// <modified>
        /// Author      Date        comment
        /// TuanVM      15/07/2012  Tạo mới
        /// </modified>
        public static void DrawLinesToCenter(this ISheet sheet, short col1, int row, int x, int y, short col2)
        {
            HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();

            HSSFClientAnchor a1 = new HSSFClientAnchor();
            if (col2 == col1)
                a1.SetAnchor(col1, row, x, y, col2, row, 1024 - x, y);
            else
            {
                int col1Width = sheet.GetColumnWidth(col1);
                int col2Width = sheet.GetColumnWidth(col2);

                a1.SetAnchor(col1, row, x, y, col2, row, 1024 - x * col1Width / col2Width, y);
            }
            HSSFSimpleShape shape1 = patriarch.CreateSimpleShape(a1);
            shape1.ShapeType = (HSSFSimpleShape.OBJECT_TYPE_LINE);
        }

        /// <summary>
        /// Ghi danh mục vào một row trong sheet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet">Sheet cần ghi danh mục</param>
        /// <param name="lst">Danh sách danh mục</param>
        /// <param name="textField">Tên cột nội dung</param>
        /// <param name="valueField">Tên cột dữ liệu</param>
        /// <param name="row">Dòng ghi, Bắt đầu từ 0</param>
        /// <param name="nameText">Tên của vùng text</param>
        /// <param name="nameValue">Tên của vùng value</param>
        /// <exception cref="Field Not Found"></exception>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static void WriteDanhMucByRow<T>(this ISheet sheet, List<T> lst, string textField, string valueField, int row, string nameText, string nameValue)
        {
            if (lst == null || lst.Count == 0)
                return;
            PropertyInfo[] p = typeof(T).GetProperties();
            PropertyInfo pText = p.First(a => a.Name == textField);
            PropertyInfo pValue = p.First(a => a.Name == valueField);
            if (pText == null || pValue == null)
                throw new Exception("TextField hoạc ValueField không tồn tại");
            IRow rowText = GetCreateRow(sheet, row);
            IRow rowID = GetCreateRow(sheet, row + 1);
            for (int i = 0; i < lst.Count; i++)
            {
                rowText.CreateCell(i).SetCellValue(pText.GetValue(lst[i], null).ToString());
                rowID.CreateCell(i).SetCellValue(pValue.GetValue(lst[i], null).ToString());
            }

            IName hsName = sheet.Workbook.GetName(nameText);
            if (hsName == null)
            {
                hsName = sheet.Workbook.CreateName();
                hsName.NameName = nameText;
            }
            CellRangeAddress cellRange = new CellRangeAddress(row, row, 0, lst.Count);
            hsName.RefersToFormula = cellRange.FormatAsString(sheet.SheetName, true);
            hsName = sheet.Workbook.GetName(nameValue);
            if (hsName == null)
            {
                hsName = sheet.Workbook.CreateName();
                hsName.NameName = nameValue;
            }
            cellRange = new CellRangeAddress(row, row + 1, 0, lst.Count);
            hsName.RefersToFormula = cellRange.FormatAsString(sheet.SheetName, true);
        }

        /// <summary>
        /// Ghi danh mục vào một row trong sheet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet">Sheet cần ghi danh mục</param>
        /// <param name="lst">Danh sách danh mục</param>
        /// <param name="textField">Tên cột nội dung</param>
        /// <param name="row">Dòng ghi, Bắt đầu từ 0</param>
        /// <param name="nameText">Tên của vùng text</param>
        /// <exception cref="Field Not Found"></exception>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static void WriteDanhMucByRow<T>(this ISheet sheet, List<T> lst, string textField, int row, string nameText)
        {
            if (lst == null || lst.Count == 0)
                return;
            PropertyInfo[] p = typeof(T).GetProperties();
            PropertyInfo pText = p.First(a => a.Name == textField);
            if (pText == null)
                throw new Exception("TextField không tồn tại");
            IRow rowText = GetCreateRow(sheet, row);

            for (int i = 0; i < lst.Count; i++)
            {
                rowText.CreateCell(i).SetCellValue(pText.GetValue(lst[i], null).ToString());
            }

            IName hsName = sheet.Workbook.GetName(nameText);
            if (hsName == null)
            {
                hsName = sheet.Workbook.CreateName();
                hsName.NameName = nameText;
            }
            CellRangeAddress cellRange = new CellRangeAddress(row, row, 0, lst.Count);
            hsName.RefersToFormula = cellRange.FormatAsString(sheet.SheetName, true);
        }

        /// <summary>
        /// Ghi danh mục vào một column trong sheet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet">Sheet cần ghi danh mục</param>
        /// <param name="lst">Danh sách danh mục</param>
        /// <param name="textField">Tên cột nội dung</param>
        /// <param name="valueField">Tên cột dữ liệu</param>
        /// <param name="col">Cột ghi, Bắt đầu từ 0</param>
        /// <param name="nameText">Tên của vùng text</param>
        /// <param name="nameValue">Tên của vùng value</param>
        /// <exception cref="Field Not Found"></exception>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static void WriteDanhMucByCol<T>(this ISheet sheet, List<T> lst, string textField, string valueField, int col, string nameText, string nameValue)
        {
            if (lst == null || lst.Count == 0)
                return;
            PropertyInfo[] p = typeof(T).GetProperties();
            PropertyInfo pText = p.First(a => a.Name == textField);
            PropertyInfo pValue = p.First(a => a.Name == valueField);
            if (pText == null || pValue == null)
                throw new Exception("TextField hoạc ValueField không tồn tại");
            IRow hsrow;
            for (int i = 0; i < lst.Count; i++)
            {
                hsrow = GetCreateRow(sheet, i);
                hsrow.CreateCell(col).SetCellValue(pText.GetValue(lst[i], null).ToString());
                hsrow.CreateCell(col + 1).SetCellValue(pValue.GetValue(lst[i], null).ToString());
            }

            IName hsName = sheet.Workbook.GetName(nameText);
            if (hsName == null)
            {
                hsName = sheet.Workbook.CreateName();
                hsName.NameName = nameText;
            }
            CellRangeAddress cellRange = new CellRangeAddress(0, lst.Count, col, col);
            hsName.RefersToFormula = cellRange.FormatAsString(sheet.SheetName, true);
            hsName = sheet.Workbook.GetName(nameValue);
            if (hsName == null)
            {
                hsName = sheet.Workbook.CreateName();
                hsName.NameName = nameValue;
            }
            cellRange = new CellRangeAddress(0, lst.Count, col, col + 1);
            hsName.RefersToFormula = cellRange.FormatAsString(sheet.SheetName, true);
        }

        /// <summary>
        /// Ghi danh mục vào một column trong sheet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet">Sheet cần ghi danh mục</param>
        /// <param name="lst">Danh sách danh mục</param>
        /// <param name="textField">Tên cột nội dung</param>
        /// <param name="col">Cột ghi, Bắt đầu từ 0</param>
        /// <param name="nameText">Tên của vùng text</param>
        /// <exception cref="Field Not Found"></exception>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static void WriteDanhMucByCol<T>(this ISheet sheet, List<T> lst, string textField, int col, string nameText)
        {
            if (lst == null || lst.Count == 0)
                return;
            PropertyInfo[] p = typeof(T).GetProperties();
            PropertyInfo pText = p.First(a => a.Name == textField);
            if (pText == null)
                throw new Exception("TextField không tồn tại");

            for (int i = 0; i < lst.Count; i++)
            {
                GetCreateRow(sheet, i).CreateCell(col).SetCellValue(pText.GetValue(lst[i], null).ToString());
            }

            IName hsName = sheet.Workbook.GetName(nameText);
            if (hsName == null)
            {
                hsName = sheet.Workbook.CreateName();
                hsName.NameName = nameText;
            }
            CellRangeAddress cellRange = new CellRangeAddress(0, lst.Count, col, col);
            hsName.RefersToFormula = cellRange.FormatAsString(sheet.SheetName, true);
        }

        public static ICell GetCreateCell(this IRow row, int column)
        {
            ICell cell = row.GetCell(column);
            if (cell == null)
                cell = row.CreateCell(column);
            return cell;
        }

        public static string GetValueCell(this IRow row, int column)
        {
            string result = string.Empty;
            ICell cell = row.GetCell(column);
            if (cell != null)
            {
                CellType _cellType = cell.CellType == CellType.Formula ? cell.CachedFormulaResultType : cell.CellType;
                switch (_cellType)
                {
                    case CellType.Numeric:
                        if (HSSFDateUtil.IsCellDateFormatted(cell))
                            result = cell.DateCellValue.ToString(Enums.FormatType.FormatDateVN);
                        else
                            result = cell.NumericCellValue.ToString();
                        break;

                    case CellType.String:
                        result = cell.StringCellValue;
                        break;
                }
            }
            return result;
        }
        public static string GetValueCell(this IRow row, int column, IFormulaEvaluator evaluator)
        {
            string result = string.Empty;
            ICell cell = row.GetCell(column);
            if (cell != null)
            {
                var cellValue = evaluator.Evaluate(cell);
                switch (cellValue.CellType)
                {
                    case CellType.Numeric:
                        if (HSSFDateUtil.IsCellDateFormatted(cell))
                            result = new DateTime((long)cellValue.NumberValue).ToString(Enums.FormatType.FormatDateVN);
                        else
                            result = cellValue.NumberValue.ToString();
                        break;

                    case CellType.String:
                        result = cellValue.StringValue;
                        break;
                }
            }
            return result;
        }

        public static void WriteDMToTable(this ISheet sheet, List<DefineSelectItem> lst, string tableName, int numCol)
        {
            if (lst == null || lst.Count == 0) return;
            XSSFSheet s = (XSSFSheet)sheet;
            XSSFTable tbl = s.GetTables().Where(a => a.Name == tableName).FirstOrDefault();
            if (tbl == null) return;
            CT_Table ctTBl = tbl.GetCTTable();
            if (numCol < 2 || ctTBl.tableColumns.count < numCol) return;
            CellReference cellRefStart = tbl.GetStartCellReference();
            int rowStart = cellRefStart.Row + 1;
            short colStart = cellRefStart.Col;
            CellReference cellRefEnd = tbl.GetEndCellReference();
            AreaReference reference = new AreaReference(cellRefStart, new CellReference(cellRefStart.Row + lst.Count, cellRefEnd.Col));
            ctTBl.insertRow = true;
            ctTBl.insertRowShift = true;
            ctTBl.@ref = reference.FormatAsString();
            IRow row = GetCreateRow(sheet, rowStart);
            switch (numCol)
            {
                case 3:
                    foreach (DefineSelectItem item in lst)
                    {
                        row = GetCreateRow(sheet, rowStart++);

                        row.GetCreateCell(colStart).SetCellValue(item.Value);

                        row.GetCreateCell(colStart + 1).SetCellValue(item.Value2);

                        row.GetCreateCell(colStart + 2).SetCellValue(item.Text);
                    }
                    break;

                case 4:
                    foreach (DefineSelectItem item in lst)
                    {
                        row = GetCreateRow(sheet, rowStart++);

                        row.GetCreateCell(colStart).SetCellValue(item.Value);

                        row.GetCreateCell(colStart + 1).SetCellValue(item.Value2);

                        row.GetCreateCell(colStart + 2).SetCellValue(item.Value3);

                        row.GetCreateCell(colStart + 3).SetCellValue(item.Text);
                    }
                    break;

                case 2:
                default:
                    foreach (DefineSelectItem item in lst)
                    {
                        row = GetCreateRow(sheet, rowStart++);

                        row.GetCreateCell(colStart).SetCellValue(item.Value);

                        row.GetCreateCell(colStart + 1).SetCellValue(item.Text);
                    }
                    break;
            }
        }

        /// <summary>
        /// Đọc excel to IWorkBook
        /// </summary>
        /// <param name="strPath">Đường dẫn đến file excel</param>
        /// <param name="isDelete">Có xóa file gốc không</param>
        /// <returns></returns>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static IWorkbook ReadExcelToIWorkBook(string strPath, bool isDelete = false)
        {
            FileInfo fi = new FileInfo(strPath);
            IWorkbook templateWorkbook;
            if (fi.Exists)
            {
                FileStream fs = new FileStream(strPath, FileMode.Open, FileAccess.Read);
                try
                {
                    if (fi.Extension.ToLower() == ".xls")
                    {
                        templateWorkbook = new HSSFWorkbook(fs);
                    }
                    else
                        templateWorkbook = new XSSFWorkbook(fs);
                    fs.Close();
                    if (isDelete)
                        fi.Delete();
                    return templateWorkbook;
                }
                catch
                {
                    fs.Close();
                    //ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook(fs);
                }
            }
            return null;
        }

        /// <summary>
        /// Move hoạc copy row từ vi trí rowFrom đến rowTo. Không copy margin
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowFrom">Vi trí nguồn. Bắt đầu từ 0</param>
        /// <param name="rowTo">Vị trí đích. Bắt đầu từ 0</param>
        /// <param name="IsMove">True - Move, False - Copy</param>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      24/07/2012  Tạo mới
        /// </modified>
        public static void MoveOrCopyRow(this ISheet sheet, int rowFrom, int rowTo, bool IsMove = true)
        {
            IRow hRowF = (IRow)sheet.GetRow(rowFrom);
            if (hRowF == null)
                return;
            IRow hRowT = (IRow)sheet.CreateRow(rowTo);
            ICell cellF;
            ICell cellT;
            //hRowT = hRowF;
            int bd = (int)hRowF.FirstCellNum;
            int kt = (int)hRowF.LastCellNum;
            foreach (ICell item in hRowF.Cells)
            {
                cellF = (ICell)item;
                cellT = (ICell)hRowT.CreateCell(item.ColumnIndex);
                cellT.CellStyle = cellF.CellStyle;
                switch (cellF.CellType)
                {
                    case CellType.Numeric:
                        cellT.SetCellValue(cellF.NumericCellValue);
                        break;

                    case CellType.String:
                        cellT.SetCellValue(cellF.StringCellValue);
                        break;

                    case CellType.Formula:
                        cellT.SetCellFormula(cellF.CellFormula);
                        break;

                    case CellType.Boolean:
                        cellT.SetCellValue(cellF.BooleanCellValue);
                        break;

                    case CellType.Error:
                        cellT.SetCellValue(cellF.ErrorCellValue);
                        break;
                }
                if (cellF.IsMergedCell)
                    continue;
            }
            if (IsMove)
                sheet.RemoveRow(hRowF);
        }

        /// <summary>
        /// Xóa row từ vị trí rowFrom đến rowTo
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowFrom">Vi trí nguồn. Bắt đầu từ 0</param>
        /// <param name="rowTo">Vị trí đích. Bắt đầu từ 0</param>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      24/07/2012  Tạo mới
        /// </modified>
        public static void RemoveRow(this ISheet sheet, int rowFrom, int rowTo)
        {
            for (int i = rowFrom; i < rowTo; i++)
            {
                IRow hRow = (IRow)sheet.GetRow(i);
                if (hRow != null)
                    sheet.RemoveRow(hRow);
            }
        }

        /// <summary>
        /// Đọc excel và ghi dữ liệu IWorkBook Chưa hoàn thành
        /// </summary>
        /// <param name="strPath">Đường dẫn đến file excel</param>
        /// <param name="dtSource"></param>
        /// <param name="rowStart"></param>
        /// <param name="colStart"></param>
        /// <param name="IsCount">True - Có STT, False - Không có STT</param>
        /// <returns></returns>
        /// <modified>
        /// Author      Date        comment
        /// anhhn      15/07/2012  Tạo mới
        /// </modified>
        public static IWorkbook WriteExcelByTemp(this DataTable dtSource, string strPath, int rowStart, int colStart, bool IsCount = false)
        {
            IWorkbook wb = ReadExcelToIWorkBook(strPath);
            if (wb == null) return wb;
            int rowCount = dtSource.Rows.Count;
            if (rowCount == 0)
                return wb;
            int colSTT = 0;
            if (IsCount)
                colSTT++;
            int colCount = dtSource.Columns.Count;
            ISheet sheet = wb.GetSheetAt(0);

            int indexRow = rowStart;
            IRow Row = sheet.GetRow(indexRow++);
            if (Row.Cells.Count < colCount + colSTT)
                return wb;

            ICellStyle[] lstCellStyle = new ICellStyle[colCount + colSTT];
            ICell cell;
            int indexCol = colStart;
            DataRow dr = dtSource.Rows[0];
            if (IsCount)
            {
                cell = Row.GetCell(indexCol);
                lstCellStyle[indexCol - colStart] = cell.CellStyle;
                cell.SetCellValue(1);
                indexCol++;
            }
            for (int i = 0; i < colCount; i++)
            {
                cell = Row.GetCell(indexCol);
                lstCellStyle[indexCol - colStart] = cell.CellStyle;
                //UNDONE Lấy kiểu của dữ liệu
                cell.SetCellValue(dr[i].ToString());
                indexCol++;
            }
            for (int i = 1; i < dtSource.Rows.Count; i++)
            {
                Row = sheet.GetCreateRow(indexRow++);
                indexCol = 0;
                dr = dtSource.Rows[i];
                indexCol = colStart;
                if (IsCount)
                {
                    cell = Row.CreateCell(indexCol);
                    cell.SetCellValue(i + 1);
                    cell.CellStyle = lstCellStyle[indexCol - colStart];
                    indexCol++;
                }
                for (int j = 0; j < colCount; j++)
                {
                    cell = Row.CreateCell(indexCol);
                    cell.SetCellValue(dr[j].ToString());
                    cell.CellStyle = lstCellStyle[indexCol - colStart];
                    indexCol++;
                }
            }
            return wb;
        }

        /// <summary>
        /// Đọc file excel ra datatable
        /// </summary>
        /// <param name="strPath">Đường dẫn đến file</param>
        /// <param name="isDelete">Có xóa file sau khi đọc</param>
        /// <returns></returns>
        public static DataTable ReadExcelToDataTable(string strPath, bool isDelete = false, int curCol = 0, int minRow = 0, int sheetIndex = 0)
        {
            IWorkbook wb = ReadExcelToIWorkBook(strPath, isDelete);
            if (wb == null) return null;

            ISheet sheet = wb.GetSheetAt(sheetIndex);
            DataTable dt = new DataTable();
            IRow headerRow = sheet.GetCreateRow(0);
            IEnumerator rows = sheet.GetRowEnumerator();

            int colCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;
            //Neu so cột không đúng thì ko đọc tiếp
            if (curCol > 0 && curCol != colCount)
                return dt;
            //Nếu số dong không đúng thì ko đọc tiếp
            if (minRow > 0 && rowCount < minRow)
                return dt;
            //add Columns
            for (int c = 0; c < colCount; c++)
                dt.Columns.Add(headerRow.GetCell(c).ToString());

            bool hashValue = false;
            DataRow dr = dt.NewRow();
            rows.MoveNext();//Bo row dau
            IRow row;
            //Add row
            while (rows.MoveNext())
            {
                row = (IRow)rows.Current;
                if (hashValue)
                    dr = dt.NewRow();
                hashValue = false;
                for (int i = 0; i < colCount; i++)
                {
                    ICell cell = row.GetCell(i);

                    if (cell != null)
                    {
                        CellType _cellType = cell.CellType == CellType.Formula ? cell.CachedFormulaResultType : cell.CellType;
                        switch (_cellType)
                        {
                            case CellType.Numeric:
                                if (HSSFDateUtil.IsCellDateFormatted(cell))
                                    dr[i] = cell.DateCellValue.ToString(Enums.FormatType.FormatDateVN);
                                else
                                    dr[i] = cell.NumericCellValue.ToString();
                                hashValue = true;
                                break;

                            case CellType.String:
                                dr[i] = cell.StringCellValue;
                                hashValue = true;
                                break;
                        }
                    }
                }
                if (hashValue)
                    dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Đọc file excel ra datatable
        /// </summary>
        /// <param name="strPath">Đường dẫn đến file</param>
        /// <param name="isDelete">Có xóa file sau khi đọc</param>
        /// <returns></returns>
        public static DataTable ReadExcelForBulkCopy(string strPath, string version, bool isDelete = false)
        {
            int maxCol = 20;
            IWorkbook wb = ReadExcelToIWorkBook(strPath, isDelete);

            if (wb == null) return null;

            ISheet sheet = wb.GetSheetAt(0);
            DataTable dt = new DataTable();
            IRow headerRow = sheet.GetRow(0);
            IEnumerator rows = sheet.GetRowEnumerator();

            int colCount = headerRow.LastCellNum > maxCol ? maxCol : headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;

            //add Columns
            for (int c = 0; c < maxCol; c++)
                dt.Columns.Add("Col" + c, typeof(string));
            dt.Columns.Add("Version");
            dt.Columns.Add("RowIndex", typeof(int));
            dt.Columns.Add("Status", typeof(int));
            bool hashValue = false;

            DataRow dr = dt.NewRow();
            //Add row
            while (rows.MoveNext())
            {
                IRow row = (IRow)rows.Current;
                if (hashValue)
                    dr = dt.NewRow();
                hashValue = false;
                for (int i = 0; i < colCount; i++)
                {
                    ICell cell = row.GetCell(i);

                    if (cell != null)
                    {
                        CellType _cellType = cell.CellType == CellType.Formula ? cell.CachedFormulaResultType : cell.CellType;
                        switch (_cellType)
                        {
                            case CellType.Numeric:
                                if (HSSFDateUtil.IsCellDateFormatted(cell))
                                    dr[i] = cell.DateCellValue.ToString(Enums.FormatType.FormatDateVN);
                                else
                                    dr[i] = cell.NumericCellValue.ToString();
                                hashValue = true;
                                break;

                            case CellType.String:
                                dr[i] = cell.StringCellValue;
                                hashValue = true;
                                break;
                        }
                    }
                }
                if (hashValue)
                {
                    dr["Version"] = version;
                    dr["RowIndex"] = row.RowNum + 1;
                    dr["Status"] = 0;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
    }
}
