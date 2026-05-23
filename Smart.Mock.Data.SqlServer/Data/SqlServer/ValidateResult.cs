namespace Smart.Mock.Data.SqlServer;

using System.Globalization;
using System.Text;

using Microsoft.SqlServer.TransactSql.ScriptDom;

public sealed class ValidateResult
{
    public bool Valid => Errors.Count == 0;

    public IList<ParseError> Errors { get; } = [];

#pragma warning disable CA1062
    public void AddErrors(IEnumerable<ParseError> errors)
    {
        foreach (var error in errors)
        {
            Errors.Add(error);
        }
    }
#pragma warning restore CA1062

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var error in Errors)
        {
            sb.Append(CultureInfo.InvariantCulture, $"Error [{error.Number}] (Line = {error.Line}, Column = {error.Column}) : '{error.Message}'\r\n");
        }

        return sb.ToString();
    }
}
