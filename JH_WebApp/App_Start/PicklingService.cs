using CCM.Code;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace JH_WebApp
{
    public class PicklingService
    {       
        static string connStr = "OracleDbContext";
        //ConnOracle connOra = new ConnOracle();
        string v_sql = "";

        #region  儲存入料時間
        public String SavePolein(string poledata)
        {
            string v_polesn = "", v_poolno = "", v_bdt = "", v_edt = "";

            var queryParam = poledata.ToJObject();

            try
            {
                if (!queryParam["polesn"].IsEmpty())
                {
                    v_polesn = queryParam["polesn"].ToString();
                }
                if (!queryParam["poolno"].IsEmpty())
                {
                    v_poolno = queryParam["poolno"].ToString();
                }
                if (!queryParam["bdt"].IsEmpty())
                {
                    v_bdt = queryParam["bdt"].ToString();
                }
                if (!queryParam["edt"].IsEmpty())
                {
                    v_edt = queryParam["edt"].ToString();
                }
                JArray InsertArray = new JArray();
                var colObject = new JObject
                {

                   {"POLESN",v_polesn },
                   {"POOLNO",v_poolno },
                   {"BDT",v_bdt },
                   {"EDT",v_edt },

                };
                InsertArray.Add(colObject);
                // 寫入桿號卷號資料
                var res = insertPool(InsertArray);

                return "success";
            }catch{
                return "error";
            }

        }

        #endregion

        #region  產生桿次,並寫入桿號卷號資料
        public String GenPolesn(string poledata)
        {
            string v_polesn="", v_poleno="", v_rollno1="", v_rollno2="", v_rollno3="", v_rollno4="",v_rust="",v_pdate="";

            v_sql = SQLSyntaxHelper.ReadSQLFile("GEN_POLESN.SQL");
            v_sql = string.Format(v_sql).Replace("\r\n", " ");
            //得到一個DataTable物件
            DataTable dt = queryDataTable(v_sql, connStr);

            // POLESN, POLENO,ROLLNO_1,ROLLNO_2,ROLLNO_3,ROLLNO_4,ROLLNO_5 

            var detail = (from p in dt.AsEnumerable()
                         select new
                         {
                             POLESN = p.Field<string>("POLESN"),
                         }).First();

            v_polesn = detail.POLESN;

            var queryParam = poledata.ToJObject();
            if (!queryParam["poleno"].IsEmpty())
            {
                v_poleno = queryParam["poleno"].ToString();
            }
            if (!queryParam["rollno1"].IsEmpty())
            {
                v_rollno1 = queryParam["rollno1"].ToString();
            }
            if (!queryParam["rollno2"].IsEmpty())
            {
                v_rollno2 = queryParam["rollno2"].ToString();
            }
            if (!queryParam["rollno3"].IsEmpty())
            {
                v_rollno3 = queryParam["rollno3"].ToString();
            }
            if (!queryParam["rollno4"].IsEmpty())
            {
                v_rollno4 = queryParam["rollno4"].ToString();
            }
            if (!queryParam["rust"].IsEmpty())
            {
                v_rust = queryParam["rust"].ToString();
            }
            if (!queryParam["pdate"].IsEmpty())
            {
                v_pdate = queryParam["pdate"].ToString();
            }
            JArray InsertArray = new JArray();
            var colObject = new JObject
                {
                   {"POLESN",v_polesn },
                   {"POLENO",v_poleno },
                   {"ROLLNO_1",v_rollno1 },
                   {"ROLLNO_2",v_rollno2 },
                   {"ROLLNO_3",v_rollno3 },
                   {"ROLLNO_4",v_rollno4 },
                   {"RUST",v_rust },
                   {"PDATE",v_pdate },
                };
            InsertArray.Add(colObject);
            // 寫入桿號卷號資料
            var res = insertPole(InsertArray);

            return v_polesn;

        }

        #endregion

        #region  查詢桿次資料 - ChoicePole , PolesnInPool PolesnOutPool
        public JArray GetPoleData(string v_type, string v_pole)
        {

            if (v_type.Equals("poleno"))
            {
                v_sql = SQLSyntaxHelper.ReadSQLFile("POLE_PICKLING_QUERY1.SQL");
            }
            else if (v_type.Equals("polesn"))
            {
                v_sql = SQLSyntaxHelper.ReadSQLFile("POLE_PICKLING_QUERY2.SQL");
            }

          
            v_sql = string.Format(v_sql, v_pole).Replace("\r\n", " ");
            //得到一個DataTable物件
            DataTable dt = queryDataTable(v_sql, connStr);

            // POLESN, POLENO,ROLLNO_1,ROLLNO_2,ROLLNO_3,ROLLNO_4,ROLLNO_5 
            JArray MixArray = new JArray();
            var detail = from p in dt.AsEnumerable()
                         select new
                         {
                             POLESN = p.Field<string>("POLESN"),
                             POLENO = p.Field<string>("POLENO"),
                             ROLLNO_1 = p.Field<string>("ROLLNO_1"),
                             ROLLNO_2 = p.Field<string>("ROLLNO_2"),
                             ROLLNO_3 = p.Field<string>("ROLLNO_3"),
                             ROLLNO_4 = p.Field<string>("ROLLNO_4"),
                             //ROLLNO_5 = p.Field<string>("ROLLNO_5"),
                         };

            int totalCount = detail.Count();
            foreach (var col in detail)
            {
                var colObject = new JObject
                {
                   {"POLESN",col.POLESN },
                   {"POLENO",col.POLENO },
                   {"ROLLNO_1",col.ROLLNO_1 },
                   {"ROLLNO_2",col.ROLLNO_2 },
                   {"ROLLNO_3",col.ROLLNO_3 },
                   {"ROLLNO_4",col.ROLLNO_4 },
                   //{"ROLLNO_5",col.ROLLNO_5 },
                };
                MixArray.Add(colObject);
            }
            return MixArray;

        }
       
        #endregion

        #region  桿號資料
        public JArray GetPolesnList(string keyword = "")
        {
            v_sql = SQLSyntaxHelper.ReadSQLFile("POLE_PICKLING_QUERY3.SQL");
         
            v_sql = string.Format(v_sql, keyword).Replace("\r\n"," ");
            //得到一個DataTable物件
            DataTable dt = queryDataTable(v_sql, connStr);

            //MACHINE_NAME,MO_LIST,TYPE1,SDT,TASK,COLOR
            JArray MixArray = new JArray();
            var detail = from p in dt.AsEnumerable()
                         select new
                         {
                             POLESN = p.Field<string>("POLESN"),
                             POLENO = p.Field<string>("POLENO"),
                             ROLLNO_1 = p.Field<string>("ROLLNO_1"),
                             ROLLNO_2 = p.Field<string>("ROLLNO_2"),
                             ROLLNO_3 = p.Field<string>("ROLLNO_3"),
                             ROLLNO_4 = p.Field<string>("ROLLNO_4"),
                             FINISHED = p.Field<string>("FINISHED"),

                         };

            int totalCount = detail.Count();
            foreach (var col in detail)
            {
                var colObject = new JObject
                {
                   {"POLESN",col.POLESN },
                   {"POLENO",col.POLENO },
                   {"ROLLNO_1",col.ROLLNO_1 },
                   {"ROLLNO_2",col.ROLLNO_2 },
                   {"ROLLNO_3",col.ROLLNO_3 },
                   {"ROLLNO_4",col.ROLLNO_4 },
                   {"FINISHED",col.FINISHED },
                };
                MixArray.Add(colObject);
            }
            return MixArray;

        }

        #endregion

        #region  桿次IO
        public JArray GetPoleIO(string v_polesn = "")
        {
            v_sql = SQLSyntaxHelper.ReadSQLFile("INOUT_PICKLING.SQL");

            v_sql = string.Format(v_sql, v_polesn).Replace("\r\n", " ");
            //得到一個DataTable物件
            DataTable dt = queryDataTable(v_sql, connStr);

            //  POLESN, POOLNO, STEP, BDT,EDT,LEAD_EDT,FINISHED,ROLLNO_1,ROLLNO_2,ROLLNO_3,ROLLNO_4
            JArray MixArray = new JArray();
            var detail = from p in dt.AsEnumerable()
                         select new
                         {
                             POLESN = p.Field<string>("POLESN"),
                             POLENO = p.Field<string>("POLENO"),
                             POOLNO = p.Field<string>("POOLNO"),
                             BDT = p.Field<DateTime?>("BDT"),
                             LEAD_EDT = p.Field<DateTime?>("LEAD_EDT"),
                             ROLLNO_1 = p.Field<string>("ROLLNO_1"),
                             ROLLNO_2 = p.Field<string>("ROLLNO_2"),
                             ROLLNO_3 = p.Field<string>("ROLLNO_3"),
                             ROLLNO_4 = p.Field<string>("ROLLNO_4"),
                             FINISHED = p.Field<string>("FINISHED"),
                             GAP_MIN = p.Field<Decimal?>("GAP_MIN"),
                         };

            int totalCount = detail.Count();
            foreach (var col in detail)
            {
                var colObject = new JObject
                {
                   {"POLESN",col.POLESN },
                   {"POLENO",col.POLENO },
                   {"POOLNO",col.POOLNO },
                   {"BDT",col.BDT },
                   {"LEAD_EDT",col.LEAD_EDT },
                   {"ROLLNO_1",col.ROLLNO_1 },
                   {"ROLLNO_2",col.ROLLNO_2 },
                   {"ROLLNO_3",col.ROLLNO_3 },
                   {"ROLLNO_4",col.ROLLNO_4 },
                   {"FINISHED",col.FINISHED },
                   {"GAP_MIN",col.GAP_MIN },
                };
                MixArray.Add(colObject);
            }
            return MixArray;

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

        #region  寫入MES_POOL
        public string insertPool(JArray data)
        {
            var list = from p in data
                       select new
                       {
                           POLESN = (string)p["POLESN"],
                           POOLNO = (string)p["POOLNO"],
                           BDT = (DateTime?)p["BDT"],
                           EDT = (DateTime?)((p["EDT"].ToString() == "") ? null : p["EDT"]),
                       };
            using (OracleConnection objConn = new OracleConnection(
                  ConfigurationManager.ConnectionStrings[connStr].ConnectionString))
            {

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = objConn;
                objCmd.CommandText = " INSERT INTO MES_INOUT_PICKLING(SN,POLESN,POOLNO,BDT,EDT) " +
                                     " Values (MES_POOL_SEQ.NEXTVAL,:POLESN,:POOLNO,:BDT,:EDT)";
                OracleParameter pPOLESN = objCmd.Parameters.Add("POLESN", OracleDbType.Varchar2);
                OracleParameter pPOOLNO = objCmd.Parameters.Add("POOLNO", OracleDbType.Varchar2);
                //OracleParameter pSTEP = objCmd.Parameters.Add("STEP", OracleDbType.Decimal);
                OracleParameter pBDT = objCmd.Parameters.Add("BDT", OracleDbType.Date);
                OracleParameter pEDT = objCmd.Parameters.Add("EDT", OracleDbType.Date);
                //OracleParameter pPDATE = objCmd.Parameters.Add("PDATE", OracleDbType.Date);

                try
                {
                    objConn.Open();
                    // 列出每筆資料做寫入
                    foreach (var col in list)
                    {
                        pPOLESN.Value = col.POLESN;
                        pPOOLNO.Value = col.POOLNO;
                        pBDT.Value = col.BDT;
                        pEDT.Value = col.EDT;
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
                           PDATE = (DateTime?)p["PDATE"],
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
                        pPDATE.Value = col.PDATE;
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

        #region  寫入MES_POLE
        public string updatePole(string polesn)
        {
            using (OracleConnection objConn = new OracleConnection(
                  ConfigurationManager.ConnectionStrings[connStr].ConnectionString))
            {

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = objConn;
                objCmd.CommandText = " UPDATE JHEIP.MES_POLE_PICKLING SET FINISHED = 'Y' WHERE POLESN =:POLESN ";

                OracleParameter pPOLESN = objCmd.Parameters.Add("POLESN", OracleDbType.Varchar2);

                try
                {
                    objConn.Open();
                    pPOLESN.Value = polesn;
                    objCmd.ExecuteNonQuery();
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

        #region  檢查上製程是否為W2
        public String CheckProcess(string v_polesn)
        {
            String v_process = "";
            v_sql = SQLSyntaxHelper.ReadSQLFile("CHK_PROCESS.SQL");

            v_sql = string.Format(v_sql, v_polesn).Replace("\r\n", " ");
            //得到一個DataTable物件
            DataTable dt = queryDataTable(v_sql, connStr);

            // 找該桿次最後製程是否為W2
            if (dt.Rows.Count > 0) { v_process = dt.Select()[0]["POOLNO"].ToString(); }

            return v_process;
  
        }
        #endregion

        #region 更新W2出料時間
        public string savePoolout(string poledata)
        {
            string v_polesn = "", v_poolno = "",v_edt = "";
            
            var queryParam = poledata.ToJObject();
            if (!queryParam["polesn"].IsEmpty())
            {
                v_polesn = queryParam["polesn"].ToString();
            }
            if (!queryParam["poolno"].IsEmpty())
            {
                v_poolno = queryParam["poolno"].ToString();
            }
            if (!queryParam["edt"].IsEmpty())
            {
                v_edt = queryParam["edt"].ToString();
            }

          
            using (OracleConnection objConn = new OracleConnection(
                  ConfigurationManager.ConnectionStrings[connStr].ConnectionString))
            {

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = objConn;
                objCmd.CommandText = " UPDATE MES_INOUT_PICKLING SET EDT= TO_DATE('"+v_edt+ "', 'YYYY-MM-DD HH24:MI') WHERE POLESN=:POLESN AND POOLNO=:POOLNO ";

                OracleParameter pPOLESN = objCmd.Parameters.Add("POLESN", OracleDbType.Varchar2);
                OracleParameter pPOOLNO = objCmd.Parameters.Add("POOLNO", OracleDbType.Varchar2);

                try
                {
                    objConn.Open();

                    pPOLESN.Value = v_polesn;
                    pPOOLNO.Value = v_poolno;
                    objCmd.ExecuteNonQuery();

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
    }
}