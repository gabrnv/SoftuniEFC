using AutoMapper;
using AutoMapper.QueryableExtensions;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(ProductShopProfile)));

            var dbContext = new ProductShopContext();

            //dbContext.Database.EnsureCreated();
            //dbContext.Database.EnsureCreated();

            string xml = File.ReadAllText("../../../Datasets/categories-products.xml");

            Console.WriteLine(GetProductsInRange(dbContext));
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {

            var dtos = Deserializer<ImportUsersDto[]>("Users", inputXml);

            var users = Mapper.Map<User[]>(dtos);

            context.Users.AddRange(users);

            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var dtos = Deserializer<ImportProductsDto[]>("Products", inputXml);

            var products = Mapper.Map<Product[]>(dtos);


            context.Products.AddRange(products);

            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var dtos = Deserializer<ImportCategoriesDto[]>("Categories", inputXml);

            List<Category> categories = new List<Category>();

            foreach (var item in dtos)
            {
                if(item.Name == null)
                {
                    continue;
                }

                Category category = Mapper.Map<Category>(item);

                categories.Add(category);
            }

            context.Categories.AddRange(categories);

            context.SaveChanges();

            return $"Successfully imported {categories.Count}";

            return "";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            List<Category> categories = context.Categories.ToList();

            List<Product> products = context.Products.ToList();

            var dtos = Deserializer<ImportCategoriesProductsDto[]>("CategoryProducts", inputXml);

            List<CategoryProduct> catPr = new List<CategoryProduct>();

            foreach (var item in dtos)
            {
                if (categories.Any(c => c.Id == item.CategoryId) == false || products.Any(p => p.Id == item.ProductId) == false)
                {
                    continue;
                }

                CategoryProduct category = Mapper.Map<CategoryProduct>(item);

                catPr.Add(category);
            }

            context.CategoryProducts.AddRange(catPr);

            context.SaveChanges();

            return $"Successfully imported {catPr.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            ExportProductsInRangeDto[] products = context.Products
                                            .Where(p => p.Price >= 500 && p.Price <= 1000)
                                            .OrderBy(p => p.Price)
                                            .Take(10)
                                            .ProjectTo<ExportProductsInRangeDto>(Mapper.Configuration)
                                            .ToArray();

            return Serializer<ExportProductsInRangeDto[]>(products, "Products");
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                               .Where(u => u.ProductsSold.Count >= 1)
                               .OrderBy(u => u.LastName)
                               .ThenBy(u => u.FirstName)
                               .Take(5)
                               .ProjectTo<ExportSoldProductsDto>(Mapper.Configuration)
                               .ToArray();

            return Serializer<ExportSoldProductsDto[]>(users, "Users");
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                                    .OrderByDescending(c => c.CategoryProducts.Count)
                                    .ProjectTo<ExportCategoriesByProductsCount>(Mapper.Configuration)
                                    .ToArray();
            return Serializer<ExportCategoriesByProductsCount[]>(categories, "Categories");
        }

        private static T Deserializer<T>(string rootTag, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootTag));

            T dtos = (T)serializer.Deserialize(new StringReader(inputXml));


            return dtos;
        }

        private static string Serializer<T>(T dto, string rootTag)
        {
            var sb = new StringBuilder();

            var root = new XmlRootAttribute(rootTag);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var serializer = new XmlSerializer(typeof(T), root);

            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, dto, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}