// <copyright file="BaseRepository.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Interfaces;

namespace DataMapper
{
    /// <summary>
    /// A class that manages direct communication between the application and the database.
    /// </summary>
    /// <typeparam name="T">A generic class. The class should be a model built after a database table.</typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseRepository<T> : IRepository<T>
        where T : class
    {
        /// <summary>
        /// The database context.
        /// </summary>
        protected MyContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
        /// </summary>
        /// <param name="dbContext">A db context.</param>
        public BaseRepository(MyContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>
        public virtual void Insert(T entity)
        {
            var dbSet = this.dbContext.Set<T>();
            dbSet.Add(entity);

            this.dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public virtual void Update(T item)
        {
            var dbSet = this.dbContext.Set<T>();
            dbSet.Attach(item);
            this.dbContext.Entry(item).State = EntityState.Modified;
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// A function that deleted an element from a database by it's ID.
        /// </summary>
        /// <param name="id">The id of the object that will be deleted.</param>
        public virtual void Delete(object id)
        {
            this.Delete(this.GetByID(id));
        }

        /// <inheritdoc/>
        public virtual void Delete(T entityToDelete)
        {
            var dbSet = this.dbContext.Set<T>();
            if (this.dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }

            dbSet.Remove(entityToDelete);
            this.dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public virtual T GetByID(object id)
        {
            return this.dbContext.Set<T>().Find(id);
        }

        /// <inheritdoc/>
        public IEnumerable<T> GetAll()
        {
            return this.dbContext.Set<T>().ToList();
        }

        /// <inheritdoc/>
        public void DeleteAll()
        {
            var entitiesToRemove = this.dbContext.Set<T>().ToList();
            this.dbContext.Set<T>().RemoveRange(entitiesToRemove);
            this.dbContext.SaveChanges();
        }
    }
}
