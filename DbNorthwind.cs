using LinqToDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace LINQ_Learn
{
    public class DbNorthwind : LinqToDB.Data.DataConnection
    {
        public DbNorthwind() : base("Northwind") { }
        public ITable<Product> Product => GetTable<Product>();
        //public ITable<Category> Category => GetTable<Category>();

        // ... other tables ...
    }
}
