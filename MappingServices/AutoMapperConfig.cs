using AutoMapper;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.MappingServices;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Auction, AuctionDto>().ReverseMap();
        CreateMap<AuctionDtoPost, Auction>();
        CreateMap<AuctionDtoPut, Auction>();
        
        CreateMap<Purchase, PurchaseDto>().ReverseMap();
        CreateMap<PurchaseDtoPost, Purchase>();
        CreateMap<PurchaseDtoPut, Purchase>();
        
        CreateMap<Movement, MovementDto>().ReverseMap();
        CreateMap<MovementDtoPost, Movement>();
    }
}
