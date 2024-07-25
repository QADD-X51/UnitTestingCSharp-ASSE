// <copyright file="IPersonService.cs" company="Transilvania University of Brasov">
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
    /// An interface for person service.
    /// </summary>
    public interface IPersonService : IService<Person>
    {
        /// <summary>
        /// Gets a person from the databse by it's email.
        /// </summary>
        /// <param name="email">A string that represents an email.</param>
        /// <returns>
        /// An entry in the Person table from the database that has the target email.
        /// If no such entry is fount it will return null.
        /// </returns>
        Person GetByEmail(string email);

        /// <summary>
        /// A function that will insert a person in the database only if it has a unique Email.
        /// </summary>
        /// <param name="person">The person to be inserted.</param>
        /// <returns>A validation result dat reflects if the person was succesfully inserted in the database.</returns>
        ValidationResult InsertCheck(Person person);
    }
}
