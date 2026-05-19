namespace Smart.Mock.Data;

using System.Collections;
using System.Data.Common;

#pragma warning disable CA1010
public sealed class MockDbBatchCommandCollection : DbBatchCommandCollection
{
    private readonly List<MockDbBatchCommand> commands = [];

    public override int Count => commands.Count;

    public override bool IsReadOnly => false;

    protected override DbBatchCommand GetBatchCommand(int index) => commands[index];

    protected override void SetBatchCommand(int index, DbBatchCommand batchCommand) => commands[index] = (MockDbBatchCommand)batchCommand;

    public override void Add(DbBatchCommand item) => commands.Add((MockDbBatchCommand)item);

    public override void Clear() => commands.Clear();

    public override bool Contains(DbBatchCommand item) => commands.Contains((MockDbBatchCommand)item);

    public override void CopyTo(DbBatchCommand[] array, int arrayIndex) =>
        ((ICollection<DbBatchCommand>)commands).CopyTo(array, arrayIndex);

    public override IEnumerator<DbBatchCommand> GetEnumerator() => commands.GetEnumerator();

    public override int IndexOf(DbBatchCommand item) => commands.IndexOf((MockDbBatchCommand)item);

    public override void Insert(int index, DbBatchCommand item) => commands.Insert(index, (MockDbBatchCommand)item);

    public override bool Remove(DbBatchCommand item) => commands.Remove((MockDbBatchCommand)item);

    public override void RemoveAt(int index) => commands.RemoveAt(index);
}
#pragma warning restore CA1010
