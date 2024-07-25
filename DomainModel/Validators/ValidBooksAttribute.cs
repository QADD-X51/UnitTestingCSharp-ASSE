// <copyright file="ValidBooksAttribute.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Validators
{
    /// <summary>
    /// A validation class that checks if all books in the list are valid.
    /// </summary>
    public class ValidBooksAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidBooksAttribute"/> class.
        /// </summary>
        public ValidBooksAttribute()
        {
        }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var books = value as IEnumerable<Book>;

            if (books == null)
            {
                return ValidationResult.Success;
            }

            if (books.Count() == 0)
            {
                return ValidationResult.Success;
            }

            foreach (var book in books)
            {
                var context = new ValidationContext(book);
                var results = new List<ValidationResult>();

                Validator.TryValidateObject(book, context, results, true);

                if (results.Count > 0)
                {
                    return new ValidationResult("One of the books is invalid.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
