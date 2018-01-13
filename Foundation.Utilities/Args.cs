namespace Foundation.Utilities
{
    using System;

    public static class Args
    {
        /// <summary>
        /// Validates argument is not null. Throws <see cref="ArgumentNullException"/> if value is null
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">name of argument being checked</param>
        /// <param name="message">Optional message to include in exception body</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void NotNull(object value, string name, string message = null)
        {
            if (value == null) throw new ArgumentNullException(name, message);
        }

        /// <summary>
        /// Validates the string argument is not null or empty.
        /// Throws <see cref="ArgumentNullException"/> if null, or <see cref="ArgumentException"/> if empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">Name of argument being checked</param>
        /// <param name="message">Optional message to include in exception body</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        public static void NotNullOrEmpty(string value, string name, string message = null)
        {
            if (value == null) throw new ArgumentNullException(name, message);
            if (value == string.Empty) throw new ArgumentException(message ?? "cannot be string.Empty", name);
        }

        /// <summary>
        /// Validates the string argument is not null, empty, or whitespace.
        /// Throws <see cref="ArgumentNullException"/> if null, or <see cref="ArgumentException"/> if empty or whitespace.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name of the argument being checked</param>
        /// <param name="message">Optional message to include in exception body</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        public static void NotNullOrWhiteSpace(string value, string name, string message = null)
        {
            if (value == null) throw new ArgumentNullException(name, message);
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException(message ?? "cannot be string.Empty or just white space", name);
        }
    }
}
