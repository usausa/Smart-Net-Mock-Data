namespace Smart.Mock.Data;

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

    public static MockColumn[] CreateColumns<T>() =>
        typeof(T).GetProperties().Select(x => new MockColumn(x.PropertyType, x.Name)).ToArray();

    public static IEnumerable<object?[]> ToRows<T>(this IEnumerable<T> source, IEnumerable<MockColumn> columns)
    {
        var properties = columns.Select(x => typeof(T).GetProperty(x.Name)!).ToList();
        foreach (var entity in source)
        {
            yield return properties.Select(x => x.GetValue(entity)).ToArray();
        }
    }
}
