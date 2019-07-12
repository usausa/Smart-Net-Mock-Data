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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public static T Using<T>(this IConnectionFactory factory, Func<IDbConnection, T> func)
        {
            using (var con = factory.CreateConnection())
            {
                return func(con);
            }
        }
    }
}
