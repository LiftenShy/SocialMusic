using System;
using System.Text;
using AutoMapper;
using SocialMusic.Api.Models;
using SocialMusic.API.Models;
using SocialMusic.Models.EntityModels;
using SocialMusic.Models.EntityModels.AuthModels;

namespace SocialMusic.API.Helper
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
