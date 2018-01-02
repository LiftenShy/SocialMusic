using System.Text;
using AutoMapper;
using DataLayer.DataLayer.Models;
using DataLayer.Models.AuthModels;
using SocialMusic.Models.EntityModels;

namespace DataLayer.DataLayer.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonModel>();
            CreateMap<PersonModel, Person>();

            CreateMap<UserProfile, UserProfileModel>()
            .ForMember(model => model.Password, upm => upm.MapFrom(up => Encoding.ASCII.GetString(up.PasswordHash)));

            CreateMap<UserProfileModel, UserProfile>();
        }
    }
}
