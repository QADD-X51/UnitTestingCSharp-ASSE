// <copyright file="IBookFieldRepository.cs" company="Transilvania University of Brasov">
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
    public interface IBookFieldRepository : IRepository<BookField>
    {
        /// <summary>
        /// Get the item in the database with the provided name.
        /// </summary>
        /// <param name="name">Target name.</param>
        /// <returns>
        /// The book field with the provided name.
        /// Will be null if the field is not found.
        /// </returns>
        BookField GetByName(string name);

        /// <summary>
        /// Gets all direct children of a field from the database.
        /// </summary>
        /// <param name="parentFieldId">The parent field's ID.</param>
        /// <returns>Returns a list of book fields.</returns>
        ICollection<BookField> GetAllDirectChildrenFields(int parentFieldId);

        /// <summary>
        /// Gets all direct children of a field from the database.
        /// </summary>
        /// <param name="rootFieldId">The root field's ID.</param>
        /// <returns>Returns a list of book fields.</returns>
        ICollection<BookField> GetAllChildrenFields(int rootFieldId);
    }
}
