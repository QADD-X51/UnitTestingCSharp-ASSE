// <copyright file="ValidLenderBorrowAttribute.cs" company="Transilvania University of Brasov">
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
    /// A validation class to check if a borrow has a valid lender.
    /// </summary>
    public class ValidLenderBorrowAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidLenderBorrowAttribute"/> class.
        /// </summary>
        public ValidLenderBorrowAttribute()
        {
        }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var field = value as Person;

            if (field.IsPersonnel == true)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("The lender is not a employee.");
        }
    }
}
