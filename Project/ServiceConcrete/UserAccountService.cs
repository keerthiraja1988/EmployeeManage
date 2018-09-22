using DomainModel;
using Insight.Database;
using Repository;
using ServiceInterface;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace ServiceConcrete
{
    public class UserAccountService : IUserAccountService
    {
        IUserAccountRepository _IUserAccountRepository;
        public UserAccountService(DbConnection Parameter)
        {
            SqlInsightDbProvider.RegisterProvider();
            string sqlConnection = "Data Source=.;Initial Catalog=EmployeeManage;Integrated Security=True";
            DbConnection c = new SqlConnection(sqlConnection);

            _IUserAccountRepository = c.As<IUserAccountRepository>();
        }

        ~UserAccountService() // destructor define
        {
            this._IUserAccountRepository?.Dispose();
        }


        public int GetTestValue()
        {
            return this._IUserAccountRepository.GetTestValue();
        }

        public bool RegisterNewUser(UserAccountModel userAccountModel)
        {
            return this._IUserAccountRepository.RegisterNewUser(userAccountModel);
        }

    }
}
