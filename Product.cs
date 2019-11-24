using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace LINQ_Learn
{
    [Table(Name = "Products")]
    public class Product
    {
        [PrimaryKey, Identity]
        public int ProductID { get; set; }

        [Column(Name = "ProductName"), NotNull]
        public string Name { get; set; }

        // ... other columns ...
    }
}
