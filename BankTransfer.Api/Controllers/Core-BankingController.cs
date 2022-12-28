using BankTransfer.Api.Contracts;
using BankTransfer.Api.Data;
using BankTransfer.Api.Models;
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
        [Route("Core-Banking/Banks")]
        [HttpGet]
        [ProducesResponseType(typeof(GenericResponse<BankList>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BankList>> Banks()
        {
            var res = await _bankRepo.GetBanks();

            return Ok(res);
        }
        [Route("Core-Banking/validateBankAccount")]
        [HttpGet]
        [ProducesResponseType(typeof(BankList), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BankList>> ValidateBankAccount()
        {
            var res = await _bankRepo.;

            return Ok(res);
        }
    }
}
