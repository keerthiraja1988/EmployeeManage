using CrossCutting.Caching;
using CrossCutting.Logging;
using DomainModel.EmployeeManage;
using DomainModel.Shared;
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

namespace ServiceConcrete
{
    [NLogging]
    public class EmployeeManageService : IEmployeeManageService
    {
        private readonly IEmployeeManageRepository _iEmployeeManageRepository;

        public EmployeeManageService(DbConnection parameter)
        {
            SqlInsightDbProvider.RegisterProvider();
            //  string sqlConnection = "Data Source=.;Initial Catalog=EmployeeManage;Integrated Security=True";
            string sqlConnection = Caching.Instance.GetApplicationConfigs("DBConnection")
                                           ;
            DbConnection c = new SqlConnection(sqlConnection);

            _iEmployeeManageRepository = c.AsParallel<IEmployeeManageRepository>();
        }

        public List<EmployeeModel> LoadEmployeeData()
        {
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
                EmployeeAddressModel employeeAddressModelP = new EmployeeAddressModel
                {
                    Address1 = Faker.Address.SecondaryAddress(),
                    Address2 = Faker.Address.SecondaryAddress(),
                    Address3 = Faker.Address.SecondaryAddress(),
                    City = Faker.Address.USCity(),
                    State = Faker.Address.State()
                };

                EmployeeAddressModel employeeAddressModelC = new EmployeeAddressModel
                {
                    Address1 = Faker.Address.SecondaryAddress(),
                    Address2 = Faker.Address.SecondaryAddress(),
                    Address3 = Faker.Address.SecondaryAddress(),
                    City = Faker.Address.USCity(),
                    State = Faker.Address.State()
                };

                List<EmployeeAddressModel> employeeAddresses = new List<EmployeeAddressModel>
                {
                    employeeAddressModelP, employeeAddressModelC
                };

                SqlInsightDbProvider.RegisterProvider();
                string sqlConnection1 = Caching.Instance.GetApplicationConfigs("DBConnection")
                                               ;
                DbConnection c1 = new SqlConnection(sqlConnection1);

                var iEmployeeManageRepository1 = c1.As<IEmployeeManageRepository>();

                var returnValue = iEmployeeManageRepository1.LoadEmployeeData(item, employeeAddresses);
            });
            return employeeDetails.ToList();
        }

        public async Task<List<EmployeeModel>> GetEmployeesDetails()
        {
            List<EmployeeModel> employeeDetails = new List<EmployeeModel>();
            employeeDetails = await Task.Run(() => this._iEmployeeManageRepository.GetEmployeesDetails().Result);
            //throw new Exception();
            return employeeDetails;
        }

        public async Task<EmployeeModel> GetEmployeeDetails(EmployeeModel employeeSearchModel)
        {
            return await Task.Run(() => this._iEmployeeManageRepository
                                                   .GetEmployeeDetails(employeeSearchModel).Result);
        }

        public async Task<List<EmployeeSearchModel>> GetEmployeesDetailsForSearch(EmployeeSearchModel employeeSearchModel)
        {
            List<EmployeeSearchModel> employeeSearchDetails = new List<EmployeeSearchModel>();
            employeeSearchDetails = await Task.Run(() => this._iEmployeeManageRepository
                                                    .GetEmployeesDetailsForSearch(employeeSearchModel).Result);
            return employeeSearchDetails;
        }

        public async Task<List<EmployeeModel>> GetEmployeesDetailsOnSearch(EmployeeSearchModel employeeSearchModel)
        {
            List<EmployeeModel> employeeDetails = new List<EmployeeModel>();
            employeeDetails = await Task.Run(() => this._iEmployeeManageRepository
                                                    .GetEmployeesDetailsOnSearch(employeeSearchModel).Result);
            return employeeDetails;
        }

        public async Task<int> EditEmployeeDetails(EmployeeModel employeeModel, List<EmployeeAddressModel> employeeAddresses)
        {
            return await Task.Run(() => this._iEmployeeManageRepository
                                                     .EditEmployeeDetails(employeeModel, employeeAddresses).Result);
        }

        public async Task<int> DeleteEmployee(EmployeeModel employeeModel)
        {
            return await Task.Run(() => this._iEmployeeManageRepository
                                                     .DeleteEmployee(employeeModel).Result);
        }

        public async Task<List<CountryModel>> GetCountries()
        {
            return await Task.Run(() => this._iEmployeeManageRepository
                                                     .GetCountries().Result);
        }

        public async Task<int> AddEmployee(EmployeeModel employeeModel, List<EmployeeAddressModel> employeeAddresses)
        {
            return await Task.Run(() => this._iEmployeeManageRepository
                                                    .AddEmployee(employeeModel, employeeAddresses).Result);
        }
    }
}