using DomainModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterface
{
    public interface ISercurityService
    {
        UserAccountModel GenerateHashAndSaltForPassword(UserAccountModel userAccountModel);

        UserAccountModel ValidateUserLoginAndCredential(UserAccountModel userAccountModel);
    }
}
