using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace JH_WebApp
{
    public class ConnOracle
    {
        //string Sql_str;
        static string connStr = "OracleDbContext";


        // 寫入MES_POLE
        #region  寫入MES_POLE
        public string insertPole(JArray data)
        {
            /*
            [
            { "_ID":"1","POLESN":"20171110001","POLENO":"001","ROLLNO_1":"6986hg09707","ROLLNO_2":"09709709h","ROLLNO_3":"","ROLLNO_4":""},
            { "_ID":"2","POLESN":"A20171120-001","POLENO":"001","ROLLNO_1":"123456","ROLLNO_2":"234567","ROLLNO_3":"345678","ROLLNO_4":""},
            {"_ID":"3","POLESN":"A20171120-002","POLENO":"002","ROLLNO_1":"234567","ROLLNO_2":"345678","ROLLNO_3":"","ROLLNO_4":""}]
            */
            var list = from p in data
                       select new
                       {
                           POLESN = (string)p["POLESN"],
                           POLENO = (string)p["POLENO"],
                           ROLLNO1 = (string)p["ROLLNO_1"],
                           ROLLNO2 = (string)p["ROLLNO_2"],
                           ROLLNO3 = (string)p["ROLLNO_3"],
                           ROLLNO4 = (string)p["ROLLNO_4"],
                           //ROLLNO5 = (string)p["rollno5"],
                           //ROLLNO6 = (string)p["rollno6"],
                           RUST = (string)p["RUST"],
                           CREATOR_TIME   = (DateTime?)p["CREATOR_TIME"],
                       };
            using (OracleConnection objConn = new OracleConnection(
                  ConfigurationManager.ConnectionStrings[connStr].ConnectionString))
            {

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = objConn;
                objCmd.CommandText = " INSERT INTO MES_POLE_PICKLING(SN,POLESN, POLENO,ROLLNO_1, ROLLNO_2,ROLLNO_3,ROLLNO_4,RUST,PDATE) " +
                                     " Values (MES_POLE_SEQ.NEXTVAL,:POLESN, :POLENO,:ROLLNO_1, :ROLLNO_2,:ROLLNO_3,:ROLLNO_4,:RUST,:PDATE)";
                OracleParameter pPOLESN = objCmd.Parameters.Add("POLESN", OracleDbType.Varchar2);
                OracleParameter pPOLENO = objCmd.Parameters.Add("POLENO", OracleDbType.Varchar2);
                OracleParameter pROLLNO1 = objCmd.Parameters.Add("ROLLNO_1", OracleDbType.Varchar2);
                OracleParameter pROLLNO2 = objCmd.Parameters.Add("ROLLNO_2", OracleDbType.Varchar2);
                OracleParameter pROLLNO3 = objCmd.Parameters.Add("ROLLNO_3", OracleDbType.Varchar2);
                OracleParameter pROLLNO4 = objCmd.Parameters.Add("ROLLNO_4", OracleDbType.Varchar2);
                //OracleParameter pROLLNO5 = objCmd.Parameters.Add("ROLLNO_5", OracleDbType.Varchar2);
                //OracleParameter pROLLNO6 = objCmd.Parameters.Add("ROLLNO_6", OracleDbType.Varchar2);
                OracleParameter pRUST = objCmd.Parameters.Add("RUST", OracleDbType.Varchar2);
                OracleParameter pPDATE = objCmd.Parameters.Add("PDATE", OracleDbType.Date);


                try
                {
                    objConn.Open();
                    // 列出每筆資料做寫入
                    foreach (var col in list)
                    {
                        pPOLESN.Value = col.POLESN;
                        pPOLENO.Value = col.POLENO;
                        pROLLNO1.Value = col.ROLLNO1;
                        pROLLNO2.Value = col.ROLLNO2;
                        pROLLNO3.Value = col.ROLLNO3;
                        pROLLNO4.Value = col.ROLLNO4;
                        //pROLLNO5.Value = col.ROLLNO5;
                        //pROLLNO6.Value = col.ROLLNO6;
                        pRUST.Value = col.RUST;
                        pPDATE.Value = col.CREATOR_TIME;
                        objCmd.ExecuteNonQuery();
                    }
                    return "success";
                }
                catch (Exception ex)
                {
                    objCmd.Dispose();
                    objConn.Dispose();
                    objConn.Close();
                    return "error";
                    throw ex;
                }
                finally
                {
                    objCmd.Dispose();
                    objConn.Dispose();
                    objConn.Close();
                }
            }

        }
        #endregion

        #region  寫入MES_POOL
        public string insertPool(JArray data)
        {
            /*
            [
            { "_ID":"1","POLESN":"20171110001","POLENO":"001","ROLLNO_1":"6986hg09707","ROLLNO_2":"09709709h","ROLLNO_3":"","ROLLNO_4":""},
            { "_ID":"2","POLESN":"A20171120-001","POLENO":"001","ROLLNO_1":"123456","ROLLNO_2":"234567","ROLLNO_3":"345678","ROLLNO_4":""},
            {"_ID":"3","POLESN":"A20171120-002","POLENO":"002","ROLLNO_1":"234567","ROLLNO_2":"345678","ROLLNO_3":"","ROLLNO_4":""}]
            */
            var list = from p in data
                       select new
                       {
                           POLESN = (string)p["POLESN"],
                           POOLNO = (string)p["POOLNO"],
                           STEP = (string)p["STEP"],
                           BDT = (DateTime?)p["BDT"],
                           EDT = (DateTime?)((p["EDT"].ToString() =="") ? null : p["EDT"]),
                           CREATOR_TIME = (DateTime?)p["CREATOR_TIME"],
                           //ROLLNO5 = (string)p["rollno5"],
                           //ROLLNO6 = (string)p["rollno6"],
                           //PDATE   = (string)p["pdate"],
                       };
            using (OracleConnection objConn = new OracleConnection(
                  ConfigurationManager.ConnectionStrings[connStr].ConnectionString))
            {

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = objConn;
                objCmd.CommandText = " INSERT INTO MES_INOUT_PICKLING(SN,POLESN,POOLNO,STEP,BDT,EDT,PDATE) " +
                                     " Values (MES_POOL_SEQ.NEXTVAL,:POLESN,:POOLNO,:STEP,:BDT,:EDT,:PDATE)";
                OracleParameter pPOLESN = objCmd.Parameters.Add("POLESN", OracleDbType.Varchar2);
                OracleParameter pPOOLNO = objCmd.Parameters.Add("POOLNO", OracleDbType.Varchar2);
                OracleParameter pSTEP = objCmd.Parameters.Add("STEP", OracleDbType.Decimal);
                OracleParameter pBDT = objCmd.Parameters.Add("BDT", OracleDbType.Date);
                OracleParameter pEDT = objCmd.Parameters.Add("EDT", OracleDbType.Date);
                OracleParameter pPDATE = objCmd.Parameters.Add("PDATE", OracleDbType.Date);

                try
                {
                    objConn.Open();
                    // 列出每筆資料做寫入
                    foreach (var col in list)
                    {
                        pPOLESN.Value = col.POLESN;
                        pPOOLNO.Value = col.POOLNO;
                        pSTEP.Value = col.STEP;
                        pBDT.Value = col.BDT;
                        pEDT.Value = col.EDT;
                        pPDATE.Value = col.CREATOR_TIME;
                        //pROLLNO5.Value = col.ROLLNO5;
                        //pROLLNO6.Value = col.ROLLNO6;
                        //pPDATE.Value = col.PDATE;
                        objCmd.ExecuteNonQuery();
                    }
                    return "success";
                }
                catch (Exception ex)
                {
                    objCmd.Dispose();
                    objConn.Dispose();
                    objConn.Close();
                    return "error";
                    throw ex;
                }
                finally
                {
                    objCmd.Dispose();
                    objConn.Dispose();
                    objConn.Close();
                }
            }

        }
        #endregion


        #region 回傳DataTable物件
        /// <summary>
        /// 依據SQL語句，回傳DataTable物件
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable queryDataTable(string sql, string connstr)
        {
            DataSet ds = new DataSet();
            using (OracleConnection objConn = new OracleConnection(ConfigurationManager.ConnectionStrings[connStr].ConnectionString))
            {
                objConn.Open();
                OracleDataAdapter da = new OracleDataAdapter(sql, objConn);
                da.Fill(ds);
                objConn.Close();//結束連線
            }
            return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }
        #endregion
    }

}