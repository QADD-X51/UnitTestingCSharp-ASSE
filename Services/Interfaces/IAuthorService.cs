// <copyright file="IAuthorService.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;

namespace Services.Interfaces
{
    /// <summary>
    /// A interface for the authro service.
    /// </summary>
    public interface IAuthorService : IService<Author>
    {
        /// <summary>
        /// Gets all authors with the provided name.
        /// </summary>
        /// <param name="name">Name of authors.</param>
        /// <returns>A collection of authors with the name provided.</returns>
        ICollection<Author> GetByName(string name);
    }
}
