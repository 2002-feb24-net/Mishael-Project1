using PZero.dtb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PZero.dtb
{
    public class Data
    {
        public static void AddLocation(string name)
        {
            using (var context = new PZeroContext())
            {
                var location = new Locations { Name = name };
                context.Add(location);
                context.SaveChanges();
            }
        }

        public static int GetQuantity(int ID, Func<int> GetInt)
        {
            using (var context = new PZeroContext())
            {
                int max = context.Products.Find(ID).Stock;
                int input = GetInt();
                while (input > max || input <0)
                {
                    Console.WriteLine("input out of range");
                    input = GetInt();
                }
                return input;
            }
        }

        public static int ProductFromName(string name)
        {
            using (var context = new PZeroContext())
            {
                int output = 0;
                foreach (var item in context.Products) if (item.Name == name) output = item.PrdId;
                return output;
            }
        }

        public static void AddProduct(string name, int LocID, decimal p, int s = -1)
        {
            using (var context = new PZeroContext())
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

        public static void AddOrder(int customer, IOrderList order)
        {
            using (var context = new PZeroContext())
            {
                decimal price = 0;
                foreach (var orderdata in order.Cart)
                {
                    price += orderdata.Price;
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

        public static decimal GetPrice(int PID)
        {
            using (var context = new PZeroContext())
            {
                return context.Products.Find(PID).Price;
            }
        }

        public static void AddCustomer(string name)
        {
            using (var context = new PZeroContext())
            {
                var customer = new Customers { Name = name };
                context.Add(customer);
                context.SaveChanges();
            }
        }

        public static void StockProduct(int ID, int stock)
        {
            using (var context = new PZeroContext())
            {
                context.Products.Find(ID).Stock += stock;
                context.SaveChanges();
            }
        }

        public static void RemoveStore(int activeStore)
        {
            using (var context = new PZeroContext())
            {
                context.Locations.Remove(context.Locations.Find(activeStore));
                context.SaveChanges();
            }
        }

        public static int FindCustomer(Action<List<string>> OutputNames, Func<string> GetName)
        {
            List<string> found = new List<string>();
            int output = -1;

            while (found.Count != 1)
            {
                OutputNames(found);
                while (found.Count() > 0) found.RemoveAt(0);
                string input = GetName();
                using (var context = new PZeroContext())
                {
                    var temp = context.Customers.Where(b => b.Name.Contains(input)).ToList();
                    foreach (var i in temp)
                    {
                        if (i.Name == input)
                        {
                            while (found.Count() > 0) found.RemoveAt(0);
                            output = i.CustId;
                            found.Add(i.Name);
                            break;
                        }
                        found.Add(i.Name);
                    }
                }
            }

            if(output == -1)
            {
                using (var context = new PZeroContext())
                {
                    output = context.Customers.Where(b => b.Name.Contains(found[0])).ToList()[0].CustId;
                }
            }

            return output;
        }

        public static int FindLocation(Action<List<string>> OutputNames, Func<string> GetName)
        {
            List<string> found = new List<string>();
            int output = -1;

            while (found.Count != 1)
            {
                OutputNames(found);
                while (found.Count() > 0) found.RemoveAt(0);
                string input = GetName();
                using (var context = new PZeroContext())
                {
                    var temp = context.Locations.Where(b => b.Name.Contains(input)).ToList();
                    foreach (var i in temp)
                    {
                        if (i.Name == input)
                        {
                            while (found.Count() > 0) found.RemoveAt(0);
                            output = i.LocId;
                            found.Add(i.Name);
                            break;
                        }
                        found.Add(i.Name);
                    }
                }
            }

            if (output == -1)
            {
                using (var context = new PZeroContext())
                {
                    output = context.Locations.Where(b => b.Name.Contains(found[0])).First().LocId;
                }
            }

            return output;
        }

        public static string GetCustomer(int ID)
        {
            using (var context = new PZeroContext())
            {
                return context.Customers.Find(ID).Name;
            }
        }

        public static string GetStore(int ID)
        {
            using (var context = new PZeroContext())
            {
                return context.Locations.Find(ID).Name;
            }
        }

        public static List<string> GetCustomers()
        {
            using (var context = new PZeroContext())
            {
                return ((from f in context.Customers select f.Name).ToList());
            }
        }
        public static List<string> GetStores()
        {
            using (var context = new PZeroContext())
            {
                return ((from f in context.Locations select f.Name).ToList());
            }
        }

        public static List<string> GetProductNames(string locationName)
        {
            using (var context = new PZeroContext())
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

        public static List<decimal> GetProductPrices(string locationName)
        {
            using (var context = new PZeroContext())
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

        public static List<int> GetProductQuantities(string locationName)
        {
            using (var context = new PZeroContext())
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

        private static int LocIdFromName(string locationName)
        {
            using (var context = new PZeroContext()) foreach (var item in context.Locations) if (item.Name == locationName) return item.LocId;
            return -1;
        }

        public static void RemoveCustomer(int ID)
        {
            using(var context = new PZeroContext())
            {
                context.Customers.Remove(context.Customers.Find(ID));
                context.SaveChanges();
            }
        }
    }
}
