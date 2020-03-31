using POne.dtb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POne.Lib
{
    internal class Validation
    {
        internal static bool CustName(string name)
        {
            int index = name.IndexOf(',');
            
            if (index != -1)
            {
                if (name.Substring(index + 1).IndexOf(',') != -1)
                {
                    return false;
                }
            }

            return true;
        }

        internal static bool LocID(int ID)
        {
            using (var context = new POneContext())
            {
                foreach (var item in context.Locations)
                {
                    if (ID == item.LocId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal static bool CustID(int ID)
        {
            using (var context = new POneContext())
            {
                foreach (var item in context.Customers)
                {
                    if (ID == item.CustId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
