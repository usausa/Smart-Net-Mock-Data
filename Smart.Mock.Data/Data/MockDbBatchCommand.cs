namespace Smart.Mock.Data;

using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

public sealed class MockDbBatchCommand : DbBatchCommand
{
#pragma warning disable IDE0032
    private readonly MockDbParameterCollection parameters = [];
#pragma warning restore IDE0032

    private int recordsAffected;

    [AllowNull]
    public override string CommandText { get; set; }

    public override CommandType CommandType { get; set; }

    public override int RecordsAffected => recordsAffected;

    internal void SetRecordsAffected(int value) => recordsAffected = value;

#pragma warning disable IDE0032
    protected override DbParameterCollection DbParameterCollection => parameters;
#pragma warning restore IDE0032

#pragma warning disable IDE0032
    internal MockDbParameterCollection MockParameters => parameters;
#pragma warning restore IDE0032

    public override bool CanCreateParameter => true;

    public override DbParameter CreateParameter() => new MockDbParameter();
}
