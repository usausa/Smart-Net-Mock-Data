namespace Smart.Mock.Services;

using Smart.Mock.Data;
using Smart.Mock.Data.SqlServer;
using Smart.Mock.Infrastructure;

public sealed class EmployeeServiceTest
{
    [Fact]
    public void QueryEmployeeList()
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
        var connection = new MockDbConnection();
#pragma warning restore CA2000
        connection.SetupCommand(cmd => cmd.SetupResult(new MockDataReader(columns, rows)));

        var service = new EmployeeService(new CallbackConnectionFactory(() => connection));

        // Test
        var list = service.QueryEmployeeList();

        // Assert
        Assert.Equal(3, list.Count);

        var result = connection.ValidateSql();
        Assert.True(result.Valid, result.ToString());
    }
}
