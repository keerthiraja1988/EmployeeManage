using Autofac;
using CrossCutting.EmailService;
using CrossCutting.IPRequest;
using CrossCutting.WeatherForecast;
using ServiceConcrete;
using ServiceInterface;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DependencyInjecionResolver
{
    public class DependencyInjecionResolver
    {
        public class ServiceDIContainer : Autofac.Module
        {
            string sqlConnection;
            public ServiceDIContainer(string _sqlConnection)
            {
                sqlConnection = _sqlConnection;
            }

            protected override void Load(ContainerBuilder builder)
            {

                DbConnection sqlDBConnection = new SqlConnection(sqlConnection);
            
                var asm = typeof(ServiceLayerRegister).Assembly;

                builder.RegisterType<IpRequestDetails>().As<IIpRequestDetails>().SingleInstance().PreserveExistingDefaults(); ;
                builder.RegisterType<WeatherForecast>().As<IWeatherForecast>().SingleInstance().PreserveExistingDefaults(); ;
                builder.RegisterType<EmailService>().As<IEmailService>().SingleInstance().PreserveExistingDefaults(); ;

                builder.RegisterAssemblyTypes(asm)
                .Where(t => t.Name.EndsWith("Service"))
                 .AsImplementedInterfaces()
                .WithParameter(new TypedParameter(typeof(DbConnection), sqlDBConnection))
                .InstancePerLifetimeScope()
                ;

                //builder.Register(ctx =>  {
                //            return new FileEncryptionService(sqlDBConnection);
                //                })
                //       .As<IFileEncryptionService>();

                //builder.RegisterType<UserAccountService>().As<IUserAccountService>()
                //                .WithParameter(new TypedParameter(typeof(DbConnection), sqlDBConnection))
                //                ;
                base.Load(builder);

            }

        }
    }
}
