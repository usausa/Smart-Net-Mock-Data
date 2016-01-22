namespace Smart.Mock.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Dapper;

    using Smart.Mock.Infrastructure;
    using Smart.Mock.Models;

    /// <summary>
    ///
    /// </summary>
    public class EmployeeService
    {
        private IConnectionFactory ConnectionFactory { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connectionFactory"></param>
        public EmployeeService(IConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IList<Employee> QueryEmployeeList()
        {
            return ConnectionFactory.Using(con =>
                con.Query<Employee>("SELECT * FROM Employee", buffered: false).ToList());
        }
    }
}
