using DomainModel;
using Insight.Database;
using System;

namespace Repository
{
    public interface IUserAccountRepository : IDisposable
    {
        [Sql("SELECT 1 AS ID")]
        int GetTestValue();

        [Sql("P_RegisterNewUser")]
        bool RegisterNewUser(UserAccountModel userAccountModel);

        [Sql("P_GetUserDetailsForLogin")]
        UserAccountModel GetUserDetailsForLogin(UserAccountModel userAccountModel);
    }
}
