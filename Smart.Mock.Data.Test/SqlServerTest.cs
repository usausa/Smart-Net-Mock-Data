namespace Smart.Mock
{
    using Dapper;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.Mock.Data;
    using Smart.Mock.Data.SqlServer;

    /// <summary>
    /// SqlServerTest の概要の説明
    /// </summary>
    [TestClass]
    public class SqlServerTest
    {
        [TestMethod]
        public void ValidSqlExecute()
        {
            using (var connection = new MockDbConnection())
            {
                connection.SetupCommand(cmd => cmd.SetupResult(0));

                connection.Execute("UPDATE Employee SET Name = @Name WHERE Id = @Id", new { Id = 1, Name = "Employee1" });

                var result = connection.ValidateSql();
                Assert.IsTrue(result.Valid, result.ToString());
            }
        }

        [TestMethod]
        public void InvalidSqlExecute()
        {
            using (var connection = new MockDbConnection())
            {
                connection.SetupCommand(cmd => cmd.SetupResult(0));

                connection.Execute("UPDA TE Employee SET Name = @Name WHERE Id = @Id", new { Id = 1, Name = "Employee1" });

                var result = connection.ValidateSql();
                Assert.IsFalse(result.Valid, result.ToString());
            }
        }
    }
}
