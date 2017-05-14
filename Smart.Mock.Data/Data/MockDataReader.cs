namespace Smart.Mock.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;

    /// <summary>
    ///
    /// </summary>
    public class MockColumn
    {
        /// <summary>
        ///
        /// </summary>
        public Type DataType { get; }

        /// <summary>
        ///
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="name"></param>
        public MockColumn(Type dataType, string name)
        {
            DataType = dataType;
            Name = name;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class MockDataReader : IDataReader
    {
        private readonly MockColumn[] columns;

        private readonly IList<object[]> rows;

        private int current = -1;

        /// <summary>
        ///
        /// </summary>
        public int Depth
        {
            get { return 0; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsClosed { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public int RecordsAffected
        {
            get
            {
                return rows.Count;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int FieldCount
        {
            get
            {
                return columns.Length;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        object IDataRecord.this[int i]
        {
            get
            {
                return rows[current][i];
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object IDataRecord.this[string name]
        {
            get
            {
                return rows[current][GetOrdinal(name)];
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        public MockDataReader(MockColumn[] columns, IList<object[]> rows)
        {
            this.columns = columns;
            this.rows = rows;
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        ///
        /// </summary>
        public void Close()
        {
            IsClosed = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public DataTable GetSchemaTable()
        {
            throw new NotSupportedException("Not supported.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool NextResult()
        {
            return current < rows.Count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool Read()
        {
            current++;
            return current < rows.Count;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool IsDBNull(int i)
        {
            return rows[current][i] is DBNull || rows[current][i] == null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetName(int i)
        {
            return columns[i].Name;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetDataTypeName(int i)
        {
            return columns[i].DataType.Name;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Type GetFieldType(int i)
        {
            return columns[i].DataType;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes", Justification = "Ignore")]
        public int GetOrdinal(string name)
        {
            for (var i = 0; i < columns.Length; i++)
            {
                if (String.Equals(columns[i].Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            throw new IndexOutOfRangeException("Column is not found.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public object GetValue(int i)
        {
            return rows[current][i];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public int GetValues(object[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var result = Math.Min(values.Length, columns.Length);
            for (var i = 0; i < result; i++)
            {
                values[i] = rows[current][i];
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool GetBoolean(int i)
        {
            return Convert.ToBoolean(rows[current][i], CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public byte GetByte(int i)
        {
            return Convert.ToByte(rows[current][i], CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <param name="fieldOffset"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferoffset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            var bytes = (byte[])rows[current][i];
            var result = Math.Min(bytes.Length - (int)fieldOffset, length);
            Array.Copy(bytes, (int)fieldOffset, buffer, length, result);
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public char GetChar(int i)
        {
            return Convert.ToChar(rows[current][i], CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <param name="fieldoffset"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferoffset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            var chars = (char[])rows[current][i];
            var result = Math.Min(chars.Length - (int)fieldoffset, length);
            Array.Copy(chars, (int)fieldoffset, buffer, length, result);
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Guid GetGuid(int i)
        {
            if (rows[current][i] is Guid)
            {
                return (Guid)rows[current][i];
            }

            return Guid.Parse(GetString(i));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public short GetInt16(int i)
        {
            return Convert.ToInt16(rows[current][i], CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int GetInt32(int i)
        {
            return Convert.ToInt32(rows[current][i], CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public long GetInt64(int i)
        {
            return Convert.ToInt64(rows[current][i], CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public float GetFloat(int i)
        {
            return Convert.ToSingle(rows[current][i], CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double GetDouble(int i)
        {
            return Convert.ToDouble(rows[current][i], CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetString(int i)
        {
            return Convert.ToString(rows[current][i], CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public decimal GetDecimal(int i)
        {
            return Convert.ToDecimal(rows[current][i], CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public DateTime GetDateTime(int i)
        {
            return Convert.ToDateTime(rows[current][i], CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public IDataReader GetData(int i)
        {
            throw new NotSupportedException("Not supported.");
        }
    }
}
