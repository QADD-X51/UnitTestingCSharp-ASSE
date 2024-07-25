// <copyright file="Person.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    /// <summary>
    /// A class that determins a person.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression("^\\S+$")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Surname.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression("^\\S+$")]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        [Required]
        [StringLength(100)]
        [EmailAddress]
        [RegularExpression("^\\S+@\\S+\\.\\S+$")]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets Phone.
        /// </summary>
        [Required]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression("^[0-9]+$")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a person is an employee.
        /// Used for special privillages.
        /// </summary>
        [Required]
        public bool IsPersonnel { get; set; }
    }
}
