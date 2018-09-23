using AutoMapper;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Areas.Security.Models;
using WebAppCore.Models;

namespace WebAppCore.Infrastructure
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<RegisterUserViewModel, UserAccountModel>()
                .ReverseMap();
            CreateMap<UserLoginViewModel, UserAccountModel>()
              .ForMember(dest => dest.UserName,
               opts => opts.MapFrom(src => src.LoginUserName))
               .ForMember(dest => dest.Password,
               opts => opts.MapFrom(src => src.LoginPassword))
                .ReverseMap();
        }
    }
}
