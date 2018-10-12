using CrossCutting.Caching;
using CrossCutting.IPRequest;
using CrossCutting.Logging;
using DomainModel;
using DomainModel.Shared;
using Insight.Database;
using Repository;
using ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace ServiceConcrete
{
    [NLogging]

    public class UserAccountService : IUserAccountService
    {
        IUserAccountRepository _IUserAccountRepository;
        ISercurityService _ISercurityService;

        public UserAccountService(DbConnection Parameter, ISercurityService iSercurityService)
        {
            SqlInsightDbProvider.RegisterProvider();
            string sqlConnection = Caching.Instance.GetApplicationConfigs("DBConnection");
            DbConnection c = new SqlConnection(sqlConnection);
            _IUserAccountRepository = c.As<IUserAccountRepository>();

            _ISercurityService = iSercurityService;
        }

        //~UserAccountService() // destructor define
        //{
        //    this._IUserAccountRepository?.Dispose();
        //}


        public int GetTestValue()
        {
            return this._IUserAccountRepository.GetTestValue();
        }

        public bool RegisterNewUser(UserAccountModel userAccountModel)
        {
           //SercurityService sercurityService = new SercurityService();
            userAccountModel = _ISercurityService.GenerateHashAndSaltForPassword(userAccountModel);

            return this._IUserAccountRepository.RegisterNewUser(userAccountModel);
        }

        public (UserAccountModel UserAccount, List<UserRolesModel> UserRoles) ValidateUserLogin(UserAccountModel userAccountModel)
        {
           // SercurityService sercurityService = new SercurityService();
          

           
            
            return _ISercurityService.ValidateUserLoginAndCredential(userAccountModel);
        }

        public UserAccountModel GetAutoGenetaratedUserData()
        {
            UserAccountModel userAccountModel = new UserAccountModel();
            userAccountModel.UserName = Faker.User.Username();
            userAccountModel.FirstName = Faker.Name.FirstName();
            userAccountModel.LastName = Faker.Name.LastName();
            userAccountModel.Email = Faker.User.Email();
            userAccountModel.Password = Faker.User.Password();

            return userAccountModel;
        }

    }
}
