using ShopLite.Services.EmailAPI.Message;
using ShopLite.Services.EmailAPI.Models.Dto;

namespace ShopLite.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task RegisterUserEmailAndLog(string email);
        Task LogOrderPlaced(RewardsMessage rewardsDto);
    }
}
