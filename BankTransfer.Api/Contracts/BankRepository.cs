using BankTransfer.Api.Data;
using BankTransfer.Api.Models;
using PayStack.Net;

namespace BankTransfer.Api.Contracts
{
    public class BankRepository : IBankRepository
    {
        private readonly IConfiguration _configuration;
        private readonly PayStackApi _payStackApi;
        private readonly string _token;
        private readonly IHttpClientFactory _clientFactory;

        public BankRepository(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _configuration = configuration;

            _token = _configuration["Payment: PaystackSK"];

            _payStackApi = new PayStackApi(_token);
            _clientFactory = clientFactory;
        }
        public async Task<GenericResponse<BankList>> GetBanks()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "https://api.paystack.co/bank");

            var client = _clientFactory.CreateClient();

            HttpResponseMessage res = await client.SendAsync(req);

            if (!res.IsSuccessStatusCode)
            {
                return new GenericResponse<BankList>()
                {                    
                    Description = $"There was an error getting the institutions currently enrolled on NIP and their respective codes: {res.ReasonPhrase}",
                    Code = (int)res.StatusCode
                };
            }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            BankList banklist = await res.Content.ReadFromJsonAsync<BankList>();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            return new GenericResponse<BankList>()
            {
                Code = (int)res.StatusCode,
                ResponseBody = banklist,
            };

        }
    }
}
