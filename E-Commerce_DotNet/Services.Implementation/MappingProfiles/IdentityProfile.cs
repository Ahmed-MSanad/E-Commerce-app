using AutoMapper;
using Domain.Models.Identity;
using Shared.IdentityDtos;

namespace Services.Implementation.MappingProfiles
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
