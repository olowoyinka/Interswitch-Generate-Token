using Interswitch_API.DTOs;

namespace Interswitch_API.Services;

public interface IInterswitchService
{
    Task<GetAllBanks?> GetFundsTransferBanksAsync();
}