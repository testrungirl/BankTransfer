﻿using BankTransfer.Api.Data;
using BankTransfer.Api.Models;
using BankTransfer.Api.ViewModels;

namespace BankTransfer.Api.Contracts
{
    public interface IBankRepository
    {
        Task<GenericResponse<IEnumerable<Bank>>> GetBanks();
        Task<GenericResponse<ValidateAccount>> ValidateAccountNumber(ValidateAccountVm validateAccountVm);
    }
}
