using ShopLite.Services.ShoppingCartAPI.Models.Dto;

namespace ShopLite.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
