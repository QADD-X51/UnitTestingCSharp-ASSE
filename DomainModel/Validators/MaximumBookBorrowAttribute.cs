// <copyright file="MaximumBookBorrowAttribute.cs" company="Transilvania University of Brasov">
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
    /// A validation class folr borrows that checks if the books count and their fields are correct.
    /// </summary>
    public class MaximumBookBorrowAttribute : ValidationAttribute
    {
        private static readonly int C;

        static MaximumBookBorrowAttribute()
        {
            C = int.Parse(System.Configuration.ConfigurationManager.AppSettings["C"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaximumBookBorrowAttribute"/> class.
        /// </summary>
        public MaximumBookBorrowAttribute()
        {
        }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var books = value as IEnumerable<Book>;
            int maxBookCount = C;

            var property = validationContext.ObjectType.GetProperty("Borrower");
            Person borrower = (Person)property.GetValue(validationContext.ObjectInstance);

            if (borrower == null)
            {
                return ValidationResult.Success;
            }

            if (borrower.IsPersonnel)
            {
                maxBookCount *= 2;
            }

            if (books == null)
            {
                return ValidationResult.Success;
            }

            if (books.Count() == 0)
            {
                return ValidationResult.Success;
            }

            if (books.Count() > maxBookCount)
            {
                return new ValidationResult("Too many books");
            }

            if (books.Count() >= 3)
            {
                List<int> uniqueRootBookFieldsIds = new List<int>();

                foreach (var book in books)
                {
                    foreach (var field in book.Fields)
                    {
                        BookField rootField = field;
                        while (rootField.ParentField != null)
                        {
                            rootField = rootField.ParentField;
                        }

                        if (uniqueRootBookFieldsIds.Contains(rootField.Id))
                        {
                            continue;
                        }

                        uniqueRootBookFieldsIds.Add(rootField.Id);
                    }
                }

                if (uniqueRootBookFieldsIds.Count() < 2)
                {
                    return new ValidationResult("Not enough book fields.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
