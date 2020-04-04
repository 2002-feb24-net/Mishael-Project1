using POne.dtb;
using System;

namespace POne.Lib
{
    public class Input
    {
        static public void AddPerson(string Fname, string Lname)
        {
            string name = Fname + "," + Lname;

            if (Validation.CustName(name))
            {
                Data.AddCustomer(name);
                return;
            }

            throw new Exception("Error: Invalid Person Name Cought By Server");
        }

        static public void AddLocation(string name) => Data.AddLocation(name);

        static public void AddProduct(string name, int LocID, decimal p, int s)
        {
            if (Validation.LocID(LocID))
            {
                Data.AddProduct(name, LocID, p, s);
                return;
            }

            throw new Exception("Error: Invalid Location Reference Cought By Server");
        }

        public static void RemovePerson(int ID)
        {
            if (Validation.CustID(ID))
            {
                Data.RemoveCustomer(ID);
                return;
            }

            throw new Exception("Error: Invalid Person Reference Cought By Server");
        }

        public static void RemoveLocation(int ID)
        {
            if (Validation.LocID(ID))
            {
                Data.RemoveStore(ID);
                return;
            }

            throw new Exception("Error: Invalid Location Reference Cought By Server");
        }

        public static void Restock(int ID, int ammount)
        {
            if (Validation.ProdID(ID))
            {
                Data.StockProduct(ID,ammount);
                return;
            }

            throw new Exception("Error: Invalid Product Reference Cought By Server");
        }
    }
}
