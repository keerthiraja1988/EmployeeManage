using CrossCutting.Caching;
using DomainModel;
using Insight.Database;
using Repository;
using ServiceInterface;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace ServiceConcrete
{
    public class UserAccountService : IUserAccountService
    {
        IUserAccountRepository _IUserAccountRepository;
        public UserAccountService(DbConnection Parameter)
        {
            SqlInsightDbProvider.RegisterProvider();
            //  string sqlConnection = "Data Source=.;Initial Catalog=EmployeeManage;Integrated Security=True";
            string sqlConnection = Caching.Instance.GetApplicationConfigs("DBConnection")
                                           ;
            DbConnection c = new SqlConnection(sqlConnection);

            _IUserAccountRepository = c.As<IUserAccountRepository>();
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
            SercurityService sercurityService = new SercurityService();
            userAccountModel = sercurityService.GenerateHashAndSaltForPassword(userAccountModel);

            return this._IUserAccountRepository.RegisterNewUser(userAccountModel);
        }

        public UserAccountModel ValidateUserLogin(UserAccountModel userAccountModel)
        {
            SercurityService sercurityService = new SercurityService();
            userAccountModel = sercurityService.ValidateUserLoginAndCredential(userAccountModel);

            return userAccountModel;
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
