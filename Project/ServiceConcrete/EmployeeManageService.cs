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
using System.Threading.Tasks;
using System.Data;
using CrossCutting.Logging;
using DomainModel.Shared;

namespace ServiceConcrete
{
    [NLogging]
    public class EmployeeManageService : IEmployeeManageService
    {
        private IEmployeeManageRepository _IEmployeeManageRepository;

        public EmployeeManageService(DbConnection Parameter)
        {
            SqlInsightDbProvider.RegisterProvider();
            //  string sqlConnection = "Data Source=.;Initial Catalog=EmployeeManage;Integrated Security=True";
            string sqlConnection = Caching.Instance.GetApplicationConfigs("DBConnection")
                                           ;
            DbConnection c = new SqlConnection(sqlConnection);

            _IEmployeeManageRepository = c.AsParallel<IEmployeeManageRepository>();
        }

        public async Task<List<EmployeeModel>> LoadEmployeeData()
        {
            //List<EmployeeModel> employeeDetails = new List<EmployeeModel>();

            var employeeDetails = Builder<EmployeeModel>.CreateListOfSize(1000)
                .All()
            .With(c => c.FullName = Faker.Name.FullName())
            .With(c => c.FirstName = Faker.Name.FirstName())
            .With(c => c.LastName = Faker.Name.LastName())
            .With(c => c.Email = Faker.User.Email())
            .With(c => c.Initial = Faker.Name.Gender())

            .With(c => c.DateOfBirth = Faker.Date.Birthday())
            .With(c => c.DateOfJoining = Faker.Date.Recent(5))
            .With(c => c.TIN = Faker.Number.RandomNumber(56123488).ToString())
            .With(c => c.PASSPORT = Faker.Number.RandomNumber(86123488).ToString())
            .With(c => c.UserId = 1)

            .Build();

            Parallel.ForEach(employeeDetails, new ParallelOptions { MaxDegreeOfParallelism = 30 }, item =>
            {
                EmployeeAddressModel employeeAddressModelP = new EmployeeAddressModel();
                employeeAddressModelP.Address1 = Faker.Address.SecondaryAddress();
                employeeAddressModelP.Address2 = Faker.Address.SecondaryAddress();
                employeeAddressModelP.Address3 = Faker.Address.SecondaryAddress();
                employeeAddressModelP.City = Faker.Address.USCity();
                employeeAddressModelP.State = Faker.Address.State();

                EmployeeAddressModel employeeAddressModelC = new EmployeeAddressModel();
                employeeAddressModelC.Address1 = Faker.Address.SecondaryAddress();
                employeeAddressModelC.Address2 = Faker.Address.SecondaryAddress();
                employeeAddressModelC.Address3 = Faker.Address.SecondaryAddress();
                employeeAddressModelC.City = Faker.Address.USCity();
                employeeAddressModelC.State = Faker.Address.State();

                List<EmployeeAddressModel> employeeAddresses = new List<EmployeeAddressModel>();
                employeeAddresses.Add(employeeAddressModelP);
                employeeAddresses.Add(employeeAddressModelC);

                IEmployeeManageRepository _IEmployeeManageRepository1;

                SqlInsightDbProvider.RegisterProvider();
                //  string sqlConnection = "Data Source=.;Initial Catalog=EmployeeManage;Integrated Security=True";
                string sqlConnection1 = Caching.Instance.GetApplicationConfigs("DBConnection")
                                               ;
                DbConnection c1 = new SqlConnection(sqlConnection1);

                _IEmployeeManageRepository1 = c1.As<IEmployeeManageRepository>();

                var returnValue = _IEmployeeManageRepository1.LoadEmployeeData(item, employeeAddresses);
                //var returnValue = this._IEmployeeManageRepository.LoadEmployeeData(item);
            });
            return employeeDetails.ToList();
        }

        public async Task<List<EmployeeModel>> GetEmployeesDetails()
        {
            List<EmployeeModel> employeeDetails = new List<EmployeeModel>();
            employeeDetails = await Task.Run(() => this._IEmployeeManageRepository.GetEmployeesDetails().Result);
            //throw new Exception();
            return employeeDetails;
        }

        public async Task<EmployeeModel> GetEmployeeDetails(EmployeeModel employeeSearchModel)
        {
            return await Task.Run(() => this._IEmployeeManageRepository
                                                   .GetEmployeeDetails(employeeSearchModel).Result);
        }

        public async Task<List<EmployeeSearchModel>> GetEmployeesDetailsForSearch(EmployeeSearchModel employeeSearchModel)
        {
            List<EmployeeSearchModel> employeeSearchDetails = new List<EmployeeSearchModel>();
            employeeSearchDetails = await Task.Run(() => this._IEmployeeManageRepository
                                                    .GetEmployeesDetailsForSearch(employeeSearchModel).Result);
            return employeeSearchDetails;
        }

        public async Task<List<EmployeeModel>> GetEmployeesDetailsOnSearch(EmployeeSearchModel employeeSearchModel)
        {
            List<EmployeeModel> employeeDetails = new List<EmployeeModel>();
            employeeDetails = await Task.Run(() => this._IEmployeeManageRepository
                                                    .GetEmployeesDetailsOnSearch(employeeSearchModel).Result);
            return employeeDetails;
        }

        public async Task<int> EditEmployeeDetails(EmployeeModel employeeModel, List<EmployeeAddressModel> employeeAddresses)
        {
            return await Task.Run(() => this._IEmployeeManageRepository
                                                     .EditEmployeeDetails(employeeModel, employeeAddresses).Result);
        }

        public async Task<int> DeleteEmployee(EmployeeModel employeeModel)
        {
            return await Task.Run(() => this._IEmployeeManageRepository
                                                     .DeleteEmployee(employeeModel).Result);
        }

        public async Task<List<CountryModel>> GetCountries()
        {
            return await Task.Run(() => this._IEmployeeManageRepository
                                                     .GetCountries().Result);
        }

        public async Task<int> AddEmployee(EmployeeModel employeeModel, List<EmployeeAddressModel> employeeAddresses)
        {
            return await Task.Run(() => this._IEmployeeManageRepository
                                                    .AddEmployee(employeeModel, employeeAddresses).Result);
        }
    }
}