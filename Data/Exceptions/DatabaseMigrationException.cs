// Copyright (c) YourCompanyName. All rights reserved.
// BusBuddy/Data/Exceptions/DatabaseMigrationException.cs
namespace BusBuddy.Data.Exceptions
{
    using System;

    /// <summary>
    /// Exception thrown when a database migration fails.
    /// </summary>
    public class DatabaseMigrationException : Exception
    {
        /// <summary>
        /// Gets or sets the migration version that failed.
        /// </summary>
        public int MigrationVersion { get; set; }

        /// <summary>
        /// Gets or sets the actual version of the database after the migration attempt.
        /// </summary>
        public int ActualVersion { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseMigrationException"/> class.
        /// </summary>
        public DatabaseMigrationException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseMigrationException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DatabaseMigrationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseMigrationException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public DatabaseMigrationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}