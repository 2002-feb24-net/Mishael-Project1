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

        [NonAction]
        private int GetCartItemID(int i) => (int)TempData[$"CartItem{i}"];

        [NonAction]
        private void SetCartItemID(int i, int ID) => TempData[$"CartItem{i}"] = ID;

        [NonAction]
        private int GetCartItemQuantity(int i) => (int)TempData[$"CartStock{i}"];

        [NonAction]
        private void SetCartItemQuantity(int i, int Quantity) => TempData[$"CartStock{i}"] = Quantity;

        [NonAction]
        private int GetCartSize() => (int)TempData[$"CartSize"];

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AddPerson()
        {
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
            Input.RemovePerson(ID ?? default);

            return RedirectToAction("ListPeople");
        }

        public IActionResult AddLocation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddLocation(LocationModel input)
        {
            if (ModelState.IsValid)
            {
                Input.AddLocation(input.Name);
                return RedirectToAction("ListLocations");
            }

            return View();
        }

        public IActionResult ListLocations()
        {
            var Models = new List<LocationModel>();

            List<int> IDs = Output.GetLocationIDs();
            List<string> names = Output.GetLocationNames();

            for (int i = 0; i < IDs.Count; i++)
            {
                Models.Add(new LocationModel
                {
                    Name = names[i],
                    LocID = IDs[i]
                });
            }

            return View(Models);
        }

        public IActionResult RemoveLocation(int? ID)
        {
            Input.RemoveLocation(ID ?? default);

            return RedirectToAction("ListLocations");
        }

        public IActionResult ListProductsEdit(int? ID)
        {
            var Models = new List<ProductModel>();

            ViewData["LocationName"] = Output.GetLocationName(ID ?? default);
            ViewData["ID"] = ID;

            List<int> IDs = Output.GetProductIDs(ID ?? default);
            List<string> names = Output.GetProductNames(ID ?? default);
            List<decimal> prices = Output.GetProductPrices(ID ?? default);
            List<int> stock = Output.GetProductStock(ID ?? default);

            if (IDs.Count == 0)
            {
                Models.Add(new ProductModel() { LocId = ID ?? default, Name = "none" });
            }

            for (int i = 0; i < IDs.Count; i++)
            {
                Models.Add(new ProductModel
                {
                    PrdId = IDs[i],
                    Name = names[i],
                    Price = prices[i],
                    Stock = stock[i],
                    LocId = ID ?? default
                });
            }

            return View(Models);
        }

        public IActionResult AddProduct(int ID)
        {
            ViewData["LocationName"] = Output.GetLocationName(ID);

            TempData["LocationID"] = ID;

            return View(new ProductModel() { LocId = ID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                Input.AddProduct(product.Name, (int)TempData["LocationID"], product.Price, product.Stock);
                return RedirectToAction("ListLocations", new { ID = (int)TempData["LocationID"] } );
            }

            return View();
        }

        public IActionResult LocationOrderHistory(int ID)
        {
            var Models = new List<OrderHistoryData>();

            ViewData["LocationName"] = Output.GetLocationName(ID);

            List<string> names = Output.GetLocationHistoryNames(ID);
            List<decimal> prices = Output.GetLocationHistoryPrice(ID);
            List<DateTime> stamps = Output.GetLocationHistoryStamp(ID);
            List<int> quantitys = Output.GetLocationHistoryQuantity(ID);

            for (int i = 0; i < names.Count; i++)
            {
                Models.Add(new OrderHistoryData
                {
                    Quantity = quantitys[i],
                    ProductName = names[i],
                    Price = prices[i],
                    Stamp = stamps[i]
                });
            }

            return View(Models);
        }

        public IActionResult CustomerOrderHistory(int ID)
        {
            var Models = new List<OrderHistoryData>();

            ViewData[""] = Output.GetPersonNames(ID);

            List<string> names = Output.GetPersonHistoryNames(ID);
            List<decimal> prices = Output.GetPersonHistoryPrice(ID);
            List<DateTime> stamps = Output.GetPersonHistoryStamp(ID);
            List<int> quantitys = Output.GetPersonHistoryQuantity(ID);

            for (int i = 0; i < names.Count; i++)
            {
                Models.Add(new OrderHistoryData
                {
                    Quantity = quantitys[i],
                    ProductName = names[i],
                    Price = prices[i],
                    Stamp = stamps[i]
                });
            }

            return View(Models);

        }

        public IActionResult Restock(int ID)
        {
            ViewData["ProductID"] = Output.GetProductName(ID);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Restock(Restock x)
        {
            Input.Restock((int)TempData["ProductID"], x.quantity);

            return RedirectToAction("ListProductsEdit", new { ID = (int)TempData["LocationID"] });
        }
    }
}
