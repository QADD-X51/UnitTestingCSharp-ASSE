// <copyright file="ValidBookFieldsAttribute.cs" company="Transilvania University of Brasov">
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
    /// A validation class that verifies if all fields of a book are valid.
    /// </summary>
    public class ValidBookFieldsAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidBookFieldsAttribute"/> class.
        /// </summary>
        public ValidBookFieldsAttribute()
        {
        }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var bookFields = value as IEnumerable<BookField>;

            if (bookFields == null)
            {
                return ValidationResult.Success;
            }

            if (bookFields.Count() == 0)
            {
                return ValidationResult.Success;
            }

            foreach (var bookField in bookFields)
            {
                var context = new ValidationContext(bookField);
                var results = new List<ValidationResult>();

                Validator.TryValidateObject(bookField, context, results, true);

                if (results.Count > 0)
                {
                    return new ValidationResult("One of the books fields is invalid.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
