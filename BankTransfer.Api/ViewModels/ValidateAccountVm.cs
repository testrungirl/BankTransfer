namespace BankTransfer.Api.ViewModels
{
    public class ValidateAccountVm
    {
        public string Code { get; set; }
        public string AccountNumber { get; set; }
        public string? Provider { get; set; }
    }
}
