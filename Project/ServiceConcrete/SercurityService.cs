using CrossCutting.Caching;
using DomainModel;
using Effortless.Net.Encryption;
using Insight.Database;
using Repository;
using ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ServiceConcrete
{
    public class SercurityService : ISercurityService
    {
        private IUserAccountRepository _IUserAccountRepository;

        public SercurityService()
        {
            SqlInsightDbProvider.RegisterProvider();
            //  string sqlConnection = "Data Source=.;Initial Catalog=EmployeeManage;Integrated Security=True";
            string sqlConnection = Caching.Instance.GetApplicationConfigs("DBConnection")
                                           ;
            DbConnection c = new SqlConnection(sqlConnection);

            _IUserAccountRepository = c.As<IUserAccountRepository>();
        }

        public UserAccountModel GenerateHashAndSaltForPassword(UserAccountModel userAccountModel)
        {
            userAccountModel.PasswordSalt = Strings.CreateSalt(20);

            string passwordConcated = userAccountModel.Password + userAccountModel.PasswordSalt;

            string generatedHashFromPassAndSalt = Hash.Create(HashType.SHA512, passwordConcated, string.Empty, false);

            userAccountModel.PasswordHash = generatedHashFromPassAndSalt;
            return userAccountModel;
        }


        public UserAccountModel ValidateUserLoginAndCredential(UserAccountModel userAccountModel)
        {
            bool isValidUser = false;
            UserAccountModel getUserAccount = new UserAccountModel();
            try
            {
                getUserAccount = this._IUserAccountRepository.GetUserDetailsForLogin(userAccountModel);
                if (getUserAccount == null)
                {
                    isValidUser = false;
                }
                else
                {
                    string passwordConcated = userAccountModel.Password + getUserAccount.PasswordSalt;
                    string generatedHashFromPassAndSalt = Hash.Create(HashType.SHA512, passwordConcated, string.Empty, false);
                    if (String.Compare(generatedHashFromPassAndSalt, getUserAccount.PasswordHash) == 0)
                    {
                        isValidUser = true;
                    }
                    userAccountModel = getUserAccount;
                }

                getUserAccount.IsLoginSuccess = isValidUser;
            }
            catch (Exception Ex)
            {

            }

            return userAccountModel;
        }

       

    }
}
