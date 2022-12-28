using BankTransfer.Api.Data;
using BankTransfer.Api.Models;

namespace BankTransfer.Api.Contracts
{
    public interface IBankRepository
    {
        Task<GenericResponse<BankList>> GetBanks();
    }
}
