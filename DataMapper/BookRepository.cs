// <copyright file="BookRepository.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Interfaces;
using DomainModel;

namespace DataMapper
{
    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookRepository"/> class.
        /// </summary>
        /// <param name="context">A database context.</param>
        public BookRepository(MyContext context)
            : base(context)
        {
        }

        /// <inheritdoc/>
        public ICollection<Book> GetByName(string name)
        {
            return this.dbContext.Set<Book>().Where(book => book.Name == name).ToList();
        }
    }
}
