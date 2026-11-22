namespace Smart.Mock;

using System.Data.Common;

using Smart.Mock.Data;

public sealed class ParameterCollectionTests
{
    private static MockDbParameterCollection CreateCollection(out MockDbParameter first, out MockDbParameter second)
    {
        first = new MockDbParameter { ParameterName = "a" };
        second = new MockDbParameter { ParameterName = "b" };
        return new MockDbParameterCollection { first, second };
    }

    [Fact]
    public void CopyToWritesParametersToDbParameterArray()
    {
        var collection = CreateCollection(out var first, out var second);
        var array = new DbParameter[3];

        collection.CopyTo(array, 1);

        Assert.Null(array[0]);
        Assert.Same(first, array[1]);
        Assert.Same(second, array[2]);
    }

    [Fact]
    public void CopyToWritesParametersToObjectArray()
    {
        var collection = CreateCollection(out var first, out var second);
        var array = new object[2];

        collection.CopyTo(array, 0);

        Assert.Same(first, array[0]);
        Assert.Same(second, array[1]);
    }

    [Fact]
    public void CopyToOnEmptyCollectionWritesNothing()
    {
        var array = new object[1];

        new MockDbParameterCollection().CopyTo(array, 0);

        Assert.Null(array[0]);
    }

    [Fact]
    public void CopyToThrowsWhenIndexIsOutOfRange()
    {
        var collection = CreateCollection(out _, out _);
        var array = new object[2];

        Assert.Throws<ArgumentException>(() => collection.CopyTo(array, 1));
    }

    [Fact]
    public void CopyToThrowsWhenIndexIsNegative()
    {
        var collection = CreateCollection(out _, out _);
        var array = new object[2];

        Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(array, -1));
    }

    [Fact]
    public void CopyToThrowsWhenArrayIsNull()
    {
        var collection = CreateCollection(out _, out _);

        Assert.Throws<ArgumentNullException>(() => collection.CopyTo(null!, 0));
    }

    [Fact]
    public void CopyToMultiDimensionalArrayThrows()
    {
        var collection = CreateCollection(out _, out _);
        var array = Array.CreateInstance(typeof(object), [2, 2]);

        Assert.Throws<ArgumentException>(() => collection.CopyTo(array, 0));
    }

    [Fact]
    public void CopyToIncompatibleArrayThrows()
    {
        var collection = CreateCollection(out _, out _);
        var array = new string[2];

        Assert.Throws<ArgumentException>(() => collection.CopyTo(array, 0));
    }
}
