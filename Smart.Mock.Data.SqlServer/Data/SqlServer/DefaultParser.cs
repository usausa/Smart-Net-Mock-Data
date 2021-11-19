namespace Smart.Mock.Data.SqlServer;

using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

public static class DefaultParser
{
    private static Func<TSqlParser> parserFactory = () => new TSql130Parser(true);

    public static void SetFactory(Func<TSqlParser> factory)
    {
        parserFactory = factory;
    }

    public static TSqlParser Create() => parserFactory();
}
