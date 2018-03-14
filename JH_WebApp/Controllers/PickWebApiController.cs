using CCM.Code;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace JH_WebApp.Controllers
{
    public class PickWebApiController : ApiController
    {
        ConnOracle connOra = new ConnOracle();
        
        /// <summary>
        /// 接收 JSON 字串
        /// 寫入MES_POLE
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet, HttpPost] 
        public HttpResponseMessage ReceivePole(string queryJson)
        {
            //var jsonstr = @"[{"poleno":"001","rollno":"1234567890"},{"poleno":"002","rollno":"0987654321"}]";
            // [{"_ID":"1","POLESN":"20171110001","POLENO":"001","ROLLNO_1":"6986hg09707","ROLLNO_2":"09709709h","ROLLNO_3":"","ROLLNO_4":""},{"_ID":"2","POLESN":"A20171120-001","POLENO":"001","ROLLNO_1":"123456","ROLLNO_2":"234567","ROLLNO_3":"345678","ROLLNO_4":""},{"_ID":"3","POLESN":"A20171120-002","POLENO":"002","ROLLNO_1":"234567","ROLLNO_2":"345678","ROLLNO_3":"","ROLLNO_4":""}]
            // 解析 JSON字串
            var data = JArray.Parse(queryJson);
            var res = connOra.insertPole(data);

            string json = JsonConvert.SerializeObject(res);
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StringContent(json);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return result;
        }

        /// <summary>
        /// 接收 JSON 字串
        /// 寫入MES_POOL
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public HttpResponseMessage ReceivePool(string queryJson)
        {
            //var jsonstr = @"[{"poleno":"001","rollno":"1234567890"},{"poleno":"002","rollno":"0987654321"}]";
            // [
            //  { "_ID":"1","POLESN":"A20171120-001","POOLNO":"W1","STEP":"1","BDT":"2017-11-20 06:32","EDT":"","CREATOR_TIME":"2017-11-20 06:32:24"},
            //  { "_ID":"2","POLESN":"A20171120-002","POOLNO":"E1","STEP":"1","BDT":"2017-11-20 06:35","EDT":"","CREATOR_TIME":"2017-11-20 06:35:23"}
            // ]
            // 解析 JSON字串
            var data = JArray.Parse(queryJson);
            var res = connOra.insertPool(data);  // 入料出料

            string json = JsonConvert.SerializeObject(res);
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StringContent(json);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return result;
        }


        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            //return "value";
            return "Hello Web API";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}