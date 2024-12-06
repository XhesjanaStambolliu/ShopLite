using AutoMapper;
using ShopLite.Services.CouponAPI.Models;
using ShopLite.Services.CouponAPI.Models.Dto;

namespace ShopLite.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Coupon>();
                config.CreateMap<Coupon, CouponDto>();
            });
            return mappingConfig;
        }
    }
}
