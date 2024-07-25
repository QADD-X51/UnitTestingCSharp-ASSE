// <copyright file="ValidPersonAttribute.cs" company="Transilvania University of Brasov">
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
    /// A validation class that defines an attribut that checks if a Person object is valid or not.
    /// </summary>
    public class ValidPersonAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidPersonAttribute"/> class.
        /// </summary>
        public ValidPersonAttribute()
        {
        }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var person = value as Person;

            var context = new ValidationContext(person);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(person, context, results, true);

            if (results.Count > 0)
            {
                return new ValidationResult("Person is invalid.");
            }

            return ValidationResult.Success;
        }
    }
}
