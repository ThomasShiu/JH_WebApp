/*******************************************************************************
 * Copyright © 2016 CCM.Framework 版權所有
 * Author: CCM
 * Description: CCM快速開發平臺
 * Website：http://www.ccm3s.com/
*********************************************************************************/
using System.Collections;
using System.Collections.Generic;

namespace CCM.Code.Extend
{
    public static class ExtList
    {
        /// <summary>
        /// 獲取表裡某頁的資料
        /// </summary>
        /// <param name="data">表數據</param>
        /// <param name="pageIndex">當前頁</param>
        /// <param name="pageSize">分頁大小</param>
        /// <param name="allPage">返回總頁數</param>
        /// <returns>返回當頁表資料</returns>
        public static List<T> GetPage<T>(this List<T> data, int pageIndex, int pageSize, out int allPage)
        {
            allPage = 1;
            return null;
        }
        /// <summary>
        /// IList轉成List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> IListToList<T>(IList list)
        {
            T[] array = new T[list.Count];
            list.CopyTo(array, 0);
            return new List<T>(array);
        }
    }
}