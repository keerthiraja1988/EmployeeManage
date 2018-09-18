using Insight.Database;
using System;

namespace Repository
{
    public interface IUserAccountRepository : IDisposable
    {
        [Sql("SELECT TOP 1 [Id]  FROM [dbo].[Log]")]
        int GetTestValue();
    }
}
