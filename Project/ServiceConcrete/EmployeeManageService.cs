using CrossCutting.Caching;
using DomainModel.EmployeeManage;
using FizzWare.NBuilder;
using Insight.Database;
using Repository;
using ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
namespace ServiceConcrete
{
    public class EmployeeManageService : IEmployeeManageService
    {
        IEmployeeManageRepository _IEmployeeManageRepository;
        public EmployeeManageService(DbConnection Parameter)
        {
            SqlInsightDbProvider.RegisterProvider();
            //  string sqlConnection = "Data Source=.;Initial Catalog=EmployeeManage;Integrated Security=True";
            string sqlConnection = Caching.Instance.GetApplicationConfigs("DBConnection")
                                           ;
            DbConnection c = new SqlConnection(sqlConnection);

            _IEmployeeManageRepository = c.As<IEmployeeManageRepository>();
        }

        public List<EmployeeModel> LoadEmployeeData()
        {
            //List<EmployeeModel> employeeDetails = new List<EmployeeModel>();

            var employeeDetails = Builder<EmployeeModel>.CreateListOfSize(100)
                .All()
            .With(c => c.FullName = Faker.Name.FullName())
            .With(c => c.FirstName = Faker.Name.FirstName())
            .With(c => c.LastName = Faker.Name.LastName())
            .With(c => c.Email = Faker.User.Email())
            .With(c => c.Initial = Faker.Name.Gender())

            .With(c => c.DateOfBirth = Faker.Date.Birthday())
            .With(c => c.DateOfJoining = Faker.Date.Recent(5))
            .With(c => c.TIN = Faker.Lorem.Letters(10).ToString())
            .With(c => c.PASSPORT = Faker.Lorem.Letters(10).ToString())
            .With(c => c.UserId = 1)

            .Build();

            foreach (var item in employeeDetails)
            {
                EmployeeAddressModel employeeAddressModelP = new EmployeeAddressModel();
                employeeAddressModelP.Address1 = Faker.Address.SecondaryAddress();
                employeeAddressModelP.Address2 = Faker.Address.SecondaryAddress();
                employeeAddressModelP.Address3 = Faker.Address.SecondaryAddress();
                employeeAddressModelP.City = Faker.Address.USCity();
                employeeAddressModelP.State = Faker.Address.State();
                employeeAddressModelP.Country = 10;
                employeeAddressModelP.AddressType ="P";

                EmployeeAddressModel employeeAddressModelC = new EmployeeAddressModel();
                employeeAddressModelC.Address1 = Faker.Address.SecondaryAddress();
                employeeAddressModelC.Address2 = Faker.Address.SecondaryAddress();
                employeeAddressModelC.Address3 = Faker.Address.SecondaryAddress();
                employeeAddressModelC.City = Faker.Address.USCity();
                employeeAddressModelC.State = Faker.Address.State();
                employeeAddressModelC.Country = 10;
                employeeAddressModelC.AddressType = "C";

               List<EmployeeAddressModel> employeeAddresses = new List<EmployeeAddressModel> ();
                employeeAddresses.Add(employeeAddressModelP);
                employeeAddresses.Add(employeeAddressModelC);

                var returnValue = this._IEmployeeManageRepository.LoadEmployeeData(item, employeeAddresses);
                //var returnValue = this._IEmployeeManageRepository.LoadEmployeeData(item);

            }




            return employeeDetails.ToList();
        }
    }
}
