namespace Smart.Mock.Data;

using System.Reflection;

public static class MockHelper
{
    public static MockDataReader CreateReader<T>(IEnumerable<T> source)
    {
        var columns = CreateColumns<T>();
        var rows = source.ToRows(columns);
        return new MockDataReader(columns, rows!);
    }

    public static MockDataReader Append<T>(this MockDataReader reader, IEnumerable<T> source)
    {
        var columns = CreateColumns<T>();
        var rows = source.ToRows(columns);
        reader.Append(columns, rows!);
        return reader;
    }

    // #8: 同一型に対する GetProperties() リフレクションを初回のみ実行してキャッシュ
    public static MockColumn[] CreateColumns<T>() => ColumnsCache<T>.Value;

    private static class ColumnsCache<T>
    {
        internal static readonly MockColumn[] Value =
            typeof(T).GetProperties()
                     .Select(static x => new MockColumn(x.PropertyType, x.Name))
                     .ToArray();
    }

    // #6: 毎行の LINQ + ToArray を直接ループに置き換えてアロケーションを削減
    public static IEnumerable<object?[]> ToRows<T>(this IEnumerable<T> source, IEnumerable<MockColumn> columns)
    {
        var props = columns.Select(static x => typeof(T).GetProperty(x.Name)!).ToArray();
        return Iterator(source, props);

        static IEnumerable<object?[]> Iterator(IEnumerable<T> src, PropertyInfo[] props)
        {
            foreach (var entity in src)
            {
                var row = new object?[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    row[i] = props[i].GetValue(entity);
                }
                yield return row;
            }
        }
    }
}
