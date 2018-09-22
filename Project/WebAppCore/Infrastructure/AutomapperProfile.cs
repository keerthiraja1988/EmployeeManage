using AutoMapper;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Areas.Security.Models;

namespace WebAppCore.Infrastructure
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<RegisterUserViewModel, UserAccountModel>()
                .ReverseMap();
        }
    }
}
