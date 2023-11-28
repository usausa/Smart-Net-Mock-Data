namespace Smart.Mock.Data;

using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

public class MockRepeatDbCommand : DbCommand
{
    private readonly object? result;

    private readonly MockDbParameterCollection parameters = [];

    protected override DbConnection? DbConnection { get; set; }

    protected override DbTransaction? DbTransaction { get; set; }

    [AllowNull]
    public override string CommandText { get; set; }

    public override int CommandTimeout { get; set; }

    public override CommandType CommandType { get; set; }

    protected override DbParameterCollection DbParameterCollection => parameters;

    public override UpdateRowSource UpdatedRowSource { get; set; }

    public override bool DesignTimeVisible { get; set; }

    public MockRepeatDbCommand(object? result)
    {
        this.result = result;
    }

    public override void Prepare()
    {
    }

    public override void Cancel()
    {
    }

    protected override DbParameter CreateDbParameter() => new MockDbParameter();

    public override int ExecuteNonQuery() => (int)result!;

    public override object? ExecuteScalar() => result;

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        (result as IRepeatDataReader)?.Reset();
        return (DbDataReader)result!;
    }
}
