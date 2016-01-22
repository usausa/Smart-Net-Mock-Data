namespace Smart.Mock.Infrastructure
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    ///
    /// </summary>
    public class DbProviderConnectionFactory : IConnectionFactory
    {
        private readonly DbProviderFactory factory;

        private readonly string connectionString;

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        public DbProviderConnectionFactory(string name)
        {
            var settings = ConfigurationManager.ConnectionStrings;
            if (settings == null)
            {
                throw new ConfigurationErrorsException("ConnectionStrings");
            }

            var css = settings.Cast<ConnectionStringSettings>().FirstOrDefault(cs => cs.Name == name);
            if (css == null)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "Provider name '{0}' is not found", name));
            }

            factory = DbProviderFactories.GetFactory(css.ProviderName);
            connectionString = css.ConnectionString;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateConnection()
        {
            var connection = factory.CreateConnection();
            if (connection != null)
            {
                connection.ConnectionString = connectionString;
            }

            return connection;
        }
    }
}
