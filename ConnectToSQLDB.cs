#region Shared uses
using System;
using System.Collections.Generic;
#endregion Shared uses

#region Connect to SQL Server Data Base with Linq2db. Uses
using System.Linq;
using LinqToDB;
using LinqToDB.Configuration;

using LinqToDB.Data;
using LinqToDB.Mapping;
#endregion Connect to SQL Server Data Base with Linq2db. Uses

#region Connect to SQL Server Data Base with Microsoft.Data.SqlClient. Uses
using Microsoft.Data.SqlClient; //Also used System.Data.SqlClient (Установка Microsoft.Data.SqlClient возможна только через NuGet)
using System.Data;
#endregion Connect to SQL Server Data Base with Microsoft.Data.SqlClient. Uses


namespace ConnectToSQLS
{
    class Program
    {
        //Flags methods, uses for connect to SQLServer Data Base
        readonly static bool ISLINQ2DB = false;
        readonly static bool ISMSDATASQLCLIENT = false;
        readonly static bool ISSYSDATASQLCLIENT = true;

        static void Main(string[] args)
        {
            if (ISLINQ2DB)
            {
                #region Connect to SQL Server Data Base with Linq2db. Realization. Output
                DataConnection.DefaultSettings = new MySettings();
                List<Product> newquery = GetData(25);
                foreach (Product custObj in newquery)
                {
                    Console.WriteLine("ProductID: {0}", custObj.ProductID);
                    Console.WriteLine("\tProductName: {0}", custObj.Name);
                }
                #endregion Connect to SQL Server Data Base with Linq2db. Realization. Output
            }
            else if (ISMSDATASQLCLIENT)
            {
                #region Connect to SQL Server Data Base with Microsoft.Data.SqlClient. Output

                string connstr = @"Data Source=.\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";//Work

                ReadOrderData(connstr);
                #endregion Connect to SQL Server Data Base with Microsoft.Data.SqlClient. Output
            }
            else if(ISSYSDATASQLCLIENT)
            {
                #region Connect to SQL Server Data Base with System.Data.SqlClient. Realization
                ReadProducts();
                #endregion Connect to SQL Server Data Base with System.Data.SqlClient. Realization
            }
            else
            {
                Console.WriteLine("Выберите способ присоединения к БД (isLinq2db или isMicrosoft_Data_SqlClient)");
            }
            Console.ReadLine();
        }
        #region Connect to SQL Server Data Base with Linq2db. Realization
        public static List<Product> GetData(int id)
        {
            using var db = new DbNorthwind();
            var query = from p in db.Product
                        where p.ProductID > id
                        orderby p.Name descending
                        select p;
            return query.ToList();
        }
        #endregion Connect to SQL Server Data Base with Linq2db. Realization

        #region Connect to SQL Server Data Base with Microsoft.Data.SqlClient. Realization
        private static void ReadOrderData(string connectionString)
        {
            string queryString =
                "SELECT OrderID, CustomerID FROM dbo.Orders;";

            using (SqlConnection connection =
                       new SqlConnection(connectionString))
            {
                SqlCommand command =
                    new SqlCommand(queryString, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                // Call Read before accessing data.
                while (reader.Read())
                {
                    ReadSingleRow((IDataRecord)reader);
                }
                // Call Close when done reading.
                reader.Close();
            }
        }

        private static void ReadSingleRow(IDataRecord record)
        {
            Console.WriteLine(String.Format("{0}, {1}", record[0], record[1]));
        }
        #endregion Connect to SQL Server Data Base with Microsoft.Data.SqlClient. Realization

        #region Connect to SQL Server Data Base with System.Data.SqlClient. Realization
        static void ReadProducts()
        {
            var connstr = @"Data Source=.\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string queryString = "SELECT ProductID, ProductName FROM dbo.Products;";
            using (var connection = new System.Data.SqlClient.SqlConnection(connstr))
            {
                var command = new System.Data.SqlClient.SqlCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(String.Format("{0}, {1}", reader[0], reader[1]));
                    }
                }
            }
        }
        #endregion Connect to SQL Server Data Base with System.Data.SqlClient. Realization
    }

    #region Connect to SQL Server Data Base with Linq2db. Classes
    public class MySettings : ILinqToDBSettings
    {
        public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();
        public string DefaultConfiguration => "SqlServer";
        public string DefaultDataProvider => "SqlServer";

        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                yield return
                    new ConnectionStringSettings
                    {
                        Name = "Northwind",
                        ProviderName = "SqlServer",
                        //ConnectionString = "Data Source=INVOTECO;Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"//Home
                        ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"//Work
                    };
            }
        }
    }
    public class ConnectionStringSettings : IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }

    [Table(Name = "Products")]
    public class Product
    {
        [PrimaryKey, Identity]
        public int ProductID { get; set; }

        [Column(Name = "ProductName"), NotNull]
        public string Name { get; set; }

        // ... other columns ...
    }

    public class DbNorthwind : LinqToDB.Data.DataConnection
    {
        public DbNorthwind() : base("Northwind") { }
        public ITable<Product> Product => GetTable<Product>();
        //public ITable<Category> Category => GetTable<Category>();

        // ... other tables ...
    }

    #endregion Connect to SQL Server Data Base with Linq2db. Classes
}
