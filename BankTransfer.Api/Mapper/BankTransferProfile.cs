using AutoMapper;
using BankTransfer.Api.ViewModels;

namespace BankTransfer.Api.Mapper
{
    public class BankTransferProfile : Profile
    {
        public BankTransferProfile()
        {
            CreateMap<TransferVm, ValidateRecipientReqVM>()
                .ForMember(dest => dest.account_number,
                opt => opt.MapFrom(src => src.BeneficiaryAccountNumber))
                .ForMember(dest => dest.name,
                opt => opt.MapFrom(src => src.BeneficiaryAccountName))
                .ForMember(dest => dest.bank_code,
                opt => opt.MapFrom(src => src.BeneficiaryBankCode))
                .ForMember(dest => dest.currency,
                opt => opt.MapFrom(src => src.CurrencyCode));

            CreateMap<TransferPaystackResponse, TransferDetails>()
                .ForMember(dest => dest.TransactionDateTime,
                opt => opt.MapFrom(src => src.createdAt.ToString("yyyy-MM-dd HH:mm:ss.f")))
                .ForMember(dest => dest.Amount,
                opt => opt.MapFrom(src => (decimal)src.amount / 100))
                .ForMember(dest => dest.CurrencyCode,
                opt => opt.MapFrom(src => src.currency))
                .ForMember(dest => dest.TransactionReference,
                opt => opt.MapFrom(src => src.reference))
                .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.status));

            CreateMap<TransferStatus, TransactionResponse>()
                .ForMember(dest => dest.Amount,
                opt => opt.MapFrom(src => (decimal)src.amount / 100))
                .ForMember(dest => dest.TransactionReference,
                opt => opt.MapFrom(src => src.reference))
                .ForMember(dest => dest.TransactionDateTime,
                opt => opt.MapFrom(src => src.createdAt.ToString("yyyy-MM-dd HH:mm:ss.f")))                
                .ForMember(dest => dest.CurrencyCode,
                opt => opt.MapFrom(src => src.currency))
                .ForMember(dest => dest.BeneficiaryAccountNumber,
                opt => opt.MapFrom(src => src.recipient.details.account_number))
                .ForMember(dest => dest.BeneficiaryAccountName,
                opt => opt.MapFrom(src => src.recipient.details.account_name))
                 .ForMember(dest => dest.BeneficiaryBankCode,
                opt => opt.MapFrom(src => src.recipient.details.bank_code))
                .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.status));
        }

    }
}
