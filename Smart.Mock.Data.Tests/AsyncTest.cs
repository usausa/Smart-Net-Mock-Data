namespace Smart.Mock
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Smart.Data.Mapper;
    using Smart.Mock.Data;
    using Smart.Mock.Models;

    using Xunit;

    public class AsyncTest
    {
        [Fact]
        public async Task ExecuteNonQueryAsync()
        {
#pragma warning disable CA2007
            await using var con = new MockDbConnection();
#pragma warning restore CA2007
            con.SetupCommand(cmd => cmd.SetupResult(1));

            var value = await con.ExecuteAsync("UPDATE Test SET NAME = 'UsaUsa' WHERE Id = 1234").ConfigureAwait(false);

            Assert.Equal(1, value);
        }

        [Fact]
        public async Task ExecuteScalarAsync()
        {
#pragma warning disable CA2007
            await using var con = new MockDbConnection();
#pragma warning restore CA2007
            con.SetupCommand(cmd => cmd.SetupResult(1));

            var value = await con.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Test").ConfigureAwait(false);

            Assert.Equal(1, value);
        }

        [Fact]
        public async Task ExecuteReaderAsync()
        {
#pragma warning disable CA2007
            await using var con = new MockDbConnection();
#pragma warning restore CA2007
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
            con.SetupCommand(cmd => cmd.SetupResult(new MockDataReader(columns, rows)));

            var list = await con.QueryAsync<Employee>("SELECT COUNT(*) FROM Employee").ToListAsync().ConfigureAwait(false);

            Assert.Equal(3, list.Count);
        }
    }
}
