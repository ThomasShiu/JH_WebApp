/*******************************************************************************
 * Copyright © 2016 CCM.Framework 版權所有
 * Author: CCM
 * Description: CCM快速開發平臺
 * Website：http://www.ccm3s.com/
*********************************************************************************/
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;

namespace CCM.Code
{
    public class FileHelper
    {
        #region 檢測指定目錄是否存在
        /// <summary>
        /// 檢測指定目錄是否存在
        /// </summary>
        /// <param name="directoryPath">目錄的絕對路徑</param>
        /// <returns></returns>
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
        #endregion

        #region 檢測指定檔是否存在,如果存在返回true
        /// <summary>
        /// 檢測指定檔是否存在,如果存在則返回true。
        /// </summary>
        /// <param name="filePath">檔的絕對路徑</param>        
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        #endregion

        #region 獲取指定目錄中的檔清單
        /// <summary>
        /// 獲取指定目錄中所有檔清單
        /// </summary>
        /// <param name="directoryPath">指定目錄的絕對路徑</param>        
        public static string[] GetFileNames(string directoryPath)
        {
            //如果目錄不存在，則拋出異常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            //獲取文件列表
            return Directory.GetFiles(directoryPath);
        }
        #endregion

        #region 獲取指定目錄中所有子目錄清單,若要搜索嵌套的子目錄列表,請使用重載方法.
        /// <summary>
        /// 獲取指定目錄中所有子目錄清單,若要搜索嵌套的子目錄列表,請使用重載方法.
        /// </summary>
        /// <param name="directoryPath">指定目錄的絕對路徑</param>        
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 獲取指定目錄及子目錄中所有檔列表
        /// <summary>
        /// 獲取指定目錄及子目錄中所有檔列表
        /// </summary>
        /// <param name="directoryPath">指定目錄的絕對路徑</param>
        /// <param name="searchPattern">模式字串，"*"代表0或N個字元，"?"代表1個字元。
        /// 範例："Log*.xml"表示搜索所有以Log開頭的Xml檔。</param>
        /// <param name="isSearchChild">是否搜索子目錄</param>
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //如果目錄不存在，則拋出異常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            try
            {
                if (isSearchChild)
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 檢測指定目錄是否為空
        /// <summary>
        /// 檢測指定目錄是否為空
        /// </summary>
        /// <param name="directoryPath">指定目錄的絕對路徑</param>        
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                //判斷是否存在檔
                string[] fileNames = GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }

