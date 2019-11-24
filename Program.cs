using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQ_Learn
{
    /// <summary>
    /// Иллюстрируется подключение к БД Northwind и извлечение из таблицы Products данных для продуктов с id>25
    /// с сортировкой по имени алфавиту (по убыванию)
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            DataConnection.DefaultSettings = new MySettings();

            List<Product> newquery = GetAllData(25);

            foreach (Product custObj in newquery)
            {
                Console.WriteLine("ProductID: {0}", custObj.ProductID);
                Console.WriteLine("\tProductName: {0}", custObj.Name);
            }
            Console.ReadLine();
        }

        public static List<Product> GetAllData(int id)
        {
            using var db = new DbNorthwind();
            var query = from p in db.Product
                        where p.ProductID > id
                        orderby p.Name descending
                        select p;
            return query.ToList();
        }
    }
}
