using BankTransfer.Api.Data;
using BankTransfer.Api.ViewModels;

namespace BankTransfer.Api.Contracts
{
    public interface IBankRepository
    {
        Task<GenericResponse<IEnumerable<Bank>>> GetBanks();
        Task<GenericResponse<AccountDet>> ValidateAccountNumber(ValidateAccountVm validateAccountVm);
        Task<GenericResponse<TransferDetails>> TransferFunds(TransferVm transferVm);
        Task<GenericResponse<TransactionResponse>> TransactionStatus(string reference);
    }
}
