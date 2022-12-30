namespace BankTransfer.Api.ViewModels
{
    public class TransferRecipient
    {
    }

    public class ValidateRecipientReqVM
    {
        public string type { get; set; }
        public string name { get; set; }
        public string account_number { get; set; }
        public string bank_code { get; set; }
        public string? currency { get; set; }
        public string? description { get; set; }
        public string? authorization_code { get; set; }
        public object? metadata { get; set; }
    }

    public class Recipient
    {
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Currency { get; set; }
        public string Domain { get; set; }
        public int Id { get; set; }
        public int Integration { get; set; }
        public string Name { get; set; }
        public string Recipient_code { get; set; }
        public string Type { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Is_deleted { get; set; }
        public Details Details { get; set; }
    }

    public class Details
    {
        public object Authorization_code { get; set; }
        public string Account_number { get; set; }
        public string Account_name { get; set; }
        public string Bank_code { get; set; }
        public string Bank_name { get; set; }
    }

}