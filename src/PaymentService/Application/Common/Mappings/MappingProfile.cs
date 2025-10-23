using AutoMapper;
using PaymentService.Application.Common.DTOs;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enum;

namespace PaymentService.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Transaction mappings
            CreateMap<Transaction, PaymentDto>();
            CreateMap<CreatePaymentDto, Transaction>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.TransactionValue, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.ReferenceCode, opt => opt.MapFrom(src => src.Description ?? string.Empty))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PaymentStatus.Pending))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // New Transaction DTO mappings
            CreateMap<CreateTransactionDto, Transaction>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.TransactionValue, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PaymentStatus.Pending))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDelete, opt => opt.MapFrom(src => false));

            // Wallet mappings
            CreateMap<Wallet, WalletDto>();

            // Refund mappings
            CreateMap<Refund, RefundDto>();
            CreateMap<CreateRefundDto, Refund>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => RefundStatus.Pending))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
