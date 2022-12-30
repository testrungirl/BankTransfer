namespace BankTransfer.Api.ViewModels
{
    public class ValidateAccountRes
    {
        public string Account_number { get; set; }
        public string Account_name { get; set; }
        public int Bank_id { get; set; }
        //public string BankName { get; set; }
    }
    public class AccountDet: ValidateAccountRes
    {
        public string BankName { get; set; }
    }
    public class ValidateAccountVm
    {
        public string Code { get; set; }
        public string AccountNumber { get; set; }
        public string? Provider { get; set; }
    }
}
