using BankTransfer.Api.Contracts;
using BankTransfer.Api.Data;
using BankTransfer.Api.Models;
using BankTransfer.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BankTransfer.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class Core_BankingController : ControllerBase
    {
        private readonly IBankRepository _bankRepo;
        public Core_BankingController(IBankRepository bankRepo)
        {
            _bankRepo = bankRepo;
        }
        [Route("~/api/v1/Core-Banking/Banks")]
        [HttpGet]
        [ProducesResponseType(typeof(GenericResponse<IEnumerable<Bank>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GenericResponse<IEnumerable<Bank>>>> Banks()
        {
            var res = await _bankRepo.GetBanks();

            return Ok(res);
        }
        [Route("~/api/v1/Core-Banking/validateBankAccount")]
        [HttpPost]
        [ProducesResponseType(typeof(GenericResponse<GenericData<ValidateAccount>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GenericResponse<ValidateAccount>>> ValidateBankAccount([FromBody]ValidateAccountVm Obj)
        {
            var res = await _bankRepo.ValidateAccountNumber(Obj);

            return Ok(res);
        }
    }
}

