using DomainModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterface
{
    public interface ISercurityService
    {
        string Encrypt(string encryptString);

        string Decrypt(string cipherText);

        UserAccountModel GenerateHashAndSaltForPassword(UserAccountModel userAccountModel);

        UserAccountModel ValidateUserLoginAndCredential(UserAccountModel userAccountModel);
    }
}
