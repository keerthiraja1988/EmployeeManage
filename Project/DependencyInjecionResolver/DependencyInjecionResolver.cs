using Autofac;
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
                //var asm = System.Reflection.Assembly.LoadFrom("BAL.dll");
                var asm = typeof(ServiceLayerRegister).Assembly;

                builder.RegisterType<DbConnection>().As<IDbConnection>()
                                .WithParameter("connectionString", "Data Source =.; Initial Catalog = FileEncryption; Integrated Security = True")
                                ;

                //builder.RegisterAggregateService<IMyAggregateService>();
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
