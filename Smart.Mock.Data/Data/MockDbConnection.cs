namespace Smart.Mock.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    ///
    /// </summary>
    public sealed class MockDbConnection : IDbConnection
    {
        private readonly Queue<MockDbCommand> setupedCommands = new Queue<MockDbCommand>();
        private readonly List<MockDbCommand> commands = new List<MockDbCommand>();
        private readonly List<MockDbTransaction> transactions = new List<MockDbTransaction>();

        /// <summary>
        ///
        /// </summary>
        public IList<MockDbCommand> Commands
        {
            get { return commands; }
        }

        /// <summary>
        ///
        /// </summary>
        public IList<MockDbTransaction> Transactions
        {
            get { return transactions; }
        }

        /// <summary>
        ///
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int ConnectionTimeout { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Database { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public ConnectionState State { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        ///
        /// </summary>
        public void Open()
        {
            State = ConnectionState.Open;
        }

        /// <summary>
        ///
        /// </summary>
        public void Close()
        {
            State = ConnectionState.Closed;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="databaseName"></param>
        public void ChangeDatabase(string databaseName)
        {
            Database = databaseName;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IDbTransaction BeginTransaction()
        {
            var tx = new MockDbTransaction(this);
            transactions.Add(tx);
            return tx;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="il"></param>
        /// <returns></returns>
        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            var tx = new MockDbTransaction(this, il);
            transactions.Add(tx);
            return tx;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Factory")]
        public IDbCommand CreateCommand()
        {
            var command = setupedCommands.Count > 0 ? setupedCommands.Dequeue() : new MockDbCommand { Connection = this };
            commands.Add(command);
            return command;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="command"></param>
        public void SetupCommand(MockDbCommand command)
        {
            setupedCommands.Enqueue(command);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Factory")]
        public void SetupCommand(Action<MockDbCommand> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var command = new MockDbCommand();
            action(command);
            setupedCommands.Enqueue(command);
        }
    }
}
