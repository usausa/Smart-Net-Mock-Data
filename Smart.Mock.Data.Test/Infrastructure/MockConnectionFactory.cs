namespace Smart.Mock.Infrastructure
{
    using System.Data;

    using Smart.Mock.Data;

    /// <summary>
    ///
    /// </summary>
    public class MockConnectionFactory : IConnectionFactory
    {
        private readonly MockDbConnection connection;

        /// <summary>
        ///
        /// </summary>
        /// <param name="connection"></param>
        public MockConnectionFactory(MockDbConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateConnection()
        {
            return connection;
        }
    }
}
