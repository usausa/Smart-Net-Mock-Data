namespace Smart.Mock.Data.SqlServer
{
    /// <summary>
    ///
    /// </summary>
    public class ErrorEntry
    {
        /// <summary>
        ///
        /// </summary>
        public int Index { get; }

        /// <summary>
        ///
        /// </summary>
        public string CommandText { get; }

        /// <summary>
        ///
        /// </summary>
        public int Number { get; }

        /// <summary>
        ///
        /// </summary>
        public int Offset { get; }

        /// <summary>
        ///
        /// </summary>
        public int Line { get; }

        /// <summary>
        ///
        /// </summary>
        public int Column { get; }

        /// <summary>
        ///
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <param name="commandText"></param>
        /// <param name="number"></param>
        /// <param name="offset"></param>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <param name="message"></param>
        public ErrorEntry(int index, string commandText, int number, int offset, int line, int column, string message)
        {
            Index = index;
            CommandText = commandText;
            Number = number;
            Offset = offset;
            Line = line;
            Column = column;
            Message = message;
        }
    }
}
