namespace Smart.Mock.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    ///
    /// </summary>
    public class MockDbParameterCollection : List<MockDbParameter>, IDataParameterCollection
    {
        /// <summary>
        ///
        /// </summary>
        public MockDbCommand Command { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="command"></param>
        public MockDbParameterCollection(MockDbCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            Command = command;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public bool Contains(string parameterName)
        {
            return IndexOf(parameterName) != -1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public int IndexOf(string parameterName)
        {
            for (var i = 0; i < Count; i++)
            {
                if (String.Equals(this[i].ParameterName, parameterName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameterName"></param>
        public void RemoveAt(string parameterName)
        {
            var index = IndexOf(parameterName);
            if (index != -1)
            {
                RemoveAt(index);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public object this[string parameterName]
        {
            get
            {
                var index = IndexOf(parameterName);
                return index != -1 ? this[index] : null;
            }
            set
            {
                var index = IndexOf(parameterName);
                if (index == -1)
                {
                    throw new ArgumentOutOfRangeException(nameof(parameterName));
                }

                this[index] = (MockDbParameter)value;
            }
        }
    }
}
