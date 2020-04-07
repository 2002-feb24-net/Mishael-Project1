using NUnit.Framework;
using POne.Lib;
using System.Data.SQLite;

namespace POneTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void IsPhysicsOk()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void Test()
        {
            
        }

        static SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source= database.db; Version = 3; New = True; Compress = True; ");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (System.Exception ex) { }
            return sqlite_conn;
        }
    }
}