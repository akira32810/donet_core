using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore;

 

namespace ConferenceBarrel.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
        //    var blob = "hello";
            ViewData["test"] = DateTime.Now;
           
       // Response.WriteAsync("s",cancellationToken:null);
            return View();
        }
        private async Task TestStringBuilderFormat(HttpContext context)
    {
     StringBuilder sb = new StringBuilder(); 

        var id = "id";
        var size = "12";
        var text = "text";
        var template = "<li id='{0}' style='font-size:{1}'>{2}</li>";

        sb.Append("<div style='display:none'>");

        for (int i = 0; i < 10000; i++)
        {
            sb.AppendFormat(template, id, size, text);
        }

        sb.Append("</div>");

        await context.Response.WriteAsync(sb.ToString());
    }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
