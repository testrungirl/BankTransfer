using BankTransfer.Api.Data;
using BankTransfer.Api.Models;
using BankTransfer.Api.ViewModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BankTransfer.Api.Contracts
{
    public class BankRepository : IBankRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _token;
        private readonly IHttpClientFactory _clientFactory;

        public BankRepository(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _configuration = configuration;
            _token = _configuration["Payment:PaystackSK"];
            _clientFactory = clientFactory;
        }
        public async Task<GenericResponse<IEnumerable<Bank>>> GetBanks()
        {
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, "https://api.paystack.co/bank");

                var client = _clientFactory.CreateClient();

                HttpResponseMessage res = await client.SendAsync(req);

                if (!res.IsSuccessStatusCode)
                {
                    return new GenericResponse<IEnumerable<Bank>> ()
                    {
                        Description = $"There was an error getting the institutions currently enrolled on NIP and their respective codes: {res.ReasonPhrase}",
                        Code = (int)res.StatusCode
                    };
                }
                GenericData<IEnumerable<Bank>>? banklistRes = await res.Content.ReadFromJsonAsync<GenericData<IEnumerable<Bank>>>();
                return new GenericResponse<IEnumerable<Bank>>()
                {
                    Code = (int)res.StatusCode,
                    ResponseBody = banklistRes?.Data,
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<IEnumerable<Bank>>()
                {
                    Code = 500,
                    Description = $"Internal Server Error: {ex.Message}"
                };
            }

        }

        public async Task<GenericResponse<ValidateAccount>> ValidateAccountNumber(ValidateAccountVm validateAccountVm)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{_token}");

                    var response = await client.GetAsync($"https://api.paystack.co/bank/resolve?account_number={validateAccountVm.AccountNumber}&bank_code={validateAccountVm.Code}");


                    if (!response.IsSuccessStatusCode)
                    {
                        return new GenericResponse<ValidateAccount>()
                        {
                            Description = $"An error occured while verifying account number: {response.ReasonPhrase}",
                            Code = (int)response.StatusCode
                        };
                    }
                    //// Read the response as a string
                    var data = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON string into a dynamic object
                    var Obj = JsonConvert.DeserializeObject<GenericData<ValidateAccount>>(data);
                    Console.WriteLine(Obj.ToString());
                    return new GenericResponse<ValidateAccount>()
                    {
                        Code = (int)response.StatusCode,
                        ResponseBody = Obj.Data,
                    };
                }
            }
            catch (Exception ex)
            {
                return new GenericResponse<ValidateAccount>()
                {
                    Code = 500,
                    Description = $"Internal Server Error: {ex.Message}"
                };
            }
        }
    }
}

public class Rootobject
{
    public bool status { get; set; }
    public string message { get; set; }
    public Data data { get; set; }
}

public class Data
{
    public string account_number { get; set; }
    public string account_name { get; set; }
    public int bank_id { get; set; }
}
