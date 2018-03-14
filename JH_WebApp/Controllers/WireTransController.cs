using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JH_WebApp.Controllers
{
    public class WireTransController : Controller
    {
        string Sql_str;
        static string connStr = "jheip";
        //static string connStr = "JHERPDB";
        //thomas_Conn tsconn = new thomas_Conn();
        //thomas_function ts_Fun = new thomas_function();

        // GET: WireTrans
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Receive(string pv_job_no, string pv_coil_no, string pd_proc_date, string pv_proc_emp, string pn_item_no)
        {
            using (OracleConnection objConn = new OracleConnection(
                  ConfigurationManager.ConnectionStrings[connStr].ConnectionString))
            {

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = objConn;
                objCmd.CommandText = "SP_INSERT_APC2210M";
                objCmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    objCmd.Parameters.Add("pv_job_no", OracleDbType.Varchar2).Value = pv_job_no;        //工令單號
                    objCmd.Parameters.Add("pv_coil_no", OracleDbType.Varchar2).Value = pv_coil_no;      //卷號
                    objCmd.Parameters.Add("pd_proc_date", OracleDbType.Date).Value = DateTime.ParseExact(pd_proc_date, "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces); //領用日期
                    objCmd.Parameters.Add("pv_proc_emp", OracleDbType.Varchar2).Value = pv_proc_emp;   //領用人員

                    if (!pn_item_no.Equals(""))
                    {
                        objCmd.Parameters.Add("pn_item_no", OracleDbType.Decimal).Value = Convert.ToDecimal(pn_item_no);   //工令項次
                    }

                    objCmd.Parameters.Add("pv_err_msg", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;


                    objConn.Open();
                    objCmd.ExecuteNonQuery();

                    //v_getValue = (string)objCmd.Parameters["pv_err_msg"].Value;
                    //Response.Write("Success," + pv_job_no + "," + pv_coil_no + "," + pd_proc_date + "," + pv_proc_emp );
                    string v_WireDate = ""; // GetWireDate(pv_coil_no);

                    if (objCmd.Parameters["pv_err_msg"].Value is DBNull)
                    {
                        Response.Write("Success," + v_WireDate);
                    }
                    else
                    {
                        Response.Write(objCmd.Parameters["pv_err_msg"].Value);
                    }
                }
                catch (Exception ex)
                {
                    objCmd.Dispose();
                    objConn.Dispose();
                    objConn.Close();
                    //err_mess4.Text = "發生異常：" + ex.ToString();
                    Response.Write(ex.ToString());
                }
                finally
                {
                    objCmd.Dispose();
                    objConn.Dispose();
                    objConn.Close();
                }
            }

            return View();
        }


        public ActionResult Finish(string pv_job_no, string pv_coil_no, string pd_proc_date, string pv_proc_emp, string pv_class_no)
        {
            using (OracleConnection objConn = new OracleConnection(
                ConfigurationManager.ConnectionStrings[connStr].ConnectionString))
            {

                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = objConn;
                objCmd.CommandText = "SP_INSERT_APC2330M";
                objCmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    objCmd.Parameters.Add("pv_job_no", OracleDbType.Varchar2).Value = pv_job_no;        //工令單號
                    objCmd.Parameters.Add("pv_coil_no", OracleDbType.Varchar2).Value = pv_coil_no;      //卷號
                    objCmd.Parameters.Add("pd_proc_date", OracleDbType.Date).Value = DateTime.ParseExact(pd_proc_date, "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces); //領用日期
                    objCmd.Parameters.Add("pv_proc_emp", OracleDbType.Varchar2).Value = pv_proc_emp;   //領用人員
                    objCmd.Parameters.Add("pv_class_no", OracleDbType.Varchar2).Value = pv_class_no;    //班別

                    objCmd.Parameters.Add("pv_err_msg", OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output;


                    objConn.Open();
                    objCmd.ExecuteNonQuery();

                    //v_getValue = (string)objCmd.Parameters["pv_err_msg"].Value;
                    //Response.Write("Success," + pv_job_no + "," + pv_coil_no + "," + pd_proc_date + "," + pv_proc_emp );
                    string v_WireDate = ""; // GetWireDate(pv_coil_no);

                    if (objCmd.Parameters["pv_err_msg"].Value is DBNull)
                    {
                        Response.Write("Success," + v_WireDate);
                    }
                    else
                    {
                        Response.Write(objCmd.Parameters["pv_err_msg"].Value);
                    }
                }
                catch (Exception ex)
                {
                    objCmd.Dispose();
                    objConn.Dispose();
                    objConn.Close();
                    //err_mess4.Text = "發生異常：" + ex.ToString();
                    Response.Write(ex.ToString());
                }
                finally
                {
                    objCmd.Dispose();
                    objConn.Dispose();
                    objConn.Close();
                }
            }

            return View();
        }


        //取得盤元基本資料
        private ActionResult GetWireDate(string v_coil_no)
        {
            Sql_str = "SELECT COIL_NO,WIR_KIND,DRAW_DIA,LUO_NO,COIL_WT FROM ASK_DRW_STOK WHERE COIL_NO = :COIL_NO";


            //建立連線
            //使用web.config conn string
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["jheip"].ConnectionString);
            conn.Open();


            OracleCommand cmd = new OracleCommand(Sql_str, conn);
            cmd.Parameters.Add(new OracleParameter("COIL_NO", OracleDbType.Varchar2, 100));
            cmd.Parameters["COIL_NO"].Value = v_coil_no;

            OracleDataReader dr = cmd.ExecuteReader();

            try
            {
                //while (dr.Read())
                //{
                //組成JSON字串
                if (dr.HasRows)
                {
                    dr.Read();
                    string COIL_NO = dr["COIL_NO"].ToString();
                    string WIR_KIND = dr["WIR_KIND"].ToString();
                    string LUO_NO = dr["LUO_NO"].ToString();
                    string DRAW_DIA = dr["DRAW_DIA"].ToString();
                    string COIL_WT = dr["COIL_WT"].ToString();
                    //材質、爐號、線徑、重量
                    return View(WIR_KIND + "," + LUO_NO + "," + DRAW_DIA + "," + COIL_WT);

                }
                else
                {
                    return View();
                }

            }
            catch (Exception ex)
            {
                return View(ex.ToString());
                //tsconn.save_log("guest", "QueryWires_01", "192.168.0.19", ex.ToString());
                //Response.Write(ex.ToString());
            }
            finally
            {
                cmd.Dispose();
                dr.Close();
                dr.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }

    }
}