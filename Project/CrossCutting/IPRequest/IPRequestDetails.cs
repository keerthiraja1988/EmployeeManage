using DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CrossCutting.IPRequest
{
    public class IPRequestDetails : IIPRequestDetails
    {
        public string IPRequestHelper(string url)
        {
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();

            StreamReader responseStream = new StreamReader(objResponse.GetResponseStream());
            string responseRead = responseStream.ReadToEnd();

            responseStream.Close();
            responseStream.Dispose();

            return responseRead;
        }

        public IpPropertiesModal GetCountryDetailsByIP(string ipAddress)
        {
            IpPropertiesModal ipProperties = new IpPropertiesModal();

            try
            {
                string ipResponse = IPRequestHelper("http://ip-api.com/xml/" + ipAddress);
                using (TextReader sr = new StringReader(ipResponse))
                {
                    using (System.Data.DataSet dataBase = new System.Data.DataSet())
                    {
                        dataBase.ReadXml(sr);
                        ipProperties.Status = dataBase.Tables[0].Rows[0][0].ToString();
                        ipProperties.Country = dataBase.Tables[0].Rows[0][1].ToString();
                        ipProperties.CountryCode = dataBase.Tables[0].Rows[0][2].ToString();
                        ipProperties.Region = dataBase.Tables[0].Rows[0][3].ToString();
                        ipProperties.RegionName = dataBase.Tables[0].Rows[0][4].ToString();
                        ipProperties.City = dataBase.Tables[0].Rows[0][5].ToString();
                        ipProperties.Zip = dataBase.Tables[0].Rows[0][6].ToString();
                        ipProperties.Lat = dataBase.Tables[0].Rows[0][7].ToString();
                        ipProperties.Lon = dataBase.Tables[0].Rows[0][8].ToString();
                        ipProperties.TimeZone = dataBase.Tables[0].Rows[0][9].ToString();
                        ipProperties.ISP = dataBase.Tables[0].Rows[0][10].ToString();
                        ipProperties.ORG = dataBase.Tables[0].Rows[0][11].ToString();
                        ipProperties.ISPDetails = dataBase.Tables[0].Rows[0][12].ToString();
                        ipProperties.Query = dataBase.Tables[0].Rows[0][13].ToString();
                    }
                }
            }
            catch (Exception)
            {
            }
            return ipProperties;
        }
    }

    public interface IIPRequestDetails
    {
        IpPropertiesModal GetCountryDetailsByIP(string ipAddress);
    }
}