namespace Smart.Mock.Data
{
    using System.Data;
    using System.Data.Common;

    public sealed class MockRepeatDbConnection : DbConnection
    {
        private readonly object result;

        private string database;

        private ConnectionState state;

        public override string ConnectionString { get; set; }

        public override string Database => database;

        public override string DataSource => string.Empty;

        public override string ServerVersion => string.Empty;

        public override ConnectionState State => state;

        public MockRepeatDbConnection(object result)
        {
            this.result = result;
        }

        public override void Open()
        {
            state = ConnectionState.Open;
        }

        public override void Close()
        {
            state = ConnectionState.Closed;
        }

        public override void ChangeDatabase(string databaseName)
        {
            database = databaseName;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new MockDbTransaction(this, isolationLevel);
        }

        protected override DbCommand CreateDbCommand()
        {
            return new MockRepeatDbCommand(result);
        }
    }
}
