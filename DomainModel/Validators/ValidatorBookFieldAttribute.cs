// <copyright file="ValidatorBookFieldAttribute.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;

namespace DomainModel.Validators
{
    /// <summary>
    /// A custom valitation class.
    /// </summary>
    public class ValidatorBookFieldAttribute : ValidationAttribute
    {
        private static readonly int DOMENII;

        static ValidatorBookFieldAttribute()
        {
            DOMENII = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DOMENII"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorBookFieldAttribute"/> class.
        /// </summary>
        public ValidatorBookFieldAttribute()
        {
        }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var fields = value as IEnumerable<BookField>;

            if (fields.Count() == 1)
            {
                return ValidationResult.Success;
            }

            if (fields.Count() > DOMENII)
            {
                return new ValidationResult("Too many fields.");
            }

            List<int> prohibitedBookFieldsIds = new List<int>();

            foreach (var field in fields)
            {
                BookField fieldToCheck = field;
                do
                {
                    if (prohibitedBookFieldsIds.Contains(fieldToCheck.Id))
                    {
                        return new ValidationResult("The book has two fields that are related");
                    }

                    fieldToCheck = fieldToCheck.ParentField;
                }
                while (fieldToCheck != null);

                fieldToCheck = field;
                prohibitedBookFieldsIds.Add(fieldToCheck.Id);
                while (fieldToCheck.ParentField != null)
                {
                    fieldToCheck = fieldToCheck.ParentField;
                    prohibitedBookFieldsIds.Add(fieldToCheck.Id);
                }
            }

            return ValidationResult.Success;
        }
    }
}
