using DomainModel;
using System;

namespace ServiceInterface
{
    public interface IUserAccountService
    {
        int GetTestValue();

        bool RegisterNewUser(UserAccountModel userAccountModel);

        UserAccountModel GetAutoGenetaratedUserData();


    }
}
