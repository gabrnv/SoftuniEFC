using AutoMapper;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<ImportUsersDto, User>();

            CreateMap<ImportProductsDto, Product>();

            CreateMap<ImportCategoriesDto, Category>();

            CreateMap<ImportCategoriesProductsDto, CategoryProduct>();

            CreateMap<Product, ExportProductsInRangeDto>().ForMember(dest => dest.BuyerName, opt => opt.MapFrom(src => src.Buyer.FirstName));

            CreateMap<User, ExportSoldProductsDto>().ForMember(dest => dest.SoldProducts, opt => opt.MapFrom(src => src.ProductsSold)); 

            CreateMap<Category, ExportCategoriesByProductsCount>().ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.CategoryProducts.Count))
                .ForMember(dest => dest.AveragePrice,
                    opt => opt.MapFrom(src => src.CategoryProducts.Average(cp => cp.Product.Price)))
                .ForMember(dest => dest.TotalRevenue,
                    opt => opt.MapFrom(src => src.CategoryProducts.Sum(cp => cp.Product.Price)));
        }
    }
}
