namespace Smart.Mock.Infrastructure
{
    using System;
    using System.Data;

    /// <summary>
    ///
    /// </summary>
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    /// <summary>
    ///
    /// </summary>
    public static class ConnectionFactoryExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="action"></param>
        public static void Using(this IConnectionFactory factory, Action<IDbConnection> action)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using (var con = factory.CreateConnection())
            {
                action(con);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T Using<T>(this IConnectionFactory factory, Func<IDbConnection, T> func)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            using (var con = factory.CreateConnection())
            {
                return func(con);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="action"></param>
        public static void UsingTx(this IConnectionFactory factory, Action<IDbConnection, IDbTransaction> action)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using (var con = factory.CreateConnection())
            {
                con.Open();
                using (var tx = con.BeginTransaction())
                {
                    action(con, tx);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T UsingTx<T>(this IConnectionFactory factory, Func<IDbConnection, IDbTransaction, T> func)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            using (var con = factory.CreateConnection())
            {
                con.Open();
                using (var tx = con.BeginTransaction())
                {
                    return func(con, tx);
                }
            }
        }
    }
}
