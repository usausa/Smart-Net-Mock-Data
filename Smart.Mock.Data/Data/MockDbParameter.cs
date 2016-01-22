namespace Smart.Mock.Data
{
    using System.Data;
    using System.Data.Common;

    /// <summary>
    ///
    /// </summary>
    public class MockDbParameter : DbParameter
    {
        /// <summary>
        ///
        /// </summary>
        public override DbType DbType { get; set; }

        /// <summary>
        ///
        /// </summary>
        public override ParameterDirection Direction { get; set; }

        /// <summary>
        ///
        /// </summary>
        public override bool IsNullable { get; set; }

        /// <summary>
        ///
        /// </summary>
        public override string ParameterName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public override string SourceColumn { get; set; }

        /// <summary>
        ///
        /// </summary>
        public override object Value { get; set; }

        /// <summary>
        ///
        /// </summary>
        public override bool SourceColumnNullMapping { get; set; }

        /// <summary>
        ///
        /// </summary>
        public override int Size { get; set; }

        /// <summary>
        ///
        /// </summary>
        public override void ResetDbType()
        {
        }
    }
}
