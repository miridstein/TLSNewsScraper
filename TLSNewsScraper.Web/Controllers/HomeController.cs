using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TLSNewsScraper.Web.Models;
using TLSNewsScraper.Data;

namespace TLSNewsScraper.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string html = Api.GetTLSHtml();
            var items = Api.GetItems(html);
            return View(items);
        }

        
    }
}
