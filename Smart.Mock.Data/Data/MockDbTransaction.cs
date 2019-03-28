namespace Smart.Mock.Data
{
    using System.Data;
    using System.Data.Common;

    public sealed class MockDbTransaction : DbTransaction
    {
        public TransactionStatus TransactionStatus { get; private set; } = TransactionStatus.Unknown;

        protected override DbConnection DbConnection { get; }

        public override IsolationLevel IsolationLevel { get; }

        public MockDbTransaction(DbConnection connection, IsolationLevel isolationLevel)
        {
            DbConnection = connection;
            IsolationLevel = isolationLevel;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (TransactionStatus == TransactionStatus.Unknown)
                {
                    TransactionStatus = TransactionStatus.RollBacked;
                }
            }

            base.Dispose(disposing);
        }

        public override void Commit()
        {
            TransactionStatus = TransactionStatus.Committed;
        }

        public override void Rollback()
        {
            TransactionStatus = TransactionStatus.RollBacked;
        }
    }
}
