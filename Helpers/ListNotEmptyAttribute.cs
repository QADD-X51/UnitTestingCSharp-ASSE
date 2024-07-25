// <copyright file="ListNotEmptyAttribute.cs" company="Transilvania University of Brasov">
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
    /// A class that overiddes ValidationAttribute.
    /// </summary>
    public class ListNotEmptyAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListNotEmptyAttribute"/> class.
        /// </summary>
        public ListNotEmptyAttribute()
        {
        }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as IList;

            if (list == null)
            {
                return new ValidationResult("The list does not exist.");
            }

            if (list.Count == 0)
            {
                return new ValidationResult("List is empty.");
            }

            return ValidationResult.Success;
        }
    }
}
