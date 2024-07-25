// <copyright file="IService.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    /// <summary>
    /// An interface that definse a base service.
    /// </summary>
    /// <typeparam name="T">A genetic class. It should represent a model that reflects a table from the database.</typeparam>
    public interface IService<T>
        where T : class
    {
        /// <summary>
        /// A function that inserts a new element into the database.
        /// </summary>
        /// <param name="entity">The object to be inserted.</param>
        /// <returns>A validation result that reflects if the insertion is succesful.</returns>
        ValidationResult Insert(T entity);

        /// <summary>
        /// A function that updates an element from the database.
        /// </summary>
        /// <param name="entity">The object to be updated.</param>
        /// <returns>A validation result that reflects if the update is succesful.</returns>
        ValidationResult Update(T entity);

        /// <summary>
        /// A function that deletes an element from the database.
        /// </summary>
        /// <param name="entity">The object to be deleted.</param>
        /// <returns>A validation result that reflects if the deletion is succesful.</returns>
        ValidationResult Delete(T entity);

        /// <summary>
        /// A function that deletes all elements from the database.
        /// USE WITH CAUTION.
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// A function that gets and object from the database by it's ID.
        /// </summary>
        /// <param name="id">The ID of the returned object.</param>
        /// <returns>An object from the database with the designated ID.</returns>
        T GetById(object id);

        /// <summary>
        /// Will get all the entries from a table in the database.
        /// </summary>
        /// <returns>An enumerable object containing the objects yielded by the database.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Returns Date.Now.</returns>
        DateTime GetCurrentTime();
    }
}
