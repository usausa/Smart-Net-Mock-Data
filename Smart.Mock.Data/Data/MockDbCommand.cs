namespace Smart.Mock.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class ExecutedCommand
    {
        public string CommandText { get; }

        public MockDbParameterCollection Parameters { get; }

        public ExecutedCommand(string commandText, MockDbParameterCollection parameters)
        {
            CommandText = commandText;
            Parameters = parameters;
        }
    }

    public sealed class MockDbCommand : DbCommand
    {
        private readonly List<ExecutedCommand> executedCommands = new List<ExecutedCommand>();

        private readonly Queue<object> setupedResults = new Queue<object>();

        private readonly MockDbParameterCollection parameters = new MockDbParameterCollection();

        public IList<ExecutedCommand> ExecutedCommands => executedCommands;

        public Action<ExecutedCommand> Executing { get; set; }

        protected override DbConnection DbConnection { get; set; }

        protected override DbTransaction DbTransaction { get; set; }

        public override string CommandText { get; set; }

        public override int CommandTimeout { get; set; }

        public override CommandType CommandType { get; set; }

        protected override DbParameterCollection DbParameterCollection => parameters;

        public override UpdateRowSource UpdatedRowSource { get; set; }

        public override bool DesignTimeVisible { get; set; }

        public override void Prepare()
        {
        }

        public override void Cancel()
        {
        }

        protected override DbParameter CreateDbParameter() => new MockDbParameter();

        public override int ExecuteNonQuery()
        {
            var command = new ExecutedCommand(CommandText, parameters);
            executedCommands.Add(command);
            Executing?.Invoke(command);
            return (int)setupedResults.Dequeue();
        }

        public override object ExecuteScalar()
        {
            var command = new ExecutedCommand(CommandText, parameters);
            executedCommands.Add(command);
            Executing?.Invoke(command);
            return setupedResults.Dequeue();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            var command = new ExecutedCommand(CommandText, parameters);
            executedCommands.Add(command);
            Executing?.Invoke(command);
            return (DbDataReader)setupedResults.Dequeue();
        }

        public void SetupResult(object result)
        {
            setupedResults.Enqueue(result);
        }
    }
}
