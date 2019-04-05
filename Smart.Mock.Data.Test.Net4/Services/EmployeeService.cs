namespace Smart.Mock.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Dapper;

    using Smart.Mock.Infrastructure;
    using Smart.Mock.Models;

    public class EmployeeService
    {
        private IConnectionFactory ConnectionFactory { get; }

        public EmployeeService(IConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public IList<Employee> QueryEmployeeList()
        {
            return ConnectionFactory.Using(con =>
                con.Query<Employee>("SELECT * FROM Employee", buffered: false).ToList());
        }
    }
}
