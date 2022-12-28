namespace BankTransfer.Api.Data
{
    public class GenericResponse<T>
    {
        public T? ResponseBody { get; set; }
        public int Code { get; set; }
        public string? Description { get; set; }
    }
}
