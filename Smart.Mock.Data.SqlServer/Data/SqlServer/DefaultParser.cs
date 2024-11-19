namespace Smart.Mock.Data.SqlServer;

using Microsoft.SqlServer.TransactSql.ScriptDom;

public static class DefaultParser
{
    private static Func<TSqlParser> parserFactory = static () => new TSql130Parser(true);

    public static void SetFactory(Func<TSqlParser> factory)
    {
        parserFactory = factory;
    }

    public static TSqlParser Create() => parserFactory();
}
