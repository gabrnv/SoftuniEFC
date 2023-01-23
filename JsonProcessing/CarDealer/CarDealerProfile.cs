using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CarDealer.DTO;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<ImportCustomersDto, Customer>();

            CreateMap<ImportCarsDto, Car>();

            CreateMap<ImportPartsDto, Part>();

            CreateMap<ImportSalesDto, Sale>();
        }
    }
}
