namespace Smart.Mock.Data.SqlServer
{
    using System;
    using System.Collections.Generic;

    using Microsoft.SqlServer.TransactSql.ScriptDom;

    /// <summary>
    ///
    /// </summary>
    public class ValidateResult
    {
        /// <summary>
        ///
        /// </summary>
        public bool Valid
        {
            get { return Errors.Count == 0; }
        }

        /// <summary>
        ///
        /// </summary>
        public IList<ParseError> Errors { get; } = new List<ParseError>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="errors"></param>
        public void AddErrors(IList<ParseError> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            foreach (var error in errors)
            {
                Errors.Add(error);
            }
        }
    }
}
