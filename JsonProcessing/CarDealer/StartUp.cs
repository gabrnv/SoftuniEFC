using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        private IMapper mapper;
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(CarDealerProfile)));

            CarDealerContext dbContext = new CarDealerContext();

            string json = File.ReadAllText("../../../Datasets/sales.json");
            Console.WriteLine(ImportSales(dbContext, json));
        }

        public static string ImportSuppliers(CarDealerContext context, string suppliersJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(suppliersJson);

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }



        public static string ImportParts(CarDealerContext context, string partsJson)
        {
            var supplierIds = context.Suppliers.Select(x => x.Id).ToList();

            var parts = JsonConvert.DeserializeObject<List<Part>>(partsJson)
                .Where(p => supplierIds.Contains(p.SupplierId))
                .ToList();

            context.Parts.AddRange(parts);

            context.SaveChanges();

            var partsCount = parts.Count();

            return $"Successfully imported {partsCount}.";
        }

        public static string ImportCustomers(CarDealerContext context, string customersJson)
        {
            var customers = JsonConvert.DeserializeObject<List<Customer>>(customersJson);

            context.Customers.AddRange(customers);

            context.SaveChanges();

            var customersCount = customers.Count();

            return $"Successfully imported {customersCount}."; ;
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

            context.Sales.AddRange(sales);

            context.SaveChanges();

            var customersCount = sales.Count();

            return $"Successfully imported {customersCount}."; ;
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                                .OrderBy(c => c.BirthDate)
                                .ThenBy(c => c.IsYoungDriver)
                                .Select(c => new
                                {
                                    c.Name,
                                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    c.IsYoungDriver
                                })
                                .ToList();

            var jsonResult = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return jsonResult;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            List<Car> cars = context.Cars
                                    .Where(c => c.Make == "Toyota")
                                    .OrderBy(c => c.Model)
                                    .ThenByDescending(c => c.TravelledDistance)
                                    .ToList();

            return JsonConvert.SerializeObject(cars, Formatting.Indented);
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                                          .Where(s => s.IsImporter == false)
                                          .Select(s => new
                                          {
                                              s.Id,
                                              s.Name,
                                              PartsCount = s.Parts.Count
                                          })
                                          .ToArray();

            return JsonConvert.SerializeObject(suppliers, Formatting.Indented);
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars.Select(car => new
            {
                car = new
                    {
                        car.Make,
                        car.Model,
                        car.TravelledDistance
                    },
                parts = car.PartCars.Select(p => new
                {
                    p.Part.Name,
                    price = $"{p.Part.Price:f2}"
                })
            }).ToList();

            return JsonConvert.SerializeObject(cars);
        }

    }
}