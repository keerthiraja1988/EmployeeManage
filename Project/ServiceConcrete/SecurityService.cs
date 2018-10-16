using CrossCutting.Caching;
using CrossCutting.IPRequest;
using CrossCutting.Logging;
using CrossCutting.WeatherForecast;
using DomainModel;
using DomainModel.Shared;
using Effortless.Net.Encryption;
using Insight.Database;
using Repository;
using ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ServiceConcrete
{
    [NLogging]
    public class SecurityService : ISecurityService
    {
        private readonly IUserAccountRepository _iUserAccountRepository;
        // ReSharper disable once IdentifierTypo
        private readonly IAppAnalyticsRepository _iAppAnalyticsRepository;
        private readonly IIpRequestDetails _iipRequestDetails;

        public SecurityService(IIpRequestDetails iIpRequestDetails
           )
        {
            SqlInsightDbProvider.RegisterProvider();
            //  string sqlConnection = "Data Source=.;Initial Catalog=EmployeeManage;Integrated Security=True";
            string sqlConnection = Caching.Instance.GetApplicationConfigs("DBConnection")
                                           ;
            DbConnection c = new SqlConnection(sqlConnection);

            _iUserAccountRepository = c.As<IUserAccountRepository>();
            _iAppAnalyticsRepository = c.As<IAppAnalyticsRepository>();
            _iipRequestDetails = iIpRequestDetails;
        }

        public UserAccountModel GenerateHashAndSaltForPassword(UserAccountModel userAccountModel)
        {
            userAccountModel.PasswordSalt = Strings.CreateSalt(20);
            userAccountModel.Password = DecryptStringAes(userAccountModel.CryptLoginPassword);
            string passwordConcated = userAccountModel.Password + userAccountModel.PasswordSalt;

            string generatedHashFromPassAndSalt = Hash.Create(HashType.SHA512, passwordConcated, string.Empty, false);
            userAccountModel.PasswordHash = generatedHashFromPassAndSalt;
            return userAccountModel;
        }


        public (UserAccountModel UserAccount, List<UserRolesModel> UserRoles) ValidateUserLoginAndCredential(UserAccountModel userAccountModel)
        {
            bool isValidUser = false;
            Guid cookieUniqueId;
            cookieUniqueId = Guid.NewGuid();
            UserAccountModel getUserAccount = new UserAccountModel();
            List<UserRolesModel> userRoles = new List<UserRolesModel>();
            IpPropertiesModal ipPropertiesModal = new IpPropertiesModal();

            getUserAccount.CookieUniqueId = cookieUniqueId;
            if (userAccountModel.UserIpAddress == "::1")
            {
                userAccountModel.UserIpAddress = Faker.Internet.IPv4();
            }

            try
            {
                var resultSet = this._iUserAccountRepository.GetUserDetailsForLogin(userAccountModel);

                ipPropertiesModal = _iipRequestDetails.GetCountryDetailsByIp(userAccountModel.UserIpAddress);
                ipPropertiesModal.IpAddress = userAccountModel.UserIpAddress;
                ipPropertiesModal.CreatedByUserName = userAccountModel.UserName;
                ipPropertiesModal.ModifiedByUserName = userAccountModel.UserName;
                ipPropertiesModal.UserName = userAccountModel.UserName;
                ipPropertiesModal.CreatedOn = DateTime.Now;
                ipPropertiesModal.ModifiedOn = DateTime.Now;
                ipPropertiesModal.CookieUniqueId = cookieUniqueId;

                if (resultSet.Set1 == null)
                {
                    // ReSharper disable once RedundantAssignment
                    isValidUser = false;
                }
                else
                {
                    getUserAccount = resultSet.Set1.FirstOrDefault();
                    userRoles = resultSet.Set2.ToList();

                    userAccountModel.Password = DecryptStringAes(userAccountModel.CryptLoginPassword);
                    if (getUserAccount != null)
                    {
                        string passwordConcated = userAccountModel.Password + getUserAccount.PasswordSalt;
                        string generatedHashFromPassAndSalt = Hash.Create(HashType.SHA512, passwordConcated, string.Empty, false);
                        if (string.CompareOrdinal(generatedHashFromPassAndSalt, getUserAccount.PasswordHash) == 0)
                        {
                            isValidUser = true;
                        }
                    }

                    userAccountModel = getUserAccount;
                    if (userAccountModel != null)
                    {
                        ipPropertiesModal.UserId = userAccountModel.UserId;
                        ipPropertiesModal.CreatedBy = userAccountModel.UserId;
                    }
                }

                if (getUserAccount != null) getUserAccount.IsLoginSuccess = isValidUser;

                ipPropertiesModal.IsLoginSuccess = isValidUser;
                var dbUpdateResult = _iAppAnalyticsRepository.SaveIpAddressDetailsOnLogin(ipPropertiesModal);
            }

            catch (Exception)
            {
                var dbUpdateResult = _iAppAnalyticsRepository.SaveIpAddressDetailsOnLogin(ipPropertiesModal);

            }

            // ReSharper disable once PossibleNullReferenceException
            userAccountModel.CookieUniqueId = cookieUniqueId;
            return (userAccountModel, userRoles);
        }

        private static string DecryptStringAes(string cipherText)
        {
            var keyBytes = Encoding.UTF8.GetBytes("8080808080808080");
            var iv = Encoding.UTF8.GetBytes("8080808080808080");

            var encrypted = Convert.FromBase64String(cipherText);
            var decryptedFromJavascript = DecryptStringFromBytes(encrypted, keyBytes, iv);
            return string.Format(decryptedFromJavascript);
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
            string plaintext = string.Empty;

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
