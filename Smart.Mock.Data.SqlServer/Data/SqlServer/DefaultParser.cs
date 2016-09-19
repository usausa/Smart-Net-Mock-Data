namespace Smart.Mock.Data.SqlServer
{
    using System;

    using Microsoft.SqlServer.TransactSql.ScriptDom;

    /// <summary>
    ///
    /// </summary>
    public static class DefaultParser
    {
        private static Func<TSqlParser> parserFactory = () => new TSql130Parser(true);

        /// <summary>
        ///
        /// </summary>
        /// <param name="factory"></param>
        public static void SetFactory(Func<TSqlParser> factory)
        {
            parserFactory = factory;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static TSqlParser Create()
        {
            return parserFactory();
        }
    }
}
