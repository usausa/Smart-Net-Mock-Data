namespace Smart.Mock.Infrastructure;

using System.Data.Common;

public interface IConnectionFactory
{
    DbConnection CreateConnection();
}

public static class ConnectionFactoryExtensions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
    public static T Using<T>(this IConnectionFactory factory, Func<DbConnection, T> func)
    {
        using var con = factory.CreateConnection();
        return func(con);
    }
}
