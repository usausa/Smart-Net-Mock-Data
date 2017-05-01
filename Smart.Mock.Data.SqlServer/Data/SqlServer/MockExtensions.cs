namespace Smart.Mock.Data.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.SqlServer.TransactSql.ScriptDom;

    /// <summary>
    ///
    /// </summary>
    public static class MockExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ValidateResult ValidateSql(this MockDbCommand command)
        {
            return ValidateSql(command, DefaultParser.Create());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        public static ValidateResult ValidateSql(this MockDbCommand command, TSqlParser parser)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (parser == null)
            {
                throw new ArgumentNullException(nameof(parser));
            }

            var result = new ValidateResult();

            foreach (var executedCommand in command.ExecutedCommands)
            {
                using (var reader = new StringReader(executedCommand.CommandText))
                {
                    parser.Parse(reader, out IList<ParseError> errors);
                    if (errors != null)
                    {
                        result.AddErrors(errors);
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static ValidateResult ValidateSql(this MockDbConnection connection)
        {
            return ValidateSql(connection, DefaultParser.Create());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        public static ValidateResult ValidateSql(this MockDbConnection connection, TSqlParser parser)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (parser == null)
            {
                throw new ArgumentNullException(nameof(parser));
            }

            var result = new ValidateResult();

            foreach (var command in connection.Commands)
            {
                foreach (var executedCommand in command.ExecutedCommands)
                {
                    using (var reader = new StringReader(executedCommand.CommandText))
                    {
                        parser.Parse(reader, out IList<ParseError> errors);
                        if (errors != null)
                        {
                            result.AddErrors(errors);
                        }
                    }
                }
            }

            return result;
        }
    }
}
