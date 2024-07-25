// <copyright file="IBookFieldService.cs" company="Transilvania University of Brasov">
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
    /// An interface for book field service.
    /// </summary>
    public interface IBookFieldService : IService<BookField>
    {
        /// <summary>
        /// Deletes a book field and all of it's children.
        /// ALWAYS USE THIS INSTEAD OF NORMAL DELETE.
        /// </summary>
        /// <param name="bookField">The parent book field to be deleted.</param>
        /// <returns>A validation result that reflects if all fields were deleted.</returns>
        ValidationResult DeleteWithChildren(BookField bookField);

        /// <summary>
        /// Validates the object, afterwards it will add it to the database.
        /// </summary>
        /// <param name="bookField">Object to be inserted.</param>
        /// <returns>A result that reflects the succes of the insertion.</returns>
        ValidationResult InsertCheck(BookField bookField);

        /// <summary>
        /// Gets the field with the provided name from the database.
        /// </summary>
        /// <param name="name">The field's name.</param>
        /// <returns>The book field with the name.</returns>
        BookField GetByName(string name);

        /// <summary>
        /// Get a list of all direct children of a book field.
        /// </summary>
        /// <param name="bookField">Target field.</param>
        /// <returns>A collection of book fields.</returns>
        ICollection<BookField> GetDirectChildren(BookField bookField);

        /// <summary>
        /// Get a list of all children of a book field.
        /// All children that also have other children will be added in their perspective ChildFields attribute.
        /// </summary>
        /// <param name="bookField">Target field.</param>
        /// <returns>A collection of book fields.</returns>
        ICollection<BookField> GetAllChildrenDeep(BookField bookField);
    }
}
