/*******************************************************************************
 * Copyright © 2016 CCM.Framework 版權所有
 * Author: CCM
 * Description: CCM快速開發平臺
 * Website：http://www.ccm3s.com/
*********************************************************************************/

namespace CCM.Code
{
    public class AjaxResult
    {
        /// <summary>
        /// 操作結果類型
        /// </summary>
        public object state { get; set; }
        /// <summary>
        /// 獲取 消息內容
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 獲取 返回資料
        /// </summary>
        public object data { get; set; }
    }
    /// <summary>
    /// 表示 ajax 操作結果類型的列舉
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 消息結果類型
        /// </summary>
        info,
        /// <summary>
        /// 成功結果類型
        /// </summary>
        success,
        /// <summary>
        /// 警告結果類型
        /// </summary>
        warning,
        /// <summary>
        /// 異常結果類型
        /// </summary>
        error
    }
}

