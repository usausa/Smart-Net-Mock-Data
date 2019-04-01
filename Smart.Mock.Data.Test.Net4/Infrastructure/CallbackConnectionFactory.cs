namespace Smart.Mock.Infrastructure
{
    using System;
    using System.Data;

    public class CallbackConnectionFactory : IConnectionFactory
    {
        private readonly Func<IDbConnection> factory;

        public CallbackConnectionFactory(Func<IDbConnection> factory)
        {
            this.factory = factory;
        }

        public IDbConnection CreateConnection()
        {
            return factory();
        }
    }
}
