using Insight.Database;
using Repository;
using ServiceInterface;
using System;
using System.Data.Common;

namespace ServiceConcrete
{
    public class UserAccountService : IUserAccountService
    {
        IUserAccountRepository _IUserAccountRepository;
        public UserAccountService(DbConnection Parameter)
        {
            // SqlInsightDbProvider.RegisterProvider();
            // string sqlConnection = "";
            // DbConnection c = new SqlConnection(sqlConnection);

            _IUserAccountRepository = Parameter.As<IUserAccountRepository>();
        }

        ~UserAccountService() // destructor define
        {
            this._IUserAccountRepository?.Dispose();
        }


        public int GetTestValue()
        {
            return this._IUserAccountRepository.GetTestValue();
        }

    }
}
