/*******************************************************************************
 * Copyright © 2016 CCM.Framework 版權所有
 * Author: CCM
 * Description: CCM快速開發平臺
 * Website：http://www.ccm3s.com/
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace CCM.Code
{
    public class WebHelper
    {
        #region ResolveUrl(解析相對Url)
        /// <summary>
        /// 解析相對Url
        /// </summary>
        /// <param name="relativeUrl">相對Url</param>
        public static string ResolveUrl(string relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl))
                return string.Empty;
            relativeUrl = relativeUrl.Replace("\\", "/");
            if (relativeUrl.StartsWith("/"))
                return relativeUrl;
            if (relativeUrl.Contains("://"))
                return relativeUrl;
            return VirtualPathUtility.ToAbsolute(relativeUrl);
        }

        #endregion

        #region HtmlEncode(對html字串進行編碼)
        /// <summary>
        /// 對html字串進行編碼
        /// </summary>
        /// <param name="html">html字串</param>
        public static string HtmlEncode(string html)
        {
            return HttpUtility.HtmlEncode(html);
        }
        /// <summary>
        /// 對html字串進行解碼
        /// </summary>
        /// <param name="html">html字串</param>
        public static string HtmlDecode(string html)
        {
            return HttpUtility.HtmlDecode(html);
        }

        #endregion

        #region UrlEncode(對Url進行編碼)

        /// <summary>
        /// 對Url進行編碼
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="isUpper">編碼字元是否轉成大寫,範例,"http://"轉成"http%3A%2F%2F"</param>
        public static string UrlEncode(string url, bool isUpper = false)
        {
            return UrlEncode(url, Encoding.UTF8, isUpper);
        }

        /// <summary>
        /// 對Url進行編碼
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">字元編碼</param>
        /// <param name="isUpper">編碼字元是否轉成大寫,範例,"http://"轉成"http%3A%2F%2F"</param>
        public static string UrlEncode(string url, Encoding encoding, bool isUpper = false)
        {
            var result = HttpUtility.UrlEncode(url, encoding);
            if (!isUpper)
                return result;
            return GetUpperEncode(result);
        }

        /// <summary>
        /// 獲取大寫編碼字串
        /// </summary>
        private static string GetUpperEncode(string encode)
        {
            var result = new StringBuilder();
            int index = int.MinValue;
            for (int i = 0; i < encode.Length; i++)
            {
                string character = encode[i].ToString();
                if (character == "%")
                    index = i;
                if (i - index == 1 || i - index == 2)
                    character = character.ToUpper();
                result.Append(character);
            }
            return result.ToString();
        }

        #endregion

        #region UrlDecode(對Url進行解碼)

        /// <summary>
        /// 對Url進行解碼,對於javascript的encodeURIComponent函數編碼參數,應使用utf-8字元編碼來解碼
        /// </summary>
        /// <param name="url">url</param>
        public static string UrlDecode(string url)
        {
            return HttpUtility.UrlDecode(url);
        }

        /// <summary>
        /// 對Url進行解碼,對於javascript的encodeURIComponent函數編碼參數,應使用utf-8字元編碼來解碼
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">字元編碼,對於javascript的encodeURIComponent函數編碼參數,應使用utf-8字元編碼來解碼</param>
        public static string UrlDecode(string url, Encoding encoding)
        {
            return HttpUtility.UrlDecode(url, encoding);
        }

        #endregion

        #region Session操作
        /// <summary>
        /// 寫Session
        /// </summary>
        /// <typeparam name="T">Session鍵值的類型</typeparam>
        /// <param name="key">Session的鍵名</param>
        /// <param name="value">Session的鍵值</param>
        public static void WriteSession<T>(string key, T value)
        {
            if (key.IsEmpty())
                return;
            HttpContext.Current.Session[key] = value;
        }

        /// <summary>
        /// 寫Session
        /// </summary>
        /// <param name="key">Session的鍵名</param>
        /// <param name="value">Session的鍵值</param>
        public static void WriteSession(string key, string value)
        {
            WriteSession<string>(key, value);
        }

        /// <summary>
        /// 讀取Session的值
        /// </summary>
        /// <param name="key">Session的鍵名</param>        
        public static string GetSession(string key)
        {
            if (key.IsEmpty())
                return string.Empty;
            return HttpContext.Current.Session[key] as string;
        }
        /// <summary>
        /// 刪除指定Session
        /// </summary>
        /// <param name="key">Session的鍵名</param>
        public static void RemoveSession(string key)
        {
            if (key.IsEmpty())
                return;
            HttpContext.Current.Session.Contents.Remove(key);
        }

        #endregion

        #region Cookie操作
        /// <summary>
        /// 寫cookie值
        /// </summary>
        /// <param name="strName">名稱</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// 寫cookie值
        /// </summary>
        /// <param name="strName">名稱</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">過期時間(分鐘)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// 讀cookie值
        /// </summary>
        /// <param name="strName">名稱</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }
            return "";
        }
        /// <summary>
        /// 刪除Cookie對象
        /// </summary>
        /// <param name="CookiesName">Cookie對象名稱</param>
        public static void RemoveCookie(string CookiesName)
        {
            HttpCookie objCookie = new HttpCookie(CookiesName.Trim());
            objCookie.Expires = DateTime.Now.AddYears(-5);
            HttpContext.Current.Response.Cookies.Add(objCookie);
        }
        #endregion

        #region GetFileControls(獲取用戶端檔控制項集合)

        /// <summary>
        /// 獲取有效用戶端檔控制項集合,檔控制項必須上傳了內容，為空將被忽略,
        /// 注意:Form標記必須加入屬性 enctype="multipart/form-data",伺服器端才能獲取用戶端file控制項.
        /// </summary>
        public static List<HttpPostedFile> GetFileControls()
        {
            var result = new List<HttpPostedFile>();
            var files = HttpContext.Current.Request.Files;
            if (files.Count == 0)
                return result;
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                if (file.ContentLength == 0)
                    continue;
                result.Add(files[i]);
            }
            return result;
        }

        #endregion

        #region GetFileControl(獲取第一個有效用戶端檔控制項)

        /// <summary>
        /// 獲取第一個有效用戶端檔控制項,檔控制項必須上傳了內容，為空將被忽略,
        /// 注意:Form標記必須加入屬性 enctype="multipart/form-data",伺服器端才能獲取用戶端file控制項.
        /// </summary>
        public static HttpPostedFile GetFileControl()
        {
            var files = GetFileControls();
            if (files == null || files.Count == 0)
                return null;
            return files[0];
        }

        #endregion

        #region HttpWebRequest(請求網路資源)

        /// <summary>
        /// 請求網路資源,返回回應的文本
        /// </summary>
        /// <param name="url">網路資源位址</param>
        public static string HttpWebRequest(string url)
        {
            return HttpWebRequest(url, string.Empty, Encoding.GetEncoding("utf-8"));
        }

        /// <summary>
        /// 請求網路資源,返回回應的文本
        /// </summary>
        /// <param name="url">網路資源Url位址</param>
        /// <param name="parameters">提交的參數,格式：參數1=參數值1&amp;參數2=參數值2</param>
        public static string HttpWebRequest(string url, string parameters)
        {
            return HttpWebRequest(url, parameters, Encoding.GetEncoding("utf-8"), true);
        }

        /// <summary>
        /// 請求網路資源,返回回應的文本
        /// </summary>
        /// <param name="url">網路資源位址</param>
        /// <param name="parameters">提交的參數,格式：參數1=參數值1&amp;參數2=參數值2</param>
        /// <param name="encoding">字元編碼</param>
        /// <param name="isPost">是否Post提交</param>
        /// <param name="contentType">內容類別型</param>
        /// <param name="cookie">Cookie容器</param>
        /// <param name="timeout">超時時間</param>
        public static string HttpWebRequest(string url, string parameters, Encoding encoding, bool isPost = false,
             string contentType = "application/x-www-form-urlencoded", CookieContainer cookie = null, int timeout = 360000)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = timeout;
            request.CookieContainer = cookie;
            if (isPost)
            {
                byte[] postData = encoding.GetBytes(parameters);
                request.Method = "POST";
                request.ContentType = contentType;
                request.ContentLength = postData.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                }
            }
            var response = (HttpWebResponse)request.GetResponse();
            string result;
            using (Stream stream = response.GetResponseStream())
            {
                if (stream == null)
                    return string.Empty;
                using (var reader = new StreamReader(stream, encoding))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }

        #endregion

        #region 去除HTML標記
        /// <summary>
        /// 去除HTML標記
        /// </summary>
        /// <param name="NoHTML">包括HTML的源碼 </param>
        /// <returns>已經去除後的文字</returns>
        public static string NoHtml(string Htmlstring)
        {
            //刪除腳本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //刪除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&hellip;", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&mdash;", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&ldquo;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring = Regex.Replace(Htmlstring, @"&rdquo;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;

        }
        #endregion

        #region 格式化文字（防止SQL注入）
        /// <summary>
        /// 格式化文字（防止SQL注入）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Formatstr(string html)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" on[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex10 = new System.Text.RegularExpressions.Regex(@"select", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex11 = new System.Text.RegularExpressions.Regex(@"update", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex12 = new System.Text.RegularExpressions.Regex(@"delete", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //過濾<script></script>標記
            html = regex2.Replace(html, ""); //過濾href=javascript: (<A>) 屬性
            html = regex3.Replace(html, " _disibledevent="); //過濾其它控制項的on...事件
            html = regex4.Replace(html, ""); //過濾iframe
            html = regex10.Replace(html, "s_elect");
            html = regex11.Replace(html, "u_pudate");
            html = regex12.Replace(html, "d_elete");
            html = html.Replace("'", "’");
            html = html.Replace("&nbsp;", " ");
            return html;
        }
        #endregion
    }
}
