using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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

        private int CartSize
        {
            get => (int?)TempData.Peek("CartSize") ?? default;
            set => TempData["CartSize"] = value;
        }

        [NonAction]
        private int GetCartItemID(int i) => (int?)TempData.Peek($"ItemID{i}") ?? default;

        [NonAction]
        private int GetCartItemQuantity(int i) => (int?)TempData.Peek($"ItemQuantiny{i}") ?? default;

        [NonAction]
        private void SetCartItemID(int i, int ID) => TempData[$"ItemID{i}"] = ID;

        [NonAction]
        private void SetCartItemQuantity(int i, int Quantity) => TempData[$"ItemQuantiny{i}"] = Quantity;

        [NonAction]
        private bool PopCart()
        {
            if (CartSize != 0)
            {
                int? ID = (int?)TempData[$"ItemID{CartSize-1}"];
                ID = (int?)TempData[$"ItemQuantity{CartSize-1}"];
                CartSize--;
                return true;
            }
            return false;
        }

        [NonAction]
        private void EmptyCart() { while (PopCart()); }

        [NonAction]
        private CartItem GetCartItem(int i) => new CartItem
        {
            cartIndex = i,
            ID = GetCartItemID(i),
            Quantity = GetCartItemQuantity(i)
        };


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

        public IActionResult ListProducts(int? ID)
        {
            var Models = new List<ProductModel>();

            ViewData["LocationName"] = Output.GetLocationName(ID ?? default);
            TempData["LocationID"] = ID ?? default;

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

            ViewData["CustomerName"] = Output.GetPersonNames(ID);

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
            TempData["ProductID"] = ID;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Restock(Restock x)
        {
            if (ModelState.IsValid)
            {
                Input.Restock((int)TempData["ProductID"], x.quantity);
                return RedirectToAction("ListProducts", new { ID = (int)TempData["LocationID"] });
            }

            return View();
        }

        public IActionResult AddToCart(int ID, bool error = false)
        {
            bool exists = false;
            int CartID = 0;
            for (int i = 0; i < CartSize; i++)
            {
                if (GetCartItem(i).ID == ID)
                {
                    exists = true;
                    CartID = i;
                }
            }
            if (exists)
            {
                return RedirectToAction("EditCartItemQuantity", new { ID = CartID } );
            }
            
            ViewData["ItemData1"] = $"{Output.GetProductName(ID)}";
            ViewData["ItemData2"] = $"Price:{Output.GetProductPrice(ID)} In Stock:{Output.GetItemStock(ID)}";
            if (error) ViewData["ItemData3"] = "Quantity must be under stock";
            else ViewData["ItemData3"] = "";

            TempData["ItemID"] = ID;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(CartItem item)
        {
            int ID = (int)TempData["ItemID"];
            
            if (ModelState.IsValid)
            {
                if (item.Quantity <= Output.GetItemStock(ID))
                {
                    SetCartItemID(CartSize, ID);
                    SetCartItemQuantity(CartSize, item.Quantity);
                    CartSize++;
                
                    return RedirectToAction("Index");
                }

                return RedirectToAction("AddToCart", new { ID = ID, error = true } );
            }

            return View();
        }

        public IActionResult ListCart()
        {
            var Models = new List<CartItem>();
            decimal sum = 0;

            for (int i = 0; i < CartSize; i++)
            {
                Models.Add(new CartItem
                {
                    ID = GetCartItemID(i),
                    Quantity = GetCartItemQuantity(i),
                    cartIndex = i
                }); ;

                sum += Models[i].Total;
            }

            ViewData["Total"] = sum;

            return View(Models);
        }

        public IActionResult EditCartItemQuantity(int ID, bool error = false)
        {
            var item = GetCartItem(ID);

            ViewData["ItemData1"] = $"{item.ItemName}";
            ViewData["ItemData2"] = $"Price:{item.Price} In Stock:{Output.GetItemStock(item.ID)}";
            if (error) ViewData["ItemData3"] = "Quantity must be under stock";
            else ViewData["ItemData3"] = "";
            ViewData["ItemData4"] = $"Editing quantity from : {item.Quantity}";

            TempData["ItemID"] = ID;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCartItemQuantity(CartItem item)
        {
            int ID = (int)TempData["ItemID"];

            if (ModelState.IsValid)
            {
                if (item.Quantity <= Output.GetItemStock(GetCartItem(ID).ID))
                {
                    SetCartItemQuantity(ID, item.Quantity);

                    return RedirectToAction("Index");
                }

                return RedirectToAction("EditCartItemQuantity", new { ID = ID, error = true });
            }

            return View();
        }

        public IActionResult DeleteCartItem(int ID)
        {
            SetCartItemID(ID, GetCartItemID(CartSize-1));
            SetCartItemQuantity(ID, GetCartItemQuantity(CartSize - 1));
            PopCart();

            if (CartSize < 1) return RedirectToAction("Index");

            return RedirectToAction("ListCart");
        }

        public IActionResult PlaceOrder(int ID)
        {
            var cart = new Output(ID);
            for (int i = 0; i < CartSize; i++)
            {
                cart.AddOrder(GetCartItemID(i),GetCartItemQuantity(i));
            }
            cart.PlaceOrder();
            EmptyCart();

            return RedirectToAction("Index");
        }
    }
}
