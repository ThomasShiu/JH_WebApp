/*******************************************************************************
 * Copyright © 2016 CCM.Framework 版權所有
 * Author: CCM
 * Description: CCM快速開發平臺
 * Website：http://www.ccm3s.com/
*********************************************************************************/
namespace CCM.Code
{
    public static partial class Ext
    {
        /// <summary>
        /// 獲取描述
        /// </summary>
        /// <param name="value">布林值</param>
        public static string Description(this bool value)
        {
            return value ? "是" : "否";
        }
        /// <summary>
        /// 獲取描述
        /// </summary>
        /// <param name="value">布林值</param>
        public static string Description(this bool? value)
        {
            return value == null ? "" : Description(value.Value);
        }
        /// <summary>
        /// 獲取格式化字串
        /// </summary>
        /// <param name="number">數值</param>
        /// <param name="defaultValue">空值顯示的預設文本</param>
        public static string Format(this int number, string defaultValue = "")
        {
            if (number == 0)
                return defaultValue;
            return number.ToString();
        }
        /// <summary>
        /// 獲取格式化字串
        /// </summary>
        /// <param name="number">數值</param>
        /// <param name="defaultValue">空值顯示的預設文本</param>
        public static string Format(this int? number, string defaultValue = "")
        {
            return Format(number.SafeValue(), defaultValue);
        }
        /// <summary>
        /// 獲取格式化字串
        /// </summary>
        /// <param name="number">數值</param>
        /// <param name="defaultValue">空值顯示的預設文本</param>
        public static string Format(this decimal number, string defaultValue = "")
        {
            if (number == 0)
                return defaultValue;
            return string.Format("{0:0.##}", number);
        }
        /// <summary>
        /// 獲取格式化字串
        /// </summary>
        /// <param name="number">數值</param>
        /// <param name="defaultValue">空值顯示的預設文本</param>
        public static string Format(this decimal? number, string defaultValue = "")
        {
            return Format(number.SafeValue(), defaultValue);
        }
        /// <summary>
        /// 獲取格式化字串
        /// </summary>
        /// <param name="number">數值</param>
        /// <param name="defaultValue">空值顯示的預設文本</param>
        public static string Format(this double number, string defaultValue = "")
        {
            if (number == 0)
                return defaultValue;
            return string.Format("{0:0.##}", number);
        }
        /// <summary>
        /// 獲取格式化字串
        /// </summary>
        /// <param name="number">數值</param>
        /// <param name="defaultValue">空值顯示的預設文本</param>
        public static string Format(this double? number, string defaultValue = "")
        {
            return Format(number.SafeValue(), defaultValue);
        }
        /// <summary>
        /// 獲取格式化字串,帶￥
        /// </summary>
        /// <param name="number">數值</param>
        public static string FormatRmb(this decimal number)
        {
            if (number == 0)
                return "￥0";
            return string.Format("￥{0:0.##}", number);
        }
        /// <summary>
        /// 獲取格式化字串,帶￥
        /// </summary>
        /// <param name="number">數值</param>
        public static string FormatRmb(this decimal? number)
        {
            return FormatRmb(number.SafeValue());
        }
        /// <summary>
        /// 獲取格式化字串,帶%
        /// </summary>
        /// <param name="number">數值</param>
        public static string FormatPercent(this decimal number)
        {
            if (number == 0)
                return string.Empty;
            return string.Format("{0:0.##}%", number);
        }
        /// <summary>
        /// 獲取格式化字串,帶%
        /// </summary>
        /// <param name="number">數值</param>
        public static string FormatPercent(this decimal? number)
        {
            return FormatPercent(number.SafeValue());
        }
        /// <summary>
        /// 獲取格式化字串,帶%
        /// </summary>
        /// <param name="number">數值</param>
        public static string FormatPercent(this double number)
        {
            if (number == 0)
                return string.Empty;
            return string.Format("{0:0.##}%", number);
        }
        /// <summary>
        /// 獲取格式化字串,帶%
        /// </summary>
        /// <param name="number">數值</param>
        public static string FormatPercent(this double? number)
        {
            return FormatPercent(number.SafeValue());
        }
    }
}

