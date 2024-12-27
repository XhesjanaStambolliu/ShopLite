using ShopLite.Services.RewardAPI.Message;

namespace ShopLite.Services.RewardAPI.Services
{
    public interface IRewardService
    {
        Task UpdateRewards(RewardsMessage rewardsMessage);
    }
}
