// <copyright file="BookField.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    /// <summary>
    /// A class the represents a book field.
    /// </summary>
    public partial class BookField
    {
        /// <summary>
        /// Gets or sets and sets Id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        [Required]
        [StringLength(30, MinimumLength = 3)]
        [Index(IsUnique = true)]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ParentField.
        /// </summary>
        public BookField ParentField
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ChildFields.
        /// </summary>
        public ICollection<BookField> ChildFields { get; set; }

        /// <summary>
        /// Gets or sets books.
        /// </summary>
        public ICollection<Book> Books
        {
            get;
            set;
        }
    }
}
