using POne.dtb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POne.dtb
{
    public class Data
    {
        /// <summary>
        /// Connection string for the database, must be set at least once before usage
        /// </summary>
        public static string connection { get; private set; }

        /// <summary>
        /// set the connection string static variable
        /// </summary>
        /// <param name="x">Connection string</param>
        public static void SetConnectionString(string x) => connection = x;

        /// <summary>
        /// Add a location to the database
        /// </summary>
        /// <param name="name">Name of location</param>
        public static void AddLocation(string name)
        {
            using (var context = new POneContext(connection))
            {
                var location = new Locations { Name = name };
                context.Add(location);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// returns a product id from the product name
        /// </summary>
        /// <param name="name">item name</param>
        /// <returns>id of item</returns>
        public static int ProductFromName(string name)
        {
            using (var context = new POneContext(connection))
            {
                int output = 0;
                foreach (var item in context.Products) if (item.Name == name) output = item.PrdId;
                return output;
            }
        }

        /// <summary>
        /// adds a product to the database
        /// </summary>
        /// <param name="name">name of product</param>
        /// <param name="LocID">id of products location</param>
        /// <param name="p">price of item</param>
        /// <param name="s">ctock of item</param>
        public static void AddProduct(string name, int LocID, decimal p, int s)
        {
            using (var context = new POneContext(connection))
            {
                var product = new Products
                {
                    Name = name,
                    Loc = context.Locations.Find(LocID),
                    Price = p,
                    Stock = s
                };
                context.Add(product);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Place an order to the database
        /// </summary>
        /// <param name="customer">id of customer placeing order</param>
        /// <param name="order">object containing the list of order items</param>
        public static void AddOrder(int customer, IOrderList order)
        {
            using (var context = new POneContext(connection))
            {
                decimal price = 0;
                foreach (var orderdata in order.Cart)
                {
                    price += orderdata.Price * orderdata.Quantity;
                }
                var ticket = new Orders
                {
                    CustId = customer,
                    Total = price,
                    Stamp = DateTime.Now,
                    OrderData = order.Cart
                };
                foreach (var orderdata in order.Cart)
                {
                    context.Products.Find(orderdata.PrdId).Stock -= orderdata.Quantity;
                    orderdata.OrdId = ticket.OrdId;
                }
                context.Add(ticket);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// retrieve the price of a product
        /// </summary>
        /// <param name="PID">id of product</param>
        /// <returns>price of product</returns>
        public static decimal GetPrice(int PID)
        {
            using (var context = new POneContext(connection))
            {
                return context.Products.Find(PID).Price;
            }
        }

        /// <summary>
        /// add a customer to the database
        /// </summary>
        /// <param name="name">name of customer</param>
        public static void AddCustomer(string name)
        {
            using (var context = new POneContext(connection))
            {
                var customer = new Customers { Name = name };
                context.Add(customer);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// increce the stock of an item
        /// </summary>
        /// <param name="ID">items id</param>
        /// <param name="stock">quantity to9 add</param>
        public static void StockProduct(int ID, int stock)
        {
            using (var context = new POneContext(connection))
            {
                context.Products.Find(ID).Stock += stock;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// remove a store from the database
        /// only works when the store has no products
        /// </summary>
        /// <param name="activeStore">id of store</param>
        public static void RemoveStore(int activeStore)
        {
            using (var context = new POneContext(connection))
            {
                context.Locations.Remove(context.Locations.Find(activeStore));
                context.SaveChanges();
            }
        }

        /// <summary>
        /// retrieves customer name from id
        /// </summary>
        /// <param name="ID">customer id</param>
        /// <returns>name of customer</returns>
        public static string GetCustomer(int ID)
        {
            using (var context = new POneContext(connection))
            {
                return context.Customers.Find(ID).Name;
            }
        }

        /// <summary>
        /// retrieves store name from id
        /// </summary>
        /// <param name="ID">store id</param>
        /// <returns>store name</returns>
        public static string GetStore(int ID)
        {
            using (var context = new POneContext(connection))
            {
                return context.Locations.Find(ID).Name;
            }
        }

        /// <summary>
        /// returns a list of customer names
        /// </summary>
        /// <returns>names</returns>
        public static List<string> GetCustomers()
        {
            using (var context = new POneContext(connection))
            {
                return ((from f in context.Customers select f.Name).ToList());
            }
        }
        
        /// <summary>
        /// returns a list of store names
        /// </summary>
        /// <returns>store names</returns>
        public static List<string> GetStores()
        {
            using (var context = new POneContext(connection))
            {
                return ((from f in context.Locations select f.Name).ToList());
            }
        }

        /// <summary>
        /// returns a list of product names
        /// </summary>
        /// <param name="locationName">name of location</param>
        /// <returns>list of product names</returns>
        public static List<string> GetProductNames(string locationName)
        {
            using (var context = new POneContext(connection))
            {
                var output = new List<string>();
                int id = context.Locations.Find(LocIdFromName(locationName)).LocId;
                foreach (var item in context.Products)
                {
                    if (item.LocId == id)
                    {
                        output.Add(item.Name);
                    }
                }
                return output;
            }
        }
        /// <summary>
        /// returns a list of product prices
        /// </summary>
        /// <param name="locationName">name of location</param>
        /// <returns>list of product prices</returns>
        public static List<decimal> GetProductPrices(string locationName)
        {
            using (var context = new POneContext(connection))
            {
                var output = new List<decimal>();
                int id = context.Locations.Find(LocIdFromName(locationName)).LocId;
                foreach (var item in context.Products)
                {
                    if (item.LocId == id)
                    {
                        output.Add(item.Price);
                    }
                }
                return output;
            }
        }

        /// <summary>
        /// returns a list of product quantities
        /// </summary>
        /// <param name="locationName">name of location</param>
        /// <returns>list of quantities</returns>
        public static List<int> GetProductQuantities(string locationName)
        {
            using (var context = new POneContext(connection))
            {
                var output = new List<int>();
                int id = context.Locations.Find(LocIdFromName(locationName)).LocId;
                foreach (var item in context.Products)
                {
                    if (item.LocId == id)
                    {
                        output.Add(item.Stock);
                    }
                }
                return output;
            }
        }

        /// <summary>
        /// retrieves the id of a product from its name
        /// </summary>
        /// <param name="locationName">name of location</param>
        /// <returns>location id</returns>
        private static int LocIdFromName(string locationName)
        {
            using (var context = new POneContext(connection)) foreach (var item in context.Locations) if (item.Name == locationName) return item.LocId;
            return -1;
        }

        /// <summary>
        /// remove a customer from the database
        /// </summary>
        /// <param name="ID">id of customer</param>
        public static void RemoveCustomer(int ID)
        {
            using(var context = new POneContext(connection))
            {
                context.Customers.Remove(context.Customers.Find(ID));
                context.SaveChanges();
            }
        }
    }
}
