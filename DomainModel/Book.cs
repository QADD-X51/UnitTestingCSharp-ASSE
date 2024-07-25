// <copyright file="Book.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using DomainModel.Validators;
using global::Validators;

namespace DomainModel
{
    /// <summary>
    /// A book.
    /// </summary>
    public partial class Book
    {
        /// <summary>
        /// Gets or sets Id.
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
        [MinLength(2)]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Authors.
        /// </summary>
        [ValidAuthor]
        public ICollection<Author> Authors
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets PagesCount.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int PagesCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Fields.
        /// </summary>
        [Required]
        [ListNotEmpty]
        [ValidatorBookField]
        [ValidBookFields]
        public ICollection<BookField> Fields
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether a book can be borrowed.
        /// </summary>
        [Required]
        public bool Borrowable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating the total number of copies.
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public int TotalCopies
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets borrows.
        /// </summary>
        public ICollection<Borrow> Borrows
        {
            get;
            set;
        }
    }
}
