namespace Smart.Mock;

using System.Data.Common;

using Smart.Mock.Data;

#pragma warning disable xUnit1051
public sealed class DataSourceTest
{
    [Fact]
    public void OpenConnectionReturnsSetupConnection()
    {
        using var dataSource = new MockDbDataSource("Server=mock");
        dataSource.SetupConnection(static con =>
            con.SetupCommand(static cmd => cmd.SetupResult(1)));

        using var con = dataSource.OpenConnection();

        Assert.NotNull(con);
    }

    [Fact]
    public void ConnectionsHistoryIsRecorded()
    {
        using var dataSource = new MockDbDataSource("Server=mock");
        dataSource.SetupConnection(static con =>
            con.SetupCommand(static cmd => cmd.SetupResult(1)));

        using var con = dataSource.OpenConnection();

        Assert.Single(dataSource.Connections);
    }

    [Fact]
    public void ConnectionStringIsAssignedToConnection()
    {
        using var dataSource = new MockDbDataSource("Server=mock");
        using var mockCon = new MockDbConnection();
        dataSource.SetupConnection(mockCon);

        using var con = dataSource.OpenConnection();

        Assert.Equal("Server=mock", con.ConnectionString);
    }

    [Fact]
    public async Task OpenConnectionAsyncReturnsSetupConnection()
    {
#pragma warning disable CA2007
        await using var dataSource = new MockDbDataSource("Server=mock");
#pragma warning restore CA2007
        dataSource.SetupConnection(static con =>
            con.SetupCommand(static cmd => cmd.SetupResult(1)));

#pragma warning disable CA2007
        await using var con = await dataSource.OpenConnectionAsync();
#pragma warning restore CA2007

        Assert.NotNull(con);
    }

    [Fact]
    public void CreateConnectionReturnsNewMockConnection()
    {
        using var dataSource = new MockDbDataSource("Server=mock");

        using var con = dataSource.CreateConnection();

        Assert.IsType<MockDbConnection>(con);
    }

    [Fact]
    public void CreateBatchReturnsMockDbBatch()
    {
        using var dataSource = new MockDbDataSource();

        using var batch = dataSource.CreateBatch();

        Assert.IsType<MockDbBatch>(batch);
    }

    [Fact]
    public void RepeatDataSourceCreatesRepeatConnection()
    {
        using var dataSource = new MockRepeatDbDataSource(42);

        using var con = dataSource.CreateConnection();

        Assert.IsType<MockRepeatDbConnection>(con);
    }
}
