using AccountManager.Data.Models;
using AccountManager.DtoModels;
using AutoMapper;

namespace AccountManager.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, AccountDto>();
            CreateMap<AccountDto, User>();
        }
    }
}
