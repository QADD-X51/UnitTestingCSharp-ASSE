// <copyright file="IBookRepository.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;

namespace DataMapper.Interfaces
{
    /// <inheritdoc/>
    public interface IBookRepository : IRepository<Book>
    {
        /// <summary>
        /// Get all the boos with a given name.
        /// </summary>
        /// <param name="name">The name of the book.</param>
        /// <returns>A collection of books with the given name.</returns>
        ICollection<Book> GetByName(string name);
    }
}
