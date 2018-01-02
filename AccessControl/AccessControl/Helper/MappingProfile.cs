using AccessControl.Models;
using AutoMapper;
using DataLayer.Models.AuthModels;

namespace AccessControl.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserProfile, UserProfileModel>()
                .ForMember(model => model.Password, upm => upm.MapFrom(up => CryptoService.Decrypto(up.PasswordHash)));

            CreateMap<UserProfileModel, UserProfile>()
                .ForMember(model => model.PasswordHash, up => up.MapFrom(upm => CryptoService.Crypto(upm.Password)));
        }

    }
}
