
namespace ProductShop
{
    using AutoMapper;
    using ProductShop.DTOs.Category;
    using ProductShop.DTOs.CategoryProduct;
    using ProductShop.DTOs.Product;
    using ProductShop.DTOs.User;
    using ProductShop.Models;
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();
        }
    }
}
