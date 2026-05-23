namespace Smart.Mock;

using Smart.Mock.Data;

#pragma warning disable xUnit1051
public sealed class BatchTest
{
    [Fact]
    public void ExecuteNonQueryReturnSetupResult()
    {
        using var con = new MockDbConnection();
        using var batch = (MockDbBatch)con.CreateBatch();

        batch.BatchCommands.Add(new MockDbBatchCommand { CommandText = "UPDATE A SET X = 1" });
        batch.BatchCommands.Add(new MockDbBatchCommand { CommandText = "UPDATE B SET Y = 2" });

        batch.SetupResult(2);

        var total = batch.ExecuteNonQuery();

        Assert.Equal(2, total);
    }

    [Fact]
    public void ExecuteNonQueryRecordsAffectedAreAssigned()
    {
        using var con = new MockDbConnection();
        using var batch = (MockDbBatch)con.CreateBatch();

        batch.BatchCommands.Add(new MockDbBatchCommand { CommandText = "UPDATE A SET X = 1" });
        batch.BatchCommands.Add(new MockDbBatchCommand { CommandText = "UPDATE B SET Y = 2" });

        batch.SetupResult(2);
        batch.SetupRecordsAffected(1, 1);

        batch.ExecuteNonQuery();

        Assert.Equal(1, batch.BatchCommands[0].RecordsAffected);
        Assert.Equal(1, batch.BatchCommands[1].RecordsAffected);
    }

    [Fact]
    public void ExecutedBatchCommandsAreRecorded()
    {
        using var con = new MockDbConnection();
        using var batch = (MockDbBatch)con.CreateBatch();

        batch.BatchCommands.Add(new MockDbBatchCommand { CommandText = "UPDATE A SET X = 1" });
        batch.BatchCommands.Add(new MockDbBatchCommand { CommandText = "UPDATE B SET Y = 2" });

        batch.SetupResult(2);

        batch.ExecuteNonQuery();

        Assert.Equal(2, batch.ExecutedBatchCommands.Count);
        Assert.Equal("UPDATE A SET X = 1", batch.ExecutedBatchCommands[0].CommandText);
        Assert.Equal("UPDATE B SET Y = 2", batch.ExecutedBatchCommands[1].CommandText);
    }

    [Fact]
    public void ExecutingCallbackIsInvoked()
    {
        using var con = new MockDbConnection();
        using var batch = (MockDbBatch)con.CreateBatch();

        batch.BatchCommands.Add(new MockDbBatchCommand { CommandText = "UPDATE A SET X = 1" });

        var mockBatch = batch;
        mockBatch.SetupResult(1);

        IReadOnlyList<ExecutedBatchCommand>? captured = null;
        mockBatch.Executing = c => captured = c;

        batch.ExecuteNonQuery();

        Assert.NotNull(captured);
        Assert.Single(captured);
        Assert.Equal("UPDATE A SET X = 1", captured[0].CommandText);
    }

    [Fact]
    public void ExecuteScalarReturnSetupResult()
    {
        using var con = new MockDbConnection();
        using var batch = (MockDbBatch)con.CreateBatch();

        batch.BatchCommands.Add(new MockDbBatchCommand { CommandText = "SELECT COUNT(*) FROM Test" });

        batch.SetupResult(42);

        var result = batch.ExecuteScalar();

        Assert.Equal(42, result);
    }

    [Fact]
    public void ExecuteReaderReturnsMockDataReader()
    {
        using var con = new MockDbConnection();
        using var batch = (MockDbBatch)con.CreateBatch();

        batch.BatchCommands.Add(new MockDbBatchCommand { CommandText = "SELECT Id FROM Test" });

        var columns = new[] { new MockColumn(typeof(int), "Id") };
        var rows = new List<object[]> { new object[] { 1 } };
        using var reader = new MockDataReader(columns, rows);

        batch.SetupResult(reader);

        using var result = batch.ExecuteReader();

        Assert.True(result.Read());
        Assert.Equal(1, result.GetInt32(0));
    }

    [Fact]
    public async Task ExecuteNonQueryAsyncReturnSetupResult()
    {
#pragma warning disable CA2007
        await using var con = new MockDbConnection();
        await using var batch = (MockDbBatch)con.CreateBatch();
#pragma warning restore CA2007

        batch.BatchCommands.Add(new MockDbBatchCommand { CommandText = "UPDATE A SET X = 1" });

        batch.SetupResult(1);

        var total = await batch.ExecuteNonQueryAsync();

        Assert.Equal(1, total);
    }

    [Fact]
    public void ThrowsWhenNoResultSetup()
    {
        using var con = new MockDbConnection();
        using var batch = (MockDbBatch)con.CreateBatch();

        batch.BatchCommands.Add(new MockDbBatchCommand { CommandText = "UPDATE A SET X = 1" });

        Assert.Throws<InvalidOperationException>(() => batch.ExecuteNonQuery());
    }

    [Fact]
    public void BatchCommandCanCreateParameter()
    {
        var cmd = new MockDbBatchCommand();
        Assert.True(cmd.CanCreateParameter);

        var param = cmd.CreateParameter();
        Assert.IsType<MockDbParameter>(param);
    }
}
