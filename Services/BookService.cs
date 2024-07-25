// <copyright file="BookService.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Interfaces;
using DomainModel;
using Services.Interfaces;

namespace Services
{
    /// <summary>
    /// A service class designated to working with books.
    /// </summary>
    public class BookService : BaseService<Book, IBookRepository>, IBookService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookService"/> class.
        /// </summary>
        /// <param name="repository">The repository object.</param>
        public BookService(IBookRepository repository)
            : base(repository)
        {
        }

        /// <inheritdoc/>
        public ValidationResult ChangeBookBorrowable(Book book, bool newBorrowable)
        {
            if (book == null)
            {
                Log.Warn("Book should not be null.");
                return new ValidationResult("Should not be null.");
            }

            book.Borrowable = newBorrowable;
            Log.Info("Book status changed.");
            return this.Update(book);
        }

        /// <inheritdoc/>
        public ValidationResult ChangeBookBorrowableById(object id, bool newBorrowable)
        {
            Book book = this.GetById(id);

            if (book == null)
            {
                Log.Warn("Book not found.");
                return new ValidationResult("Book not found.");
            }

            return this.ChangeBookBorrowable(book, newBorrowable);
        }

        /// <inheritdoc/>
        public ICollection<Book> GetByName(string name)
        {
            return this.repository.GetByName(name);
        }
    }
}
