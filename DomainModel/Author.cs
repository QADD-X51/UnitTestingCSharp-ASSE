// <copyright file="Author.cs" company="Transilvania University of Brasov">
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
    /// An author for a book.
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        [Required]
        [StringLength(150, MinimumLength = 2)]
        [RegularExpression(@"^(?!\s*$).+")] // Everything, but blank or empty.
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the books written by the author.
        /// </summary>
        public ICollection<Book> Books { get; set; }
    }
}
