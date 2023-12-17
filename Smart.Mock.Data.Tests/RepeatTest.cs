namespace Smart.Mock;

using Smart.Data.Mapper;
using Smart.Mock.Data;
using Smart.Mock.Models;

using Xunit;

public sealed class RepeatTest
{
    [Fact]
    public void ExecuteNonQueryRepeat()
    {
        using var con = new MockRepeatDbConnection(1);
        var value = con.Execute("UPDATE Test SET NAME = 'UsaUsa' WHERE Id = 1234");
        Assert.Equal(1, value);

        value = con.Execute("UPDATE Test SET NAME = 'UsaUsa' WHERE Id = 1234");
        Assert.Equal(1, value);
    }

    [Fact]
    public void ExecuteReaderRepeat()
    {
        var columns = new[]
        {
            new MockColumn(typeof(int), "Id"),
            new MockColumn(typeof(string), "Name")
        };
        var rows = new List<object[]>
        {
            new object[] { 1, "Employee1" },
            new object[] { 2, "Employee2" },
            new object[] { 3, "Employee3" }
        };

#pragma warning disable CA2000
        using var con = new MockRepeatDbConnection(new MockDataReader(columns, rows));
#pragma warning restore CA2000
        var list = con.Query<Employee>("SELECT COUNT(*) FROM Employee").ToList();
        Assert.Equal(3, list.Count);

        list = con.Query<Employee>("SELECT COUNT(*) FROM Employee").ToList();
        Assert.Equal(3, list.Count);
    }
}
