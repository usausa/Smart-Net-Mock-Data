namespace Smart.Mock.Services
{
    using System.Collections.Generic;

    using Smart.Mock.Data;
    using Smart.Mock.Data.SqlServer;
    using Smart.Mock.Infrastructure;

    using Xunit;

    public class EmployeeServiceTest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Ignore")]
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

            var connection = new MockDbConnection();
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
}
