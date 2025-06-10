namespace Smart.Mock.Data;

using System.Collections;
using System.Data.Common;
using System.Globalization;

public sealed class MockColumn
{
    public Type DataType { get; }

    public string Name { get; }

    public MockColumn(Type dataType, string name)
    {
        DataType = dataType;
        Name = new string(name);    // Convert to other object
    }
}

#pragma warning disable CA1010
public sealed class MockDataReader : DbDataReader, IRepeatDataReader
{
    private readonly MockColumn[] columns;

    private readonly List<object?[][]> rowSet = new();

    private object?[][] currentRows;

    private bool closed;

    private int currentRow = -1;

    private int currentSet;

    public override bool IsClosed => closed;

    public override int Depth => 0;

    public override int FieldCount => columns.Length;

    public override int RecordsAffected => currentRows.Length;

    public override bool HasRows => currentRows.Length > 0;

    public override object this[int ordinal] => currentRows[currentRow][ordinal] ?? DBNull.Value;

    public override object this[string name] => currentRows[currentRow][GetOrdinal(name)] ?? DBNull.Value;

    public MockDataReader(MockColumn[] columns, IEnumerable<object[]> rows)
    {
        this.columns = columns;
        currentRows = rows.ToArray();
        rowSet.Add(currentRows);
    }

    public MockDataReader Append(IEnumerable<object[]> rows)
    {
        rowSet.Add(rows.ToArray());
        return this;
    }

    public void Reset()
    {
        closed = false;
        currentSet = 0;
        currentRow = -1;
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
        if (currentSet < rowSet.Count)
        {
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

    public override bool IsDBNull(int ordinal) =>
        currentRows[currentRow][ordinal] is DBNull || currentRows[currentRow][ordinal] is null;

    public override string GetDataTypeName(int ordinal) => columns[ordinal].DataType.Name;

    public override Type GetFieldType(int ordinal) => columns[ordinal].DataType;

    public override string GetName(int ordinal) => columns[ordinal].Name;

    public override int GetOrdinal(string name)
    {
        var columnsLocal = columns;
        for (var i = 0; i < columnsLocal.Length; i++)
        {
            if (String.Equals(columnsLocal[i].Name, name, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        throw new ArgumentException($"Column {name} is not found.", nameof(name));
    }

    public override object GetValue(int ordinal) =>
        IsDBNull(ordinal) ? DBNull.Value : (currentRows[currentRow][ordinal] ?? DBNull.Value);

#pragma warning disable CA1062
    public override int GetValues(object[] values)
    {
        var length = Math.Min(values.Length, columns.Length);
        for (var i = 0; i < length; i++)
        {
            values[i] = GetValue(i);
        }

        return length;
    }
#pragma warning restore CA1062

    public override bool GetBoolean(int ordinal) =>
        Convert.ToBoolean(currentRows[currentRow][ordinal], CultureInfo.InvariantCulture);

    public override byte GetByte(int ordinal) =>
        Convert.ToByte(currentRows[currentRow][ordinal], CultureInfo.InvariantCulture);

    public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length)
    {
        if ((buffer is not null) && (currentRows[currentRow][ordinal] is byte[] bytes))
        {
            var result = Math.Min(bytes.Length - (int)dataOffset, length);
            Buffer.BlockCopy(bytes, (int)dataOffset, buffer, bufferOffset, result);
            return result;
        }

        return 0;
    }

    public override char GetChar(int ordinal) =>
        Convert.ToChar(currentRows[currentRow][ordinal], CultureInfo.InvariantCulture);

    public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length)
    {
        if ((buffer is not null) && (currentRows[currentRow][ordinal] is char[] chars))
        {
            var result = Math.Min(chars.Length - (int)dataOffset, length);
            Array.Copy(chars, (int)dataOffset, buffer, bufferOffset, result);
            return result;
        }

        return 0;
    }

    public override DateTime GetDateTime(int ordinal) =>
        Convert.ToDateTime(currentRows[currentRow][ordinal], CultureInfo.InvariantCulture);

    public override decimal GetDecimal(int ordinal) =>
        Convert.ToDecimal(currentRows[currentRow][ordinal], CultureInfo.InvariantCulture);

    public override double GetDouble(int ordinal) =>
        Convert.ToDouble(currentRows[currentRow][ordinal], CultureInfo.InvariantCulture);

    public override float GetFloat(int ordinal) =>
        Convert.ToSingle(currentRows[currentRow][ordinal], CultureInfo.InvariantCulture);

    public override Guid GetGuid(int ordinal)
    {
        if (currentRows[currentRow][ordinal] is Guid guid)
        {
            return guid;
        }

        return Guid.Parse(GetString(ordinal));
    }

    public override short GetInt16(int ordinal) =>
        Convert.ToInt16(currentRows[currentRow][ordinal], CultureInfo.InvariantCulture);

    public override int GetInt32(int ordinal) =>
        Convert.ToInt32(currentRows[currentRow][ordinal], CultureInfo.InvariantCulture);

    public override long GetInt64(int ordinal) =>
        Convert.ToInt64(currentRows[currentRow][ordinal], CultureInfo.InvariantCulture);

    public override string GetString(int ordinal) =>
        Convert.ToString(currentRows[currentRow][ordinal], CultureInfo.InvariantCulture)!;
}
#pragma warning restore CA1010
