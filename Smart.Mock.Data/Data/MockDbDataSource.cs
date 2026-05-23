namespace Smart.Mock.Data;

using System.Data.Common;

#pragma warning disable IDE0032
public sealed class MockDbDataSource : DbDataSource
{
    private readonly Queue<MockDbConnection> setupedConnections = new();

    private readonly List<MockDbConnection> connections = [];

    public override string ConnectionString { get; }

    public IList<MockDbConnection> Connections => connections;

    public MockDbDataSource(string connectionString = "")
    {
        ConnectionString = connectionString;
    }

    protected override DbConnection CreateDbConnection()
    {
        var connection = setupedConnections.Count > 0 ? setupedConnections.Dequeue() : new MockDbConnection();
        connection.ConnectionString = ConnectionString;
        connections.Add(connection);
        return connection;
    }

    protected override DbBatch CreateDbBatch() => new MockDbBatch();

    public void SetupConnection(MockDbConnection connection) => setupedConnections.Enqueue(connection);

#pragma warning disable CA1062
    public void SetupConnection(Action<MockDbConnection> action)
    {
#pragma warning disable CA2000
        var connection = new MockDbConnection();
#pragma warning restore CA2000
        action(connection);
        setupedConnections.Enqueue(connection);
    }
#pragma warning restore CA1062
}
#pragma warning restore IDE0032
