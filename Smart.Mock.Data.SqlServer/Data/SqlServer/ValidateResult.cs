namespace Smart.Mock.Data.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    using Microsoft.SqlServer.TransactSql.ScriptDom;

    public class ValidateResult
    {
        public bool Valid => Errors.Count == 0;

        public IList<ParseError> Errors { get; } = new List<ParseError>();

        public void AddErrors(IList<ParseError> errors)
        {
            if (errors is null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            foreach (var error in errors)
            {
                Errors.Add(error);
            }
        }

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
}
