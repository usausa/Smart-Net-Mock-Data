namespace Smart.Mock.Data
{
    using System.Data;

    /// <summary>
    ///
    /// </summary>
    public sealed class MockDbTransaction : IDbTransaction
    {
        /// <summary>
        ///
        /// </summary>
        public TransactionStatus TransactionStatus { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public IDbConnection Connection { get; }

        /// <summary>
        ///
        /// </summary>
        public IsolationLevel IsolationLevel { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connection"></param>
        public MockDbTransaction(IDbConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="isolationLevel"></param>
        public MockDbTransaction(IDbConnection connection, IsolationLevel isolationLevel)
        {
            Connection = connection;
            IsolationLevel = isolationLevel;
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            if (TransactionStatus == TransactionStatus.Unknown)
            {
                TransactionStatus = TransactionStatus.RollBacked;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void Commit()
        {
            TransactionStatus = TransactionStatus.Commited;
        }

        /// <summary>
        ///
        /// </summary>
        public void Rollback()
        {
            TransactionStatus = TransactionStatus.RollBacked;
        }
    }
}
