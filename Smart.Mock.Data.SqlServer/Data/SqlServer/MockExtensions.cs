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

            var index = 0;
            foreach (var executedCommand in command.ExecutedCommands)
            {
                ParseError(parser, index, executedCommand.CommandText, result);

                index++;
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

            var index = 0;
            foreach (var command in connection.Commands)
            {
                foreach (var executedCommand in command.ExecutedCommands)
                {
                    ParseError(parser, index, executedCommand.CommandText, result);

                    index++;
                }
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="result"></param>
        private static void ParseError(TSqlParser parser, int index, string commandText, ValidateResult result)
        {
            using (var reader = new StringReader(commandText))
            {
                IList<ParseError> errors;
                parser.Parse(reader, out errors);
                if (errors != null)
                {
                    foreach (var error in errors)
                    {
                        result.AddError(new ErrorEntry(
                            index,
                            commandText,
                            error.Number,
                            error.Offset,
                            error.Line,
                            error.Column,
                            error.Message));
                    }
                }
            }
        }
    }
}
