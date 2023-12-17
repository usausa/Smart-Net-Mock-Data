namespace Smart.Mock.Infrastructure;

using System.Data.Common;

public interface IConnectionFactory
{
    DbConnection CreateConnection();
}

#pragma warning disable CA1062
public static class ConnectionFactoryExtensions
{
    public static T Using<T>(this IConnectionFactory factory, Func<DbConnection, T> func)
    {
        using var con = factory.CreateConnection();
        return func(con);
    }
}
#pragma warning restore CA1062
