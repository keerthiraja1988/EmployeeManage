using AutoMapper;
using DomainModel;
using DomainModel.DashBoard;
using DomainModel.EmployeeManage;
using DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Areas.DashBoard.Models;
using WebAppCore.Areas.EmployeeManage.Models;
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

            CreateMap<EmployeeAddressViewModel, EmployeeAddressModel>()
                .ReverseMap();
            CreateMap<EmployeeSearchViewModel, EmployeeSearchModel>()
               .ReverseMap();
            CreateMap<EmployeeViewModel, EmployeeModel>()
                .ReverseMap();

            CreateMap<CountryViewModel, CountryModel>()
                .ReverseMap();

            CreateMap<DashBoardRow1WidgetsViewModel, DashBoardRow1WidgetsModel>()
                .ReverseMap();

        }
    }
}
