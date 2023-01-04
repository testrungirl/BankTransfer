using BankTransfer.Api.ViewModels;

namespace BankTransfer.Api.Data
{
    public class GenericResponse<T>
    {
        public T? ResponseBody { get; set; }
        public int Code { get; set; }
        public string? Description { get; set; }
    }
    public class GenericData<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
    }
    //public class TransactionsList<T>: GenericData<T>
    //{
    //    public Meta meta { get; set; }
    //}
}

