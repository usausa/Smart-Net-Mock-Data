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
