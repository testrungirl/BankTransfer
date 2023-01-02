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

   public class TransactionReference
{
    public int integration { get; set; }
    public Recipient recipient { get; set; }
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
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
}

public class Recipient
{
    public string domain { get; set; }
    public string type { get; set; }
    public string currency { get; set; }
    public string name { get; set; }
    public Details details { get; set; }
    public string description { get; set; }
    public string metadata { get; set; }
    public string recipient_code { get; set; }
    public bool active { get; set; }
    public object email { get; set; }
    public int id { get; set; }
    public int integration { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
}

public class Details
{
    public string account_number { get; set; }
    public object account_name { get; set; }
    public string bank_code { get; set; }
    public string bank_name { get; set; }
}
}