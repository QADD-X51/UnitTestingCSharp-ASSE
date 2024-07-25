// <copyright file="BaseService.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using DataMapper;
using DataMapper.Interfaces;
using DomainModel;
using log4net;
using Services.Interfaces;

namespace Services
{
    /// <summary>
    /// A base service class.
    /// </summary>
    /// <typeparam name="T">A class that represents a model of a table in the database.</typeparam>
    /// <typeparam name="TR">An interface that represents the repository of a table in the database.</typeparam>
    public abstract class BaseService<T, TR> : IService<T>
        where T : class
        where TR : IRepository<T>
    {
        /// <summary>
        /// The log item.
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger("Sevice: " + typeof(T).Name);

        /// <summary>
        /// The repository object.
        /// </summary>
        protected TR repository;

        static BaseService()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService{T, TR}"/> class.
        /// </summary>
        /// <param name="repository">A repository object.</param>
        public BaseService(TR repository)
        {
            this.repository = repository;
        }

        /// <inheritdoc/>
        public ValidationResult Delete(T entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(entity, context, results, true))
            {
                return new ValidationResult("Invalid object.");
            }

            this.repository.Delete(entity);
            return ValidationResult.Success;
        }

        /// <inheritdoc/>
        public void DeleteAll()
        {
            this.repository.DeleteAll();
            Log.Info("Deleted all items.");
        }

        /// <inheritdoc/>
        public IEnumerable<T> GetAll()
        {
            Log.Info("Got all items.");
            return this.repository.GetAll();
        }

        /// <inheritdoc/>
        public T GetById(object id)
        {
            Log.Info("Got items by id: " + id.ToString());
            return this.repository.GetByID(id);
        }

        /// <inheritdoc/>
        public ValidationResult Insert(T entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(entity, context, results, true))
            {
                Log.Warn("Item could not be inserted because: " + results.First().ErrorMessage);
                return new ValidationResult("Invalid object.");
            }

            this.repository.Insert(entity);

            Log.Info("Item inserted.");

            return ValidationResult.Success;
        }

        /// <inheritdoc/>
        public ValidationResult Update(T entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(entity, context, results, true))
            {
                Log.Warn("Item could not be updated because: " + results.First().ErrorMessage);
                return new ValidationResult("Invalid object.");
            }

            this.repository.Update(entity);

            Log.Info("Item updated.");

            return ValidationResult.Success;
        }

        /// <inheritdoc/>
        public virtual DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}
