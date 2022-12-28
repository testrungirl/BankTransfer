namespace BankTransfer.Api.Models
{
    public class ValidateAccount
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
    }
}
