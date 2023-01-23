

namespace ProductShop
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;

    using Data;
    using DTOs.Category;
    using DTOs.CategoryProduct;
    using DTOs.Product;
    using DTOs.User;
    using Models;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Newtonsoft.Json;
    public class StartUp
    {
        private static IMapper mapper;
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(ProductShopProfile)));

            ProductShopContext dbContext = new ProductShopContext();

            string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");

            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();
            Console.WriteLine(GetSoldProducts(dbContext));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            //deserialize json
            List<ImportUserDto> users = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson).ToList();
            //list for valid users
            List<User> validUsers = new List<User>();
            foreach (var user in users)
            {
                //map a json user to into a normal User to Add into context
                User validUser = Mapper.Map<User>(user);
                validUsers.Add(validUser);
            }

            //add list of users to the context
            context.Users.AddRange(validUsers);

            //save changer
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            ImportProductDto[] dtoProducts = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);

            List<Product> products = new List<Product>();

            foreach (var dtoP in dtoProducts)
            {
                Product product = Mapper.Map<Product>(dtoP);
                products.Add(product);
            }

            context.AddRange(products);

            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            ImportCategoryDto[] categoryDtos = JsonConvert.DeserializeObject<ImportCategoryDto[]>(inputJson);

            List<Category> categories = new List<Category>();

            foreach (var dtoC in categoryDtos)
            {
                if(dtoC.Name == null)
                {
                    continue;
                }

                Category category = Mapper.Map<Category>(dtoC);
                categories.Add(category);
            }

            context.AddRange(categories);

            context.SaveChanges ();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            ImportCategoryProductDto[] cpDtos = JsonConvert.DeserializeObject<ImportCategoryProductDto[]> (inputJson);

            List<CategoryProduct> ctProducts = new List<CategoryProduct>();

            foreach(var dtoC in cpDtos)
            {
                CategoryProduct categoryProduct = Mapper.Map<CategoryProduct>(dtoC);
                ctProducts.Add(categoryProduct);
            }

            context.AddRange(ctProducts);
            context.SaveChanges();

            return $"Successfully imported {ctProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            ExportProductsInRangeDto[] products = context.Products
                                        .Where(p => p.Price >= 500 && p.Price <= 1000)
                                        .OrderBy(p => p.Price)
                                        .ProjectTo<ExportProductsInRangeDto>()
                                        .ToArray();

            string json = JsonConvert.SerializeObject(products, Formatting.Indented);

            return json;

        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            ExportUsersWithSoldProductsDto[] users = context
                .Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId.HasValue))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ProjectTo<ExportUsersWithSoldProductsDto>()
                .ToArray();

            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            return json;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            
            
        }


    }

}
