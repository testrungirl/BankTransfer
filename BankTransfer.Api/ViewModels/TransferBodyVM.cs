namespace BankTransfer.Api.ViewModels
{
    public class TransferResponseVM
    {
        public int Integration { get; set; }
        public string Domain { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Source { get; set; }
        public string Reason { get; set; }
        public int Recipient { get; set; }
        public string Status { get; set; }
        public string Transfer_code { get; set; }
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
