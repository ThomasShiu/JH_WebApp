/*******************************************************************************
 * Copyright © 2016 CCM.Framework 版權所有
 * Author: CCM
 * Description: CCM快速開發平臺
 * Website：http://www.ccm3s.com/
*********************************************************************************/
using System.Data;

namespace CCM.Code
{
    public static class ExtTable
    {
        /// <summary>
        /// 獲取表裡某頁的資料
        /// </summary>
        /// <param name="data">表數據</param>
        /// <param name="pageIndex">當前頁</param>
        /// <param name="pageSize">分頁大小</param>
        /// <param name="allPage">返回總頁數</param>
        /// <returns>返回當頁表資料</returns>
        public static DataTable GetPage(this DataTable data, int pageIndex, int pageSize, out int allPage)
        {
            allPage = data.Rows.Count / pageSize;
            allPage += data.Rows.Count % pageSize == 0 ? 0 : 1;
            DataTable Ntable = data.Clone();
            int startIndex = pageIndex * pageSize;
            int endIndex = startIndex + pageSize > data.Rows.Count ? data.Rows.Count : startIndex + pageSize;
            if (startIndex < endIndex)
                for (int i = startIndex; i < endIndex; i++)
                {
                    Ntable.ImportRow(data.Rows[i]);
                }
            return Ntable;
        }
        /// <summary>
        /// 根據欄位過濾表的內容
        /// </summary>
        /// <param name="data">表數據</param>
        /// <param name="condition">條件</param>
        /// <returns></returns>
        /// 
        public static DataTable GetDataFilter(DataTable data, string condition)
        {
            if (data != null && data.Rows.Count > 0)
            {
                if (condition.Trim() == "")
                {
                    return data;
                }
                else
                {
                    DataTable newdt = new DataTable();
                    newdt = data.Clone();
                    DataRow[] dr = data.Select(condition);
                    for (int i = 0; i < dr.Length; i++)
                    {
                        newdt.ImportRow((DataRow)dr[i]);
                    }
                    return newdt;
                }
            }
            else
            {
                return null;
            }
        }
    }
}

