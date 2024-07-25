// <copyright file="IPersonRepository.cs" company="Transilvania University of Brasov">
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
    public interface IPersonRepository : IRepository<Person>
    {
        /// <summary>
        /// A method that gets the person with a certain email.
        /// </summary>
        /// <param name="email">The target email.</param>
        /// <returns>The person with the target email.</returns>
        Person GetByEmail(string email);
    }
}
