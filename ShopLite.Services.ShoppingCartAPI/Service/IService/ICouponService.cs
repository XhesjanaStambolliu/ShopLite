using ShopLite.Services.ShoppingCartAPI.Models.Dto;

namespace ShopLite.Services.ShoppingCartAPI.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}
