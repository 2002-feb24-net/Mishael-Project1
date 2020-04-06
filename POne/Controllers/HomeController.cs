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
                _logger.LogInformation($"Adding {input.FName}, {input.LName} to database");
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

        public IActionResult RemoveItem(int ID)
        {
            _logger.LogInformation($"removing product {Output.GetProductName(ID)} from database");

            Input.RemoveProduct(ID);

            return RedirectToAction("ListProducts", new { ID = TempData["LocationID"] });
        }

        public IActionResult ListPeople()
        {
            var Models = new List<CustomerModel>();

            List<int> IDs = Output.GetCustomerIDs();
            List<string> names = Output.GetCustomerNames();

            _logger.LogInformation("retrieveing list of people from database");

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

            _logger.LogInformation($"removeing {Output.GetPersonName(ID)} from database");

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
                _logger.LogInformation($"adding {input.Name} to database");
                Input.AddLocation(input.Name);
                return RedirectToAction("ListLocations");
            }

            return View();
        }

        public IActionResult ListLocations()
        {
            var Models = new List<LocationModel>();

            _logger.LogInformation("quarying database for locations");

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
            _logger.LogInformation($"removeing {Output.GetLocationName(ID ?? default)} from database");
            
            Input.RemoveLocation(ID ?? default);

            return RedirectToAction("ListLocations");
        }

        public IActionResult ListProducts(int? ID)
        {
            var Models = new List<ProductModel>();

            ViewData["LocationName"] = Output.GetLocationName(ID ?? default);
            TempData["LocationID"] = ID ?? default;

            _logger.LogInformation($"quarying database for products within {Output.GetLocationName(ID ?? default)}");

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
                _logger.LogInformation($"adding product \'{product.Name}\' to database");
                Input.AddProduct(product.Name, (int)TempData["LocationID"], product.Price, product.Stock);
                return RedirectToAction("ListLocations", new { ID = (int)TempData["LocationID"] } );
            }

            return View();
        }

        public IActionResult LocationOrderHistory(int ID)
        {
            var Models = new List<OrderHistoryData>();

            ViewData["LocationName"] = Output.GetLocationName(ID);

            _logger.LogInformation($"quarying database for order data of location \'{Output.GetLocationName(ID)}\'");

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

            _logger.LogInformation($"quarying database for order data of person \'{Output.GetPersonName(ID)}\'");

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
            TempData["ProductName"] = Output.GetProductName(ID);
            TempData["ProductID"] = ID;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Restock(Restock x)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"adding {x.quantity} to {TempData["ProductName"]}");
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
                    _logger.LogInformation($"adding {item.Quantity} of {item.ItemName} to cart");
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
                _logger.LogInformation("quarying cart for items");

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
                    _logger.LogInformation($"changeing quantity of {item.ItemName} in cart to {item.Quantity}");
                    
                    SetCartItemQuantity(ID, item.Quantity);

                    return RedirectToAction("Index");
                }

                return RedirectToAction("EditCartItemQuantity", new { ID = ID, error = true });
            }

            return View();
        }

        public IActionResult DeleteCartItem(int ID)
        {
            _logger.LogInformation($"removeing {GetCartItem(ID).ItemName} from the cart");
            
            SetCartItemID(ID, GetCartItemID(CartSize-1));
            SetCartItemQuantity(ID, GetCartItemQuantity(CartSize - 1));
            PopCart();

            if (CartSize < 1) return RedirectToAction("Index");

            return RedirectToAction("ListCart");
        }

        public IActionResult PlaceOrder(int ID)
        {
            var cart = new Output(ID);

            if (!CheckCartForError())
            {
                return RedirectToAction("CartError");
            }

            for (int i = 0; i < CartSize; i++)
            {
                cart.AddOrder(GetCartItemID(i),GetCartItemQuantity(i));
            }
            _logger.LogInformation($"placeing order under {Output.GetPersonName(ID)}");
            cart.PlaceOrder();
            EmptyCart();

            return RedirectToAction("Index");
        }

        [NonAction]
        public bool CheckCartForError()
        {
            bool valid = true;
            TempData["ErrorData"] = "";

            for (int i = 0; i < CartSize; i++)
            {
                if (Output.ProductExists(GetCartItemID(i)))
                {
                    if (GetCartItemQuantity(i) > Output.GetItemStock(GetCartItemID(i)))
                    {
                        TempData["ErrorData"] += $"Product \'{GetCartItem(i).ItemName}\'" +
                            $" has decreaced in stock, its quantity in the cart has been updated\n";
                        SetCartItemQuantity(i, Output.GetItemStock(GetCartItemID(i)));
                        valid = false;
                    }
                }
                else
                {
                    valid = false;
                    TempData["ErrorData"] += $"A product no longer exists, it has been removed from the cart\n";
                    DeleteCartItem(i--);
                }
            }

            return valid;
        }

        public IActionResult CartError()
        {
            return View();
        }
    }
}
