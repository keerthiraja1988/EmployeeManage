using DomainModel;
using DomainModel.Shared;
using Insight.Database;
using System;
using System.Collections.Generic;

namespace Repository
{
    public interface IUserAccountRepository : IDisposable
    {
        [Sql("SELECT 1 AS ID")]
        int GetTestValue();

        [Sql("P_RegisterNewUser")]
        bool RegisterNewUser(UserAccountModel userAccountModel);

        [Sql("P_GetUserDetailsForLogin")]      
        Results<UserAccountModel, UserRolesModel> GetUserDetailsForLogin(UserAccountModel userAccountModel);


        [Sql("P_GetUserDetailsForLastLogin")]
        Results<IpPropertiesModal, IpPropertiesModal> GetUserDetailsForLastLogin(UserAccountModel userAccountModel);

    }
}
