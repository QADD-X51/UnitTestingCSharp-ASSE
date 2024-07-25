// <copyright file="IBookService.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;

namespace Services.Interfaces
{
    /// <summary>
    /// An interface for book service.
    /// </summary>
    public interface IBookService : IService<Book>
    {
        /// <summary>
        /// Changes the borrowable status of a book.
        /// </summary>
        /// <param name="book">The target book.</param>
        /// <param name="newBorrowable">The new borrow status of the book.</param>
        /// <returns>A validation result that reflects if the opperation was succesfull.</returns>
        ValidationResult ChangeBookBorrowable(Book book, bool newBorrowable);

        /// <summary>
        /// Changes the borrowable status of a book by it's ID.
        /// </summary>
        /// <param name="id">The ID of the target book.</param>
        /// <param name="newBorrowable">The new borrow status of the book.</param>
        /// <returns>A validation result that reflects if the opperation was succesfull.</returns>
        ValidationResult ChangeBookBorrowableById(object id, bool newBorrowable);

        /// <summary>
        /// Gets all the books with a certain name.
        /// </summary>
        /// <param name="name">The name of the books to be searched.</param>
        /// <returns>A list of books that have a certain name.</returns>
        ICollection<Book> GetByName(string name);
    }
}
