// <copyright file="NoDuplicateBooksAttribute.cs" company="Transilvania University of Brasov">
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
    /// A class that validates if a borrow has duplicate books.
    /// </summary>
    public class NoDuplicateBooksAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoDuplicateBooksAttribute"/> class.
        /// </summary>
        public NoDuplicateBooksAttribute()
        {
        }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var books = value as IEnumerable<Book>;

            if (books.Count() < 2)
            {
                return ValidationResult.Success;
            }

            List<int> booksId = new List<int>();
            foreach (var book in books)
            {
                if (booksId.Contains(book.Id))
                {
                    return new ValidationResult("Suplicate book found.");
                }

                booksId.Add(book.Id);
            }

            return ValidationResult.Success;
        }
    }
}
