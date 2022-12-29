﻿namespace BankTransfer.Api.Models
{
    public class Bank
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Longcode { get; set; }
    }
    public class GenericData<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
    }
}


