using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using POne.Lib;
using POne.Models;

namespace POne.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AddPerson()
        {
            ViewBag.Message = "";

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPerson(CustomerModel input)
        {
            if(ModelState.IsValid)
            {
                Input.AddPerson(input.FName, input.LName);
                return RedirectToAction("ListPeople");
            }

            return View();
        }

        [NonAction]
        public string FistNameOf(string x)
        {
            int index = x.IndexOf(',');
            if (index == -1) return x;
            else return x.Substring(0, index);
        }

        [NonAction]
        public string LastNameOf(string x)
        {
            int index = x.IndexOf(',');
            if (index == -1) return "";
            else return x.Substring(index + 1, x.Length - index - 1);
        }

        public IActionResult ListPeople()
        {
            var Models = new List<CustomerModel>();

            List<int> IDs = Output.GetCustomerIDs();
            List<string> names = Output.GetCustomerNames();

            for (int i = 0; i < IDs.Count; i++)
            {
                Models.Add(new CustomerModel {
                    FName = FistNameOf(names[i]),
                    LName = LastNameOf(names[i]),
                    CustId = IDs[i]
                });
            }
            
            return View(Models);
        }

        public IActionResult RemovePerson(int? ID)
        {
            Input.RemovePerson(ID ?? default(int));

            return RedirectToAction("ListPeople");
        }
    }
}
