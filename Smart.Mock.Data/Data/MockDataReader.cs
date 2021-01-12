namespace Smart.Mock.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Globalization;
    using System.Linq;

    public class MockColumn
    {
        public Type DataType { get; }

        public string Name { get; }

        public MockColumn(Type dataType, string name)
        {
            DataType = dataType;
            Name = name;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGeneric", Justification = "Ignore")]
    public sealed class MockDataReader : DbDataReader, IRepeatDataReader
    {
        private readonly MockColumn[] columns;

        private readonly object?[][] rows;

        private bool closed;

        private int current = -1;

        public override bool IsClosed => closed;

        public override int Depth => 0;

        public override int FieldCount => columns.Length;

        public override int RecordsAffected => rows.Length;

        public override bool HasRows => rows.Length > 0;

        public override object this[int ordinal] => rows[current][ordinal] ?? DBNull.Value;

        public override object this[string name] => rows[current][GetOrdinal(name)] ?? DBNull.Value;

        public MockDataReader(MockColumn[] columns, IEnumerable<object[]> rows)
        {
            this.columns = columns;
            this.rows = rows.ToArray();
        }

        public void Reset()
        {
            closed = false;
            current = -1;
        }

        public override void Close()
        {
            closed = true;
            base.Close();
        }

        public override IEnumerator GetEnumerator() => new DbEnumerator(this, false);

        public override bool NextResult() => current < rows.Length;

        public override bool Read()
        {
            current++;
            return current < rows.Length;
        }

        public override bool IsDBNull(int ordinal) =>
            rows[current][ordinal] is DBNull || rows[current][ordinal] is null;

        public override string GetDataTypeName(int ordinal) => columns[ordinal].DataType.Name;

        public override Type GetFieldType(int ordinal) => columns[ordinal].DataType;

        public override string GetName(int ordinal) => columns[ordinal].Name;

        public override int GetOrdinal(string name)
        {
            for (var i = 0; i < columns.Length; i++)
            {
                if (String.Equals(columns[i].Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            throw new ArgumentException($"Column {name} is not found.", nameof(name));
        }

        public override object GetValue(int ordinal) =>
            IsDBNull(ordinal) ? DBNull.Value : (rows[current][ordinal] ?? DBNull.Value);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public override int GetValues(object[] values)
        {
            var length = Math.Min(values.Length, columns.Length);
            for (var i = 0; i < length; i++)
            {
                values[i] = GetValue(i);
            }

            return length;
        }

        public override bool GetBoolean(int ordinal) =>
            Convert.ToBoolean(rows[current][ordinal], CultureInfo.InvariantCulture);

        public override byte GetByte(int ordinal) =>
            Convert.ToByte(rows[current][ordinal], CultureInfo.InvariantCulture);

        public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length)
        {
            if ((buffer is not null) && (rows[current][ordinal] is byte[] bytes))
            {
                var result = Math.Min(bytes.Length - (int)dataOffset, length);
                Buffer.BlockCopy(bytes, (int)dataOffset, buffer, bufferOffset, result);
                return result;
            }

            return 0;
        }

        public override char GetChar(int ordinal) =>
            Convert.ToChar(rows[current][ordinal], CultureInfo.InvariantCulture);

        public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length)
        {
            if ((buffer is not null) && (rows[current][ordinal] is char[] chars))
            {
                var result = Math.Min(chars.Length - (int)dataOffset, length);
                Array.Copy(chars, (int)dataOffset, buffer, bufferOffset, result);
                return result;
            }

            return 0;
        }

        public override DateTime GetDateTime(int ordinal) =>
            Convert.ToDateTime(rows[current][ordinal], CultureInfo.InvariantCulture);

        public override decimal GetDecimal(int ordinal) =>
            Convert.ToDecimal(rows[current][ordinal], CultureInfo.InvariantCulture);

        public override double GetDouble(int ordinal) =>
            Convert.ToDouble(rows[current][ordinal], CultureInfo.InvariantCulture);

        public override float GetFloat(int ordinal) =>
            Convert.ToSingle(rows[current][ordinal], CultureInfo.InvariantCulture);

        public override Guid GetGuid(int ordinal)
        {
            if (rows[current][ordinal] is Guid guid)
            {
                return guid;
            }

            return Guid.Parse(GetString(ordinal));
        }

        public override short GetInt16(int ordinal) =>
            Convert.ToInt16(rows[current][ordinal], CultureInfo.InvariantCulture);

        public override int GetInt32(int ordinal) =>
            Convert.ToInt32(rows[current][ordinal], CultureInfo.InvariantCulture);

        public override long GetInt64(int ordinal) =>
            Convert.ToInt64(rows[current][ordinal], CultureInfo.InvariantCulture);

        public override string GetString(int ordinal) =>
            Convert.ToString(rows[current][ordinal], CultureInfo.InvariantCulture)!;
    }
}
