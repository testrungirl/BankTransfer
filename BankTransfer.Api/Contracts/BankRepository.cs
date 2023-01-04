using AutoMapper;
using BankTransfer.Api.Data;
using BankTransfer.Api.Models;
using BankTransfer.Api.ViewModels;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Text;

namespace BankTransfer.Api.Contracts
{
    public class BankRepository : IBankRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _token;
        private readonly string _baseUrl;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;

        public BankRepository(IConfiguration configuration, IHttpClientFactory clientFactory, IMapper mapper)
        {
            _configuration = configuration;
            _token = _configuration["Paystack_Payment:PaystackSK"];
            _baseUrl = _configuration["Paystack_Payment:BaseUrl"];
            _clientFactory = clientFactory;
            _mapper = mapper;
        }
        public async Task<GenericResponse<IEnumerable<Bank>>> GetBanks()
        {
            try
            {
                string url = _baseUrl + "bank";
                var req = new HttpRequestMessage(HttpMethod.Get, url);

                var client = _clientFactory.CreateClient();

                HttpResponseMessage res = await client.SendAsync(req);

                GenericData<IEnumerable<Bank>>? banklistRes = await res.Content.ReadFromJsonAsync<GenericData<IEnumerable<Bank>>>();
                return new GenericResponse<IEnumerable<Bank>>()
                {
                    Code = (int)res.StatusCode,
                    Description = banklistRes?.Message,
                    ResponseBody = banklistRes?.Data,
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<IEnumerable<Bank>>()
                {
                    Code = 500,
                    Description = $"Internal server error: {ex.Message}"
                };
            }

        }
        public async Task<GenericResponse<AccountDet>> ValidateAccountNumber(ValidateAccountVm validateAccountVm)
        {
            try
            {
                if (string.IsNullOrEmpty(validateAccountVm.Code))
                {
                    return new GenericResponse<AccountDet>()
                    {
                        Description = "Code is required",
                        Code = 400
                    };
                }
                if (string.IsNullOrEmpty(validateAccountVm.AccountNumber))
                {
                    return new GenericResponse<AccountDet>()
                    {
                        Description = "Account Number is required",
                        Code = 400
                    };
                }
                using HttpClient client = new();
                string? BankName = "";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{_token}");

                string url = $"{_baseUrl}bank/resolve?account_number={validateAccountVm.AccountNumber}&bank_code={validateAccountVm.Code}";

                var response = await client.GetAsync(url);

                var data = await response.Content.ReadAsStringAsync();

                var Obj = JsonConvert.DeserializeObject<GenericData<ValidateAccountRes>>(data);


                BankName = (await GetBanks()).ResponseBody?.FirstOrDefault(x => x.Code == validateAccountVm.Code)?.Name;

                return new GenericResponse<AccountDet>()
                {
                    Code = (int)response.StatusCode,
                    Description = Obj?.Message,
                    ResponseBody = new AccountDet { Account_name = Obj.Data.Account_name, Account_number = Obj.Data.Account_number, BankName = BankName, Bank_id = Obj.Data.Bank_id },
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<AccountDet>()
                {
                    Code = 500,
                    Description = $"Internal Server Error: {ex.Message}"
                };
            }
        }
        private async Task<GenericResponse<Recipient>> CreateRecipient(ValidateRecipientReqVM recipientReqVM)
        {
            try
            {

                using HttpClient client = new();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.ConnectionClose = true;
                var url = $"{_baseUrl}transferrecipient";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{_token}");

                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestBody = JsonConvert.SerializeObject(recipientReqVM);



                var response = await client.PostAsync(url, new StringContent(requestBody, Encoding.UTF8, "application/json"));

                var data = await response.Content.ReadAsStringAsync();

                var Obj = JsonConvert.DeserializeObject<GenericData<Recipient>>(data);

                return new GenericResponse<Recipient>()
                {
                    Code = (int)response.StatusCode,
                    Description = Obj.Message,
                    ResponseBody = Obj.Data
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<Recipient>()
                {
                    Code = 500,
                    Description = $"Internal Server Error: {ex.Message}"
                };
            }
        }
        private async Task<GenericData<TransferDetails>> TransferCall(TransferPaystackRequest req, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                using (HttpClient client = new())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.ConnectionClose = true;
                    var url = $"{_baseUrl}transfer";

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{_token}");

                    client.DefaultRequestHeaders
                          .Accept
                          .Add(new MediaTypeWithQualityHeaderValue("application/json"));



                    var requestBody = JsonConvert.SerializeObject(req);

                    var response = await client.PostAsync(url, new StringContent(requestBody, Encoding.UTF8, "application/json"));

                    var data = await response.Content.ReadAsStringAsync();

                    var Obj = JsonConvert.DeserializeObject<GenericData<TransferPaystackResponse>>(data);
                    if (Obj.Data != null && Obj.Data.status.ToLower() == "FAILURE".ToLower())
                    {
                        if (retryForAvailability > 0)
                        {
                            retryForAvailability--;
                            System.Threading.Thread.Sleep(3000);
                            await TransferCall(req, retryForAvailability);
                        }
                    }

                    var res = _mapper.Map<TransferDetails>(Obj.Data);
                    if (res != null)
                    {
                        res.ResponseCode = response.StatusCode.ToString();
                        res.ResponseMessage = Obj.Message;
                    }

                    return new GenericData<TransferDetails>()
                    {
                        Status = Obj.Status,
                        Message = Obj.Message,
                        Data = res
                    };
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability > 0)
                {
                    retryForAvailability--;
                    System.Threading.Thread.Sleep(3000);
                    await TransferCall(req, retryForAvailability);
                }
                return new GenericData<TransferDetails>()
                {
                    Status = false,
                    Message = $"Internal Server Error: {ex.Message}",
                };
            }

        }
        public async Task<GenericResponse<TransferDetails>> TransferFunds(TransferVm transferVm)
        {
            try
            {
                var RecipientReq = _mapper.Map<ValidateRecipientReqVM>(transferVm);
                RecipientReq.type = "nuban";
                var Recipient = await CreateRecipient(RecipientReq);
                if (Recipient.Code != 201)
                {
                    return new GenericResponse<TransferDetails>()
                    {
                        Description = Recipient.Description,
                        Code = Recipient.Code
                    };
                }

                int AmountInKobo = (int)(transferVm.Amount * 100);
                TransferPaystackRequest reqBody = new()
                {
                    reference = transferVm.TransactionReference ?? Guid.NewGuid().ToString("N"),
                    source = "balance",
                    amount = AmountInKobo,
                    reason = transferVm.Narration,
                    recipient = Recipient.ResponseBody.recipient_code,
                };
                var res = await TransferCall(reqBody, transferVm.MaxRetryAttempt);
                if (res.Status)
                {
                    res.Data.BeneficiaryAccountName = Recipient.ResponseBody.name;
                    res.Data.BeneficiaryAccountNumber = transferVm.BeneficiaryAccountNumber;
                    res.Data.BeneficiaryBankCode = transferVm.BeneficiaryBankCode;

                }
                _ = short.TryParse(res?.Data?.ResponseCode, out short Code);
                return new GenericResponse<TransferDetails>()
                {
                    Code = Code,
                    Description = res.Message,
                    ResponseBody = res.Data
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<TransferDetails>()
                {
                    Code = 500,
                    Description = $"Internal Server Error: {ex.Message}"
                };
            }
        }
        public async Task<GenericResponse<TransactionResponse>> TransactionStatus(string reference)
        {
            try
            {
                if (string.IsNullOrEmpty(reference))
                {
                    return new GenericResponse<TransactionResponse>()
                    {
                        Description = "Transaction reference is required",
                        Code = 400
                    };
                }
                using (HttpClient client = new())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.ConnectionClose = true;

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{_token}");

                    var url = $"{_baseUrl}transfer/verify/{reference}";
                    var response = await client.GetAsync(url);

                    var data = await response.Content.ReadAsStringAsync();

                    var Obj = JsonConvert.DeserializeObject<GenericData<TransferStatus>>(data);
                    var res = _mapper.Map<TransactionResponse>(Obj.Data);
                    if (res != null)
                    {
                        res.ResponseMessage = Obj.Message;
                        res.Status = response.StatusCode.ToString();
                    }
                    return new GenericResponse<TransactionResponse>()
                    {
                        Code = (int)response.StatusCode,
                        Description = Obj?.Message,
                        ResponseBody = res,
                    };
                }
            }
            catch (Exception ex)
            {
                return new GenericResponse<TransactionResponse>()
                {
                    Code = 500,
                    Description = $"Internal server error: {ex.Message}"
                };
            }
        }
    }
}


