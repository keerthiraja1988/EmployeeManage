using DomainModel;
using System;
using System.Collections.Generic;

namespace ServiceInterface
{
    public interface IUserAccountService
    {
        int GetTestValue();

        bool RegisterNewUser(UserAccountModel userAccountModel);

        UserAccountModel GetAutoGenetaratedUserData();

        (UserAccountModel UserAccount, List<UserRolesModel> UserRoles) ValidateUserLogin(UserAccountModel userAccountModel);

    }
}
