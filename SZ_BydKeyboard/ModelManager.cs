using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZ_BydKeyboard
{
    /// <summary>
    /// 型号管理，根据SN的倒数第6~3个字符来查找对应的型号
    /// </summary>
    public class ModelManager
    {
        Dictionary<string,string> sn2Model = new Dictionary<string,string>();
        string lastModel = String.Empty;

        public int LoadConfig()
        {
            string strConfig = $"{Application.StartupPath}\\config\\byd-sn.xlsx";
            if(File.Exists(strConfig))
            {
                ISheet sheet = null;
                IWorkbook workbook = null;
                try
                {
                    FileStream fs = new FileStream(strConfig, FileMode.Open, FileAccess.Read);
                    if (strConfig.IndexOf(".xlsx") > 0) // 2007版本
                        workbook = new XSSFWorkbook(fs);
                    else if (strConfig.IndexOf(".xls") > 0) // 2003版本
                        workbook = new HSSFWorkbook(fs);
                    sheet = workbook.GetSheetAt(0);
                    if(sheet != null)
                    {
                        int firstRow = sheet.FirstRowNum;
                        int lastRow = sheet.LastRowNum;
                        IRow row = GetFirstRow(sheet,ref firstRow);//sheet.GetRow(firstRow);
                        int firstCol = row.FirstCellNum;
                        int lastCol = row.LastCellNum;
                        string[] models = new string[lastCol - firstCol + 1];
                        for(int i=firstCol; i<lastCol; i++)
                        {
                            models[i] = row.Cells[i].ToString();
                        }
                        for(int j=firstRow+1;j<lastRow; j++)
                        {
                            IRow row2 = sheet.GetRow(j);
                            for(int k=firstCol; k<lastCol; k++)
                            {
                                string sn = row2.Cells[k].ToString();
                                sn2Model[sn] = models[k];
                            }
                        }
                        return 0;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }
            }
            return -1;
        }

        private IRow GetFirstRow(ISheet sheet,ref int firstRow)
        {
            int firstRow2 = sheet.FirstRowNum;
            int lastRow = sheet.LastRowNum;
            for(int i=firstRow2;i<lastRow;i++)
            {
                IRow row = sheet.GetRow(i);
                for(int j=row.FirstCellNum;j<row.LastCellNum;j++)
                {
                    string content = row.Cells[j].ToString(); ;
                    if(content.Length > 0)
                    {
                        firstRow = i;
                        return row;
                    }
                }

            }
            return null;
        }

        public string SN2Model(string sn)
        {
            if(sn2Model.ContainsKey(sn))
            {
                return sn2Model[sn];
            }
            return string.Empty;
        }

        public delegate void OnChangeBarcode(object sender,string model);
        public event OnChangeBarcode BarcodeChanged;
        public void OnReceiveBarcode(string sn)
        {
            if(sn.Length < 6)
            {
                return;
            }
            string mid = sn.Substring(sn.Length - 6, 4);
            string model = SN2Model(mid);
            if (!lastModel.Equals(model))
            {
                BarcodeChanged?.Invoke(this, model);
                lastModel = model;
            }
        }

        public void SetLastModel(string model)
        {
            lastModel = model;
        }
    }
}
