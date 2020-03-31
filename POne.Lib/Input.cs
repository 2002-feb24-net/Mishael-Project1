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

            throw new Exception("Error: Invalid Name Cought By Server");
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
    }
}
