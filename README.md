# Smart.Mock.Data .NET - mock data library for .NET

[![NuGet](https://img.shields.io/nuget/v/Usa.Smart.Mock.Data.svg)](https://www.nuget.org/packages/Usa.Smart.Mock.Data)

## What is this?

* ADO.NET mock library

### Usage example

```csharp
// ExecuteNonQuery
using (var con = new MockDbConnection())
{
    con.SetupCommand(cmd => cmd.SetupResult(1));

    var value = await con.ExecuteAsync("UPDATE Test SET NAME = 'UsaUsa' WHERE Id = 1234");

    Assert.Equal(1, value);
}
```

```csharp
// ExecuteScalar
using (var con = new MockDbConnection())
{
    con.SetupCommand(cmd => cmd.SetupResult(1));

    var value = await con.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Test");

    Assert.Equal(1, value);
}
```

```csharp
// ExecuteReader
using (var con = new MockDbConnection())
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
    con.SetupCommand(cmd => cmd.SetupResult(new MockDataReader(columns, rows)));

    var list = (await con.QueryAsync<Employee>("SELECT COUNT(*) FROM Employee")).ToList();

    Assert.Equal(3, list.Count);
}
```

```csharp
// ExecureReader - DataReader Objects List
var entities = new List<Employee>
{
    new Employee { Id = 1, Name = "Employee1" },
    new Employee { Id = 2, Name = "Employee2" },
    new Employee { Id = 3, Name = "Employee3" }
};

var connection = new MockDbConnection();
connection.SetupCommand(cmd => cmd.SetupResult(new MockDataReader().Append(entities)));

var service = new EmployeeService(new CallbackConnectionFactory(() => connection));

// Test
var list = service.QueryEmployeeList();

// Assert
Assert.Equal(3, list.Count);
```
