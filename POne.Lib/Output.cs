using POne.dtb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POne.Lib
{
    public class Output
    {
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
    }
}
