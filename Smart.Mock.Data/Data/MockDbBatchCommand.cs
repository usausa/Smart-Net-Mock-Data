namespace Smart.Mock.Data;

using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

public sealed class MockDbBatchCommand : DbBatchCommand
{
    private readonly MockDbParameterCollection parameters = [];

    private int recordsAffected;

    [AllowNull]
    public override string CommandText { get; set; }

    public override CommandType CommandType { get; set; }

    public override int RecordsAffected => recordsAffected;

    internal void SetRecordsAffected(int value) => recordsAffected = value;

    protected override DbParameterCollection DbParameterCollection => parameters;

    internal MockDbParameterCollection ParametersInternal => parameters;

    public override bool CanCreateParameter => true;

    public override DbParameter CreateParameter() => new MockDbParameter();
}
