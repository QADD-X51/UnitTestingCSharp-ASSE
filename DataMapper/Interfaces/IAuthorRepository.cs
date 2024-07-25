// <copyright file="IAuthorRepository.cs" company="Transilvania University of Brasov">
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
    /// <summary>
    /// A interface for authors repository.
    /// </summary>
    public interface IAuthorRepository : IRepository<Author>
    {
        /// <summary>
        /// A function that provides all authors with a given name.
        /// </summary>
        /// <param name="name">The name to be searched.</param>
        /// <returns>A collection of authors that have the provided name.</returns>
        ICollection<Author> GetByName(string name);
    }
}
