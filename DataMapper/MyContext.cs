// <copyright file="MyContext.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System.Configuration;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using DomainModel;

namespace DataMapper
{
    /// <summary>
    /// A class that establishes connection with a database.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MyContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyContext"/> class.
        /// The instance will connect to the database using the connection string provided in App.config.
        /// </summary>
        public MyContext()
            : base(System.Configuration.ConfigurationManager.AppSettings["connStr"])
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        /// <summary>
        /// Gets or sets an object that stores database items from the book table.
        /// </summary>
        public virtual DbSet<Book> Books { get; set; }

        /// <summary>
        /// Gets or sets an object that stores database items from the person table.
        /// </summary>
        public virtual DbSet<Person> People { get; set; }

        /// <summary>
        /// Gets or sets an object that stores database items from the book field table.
        /// </summary>
        public virtual DbSet<BookField> BookFields { get; set; }

        /// <summary>
        /// Gets or sets an object that stores database items from the borrow table.
        /// </summary>
        public virtual DbSet<Borrow> Borrows { get; set; }

        /// <summary>
        /// Gets or sets an object that stores database items from the borrow table.
        /// </summary>
        public virtual DbSet<Author> Authors { get; set; }
    }
}
