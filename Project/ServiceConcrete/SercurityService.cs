using CrossCutting.Caching;
using DomainModel;
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

        public string Encrypt(string encryptString)
        {
            string EncryptionKey = "0ram@1234xxxxxxxxxxtttttuuuuuiiiiio";  //we can change the code converstion key as per our requirement    
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "0ram@1234xxxxxxxxxxtttttuuuuuiiiiio";  //we can change the code converstion key as per our requirement, but the decryption key should be same as encryption key    
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public UserAccountModel GenerateHashAndSaltForPassword(UserAccountModel userAccountModel)
        {
            userAccountModel = GenerateSaltAndHash(userAccountModel);
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

                    isValidUser = ReGenerateSaltAndHash(getUserAccount);
                    getUserAccount.IsLoginSuccess = isValidUser;
                    userAccountModel = getUserAccount;
                }
                
            }
            catch (Exception Ex)
            {


            }

            return userAccountModel;
        }

        private bool ReGenerateSaltAndHash(UserAccountModel userAccountModel)
        {

            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(userAccountModel.PasswordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(userAccountModel.Password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)

                return true;

            return false;


        }
        private UserAccountModel GenerateSaltAndHash(UserAccountModel userAccountModel)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(userAccountModel.Password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            userAccountModel.PasswordHash = Convert.ToBase64String(hashBytes);
            userAccountModel.PasswordSalt = Convert.ToBase64String(salt);

            return userAccountModel;
        }


    }
}
