namespace Smart.Mock.Data;

using System.Collections;
using System.Data.Common;

#pragma warning disable CA1010
public sealed class MockDbParameterCollection : DbParameterCollection
{
    private readonly List<MockDbParameter> parameters = [];

    public override object SyncRoot => ((ICollection)parameters).SyncRoot;

    public override int Count => parameters.Count;

    public override IEnumerator GetEnumerator() => parameters.GetEnumerator();

    protected override DbParameter GetParameter(int index) => parameters[index];

    protected override void SetParameter(int index, DbParameter value) => parameters[index] = (MockDbParameter)value;

    protected override void SetParameter(string parameterName, DbParameter value) => parameters[IndexOfChecked(parameterName)] = (MockDbParameter)value;

    protected override DbParameter GetParameter(string parameterName) => parameters[IndexOfChecked(parameterName)];

    public override int Add(object value)
    {
        parameters.Add((MockDbParameter)value);
        return parameters.Count - 1;
    }

    public override void AddRange(Array values) => parameters.AddRange(values.Cast<MockDbParameter>());

    public override void Clear() => parameters.Clear();

    public override bool Contains(object value) => parameters.Contains((MockDbParameter)value);

    public override bool Contains(string value) => IndexOf(value) != -1;

    public override void CopyTo(Array array, int index) => parameters.CopyTo((MockDbParameter[])array, index);

    public override int IndexOf(object value) => parameters.IndexOf((MockDbParameter)value);

    public override int IndexOf(string parameterName)
    {
        for (var i = 0; i < parameters.Count; i++)
        {
            if (String.Equals(parameters[i].ParameterName, parameterName, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }

    public override void Insert(int index, object value) => parameters.Insert(index, (MockDbParameter)value);

    public override void Remove(object value) => parameters.Remove((MockDbParameter)value);

    public override void RemoveAt(int index) => parameters.RemoveAt(index);

    public override void RemoveAt(string parameterName) => parameters.RemoveAt(IndexOfChecked(parameterName));

    private int IndexOfChecked(string parameterName)
    {
        var index = IndexOf(parameterName);
        if (index == -1)
        {
            throw new ArgumentException($"Parameter {parameterName} is not found.", nameof(parameterName));
        }

        return index;
    }
}
#pragma warning restore CA1010
