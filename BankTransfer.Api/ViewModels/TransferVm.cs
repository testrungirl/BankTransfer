using System.ComponentModel.DataAnnotations;

namespace BankTransfer.Api.ViewModels
{
    public class TransferVm
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Narration { get; set; }
        public string BeneficiaryAccountNumber { get; set; }
        public string BeneficiaryAccountName { get; set; }
        public string BeneficiaryBankCode { get; set; }
        [Key]
        public string TransactionReference { get; set; }
        public int MaxRetryAttempt { get; set; }
        public string CallBackUrl { get; set; }
        public string Provider { get; set; }
    }

    public class TransferPaystackRequest
    {
        public string reference { get; set; }
        public string source { get; set; }
        public string reason { get; set; }
        public int amount { get; set; }
        public string recipient { get; set; }
    }
    public class TransferPaystackResponse
    {
        public object[] transfersessionid { get; set; }
        public string domain { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string reference { get; set; }
        public string source { get; set; }
        public object source_details { get; set; }
        public string reason { get; set; }
        public string status { get; set; }
        public object failures { get; set; }
        public string transfer_code { get; set; }
        public object titan_code { get; set; }
        public object transferred_at { get; set; }
        public int id { get; set; }
        public int integration { get; set; }
        public int request { get; set; }
        public int recipient { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

}
