using System.Web;
using System.Web.Mvc;

namespace PLM
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    if (!filterContext.ExceptionHandled)
        //    {
        //        string controller = filterContext.RouteData.Values["controller"].ToString();
        //        string action = filterContext.RouteData.Values["action"].ToString();
        //        Exception ex = filterContext.Exception;
        //        //do something with these details here
        //        RedirectToAction("Error", "Home");
        //    }

        //}
    }
}
