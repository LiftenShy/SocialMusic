using AccountManager.Data.Models;
using AccountManager.Data.Models.AccountViewModels;
using AutoMapper;

namespace AccountManager.Mapping
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            var map = CreateMap<RegisterViewModel, ApplicationUser>();

            map.ForMember(user => user.UserName, model => model.MapFrom(x => x.Email));

            var reverseMap = CreateMap<ApplicationUser, RegisterViewModel>();

            reverseMap.ForMember(model => model.Email, user => user.MapFrom(x => x.UserName));
        }
    }
}
