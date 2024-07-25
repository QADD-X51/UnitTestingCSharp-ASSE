using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Validators
{
    /// <summary>
    /// A validation class design to check if a list of authors contains at least one invalid author.
    /// </summary>
    internal class ValidAuthorAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidAuthorAttribute"/> class.
        /// </summary>
        public ValidAuthorAttribute()
        {
        }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var authors = value as IEnumerable<Author>;

            if (authors == null)
            {
                return ValidationResult.Success;
            }

            if (authors.Count() == 0)
            {
                return ValidationResult.Success;
            }

            foreach (var author in authors)
            {
                var context = new ValidationContext(author);
                var results = new List<ValidationResult>();

                Validator.TryValidateObject(author, context, results, true);

                if (results.Count > 0)
                {
                    return new ValidationResult("One of the authors is invalid.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
