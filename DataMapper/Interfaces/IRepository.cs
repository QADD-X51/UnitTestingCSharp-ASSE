// <copyright file="IRepository.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataMapper.Interfaces
{
    /// <summary>
    /// An interface of a repository class.
    /// </summary>
    /// <typeparam name="T">A generic class.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Adds an object to the database.
        /// </summary>
        /// /// <param name="entity">The object to be added.</param>
        void Insert(T entity);

        /// <summary>
        /// Updates an object in the database.
        /// </summary>
        /// /// <param name="item">The object to be updated.</param>
        void Update(T item);

        /// <summary>
        /// Deletes an object in the database.
        /// </summary>
        /// /// <param name="entity">The object to be deleted.</param>
        void Delete(T entity);

        /// <summary>
        /// Returns an object found by an Id.
        /// </summary>
        /// <param name="id">The id of the object that will be searched.</param>
        /// <returns>The object found in the database.</returns>
        T GetByID(object id);

        /// <summary>
        /// Returns all objects from a table in the database.
        /// </summary>
        /// <returns>The objects yielded by the database.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Deletes all object in the database.
        /// ONLY USED FOR TESTING. USE WITH CARE.
        /// </summary>
        void DeleteAll();
    }
}
