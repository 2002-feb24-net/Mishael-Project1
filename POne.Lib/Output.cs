using POne.dtb;
using POne.dtb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POne.Lib
{
    public class Output : IOrderList
    {
        /// <summary>
        /// id of customer to place the order
        /// 
        /// active when output is used as an object
        /// </summary>
        public int CustID { get; set; }
        /// <summary>
        /// list of orders
        /// 
        /// active when output is used as an object
        /// </summary>
        public List<OrderData> Cart { get; set; }

        /// <summary>
        /// checks if the location is removable
        /// </summary>
        /// <param name="ID">location id</param>
        /// <returns>true if removable</returns>
        public static bool IsRemovableLocation(int? ID)
        {
            using (var context = new POneContext(Data.connection))
            {
                bool hasDependencies = false;
                foreach (var item in context.Products)
                {
                    hasDependencies = hasDependencies || item.LocId == ID;
                }
                return !hasDependencies;
            }
        }

        /// <summary>
        /// constructs an object to maintain an order cart
        /// </summary>
        /// <param name="ID">id of person to place order</param>
        public Output(int ID)
        {
            Cart = new List<OrderData>();
            CustID = ID;
        }

        /// <summary>
        /// checks if posible to remove the customer from the database
        /// </summary>
        /// <param name="ID">id of customer</param>
        /// <returns>returns true if removable</returns>
        public static bool IsRemovableCustomer(int ID)
        {
            using (var context = new POneContext(Data.connection))
            {
                bool hasDependencies = false;
                foreach (var item in context.Orders)
                {
                    hasDependencies = hasDependencies || item.CustId == ID;
                }
                return hasDependencies;
            }
        }

        /// <summary>
        /// checks if the product is removable
        /// </summary>
        /// <param name="ID">product id</param>
        /// <returns>true if removable</returns>
        public static bool IsRemovableProduct(int ID)
        {
            using (var context = new POneContext(Data.connection))
            {
                bool hasDependencies = false;
                foreach (var item in context.OrderData)
                {
                    hasDependencies = hasDependencies || item.PrdId == ID;
                }
                return !hasDependencies;
            }
        }

        /// <summary>
        /// adds an item to the cart
        /// </summary>
        /// <param name="ID">id of item</param>
        /// <param name="quantity">ammount</param>
        public void AddOrder(int ID, int quantity)
        {
            Cart.Add(new OrderData
            {
                PrdId = ID,
                Quantity = quantity,
                Price = Data.GetPrice(ID)
            });
        }

        /// <summary>
        /// submits the order
        /// does not empty cart
        /// </summary>
        public void PlaceOrder()
        {
            Data.AddOrder(CustID, this);
        }

        /// <summary>
        /// retrieves a list of customer ids
        /// </summary>
        /// <returns>ids</returns>
        public static List<int> GetCustomerIDs()
        {
            var output = new List<int>();

            using (var context = new POneContext(POne.dtb.Data.connection))
            {
                foreach (var item in context.Customers)
                {
                    output.Add(item.CustId);
                }
            }

            return output;
        }

        /// <summary>
        /// retrieves the price of a product
        /// </summary>
        /// <param name="ID">product id</param>
        /// <returns>price</returns>
        public static decimal GetProductPrice(int ID)
        {
            using (var context = new POneContext(dtb.Data.connection))
            {
                foreach (var item in context.Products)
                {
                    if (item.PrdId == ID)
                    {
                        return item.Price;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// retrives a list of all customer names
        /// </summary>
        /// <returns>names</returns>
        public static List<string> GetCustomerNames()
        {
            var output = new List<string>();

            using (var context = new POneContext(POne.dtb.Data.connection))
            {
                foreach (var item in context.Customers)
                {
                    output.Add(item.Name);
                }
            }

            return output;
        }

        /// <summary>
        /// retrieves a list of location ids
        /// </summary>
        /// <returns>ids</returns>
        public static List<int> GetLocationIDs()
        {
            var output = new List<int>();

            using (var context = new POneContext(POne.dtb.Data.connection))
            {
                foreach (var item in context.Locations)
                {
                    output.Add(item.LocId);
                }
            }

            return output;
        }

        /// <summary>
        /// retrieves a list of location names
        /// </summary>
        /// <returns>names</returns>
        public static List<string> GetLocationNames()
        {
            var output = new List<string>();

            using (var context = new POneContext(POne.dtb.Data.connection))
            {
                foreach (var item in context.Locations)
                {
                    output.Add(item.Name);
                }
            }

            return output;
        }

        /// <summary>
        /// retrieves name of customer
        /// </summary>
        /// <param name="ID">id of customer</param>
        /// <returns>name of person</returns>
        public static string GetPersonName(int? ID)
        {
            return Data.GetCustomer(ID ?? default);
        }

        /// <summary>
        /// retrieves a list of product ids
        /// </summary>
        /// <param name="ID">id of location to search</param>
        /// <returns>ids</returns>
        public static List<int> GetProductIDs(int ID)
        {
            if (Validation.LocID(ID))
            {
                var output = new List<int>();

                using (var context = new POneContext(POne.dtb.Data.connection))
                {
                    foreach (var item in context.Products)
                    {
                        if (ID == item.LocId)
                        {
                            output.Add(item.PrdId);
                        }
                    }
                }

                return output;
            }

            throw new Exception("Error: Invalid Location Reference Cought By Server");
        }

        /// <summary>
        /// retrieves a list of product names
        /// </summary>
        /// <param name="ID">id of location to search</param>
        /// <returns>names</returns>
        public static List<string> GetProductNames(int ID)
        {
            if (Validation.LocID(ID))
            {
                var output = new List<string>();

                using (var context = new POneContext(POne.dtb.Data.connection))
                {
                    foreach (var item in context.Products)
                    {
                        if (ID == item.LocId)
                        {
                            output.Add(item.Name);
                        }
                    }
                }

                return output;
            }

            throw new Exception("Error: Invalid Location Reference Cought By Server");
        }

        /// <summary>
        /// retrieves a list of product prices
        /// </summary>
        /// <param name="ID">id of location to search</param>
        /// <returns>list of prices</returns>
        public static List<decimal> GetProductPrices(int ID)
        {
            if (Validation.LocID(ID))
            {
                var output = new List<decimal>();

                using (var context = new POneContext(POne.dtb.Data.connection))
                {
                    foreach (var item in context.Products)
                    {
                        if (ID == item.LocId)
                        {
                            output.Add(item.Price);
                        }
                    }
                }

                return output;
            }

            throw new Exception("Error: Invalid Location Reference Cought By Server");
        }

        /// <summary>
        /// retrieves the stock of an item
        /// </summary>
        /// <param name="ID">id of product</param>
        /// <returns>stock</returns>
        public static List<int> GetProductStock(int ID)
        {
            if (Validation.LocID(ID))
            {
                var output = new List<int>();

                using (var context = new POneContext(POne.dtb.Data.connection))
                {
                    foreach (var item in context.Products)
                    {
                        if (ID == item.LocId)
                        {
                            output.Add(item.Stock);
                        }
                    }
                }

                return output;
            }

            throw new Exception("Error: Invalid Location Reference Cought By Server");
        }

        /// <summary>
        /// retrieves the name of a product
        /// </summary>
        /// <param name="ID">id of product</param>
        /// <returns>name</returns>
        public static string GetLocationName(int ID)
        {
            if (Validation.LocID(ID))
            {
                string output = "";

                using (var context = new POneContext(dtb.Data.connection))
                {
                    output = context.Locations.Find(ID).Name;
                }

                return output;
            }

            throw new Exception("Error: Invalid Location Reference Cought By Server");
        }

        /// <summary>
        /// retrieves a list of product names as they apear in the order history
        /// </summary>
        /// <param name="ID">id of location</param>
        /// <returns>list of product names</returns>
        public static List<string> GetLocationHistoryNames(int ID)
        {
            if (Validation.LocID(ID))
            {
                var output = new List<string>();

                using (var context = new POneContext(POne.dtb.Data.connection))
                {
                    var orders = context.Orders.ToList();
                    var data = context.OrderData.ToList();
                    foreach (var itemX in orders)
                    {
                        foreach (var itemY in data)
                        {
                            if (itemX.OrdId == itemY.OrdId && context.Products.Find(itemY.PrdId).LocId == ID)
                            {
                                output.Add(context.Products.Find(itemY.PrdId).Name);
                            }
                        }
                    }
                }

                return output;
            }

            throw new Exception("Error: Invalid Location Reference Cought By Server");
        }

        /// <summary>
        /// retieves a list of product prices as the apear in the order history
        /// </summary>
        /// <param name="ID">location id</param>
        /// <returns>list of prices</returns>
        public static List<decimal> GetLocationHistoryPrice(int ID)
        {
            if (Validation.LocID(ID))
            {
                var output = new List<decimal>();

                using (var context = new POneContext(POne.dtb.Data.connection))
                {
                    var orders = context.Orders.ToList();
                    var data = context.OrderData.ToList();
                    foreach (var itemX in orders)
                    {
                        foreach (var itemY in data)
                        {
                            if (itemX.OrdId == itemY.OrdId && context.Products.Find(itemY.PrdId).LocId == ID)
                            {
                                output.Add(itemY.Price);
                            }
                        }
                    }
                }

                return output;
            }

            throw new Exception("Error: Invalid Location Reference Cought By Server");
        }

        /// <summary>
        /// retrieves a list of timestamps of orders
        /// </summary>
        /// <param name="ID">id of location to search</param>
        /// <returns>list of timestamps</returns>
        public static List<DateTime> GetLocationHistoryStamp(int ID)
        {
            if (Validation.LocID(ID))
            {
                var output = new List<DateTime>();

                using (var context = new POneContext(POne.dtb.Data.connection))
                {
                    var orders = context.Orders.ToList();
                    var data = context.OrderData.ToList();
                    foreach (var itemX in orders)
                    {
                        foreach (var itemY in data)
                        {
                            if (itemX.OrdId == itemY.OrdId && context.Products.Find(itemY.PrdId).LocId == ID)
                            {
                                output.Add(itemX.Stamp);
                            }
                        }
                    }
                }

                return output;
            }

            throw new Exception("Error: Invalid Location Reference Cought By Server");
        }

        /// <summary>
        /// retrieve a list of names of products
        /// </summary>
        /// <param name="ID">id of location to search</param>
        /// <returns></returns>
        public static string GetPersonNames(int ID)
        {
            if (Validation.CustID(ID))
            {
                using (var context = new POneContext(dtb.Data.connection))
                {
                    return context.Customers.Find(ID).Name;
                }
            }

            throw new Exception("Error: Invalid Customer Reference Cought By Server");
        }

        /// <summary>
        /// retrieves the stock of an item
        /// </summary>
        /// <param name="ID">id of item</param>
        /// <returns>stock</returns>
        public static int GetItemStock(int ID)
        {
            using (var context = new POneContext(dtb.Data.connection))
            {
                foreach (var item in context.Products)
                {
                    if (item.PrdId == ID)
                    {
                        return item.Stock;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// checks if a product still exists in the database
        /// </summary>
        /// <param name="ID">ID of item</param>
        /// <returns>true if product exists</returns>
        public static bool ProductExists(int ID)
        {
            return Validation.ProdID(ID);
        }

        /// <summary>
        /// retrieves name of product
        /// </summary>
        /// <param name="ID">id of product</param>
        /// <returns>name</returns>
        public static string GetProductName(int ID)
        {
            using (var context = new POneContext(dtb.Data.connection))
            {
                foreach (var item in context.Products)
                {
                    if (item.PrdId == ID)
                    {
                        return item.Name;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// retrieves quantities of orders placed
        /// </summary>
        /// <param name="ID">id of location</param>
        /// <returns>list of quantities</returns>
        public static List<int> GetLocationHistoryQuantity(int ID)
        {
            if (Validation.LocID(ID))
            {
                var output = new List<int>();

                using (var context = new POneContext(POne.dtb.Data.connection))
                {
                    var orders = context.Orders.ToList();
                    var data = context.OrderData.ToList();
                    foreach (var itemX in orders)
                    {
                        foreach (var itemY in data)
                        {
                            if (itemX.OrdId == itemY.OrdId && context.Products.Find(itemY.PrdId).LocId == ID)
                            {
                                output.Add(itemY.Quantity);
                            }
                        }
                    }
                }

                return output;
            }

            throw new Exception("Error: Invalid Location Reference Cought By Server");
        }

        /// <summary>
        /// quaries the database order history
        /// </summary>
        /// <typeparam name="T">type of property to search</typeparam>
        /// <param name="ID">id of person</param>
        /// <param name="get">function that retrieves the quaried item from the order history instance </param>
        /// <returns>list of quaried data</returns>
        private static List<T> SearchPerson<T>(int ID, Func<Orders, OrderData, T> get)
        {
            if (Validation.CustID(ID))
            {
                var output = new List<T>();

                using (var context = new POneContext(POne.dtb.Data.connection))
                {
                    var orders = context.Orders.ToList();
                    var data = context.OrderData.ToList();
                    foreach (var itemX in orders)
                    {
                        foreach (var itemY in data)
                        {
                            if (itemX.OrdId == itemY.OrdId && itemX.CustId == ID)
                            {
                                output.Add(get(itemX, itemY));
                            }
                        }
                    }
                }

                return output;
            }

            throw new Exception("Error: Invalid Customer Reference Cought By Server");
        }

        /// <summary>
        /// retrieves a list of product names as they appear in the order history
        /// </summary>
        /// <param name="ID">person id</param>
        /// <returns>list of names</returns>
        public static List<string> GetPersonHistoryNames(int ID) =>
            SearchPerson<string>(ID, (order,orderData) => GetProductName(orderData.PrdId));

        /// <summary>
        /// retrieves a list of time stamps as they appear in the order history
        /// </summary>
        /// <param name="ID">id of person</param>
        /// <returns>list of time stamps</returns>
        public static List<DateTime> GetPersonHistoryStamp(int ID) =>
            SearchPerson<DateTime>(ID, (order, orderData) => order.Stamp);

        /// <summary>
        /// retrieves a list of quantities as they appear in the order history
        /// </summary>
        /// <param name="ID">id of person</param>
        /// <returns>list of order quantities</returns>
        public static List<int> GetPersonHistoryQuantity(int ID) =>
            SearchPerson<int>(ID, (order, orderData) => orderData.Quantity);

        /// <summary>
        /// retrieves a list of prices as they appear in the order history
        /// </summary>
        /// <param name="ID">id of person</param>
        /// <returns>list of prices</returns>
        public static List<decimal> GetPersonHistoryPrice(int ID) =>
            SearchPerson<decimal>(ID, (order, orderData) => orderData.Price);
    }
}
