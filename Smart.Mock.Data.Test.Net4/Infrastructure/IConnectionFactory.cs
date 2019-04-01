namespace Smart.Mock.Infrastructure
{
    using System;
    using System.Data;

    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public static class ConnectionFactoryExtensions
    {
        public static T Using<T>(this IConnectionFactory factory, Func<IDbConnection, T> func)
        {
            using (var con = factory.CreateConnection())
            {
                return func(con);
            }
        }
    }
}
