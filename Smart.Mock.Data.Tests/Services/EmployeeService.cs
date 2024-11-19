namespace Smart.Mock.Services;

using Smart.Data.Mapper;
using Smart.Mock.Infrastructure;
using Smart.Mock.Models;

public sealed class EmployeeService
{
    private IConnectionFactory ConnectionFactory { get; }

    public EmployeeService(IConnectionFactory connectionFactory)
    {
        ConnectionFactory = connectionFactory;
    }

    public IList<Employee> QueryEmployeeList()
    {
        return ConnectionFactory.Using(static con =>
            con.QueryList<Employee>("SELECT * FROM Employee"));
    }
}
