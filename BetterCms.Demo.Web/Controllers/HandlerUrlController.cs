using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetterCms.Demo.Web.Controllers
{
    public class HandlerUrlController : Controller
    {
        //
        // GET: /HandlerController/

        public ActionResult News(string id)
        {
            string url = "/news?id=" + id;
            Server.TransferRequest(url);
            return new EmptyResult();
        }

    }
}
