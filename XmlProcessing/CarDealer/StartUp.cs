using AutoMapper;
using CarDealer.Data;
using CarDealer.Dto;
using CarDealer.Models;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using CarDealer.Dto.Import;
using CarDealer.Dto.Exmport;
using CarDealer.Dto.Export;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext dbContext = new CarDealerContext();

            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            string xml = File.ReadAllText("../../../Datasets/sales.xml");

            Console.WriteLine(GetCarsWithTheirListOfParts(dbContext));
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {

            var dtos = Deserializer<ImportSuppliersDto[]>("Suppliers", inputXml);

            Supplier[] suppliers = dtos.Select(dto => new Supplier()
            {
                Name = dto.Name,
                IsImporter = dto.IsImporter
            }).ToArray();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            List<ImportPartsDto> dtos = Deserializer<List<ImportPartsDto>>("Parts", inputXml);

            List<Part> parts = new List<Part>();

            foreach (ImportPartsDto pDto in dtos)
            {
                
                if(!context.Suppliers.Any(s => s.Id == pDto.SupplierId))
                {
                    continue;
                }

                Part part = new Part(){
                    Name = pDto.Name,
                    Price = pDto.Price,
                    Quantity = pDto.Quantity,
                    SupplierId = pDto.SupplierId
                };

                parts.Add(part);
            }

            context.Parts.AddRange(parts);

            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var dtos = Deserializer<ImportCarsDto[]>("Cars", inputXml);

            var cars = new List<Car>();

            foreach (var dto in dtos)
            {
                Car car = new Car()
                {
                    Make = dto.Make,
                    Model = dto.Model,
                    TravelledDistance = dto.TraveledDistance,
                };

                List<PartCar> partCars = new List<PartCar>();

                foreach (var partId in dto.Parts.Select(p => p.Id).Distinct())
                {
                    if(!context.Parts.Any(p => p.Id == partId))
                    {
                        continue ;
                    }

                    partCars.Add(new PartCar
                    {
                        Car = car,
                        PartId = partId,
                    });

                }

                car.PartCars = partCars;
                cars.Add(car);
                
            }

            context.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            List<Customer> customers = Deserializer<ImportCustomersDto[]>("Customers", inputXml).Select(x => new Customer()
            {
                Name = x.Name,
                BirthDate = x.BirthDate,
                IsYoungDriver = x.IsYoungDriver
            }).ToList();

            context.Customers.AddRange(customers);
            
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            ImportSalesDto[] dtos = Deserializer<ImportSalesDto[]>("Sales", inputXml);

            List<Sale> sales = new List<Sale>();

            foreach (var dto in dtos)
            {
                if(!context.Cars.Any(c => c.Id == dto.CarId))
                {
                    continue;
                }

                Sale sale = new Sale()
                {
                    CarId = dto.CarId,
                    CustomerId = dto.CustomerId,
                    Discount = dto.Discount
                };

                sales.Add(sale);
            }

            context.Sales.AddRange(sales);

            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            List<ExportCarWithDistanceDto> cars = context.Cars
                                .Where(c => c.TravelledDistance > 2000000)
                                .OrderBy(c => c.Make)
                                .ThenBy(c => c.Model)
                                .Take(10)
                                .Select(c => new ExportCarWithDistanceDto()
                                {
                                    Make = c.Make,
                                    Model = c.Model,
                                    TravelledDistance = c.TravelledDistance
                                })
                                .ToList();

            return Serializer<List<ExportCarWithDistanceDto>>(cars, "cars");
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            List<ExportCarByBMWDto> cars = context.Cars
                                                  .Where(c => c.Make == "BMW")
                                                  .OrderBy(c => c.Model)
                                                  .ThenByDescending(c => c.TravelledDistance)
                                                  .Select(c => new ExportCarByBMWDto()
                                                  {
                                                      Id = c.Id,
                                                      Model = c.Model,
                                                      TravelledDistance = c.TravelledDistance
                                                  }).ToList();
            return Serializer<List<ExportCarByBMWDto>>(cars,"cars");
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            List<ExportLocalSupplierDto> suppliers = context.Suppliers
                                                            .Where(s => s.IsImporter == false)
                                                            .Select(s => new ExportLocalSupplierDto()
                                                            {
                                                                Id = s.Id,
                                                                Name = s.Name,
                                                                PartsCount = s.Parts.Count(),
                                                            }).ToList();
            return Serializer<List<ExportLocalSupplierDto>>(suppliers, "suppliers");
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            List<ExportCarWithListOfPartsDto> cars = context.Cars
                                                            .Select(c => new ExportCarWithListOfPartsDto()
                                                            {
                                                                Make = c.Make,
                                                                Model = c.Model,
                                                                TravelledDistance = c.TravelledDistance,
                                                                Parts = c.PartCars
                                                                                .Select(cp => new ExportPartDto()
                                                                                {
                                                                                    Name = cp.Part.Name,
                                                                                    Price = cp.Part.Price
                                                                                })
                                                                                .OrderByDescending(p => p.Price)
                                                                                .ToList()
                                                            })
                                                            .OrderByDescending(c => c.TravelledDistance)
                                                            .ThenBy(c => c.Model)
                                                            .Take(5)
                                                            .ToList();
            return Serializer<List<ExportCarWithListOfPartsDto>>(cars, "cars");
        }

        private static T Deserializer<T>(string rootTag, string inputXml )
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