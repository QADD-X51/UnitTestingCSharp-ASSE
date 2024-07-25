// <copyright file="IBorrowService.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;

namespace Services.Interfaces
{
    /// <summary>
    /// An interface for the borrow service class.
    /// </summary>
    public interface IBorrowService : IService<Borrow>
    {
        /// <summary>
        /// Checks if a borrow is eligible to be inserted in the database.
        /// </summary>
        /// <param name="borrow">Target borrow.</param>
        /// <param name="referenceTime">
        /// The time that is used for certain validations.
        /// By default is null, which means Date.Now will be used.
        /// </param>
        /// <returns>A validation result that reflects the validity of the borrow.</returns>
        ValidationResult VerifyBorrow(Borrow borrow, DateTime? referenceTime = null);

        /// <summary>
        /// Insert a new borrow in the database if it's valid.
        /// </summary>
        /// <param name="borrow">Borrow to be added.</param>
        /// <param name="referenceTime">Default is null, so it will get the current time.</param>
        /// <returns>A results that reflects if borrow was added.</returns>
        ValidationResult InsertCheck(Borrow borrow, DateTime? referenceTime);

        /// <summary>
        /// Gets the book field from database that has the exact info from the borrow provided.
        /// </summary>
        /// <param name="borrow">Target borrow.</param>
        /// <returns>The borrow that is searched for.</returns>
        Borrow GetByInfo(Borrow borrow);
    }
}
