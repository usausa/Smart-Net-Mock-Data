namespace Smart.Mock.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    ///
    /// </summary>
    public class ExecutedCommand
    {
        /// <summary>
        ///
        /// </summary>
        public string CommandText { get; }

        /// <summary>
        ///
        /// </summary>
        public IList<MockDbParameter> Parameters { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        public ExecutedCommand(string commandText, IList<MockDbParameter> parameters)
        {
            CommandText = commandText;
            Parameters = parameters;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class MockDbCommand : IDbCommand
    {
        private readonly List<ExecutedCommand> executedCommands = new List<ExecutedCommand>();

        private readonly Queue<object> setupedResults = new Queue<object>();

        private readonly MockDbParameterCollection parameters;

        /// <summary>
        ///
        /// </summary>
        public IList<ExecutedCommand> ExecutedCommands => executedCommands;

        /// <summary>
        ///
        /// </summary>
        public Action<ExecutedCommand> Executing { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IDbConnection Connection { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IDbTransaction Transaction { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int CommandTimeout { get; set; }

        /// <summary>
        ///
        /// </summary>
        public CommandType CommandType { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IDataParameterCollection Parameters => parameters;

        /// <summary>
        ///
        /// </summary>
        public UpdateRowSource UpdatedRowSource { get; set; }

        /// <summary>
        ///
        /// </summary>
        public MockDbCommand()
        {
            parameters = new MockDbParameterCollection(this);
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public void Prepare()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public void Cancel()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IDbDataParameter CreateParameter()
        {
            return new MockDbParameter();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public int ExecuteNonQuery()
        {
            var command = new ExecutedCommand(CommandText, parameters);
            executedCommands.Add(command);
            Executing?.Invoke(command);
            return (int)setupedResults.Dequeue();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IDataReader ExecuteReader()
        {
            var command = new ExecutedCommand(CommandText, parameters);
            executedCommands.Add(command);
            Executing?.Invoke(command);
            return (IDataReader)setupedResults.Dequeue();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            var command = new ExecutedCommand(CommandText, parameters);
            executedCommands.Add(command);
            Executing?.Invoke(command);
            return (IDataReader)setupedResults.Dequeue();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public object ExecuteScalar()
        {
            var command = new ExecutedCommand(CommandText, parameters);
            executedCommands.Add(command);
            Executing?.Invoke(command);
            return setupedResults.Dequeue();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="result"></param>
        public void SetupResult(object result)
        {
            setupedResults.Enqueue(result);
        }
    }
}
