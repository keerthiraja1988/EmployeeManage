using CrossCutting.Caching;
using CrossCutting.Logging;
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
    [NLogging]

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
            userAccountModel.Password = DecryptStringAES(userAccountModel.CryptLoginPassword);
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
                    userAccountModel.Password = DecryptStringAES(userAccountModel.CryptLoginPassword);
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

        private static string DecryptStringAES(string cipherText)
        {
            var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
            var iv = Encoding.UTF8.GetBytes("8080808080808080");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.  
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.  
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.  
            return encrypted;
        }
    }
}
