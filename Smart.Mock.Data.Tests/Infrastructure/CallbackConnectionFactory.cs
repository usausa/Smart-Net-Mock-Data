namespace Smart.Mock.Infrastructure;

using System.Data.Common;

public class CallbackConnectionFactory : IConnectionFactory
{
    private readonly Func<DbConnection> factory;

    public CallbackConnectionFactory(Func<DbConnection> factory)
    {
        this.factory = factory;
    }

    public DbConnection CreateConnection()
    {
        return factory();
    }
}
