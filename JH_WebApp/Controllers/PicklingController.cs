using CCM.Code;
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
    public class PicklingController : Controller
    {

        private PicklingService picks = new PicklingService();

        // 首頁
        [HandlerAuthorize]
        public ActionResult Index()
        {
            return View();
        }
        // 掃描桿號卷號
        [HandlerAuthorize]
        public ActionResult BarcodeScan()
        {
            return View();
        }

        // 選擇桿次
        [HandlerAuthorize]
        public ActionResult PolesnChoice()
        {
            return View();
        }

        // 桿次入料時間
        [HandlerAuthorize]
        public ActionResult PolesnInPool()
        {
            return View();
        }

        // 桿次出料時間
        [HandlerAuthorize]
        public ActionResult PolesnOutPool()
        {
            return View();
        }

        // 查詢入料出料
        [HandlerAuthorize]
        public ActionResult QueryIO()
        {
            return View();
        }
        // 查詢桿次
        [HandlerAuthorize]
        public ActionResult QueryPole()
        {
            return View();
        }

        // 產生桿號,並寫入桿次資料庫
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult SavePolein(string poledata)
        {
            var result = picks.SavePolein(poledata);
            return Content(result);
        }

        // 產生桿號,並寫入桿次資料庫
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GenPolesn(string poledata)
        {
            var result = picks.GenPolesn(poledata);
            return Content(result);
        }

        // 取得桿次資料 by no
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetPoleList(string poleno)
        { 
           var result = picks.GetPoleData("poleno",poleno);
            var data = new
            {
                pole = result
            };

            return Content(data.ToJson());
        }

        // 取得桿次資料 by sn
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetPoleList2(string polesn)
        {
            var result = picks.GetPoleData("polesn",polesn);
            var data = new
            {
                pole = result
            };

            return Content(data.ToJson());
        }

        // 取得桿次,桿號,卷號 清單
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetPoleList3(string keyword)
        {
            var result = picks.GetPolesnList(keyword);
            var data = new
            {
                pole = result
            };

            return Content(data.ToJson());
        }

        // 取得桿次 IO 時間
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetPoleIO(string polesn)
        {
            var result = picks.GetPoleIO(polesn);
            var data = new
            {
                pole = result
            };

            return Content(data.ToJson());
        }

        // 取得桿次 IO 時間
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult SetPoleDone(string polesn)
        {
            var result = picks.updatePole(polesn);
            return Content(result);

           
        }
    }
}