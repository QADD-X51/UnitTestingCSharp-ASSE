// <copyright file="Borrow.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Validators;
using global::Validators;

namespace DomainModel
{
    /// <summary>
    /// A class that represents a borrow.
    /// The borrow is made of start date, end date, brrower, lender and books.
    /// </summary>
    public class Borrow
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets StartDate.
        /// </summary>
        [Required]
        [DateLessThan("DueDate")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets DueDate.
        /// </summary>
        [Required]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Gets or sets Borrower.
        /// </summary>
        [Required]
        [ValidPerson]
        public Person Borrower { get; set; }

        /// <summary>
        /// Gets or sets Lender.
        /// </summary>
        [Required]
        [ValidLenderBorrow]
        [ValidPerson]
        public Person Lender { get; set; }

        /// <summary>
        /// Gets or sets Books.
        /// </summary>
        [Required]
        [ListNotEmpty]
        [NoDuplicateBooks]
        [OnlyBorrowableBooks]
        [ValidBooks]
        [MaximumBookBorrow]
        public ICollection<Book> Books { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a borrow is still ongoing or not.
        /// Used to determin if borrower got back the books faster.
        /// </summary>
        [Required]
        [ShouldBeTrue]
        public bool Active { get; set; }
    }
}