                //判斷是否存在資料夾
                string[] directoryNames = GetDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                //這裡記錄日誌
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return true;
            }
        }
        #endregion

        #region 檢測指定目錄中是否存在指定的檔
        /// <summary>
        /// 檢測指定目錄中是否存在指定的檔,若要搜索子目錄請使用重載方法.
        /// </summary>
        /// <param name="directoryPath">指定目錄的絕對路徑</param>
        /// <param name="searchPattern">模式字串，"*"代表0或N個字元，"?"代表1個字元。
        /// 範例："Log*.xml"表示搜索所有以Log開頭的Xml檔。</param>        
        public static bool Contains(string directoryPath, string searchPattern)
        {
            try
            {
                //獲取指定的檔列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, false);

                //判斷指定檔是否存在
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
            }
        }

        /// <summary>
        /// 檢測指定目錄中是否存在指定的檔
        /// </summary>
        /// <param name="directoryPath">指定目錄的絕對路徑</param>
        /// <param name="searchPattern">模式字串，"*"代表0或N個字元，"?"代表1個字元。
        /// 範例："Log*.xml"表示搜索所有以Log開頭的Xml檔。</param> 
        /// <param name="isSearchChild">是否搜索子目錄</param>
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                //獲取指定的檔列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, true);

                //判斷指定檔是否存在
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
            }
        }
        #endregion

        #region 創建目錄
        /// <summary>
        /// 創建目錄
        /// </summary>
        /// <param name="dir">要創建的目錄路徑包括目錄名</param>
        public static void CreateDir(string dir)
        {
            if (dir.Length == 0) return;
            if (!Directory.Exists(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir))
                Directory.CreateDirectory(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir);
        }
        #endregion

        #region 刪除目錄
        /// <summary>
        /// 刪除目錄
        /// </summary>
        /// <param name="dir">要刪除的目錄路徑和名稱</param>
        public static void DeleteDir(string dir)
        {
            if (dir.Length == 0) return;
            if (Directory.Exists(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir))
                Directory.Delete(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir);
        }
        #endregion

        #region 刪除檔
        /// <summary>
        /// 刪除檔
        /// </summary>
        /// <param name="file">要刪除的檔路徑和名稱</param>
        public static void DeleteFile(string file)
        {
            if (File.Exists(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + file))
            {
                File.Delete(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + file);
            }
        }
        #endregion

        #region 創建文件
        /// <summary>
        /// 創建文件
        /// </summary>
        /// <param name="dir">帶尾碼的檔案名</param>
        /// <param name="pagestr">檔內容</param>
        public static void CreateFile(string dir, string pagestr)
        {
            dir = dir.Replace("/", "\\");
            if (dir.IndexOf("\\") > -1)
                CreateDir(dir.Substring(0, dir.LastIndexOf("\\")));
            StreamWriter sw = new StreamWriter(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir, false, System.Text.Encoding.GetEncoding("GB2312"));
            sw.Write(pagestr);
            sw.Close();
        }
        /// <summary>
        /// 創建文件
        /// </summary>
        /// <param name="path">路徑</param>
        /// <param name="content">內容</param>
        public static void CreateFileContent(string path, string content)
        {
            FileInfo fi = new FileInfo(path);
            var di = fi.Directory;
            if (!di.Exists)
            {
                di.Create();
            }
            StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.GetEncoding("GB2312"));
            sw.Write(content);
            sw.Close();
        }
        #endregion

        #region 移動文件(剪貼--粘貼)
        /// <summary>
        /// 移動文件(剪貼--粘貼)
        /// </summary>
        /// <param name="dir1">要移動的檔的路徑及全名(包括尾碼)</param>
        /// <param name="dir2">檔移動到新的位置,並指定新的檔案名</param>
        public static void MoveFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (File.Exists(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1))
                File.Move(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1, System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir2);
        }
        #endregion

        #region 複製檔
        /// <summary>
        /// 複製檔
        /// </summary>
        /// <param name="dir1">要複製的檔的路徑已經全名(包括尾碼)</param>
        /// <param name="dir2">目標位置,並指定新的檔案名</param>
        public static void CopyFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            var str = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1;

            if (File.Exists(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1))
            {
                File.Copy(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1, System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir2, true);
            }
        }
        #endregion

        #region 根據時間得到目錄名 / 格式:yyyyMMdd 或者 HHmmssff
        /// <summary>
        /// 根據時間得到目錄名yyyyMMdd
        /// </summary>
        /// <returns></returns>
        public static string GetDateDir()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }
        /// <summary>
        /// 根據時間得到檔案名HHmmssff
        /// </summary>
        /// <returns></returns>
        public static string GetDateFile()
        {
            return DateTime.Now.ToString("HHmmssff");
        }
        #endregion

        #region 根據時間獲取指定路徑的 尾碼名的 的所有檔
        /// <summary>
        /// 根據時間獲取指定路徑的 尾碼名的 的所有檔
        /// </summary>
        /// <param name="path">檔路徑</param>
        /// <param name="Extension">尾碼名 比如.txt</param>
        /// <returns></returns>
        public static DataRow[] GetFilesByTime(string path, string Extension)
        {
            if (Directory.Exists(path))
            {
                string fielExts = string.Format("*{0}", Extension);
                string[] files = Directory.GetFiles(path, fielExts);
                if (files.Length > 0)
                {
                    DataTable table = new DataTable();
                    table.Columns.Add(new DataColumn("filename", Type.GetType("System.String")));
                    table.Columns.Add(new DataColumn("createtime", Type.GetType("System.DateTime")));
                    for (int i = 0; i < files.Length; i++)
                    {
                        DataRow row = table.NewRow();
                        DateTime creationTime = File.GetCreationTime(files[i]);
                        string fileName = Path.GetFileName(files[i]);
                        row["filename"] = fileName;
                        row["createtime"] = creationTime;
                        table.Rows.Add(row);
                    }
                    return table.Select(string.Empty, "createtime desc");
                }
            }
            return new DataRow[0];
        }
        #endregion

        #region 複製資料夾
        /// <summary>
        /// 複製資料夾(遞迴)
        /// </summary>
        /// <param name="varFromDirectory">原始檔案夾路徑</param>
        /// <param name="varToDirectory">目的檔案夾路徑</param>
        public static void CopyFolder(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    CopyFolder(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }
            string[] files = Directory.GetFiles(varFromDirectory);
            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Copy(s, varToDirectory + s.Substring(s.LastIndexOf("\\")), true);
                }
            }
        }
        #endregion

        #region 檢查檔,如果檔不存在則創建
        /// <summary>
        /// 檢查檔,如果檔不存在則創建  
        /// </summary>
        /// <param name="FilePath">路徑,包括檔案名</param>
        public static void ExistsFile(string FilePath)
        {
            //if(!File.Exists(FilePath))    
            //File.Create(FilePath);    
            //以上寫法會報錯,詳細解釋請看下文.........   
            if (!File.Exists(FilePath))
            {
                FileStream fs = File.Create(FilePath);
                fs.Close();
            }
        }
        #endregion

        #region 刪除指定資料夾對應其他資料夾裡的檔
        /// <summary>
        /// 刪除指定資料夾對應其他資料夾裡的檔
        /// </summary>
        /// <param name="varFromDirectory">指定資料夾路徑</param>
        /// <param name="varToDirectory">對應其他資料夾路徑</param>
        public static void DeleteFolderFiles(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    DeleteFolderFiles(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }


            string[] files = Directory.GetFiles(varFromDirectory);

            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Delete(varToDirectory + s.Substring(s.LastIndexOf("\\")));
                }
            }
        }
        #endregion

        #region 從檔的絕對路徑中獲取檔案名( 包含副檔名 )
        /// <summary>
        /// 從檔的絕對路徑中獲取檔案名( 包含副檔名 )
        /// </summary>
        /// <param name="filePath">檔的絕對路徑</param>        
        public static string GetFileName(string filePath)
        {
            //獲取檔的名稱
            FileInfo fi = new FileInfo(filePath);
            return fi.Name;
        }
        #endregion

        #region 複製檔參考方法,頁面中引用
        /// <summary>
        /// 複製檔參考方法,頁面中引用
        /// </summary>
        /// <param name="cDir">新路徑</param>
        /// <param name="TempId">範本引擎替換編號</param>
        public static void CopyFiles(string cDir, string TempId)
        {
            //if (Directory.Exists(Request.PhysicalApplicationPath + "\\Controls"))
            //{
            //    string TempStr = string.Empty;
            //    StreamWriter sw;
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Default.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Default.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Default.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Column.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Column.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\List.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Content.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Content.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\View.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\MoreDiss.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\MoreDiss.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\DissList.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\ShowDiss.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\ShowDiss.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Diss.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Review.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Review.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Review.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Search.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Search.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Search.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //}
        }
        #endregion

        #region 創建一個目錄
        /// <summary>
        /// 創建一個目錄
        /// </summary>
        /// <param name="directoryPath">目錄的絕對路徑</param>
        public static void CreateDirectory(string directoryPath)
        {
            //如果目錄不存在則創建該目錄
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        #endregion

        #region 創建一個檔
        /// <summary>
        /// 創建一個檔。
        /// </summary>
        /// <param name="filePath">檔的絕對路徑</param>
        public static void CreateFile(string filePath)
        {
            try
            {
                //如果檔不存在則創建該檔
                if (!IsExistFile(filePath))
                {
                    //創建一個FileInfo物件
                    FileInfo file = new FileInfo(filePath);

                    //創建文件
                    FileStream fs = file.Create();

                    //關閉文件流
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 創建一個檔,並將位元組流寫入檔。
        /// </summary>
        /// <param name="filePath">檔的絕對路徑</param>
        /// <param name="buffer">二進位流資料</param>
        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                //如果檔不存在則創建該檔
                if (!IsExistFile(filePath))
                {
                    //創建一個FileInfo物件
                    FileInfo file = new FileInfo(filePath);

                    //創建文件
                    FileStream fs = file.Create();

                    //寫入二進位流
                    fs.Write(buffer, 0, buffer.Length);

                    //關閉文件流
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 獲取文字檔的行數
        /// <summary>
        /// 獲取文字檔的行數
        /// </summary>
        /// <param name="filePath">檔的絕對路徑</param>        
        public static int GetLineCount(string filePath)
        {
            //將文字檔的各行讀到一個字串陣列中
            string[] rows = File.ReadAllLines(filePath);

            //返回行數
            return rows.Length;
        }
        #endregion

        #region 獲取一個檔的長度
        /// <summary>
        /// 獲取一個檔的長度,單位為Byte
        /// </summary>
        /// <param name="filePath">檔的絕對路徑</param>        
        public static long GetFileSize(string filePath)
        {
            //創建一個檔物件
            FileInfo fi = new FileInfo(filePath);

            //獲取文件的大小
            return fi.Length;
        }
        #endregion

        #region 獲取文件大小並以B，KB，GB，TB
        /// <summary>
        /// 計算檔大小函數(保留兩位元小數),Size為位元組大小
        /// </summary>
        /// <param name="size">初始文件大小</param>
        /// <returns></returns>
        public static string ToFileSize(long size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " 位元組";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " KB";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " MB";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " GB";
            return m_strSize;
        }
        #endregion

        #region 獲取指定目錄中的子目錄清單
        /// <summary>
        /// 獲取指定目錄及子目錄中所有子目錄列表
        /// </summary>
        /// <param name="directoryPath">指定目錄的絕對路徑</param>
        /// <param name="searchPattern">模式字串，"*"代表0或N個字元，"?"代表1個字元。
        /// 範例："Log*.xml"表示搜索所有以Log開頭的Xml檔。</param>
        /// <param name="isSearchChild">是否搜索子目錄</param>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 向文字檔寫入內容

        /// <summary>
        /// 向文字檔中寫入內容
        /// </summary>
        /// <param name="filePath">檔的絕對路徑</param>
        /// <param name="text">寫入的內容</param>
        /// <param name="encoding">編碼</param>
        public static void WriteText(string filePath, string text, Encoding encoding)
        {
            //向檔寫入內容
            File.WriteAllText(filePath, text, encoding);
        }
        #endregion

        #region 向文字檔的尾部追加內容
        /// <summary>
        /// 向文字檔的尾部追加內容
        /// </summary>
        /// <param name="filePath">檔的絕對路徑</param>
        /// <param name="content">寫入的內容</param>
        public static void AppendText(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }
        #endregion

        #region 將現有檔的內容複製到新檔中
        /// <summary>
        /// 將原始檔案的內容複製到目的檔案中
        /// </summary>
        /// <param name="sourceFilePath">原始檔案的絕對路徑</param>
        /// <param name="destFilePath">目的檔案的絕對路徑</param>
        public static void Copy(string sourceFilePath, string destFilePath)
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }
        #endregion

        #region 將檔移動到指定目錄
        /// <summary>
        /// 將檔移動到指定目錄
        /// </summary>
        /// <param name="sourceFilePath">需要移動的原始檔案的絕對路徑</param>
        /// <param name="descDirectoryPath">移動到的目錄的絕對路徑</param>
        public static void Move(string sourceFilePath, string descDirectoryPath)
        {
            //獲取原始檔案的名稱
            string sourceFileName = GetFileName(sourceFilePath);

            if (IsExistDirectory(descDirectoryPath))
            {
                //如果目標中存在同名檔,則刪除
                if (IsExistFile(descDirectoryPath + "\\" + sourceFileName))
                {
                    DeleteFile(descDirectoryPath + "\\" + sourceFileName);
                }
                //將檔移動到指定目錄
                File.Move(sourceFilePath, descDirectoryPath + "\\" + sourceFileName);
            }
        }
        #endregion

        #region 從檔的絕對路徑中獲取檔案名( 不包含副檔名 )
        /// <summary>
        /// 從檔的絕對路徑中獲取檔案名( 不包含副檔名 )
        /// </summary>
        /// <param name="filePath">檔的絕對路徑</param>        
        public static string GetFileNameNoExtension(string filePath)
        {
            //獲取檔的名稱
            FileInfo fi = new FileInfo(filePath);
            return fi.Name.Split('.')[0];
        }
        #endregion

        #region 從檔的絕對路徑中獲取副檔名
        /// <summary>
        /// 從檔的絕對路徑中獲取副檔名
        /// </summary>
        /// <param name="filePath">檔的絕對路徑</param>        
        public static string GetExtension(string filePath)
        {
            //獲取檔的名稱
            FileInfo fi = new FileInfo(filePath);
            return fi.Extension;
        }
        #endregion

        #region 清空指定目錄
        /// <summary>
        /// 清空指定目錄下所有檔及子目錄,但該目錄依然保存.
        /// </summary>
        /// <param name="directoryPath">指定目錄的絕對路徑</param>
        public static void ClearDirectory(string directoryPath)
        {
            directoryPath = HttpContext.Current.Server.MapPath(directoryPath);
            if (IsExistDirectory(directoryPath))
            {
                //刪除目錄中所有的檔
                string[] fileNames = GetFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    DeleteFile(fileNames[i]);
                }
                //刪除目錄中所有的子目錄
                string[] directoryNames = GetDirectories(directoryPath);
                for (int i = 0; i < directoryNames.Length; i++)
                {
                    DeleteDirectory(directoryNames[i]);
                }
            }
        }
        #endregion

        #region 清空檔內容
        /// <summary>
        /// 清空檔內容
        /// </summary>
        /// <param name="filePath">檔的絕對路徑</param>
        public static void ClearFile(string filePath)
        {
            //刪除檔
            File.Delete(filePath);

            //重新創建該檔
            CreateFile(filePath);
        }
        #endregion

        #region 刪除指定目錄
        /// <summary>
        /// 刪除指定目錄及其所有子目錄
        /// </summary>
        /// <param name="directoryPath">指定目錄的絕對路徑</param>
        public static void DeleteDirectory(string directoryPath)
        {
            directoryPath = HttpContext.Current.Server.MapPath(directoryPath);
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }
        #endregion

        #region 本地路徑
        /// <summary>
        /// 本地路徑
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string MapPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }
        #endregion
    }
}
