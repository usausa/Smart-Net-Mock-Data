namespace Smart.Mock.Data;

using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

public sealed class MockDbConnection : DbConnection
{
    private readonly Queue<MockDbCommand> setupedCommands = new();
    private readonly List<MockDbCommand> commands = [];
    private readonly List<MockDbTransaction> transactions = [];

    private string database = string.Empty;

    private ConnectionState state;

    public IList<MockDbCommand> Commands => commands;

    public IList<MockDbTransaction> Transactions => transactions;

    [AllowNull]
    public override string ConnectionString { get; set; }

    public override string Database => database;

    public override string DataSource => string.Empty;

    public override string ServerVersion => string.Empty;

    public override ConnectionState State => state;

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
        var tx = new MockDbTransaction(this, isolationLevel);
        transactions.Add(tx);
        return tx;
    }

    protected override DbCommand CreateDbCommand()
    {
        var command = setupedCommands.Count > 0 ? setupedCommands.Dequeue() : new MockDbCommand { Connection = this };
        commands.Add(command);
        return command;
    }

    public void SetupCommand(MockDbCommand command)
    {
        setupedCommands.Enqueue(command);
    }

#pragma warning disable CA1062
    public void SetupCommand(Action<MockDbCommand> action)
    {
#pragma warning disable CA2000
        var command = new MockDbCommand();
#pragma warning restore CA2000
        action(command);
        setupedCommands.Enqueue(command);
    }
#pragma warning restore CA1062
}
