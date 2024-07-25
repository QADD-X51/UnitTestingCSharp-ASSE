// <copyright file="BookFieldService.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Interfaces;
using DomainModel;
using Services.Interfaces;

namespace Services
{
    /// <summary>
    /// A service class designated to working with book fields.
    /// </summary>
    public class BookFieldService : BaseService<BookField, IBookFieldRepository>, IBookFieldService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookFieldService"/> class.
        /// </summary>
        /// <param name="repository">The repository of this service.</param>
        public BookFieldService(IBookFieldRepository repository)
            : base(repository)
        {
        }

        /// <inheritdoc/>
        public ValidationResult DeleteWithChildren(BookField bookField)
        {
            Stack<BookField> bookFieldsToDelete = new Stack<BookField>();
            Stack<BookField> toAdd = new Stack<BookField>();

            List<string> invalidFields = new List<string>();

            bookFieldsToDelete.Push(bookField);
            toAdd.Push(bookField);
            while (toAdd.Count > 0)
            {
                var currentBookField = toAdd.Pop();

                foreach (var childField in this.repository.GetAllDirectChildrenFields(currentBookField.Id))
                {
                    bookFieldsToDelete.Push(childField);
                    toAdd.Push(childField);
                }
            }

            while (bookFieldsToDelete.Count() != 0)
            {
                var currentBookField = bookFieldsToDelete.Pop();
                currentBookField.ParentField = null;
                this.Update(currentBookField);

                this.Delete(currentBookField);
            }

            Log.Info("Deleted field and it's children.");
            return ValidationResult.Success;
        }

        /// <inheritdoc/>
        public ICollection<BookField> GetAllChildrenDeep(BookField bookField)
        {
            Log.Info("Getting all deep children.");
            return this.repository.GetAllChildrenFields(bookField.Id);
        }

        /// <inheritdoc/>
        public BookField GetByName(string name)
        {
            Log.Info("Getting book field with name: " + name);
            return this.repository.GetByName(name);
        }

        /// <inheritdoc/>
        public ICollection<BookField> GetDirectChildren(BookField bookField)
        {
            Log.Info("Getting all direct children.");
            return this.repository.GetAllDirectChildrenFields(bookField.Id);
        }

        /// <inheritdoc/>
        public ValidationResult InsertCheck(BookField bookField)
        {
            var exists = this.GetByName(bookField.Name);

            if (exists != null)
            {
                Log.Warn("The current name is taken by another field: " + bookField.Name);
                return new ValidationResult("The name of the book field is taken.");
            }

            var result = this.Insert(bookField);

            if (result != ValidationResult.Success)
            {
                Log.Warn("The book field is invalid: " + bookField.Name);
                return result;
            }

            Log.Warn("The book field was inserted: " + bookField.Name);
            return result;
        }
    }
}
