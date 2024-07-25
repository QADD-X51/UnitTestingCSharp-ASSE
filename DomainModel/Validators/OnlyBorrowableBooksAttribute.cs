// <copyright file="OnlyBorrowableBooksAttribute.cs" company="Transilvania University of Brasov">
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
    /// A validation class that checks if books in a borrow are borrowable.
    /// </summary>
    public class OnlyBorrowableBooksAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnlyBorrowableBooksAttribute"/> class.
        /// </summary>
        public OnlyBorrowableBooksAttribute()
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
                if (book.Borrowable == false || book.TotalCopies == 0)
                {
                    return new ValidationResult("Book should be borrowable.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
