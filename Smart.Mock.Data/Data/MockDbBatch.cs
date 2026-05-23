namespace Smart.Mock.Data;

using System.Data;
using System.Data.Common;

public sealed class ExecutedBatchCommand
{
    public string CommandText { get; }

    public CommandType CommandType { get; }

    public MockDbParameterCollection Parameters { get; }

    public ExecutedBatchCommand(string commandText, CommandType commandType, MockDbParameterCollection parameters)
    {
        CommandText = commandText;
        CommandType = commandType;
        Parameters = parameters;
    }
}

public sealed class MockDbBatch : DbBatch
{
    private readonly List<ExecutedBatchCommand> executedBatchCommands = [];

    private readonly Queue<object?> setupedResults = new();

    private readonly Queue<int[]> setupedRecordsAffected = new();

    private readonly MockDbBatchCommandCollection batchCommands = [];

    public IList<ExecutedBatchCommand> ExecutedBatchCommands => executedBatchCommands;

    public Action<IReadOnlyList<ExecutedBatchCommand>>? Executing { get; set; }

    protected override DbConnection? DbConnection { get; set; }

    protected override DbTransaction? DbTransaction { get; set; }

    protected override DbBatchCommandCollection DbBatchCommands => batchCommands;

    public override int Timeout { get; set; }

    protected override DbBatchCommand CreateDbBatchCommand() => new MockDbBatchCommand();

    public override int ExecuteNonQuery()
    {
        if (setupedResults.Count == 0)
        {
            throw new InvalidOperationException("No result has been setup for this batch.");
        }

        var snapshot = CaptureAndRecord();
        AssignRecordsAffected();
        Executing?.Invoke(snapshot);
        return (int)setupedResults.Dequeue()!;
    }

    public override object? ExecuteScalar()
    {
        if (setupedResults.Count == 0)
        {
            throw new InvalidOperationException("No result has been setup for this batch.");
        }

        var snapshot = CaptureAndRecord();
        AssignRecordsAffected();
        Executing?.Invoke(snapshot);
        return setupedResults.Dequeue();
    }

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        if (setupedResults.Count == 0)
        {
            throw new InvalidOperationException("No result has been setup for this batch.");
        }

        var snapshot = CaptureAndRecord();
        AssignRecordsAffected();
        Executing?.Invoke(snapshot);
        return (DbDataReader)setupedResults.Dequeue()!;
    }

    public override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(ExecuteNonQuery());
    }

    public override Task<object?> ExecuteScalarAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(ExecuteScalar());
    }

    protected override Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(ExecuteDbDataReader(behavior));
    }

    public override void Prepare()
    {
    }

    public override Task PrepareAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }

    public override void Cancel()
    {
    }

    public void SetupResult(object? result) => setupedResults.Enqueue(result);

    public void SetupRecordsAffected(params int[] recordsAffected) => setupedRecordsAffected.Enqueue(recordsAffected);

    private List<ExecutedBatchCommand> CaptureAndRecord()
    {
        var list = new List<ExecutedBatchCommand>(batchCommands.Count);
        foreach (var cmd in batchCommands)
        {
            var snapshot = new ExecutedBatchCommand(cmd.CommandText, cmd.CommandType, ((MockDbBatchCommand)cmd).MockParameters);
            executedBatchCommands.Add(snapshot);
            list.Add(snapshot);
        }

        return list;
    }

    private void AssignRecordsAffected()
    {
        if (setupedRecordsAffected.Count == 0)
        {
            return;
        }

        var values = setupedRecordsAffected.Dequeue();
        for (var i = 0; i < batchCommands.Count && i < values.Length; i++)
        {
            ((MockDbBatchCommand)batchCommands[i]).SetRecordsAffected(values[i]);
        }
    }
}
