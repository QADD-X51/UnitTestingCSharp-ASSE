// <copyright file="DateLessThanAttribute.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validators
{
    /// <summary>
    /// A validation class that checks if an object's date is smaller than another date.
    /// </summary>
    public class DateLessThanAttribute : ValidationAttribute
    {
        private readonly string property;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateLessThanAttribute"/> class.
        /// </summary>
        /// <param name="property">The name of the property to be compared.</param>
        public DateLessThanAttribute(string property)
        {
            this.property = property;
        }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime currentValue = (DateTime)value;

            var property = validationContext.ObjectType.GetProperty(this.property);

            DateTime compare = (DateTime)property.GetValue(validationContext.ObjectInstance);

            if (compare == null)
            {
                return ValidationResult.Success;
            }

            if (compare < currentValue)
            {
                return new ValidationResult("The date is less than target.");
            }

            return ValidationResult.Success;
        }
    }
}
