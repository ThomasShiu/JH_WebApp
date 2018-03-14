using CCM.Code;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace JH_WebApp
{
    /// <summary>
    /// 身分驗證
    /// </summary>
    public class HandlerAuthorizeAttribute : ActionFilterAttribute
    {
       
        public bool Ignore { get; set; }
        public HandlerAuthorizeAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {


            if (!Expired())
            {
                StringBuilder sbScript = new StringBuilder();
                sbScript.Append("<script type='text/javascript'>alert('很抱歉！您的許可權過期，訪問被拒絕！');</script>");
                filterContext.Result = new ContentResult() { Content = sbScript.ToString() };
                return;
            }
           
        }
        public bool Expired()
        {
            int vNow = int.Parse(DateTime.Now.ToString("yyyyMMdd"));

            //var result = false;
            string a = Configs.GetValue("abcedfg");
            if (a == "") { a = "MjAxODAzMzA="; }  // 20180330
            //還原
            string vExpaird = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(a));
            int expire = int.Parse(vExpaird);

            if (expire >= vNow)
            {
                return true;
            }

            return false;
        }
    }
}