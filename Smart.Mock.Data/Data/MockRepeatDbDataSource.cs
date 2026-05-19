namespace Smart.Mock.Data;

using System.Data.Common;

public sealed class MockRepeatDbDataSource : DbDataSource
{
    private readonly object? result;

    public override string ConnectionString { get; }

    public MockRepeatDbDataSource(object? result, string connectionString = "")
    {
        this.result = result;
        ConnectionString = connectionString;
    }

    protected override DbConnection CreateDbConnection() => new MockRepeatDbConnection(result);
}
