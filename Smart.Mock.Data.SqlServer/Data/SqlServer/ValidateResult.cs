namespace Smart.Mock.Data.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

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
        public IList<ErrorEntry> Errors { get; } = new List<ErrorEntry>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="entry"></param>
        public void AddError(ErrorEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            Errors.Add(entry);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var error in Errors)
            {
                sb.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "Error [{0}] (Line = {1}, Column = {2}) '{3}'\r\n",
                    error.Number,
                    error.Line,
                    error.Column,
                    error.Message);
            }

            return sb.ToString();
        }
    }
}
