namespace Smart.Mock.Data.SqlServer;

using System.Globalization;
using System.Text;

using Microsoft.SqlServer.TransactSql.ScriptDom;

public sealed class ValidateResult
{
    public bool Valid => Errors.Count == 0;

    public IList<ParseError> Errors { get; } = [];

#pragma warning disable CA1062
    public void AddErrors(IList<ParseError> errors)
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
            sb.AppendFormat(
                CultureInfo.InvariantCulture,
                "Error [{0}] (Line = {1}, Column = {2}) : '{3}'\r\n",
                error.Number,
                error.Line,
                error.Column,
                error.Message);
        }

        return sb.ToString();
    }
}
