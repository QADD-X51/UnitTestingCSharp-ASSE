// <copyright file="ShouldBeTrueAttribute.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validators
{
    /// <summary>
    /// A validation class.
    /// </summary>
    public class ShouldBeTrueAttribute : ValidationAttribute
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ShouldBeTrueAttribute"/> class.
        /// </summary>
        public ShouldBeTrueAttribute()
        {
        }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = (bool)value;

            if (result == false)
            {
                return new ValidationResult("The boolean should be true.");
            }

            return ValidationResult.Success;
        }
    }
}
