using POne.dtb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POne.Lib
{
    public class Output
    {
        public static List<int> GetCustomerIDs()
        {
            var output = new List<int>();

            using (var context = new POneContext())
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

            using (var context = new POneContext())
            {
                foreach (var item in context.Customers)
                {
                    output.Add(item.Name);
                }
            }

            return output;
        }
    }
}
