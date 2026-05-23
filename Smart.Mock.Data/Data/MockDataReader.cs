namespace Smart.Mock.Data;

using System.Collections;
using System.Data.Common;
using System.Globalization;
using System.Runtime.CompilerServices;

public sealed class MockColumn
{
    public Type DataType { get; }

    public string Name { get; }

    public MockColumn(Type dataType, string name)
    {
        DataType = dataType;
        Name = new string(name.AsSpan());    // Ensure distinct string instance
    }
}

#pragma warning disable IDE0032
#pragma warning disable CA1010
public sealed class MockDataReader : DbDataReader, IRepeatDataReader
{
    private readonly List<MockColumn[]> columnsSet = [];

    private readonly List<object?[][]> rowSet = [];

    private readonly List<Dictionary<string, int>> ordinalsSet = [];

    private MockColumn[] currentColumns;

    private Dictionary<string, int> currentOrdinals;

    private object?[][] currentRows;

    private bool closed;

    private int currentRow = -1;

    private int currentSet;

    public override bool IsClosed => closed;

    public override int Depth => 0;

    public override int FieldCount => currentColumns.Length;

    public override int RecordsAffected => currentRows.Length;

    public override bool HasRows => currentRows.Length > 0;

    public override object this[int ordinal] => currentRows[currentRow][ordinal] ?? DBNull.Value;

    public override object this[string name] => currentRows[currentRow][GetOrdinal(name)] ?? DBNull.Value;

    public MockDataReader(MockColumn[] columns, IEnumerable<object[]> rows)
    {
        currentColumns = columns;
        currentRows = rows.ToArray();
        currentOrdinals = BuildOrdinals(columns);
        columnsSet.Add(currentColumns);
        rowSet.Add(currentRows);
        ordinalsSet.Add(currentOrdinals);
    }

    private static Dictionary<string, int> BuildOrdinals(MockColumn[] columns)
    {
        var ordinals = new Dictionary<string, int>(columns.Length, StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < columns.Length; i++)
        {
            ordinals.TryAdd(columns[i].Name, i);
        }
        return ordinals;
    }

    public MockDataReader Append(MockColumn[] columns, IEnumerable<object[]> rows)
    {
        columnsSet.Add(columns);
        rowSet.Add(rows.ToArray());
        ordinalsSet.Add(BuildOrdinals(columns));
        return this;
    }

    public void Reset()
    {
        closed = false;
        currentSet = 0;
        currentRow = -1;
        currentColumns = columnsSet[0];
        currentOrdinals = ordinalsSet[0];
        currentRows = rowSet[0];
    }

    public override void Close()
    {
        closed = true;
        base.Close();
    }

    public override IEnumerator GetEnumerator() => new DbEnumerator(this, false);

    public override bool NextResult()
    {
        currentSet++;
        if (currentSet < columnsSet.Count)
        {
            currentRow = -1;
            currentColumns = columnsSet[currentSet];
            currentOrdinals = ordinalsSet[currentSet];
            currentRows = rowSet[currentSet];
            return true;
        }

        return false;
    }

    public override bool Read()
    {
        currentRow++;
        return currentRow < currentRows.Length;
    }

    private object?[] CurrentRow => currentRows[currentRow];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool IsDBNull(int ordinal)
    {
        var val = CurrentRow[ordinal];
        return val is null or DBNull;
    }

    public override string GetDataTypeName(int ordinal) => currentColumns[ordinal].DataType.Name;

    public override Type GetFieldType(int ordinal) => currentColumns[ordinal].DataType;

    public override string GetName(int ordinal) => currentColumns[ordinal].Name;

    public override int GetOrdinal(string name)
    {
        if (currentOrdinals.TryGetValue(name, out var ordinal))
        {
            return ordinal;
        }

        throw new ArgumentException($"Column {name} is not found.", nameof(name));
    }

    public override object GetValue(int ordinal)
    {
        var val = CurrentRow[ordinal];
        return val is null or DBNull ? DBNull.Value : val;
    }

#pragma warning disable CA1062
    public override int GetValues(object[] values)
    {
        var row = CurrentRow;
        var length = Math.Min(values.Length, row.Length);
        for (var i = 0; i < length; i++)
        {
            var val = row[i];
            values[i] = val is null or DBNull ? DBNull.Value : val;
        }

        return length;
    }
#pragma warning restore CA1062

    public override bool GetBoolean(int ordinal) =>
        Convert.ToBoolean(CurrentRow[ordinal], CultureInfo.InvariantCulture);

    public override byte GetByte(int ordinal) =>
        Convert.ToByte(CurrentRow[ordinal], CultureInfo.InvariantCulture);

    public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length)
    {
        if ((buffer is not null) && (CurrentRow[ordinal] is byte[] bytes))
        {
            var result = Math.Min(bytes.Length - (int)dataOffset, length);
            bytes.AsSpan((int)dataOffset, result).CopyTo(buffer.AsSpan(bufferOffset, result));
            return result;
        }

        return 0;
    }

    public override char GetChar(int ordinal) =>
        Convert.ToChar(CurrentRow[ordinal], CultureInfo.InvariantCulture);

    public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length)
    {
        if ((buffer is not null) && (CurrentRow[ordinal] is char[] chars))
        {
            var result = Math.Min(chars.Length - (int)dataOffset, length);
            chars.AsSpan((int)dataOffset, result).CopyTo(buffer.AsSpan(bufferOffset, result));
            return result;
        }

        return 0;
    }

    public override DateTime GetDateTime(int ordinal) =>
        Convert.ToDateTime(CurrentRow[ordinal], CultureInfo.InvariantCulture);

    public override decimal GetDecimal(int ordinal) =>
        Convert.ToDecimal(CurrentRow[ordinal], CultureInfo.InvariantCulture);

    public override double GetDouble(int ordinal) =>
        Convert.ToDouble(CurrentRow[ordinal], CultureInfo.InvariantCulture);

    public override float GetFloat(int ordinal) =>
        Convert.ToSingle(CurrentRow[ordinal], CultureInfo.InvariantCulture);

    public override Guid GetGuid(int ordinal)
    {
        if (CurrentRow[ordinal] is Guid guid)
        {
            return guid;
        }

        return Guid.Parse(GetString(ordinal));
    }

    public override short GetInt16(int ordinal) =>
        Convert.ToInt16(CurrentRow[ordinal], CultureInfo.InvariantCulture);

    public override int GetInt32(int ordinal) =>
        Convert.ToInt32(CurrentRow[ordinal], CultureInfo.InvariantCulture);

    public override long GetInt64(int ordinal) =>
        Convert.ToInt64(CurrentRow[ordinal], CultureInfo.InvariantCulture);

    public override string GetString(int ordinal) =>
        Convert.ToString(CurrentRow[ordinal], CultureInfo.InvariantCulture)!;
}
#pragma warning restore CA1010
#pragma warning disable IDE0032
