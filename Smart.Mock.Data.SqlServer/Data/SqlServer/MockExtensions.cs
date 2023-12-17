namespace Smart.Mock.Data.SqlServer;

using Microsoft.SqlServer.TransactSql.ScriptDom;

public static class MockExtensions
{
    public static ValidateResult ValidateSql(this MockDbCommand command)
    {
        return ValidateSql(command, DefaultParser.Create());
    }

#pragma warning disable CA1062
    public static ValidateResult ValidateSql(this MockDbCommand command, TSqlParser parser)
    {
        var result = new ValidateResult();
        foreach (var executedCommand in command.ExecutedCommands)
        {
            using var reader = new StringReader(executedCommand.CommandText);
            parser.Parse(reader, out var errors);
            if (errors is not null)
            {
                result.AddErrors(errors);
            }
        }

        return result;
    }
#pragma warning restore CA1062

    public static ValidateResult ValidateSql(this MockDbConnection connection)
    {
        return ValidateSql(connection, DefaultParser.Create());
    }

#pragma warning disable CA1062
    public static ValidateResult ValidateSql(this MockDbConnection connection, TSqlParser parser)
    {
        var result = new ValidateResult();
        foreach (var command in connection.Commands)
        {
            foreach (var executedCommand in command.ExecutedCommands)
            {
                using var reader = new StringReader(executedCommand.CommandText);
                parser.Parse(reader, out var errors);
                if (errors is not null)
                {
                    result.AddErrors(errors);
                }
            }
        }

        return result;
    }
#pragma warning restore CA1062
}
