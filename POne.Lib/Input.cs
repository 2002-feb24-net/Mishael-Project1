using POne.dtb;
using System;

namespace POne.Lib
{
    public class Input
    {
        /// <summary>
        /// adds a person from the database
        /// </summary>
        /// <param name="Fname">first name</param>
        /// <param name="Lname">last name</param>
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

        /// <summary>
        /// adds a location to the database
        /// </summary>
        /// <param name="name">name of location</param>
        static public void AddLocation(string name) => Data.AddLocation(name);

        /// <summary>
        /// add a product to the database
        /// </summary>
        /// <param name="name">name of product</param>
        /// <param name="LocID">id of location</param>
        /// <param name="p">price of product</param>
        /// <param name="s">stock of product</param>
        static public void AddProduct(string name, int LocID, decimal p, int s)
        {
            if (Validation.LocID(LocID))
            {
                Data.AddProduct(name, LocID, p, s);
                return;
            }

            throw new Exception("Error: Invalid Location Reference Cought By Server");
        }

        /// <summary>
        /// remove a person from the database
        /// </summary>
        /// <param name="ID">id of person</param>
        public static void RemovePerson(int ID)
        {
            if (Validation.CustID(ID))
            {
                Data.RemoveCustomer(ID);
                return;
            }

            throw new Exception("Error: Invalid Person Reference Cought By Server");
        }

        /// <summary>
        /// removes a location from database
        /// </summary>
        /// <param name="ID">id of location</param>
        public static void RemoveLocation(int ID)
        {
            if (Validation.LocID(ID))
            {
                Data.RemoveStore(ID);
                return;
            }

            throw new Exception("Error: Invalid Location Reference Cought By Server");
        }

        /// <summary>
        /// adds to the items stock
        /// </summary>
        /// <param name="ID">id of item</param>
        /// <param name="ammount">ammount to add</param>
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
