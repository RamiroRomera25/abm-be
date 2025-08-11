using AutoMapper;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.MappingServices;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Auction, AuctionDto>().ReverseMap();
        CreateMap<AuctionDtoPost, Auction>()
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.IsOpen, opt => opt.Ignore());
        CreateMap<AuctionDtoPut, Auction>()
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.IsOpen, opt => opt.Ignore());
        
        CreateMap<Purchase, PurchaseDto>().ReverseMap();
        CreateMap<PurchaseDtoPost, Purchase>()
            .ForMember(dest => dest.IsActive, opt => opt.Ignore());
        CreateMap<PurchaseDtoPut, Purchase>()
            .ForMember(dest => dest.IsActive, opt => opt.Ignore());
        
        CreateMap<Movement, MovementDto>().ReverseMap();
        CreateMap<MovementDtoPost, Movement>()
            .ForMember(dest => dest.IsActive, opt => opt.Ignore());
    }
}
