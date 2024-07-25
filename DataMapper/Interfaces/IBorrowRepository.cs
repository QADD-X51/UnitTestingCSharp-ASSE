// <copyright file="IBorrowRepository.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;

namespace DataMapper.Interfaces
{
    /// <inheritdoc/>
    public interface IBorrowRepository : IRepository<Borrow>
    {
        /// <summary>
        /// Gets all active borrows from the database.
        /// </summary>
        /// <returns>An enumerable of the resulted items.</returns>
        IEnumerable<Borrow> GetAllActive();

        /// <summary>
        /// Gets all active borrows of a certain user from the database starting from a certain date, going back a number of days.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="days">The days count to go back.</param>
        /// <param name="targetDate">The interval will be checked from targetDate minus days to targetDate.</param>
        /// <returns>An enumerable of the resulted items.</returns>
        IEnumerable<Borrow> GetAllActiveOfUserInLastDays(int userId, int days, DateTime targetDate);

        /// <summary>
        /// Gets the borrow, of a certain user, that contains a certain book.
        /// The borrow with the largest DueDate is returned.
        /// </summary>
        /// <param name="userId">User's ID.</param>
        /// <param name="bookId">Book's ID.</param>
        /// <returns>The borrow.</returns>
        Borrow GetLastBorrowOfUserContainingBook(int userId, int bookId);

        /// <summary>
        /// Gets all active borrows that contain a certain book.
        /// </summary>
        /// <param name="bookId">Book's ID.</param>
        /// <returns>A list of borrows containing the target book.</returns>
        IEnumerable<Borrow> GetAllActiveBorrowsContainingBook(int bookId);

        /// <summary>
        /// Gets all borrows of a certain user in a specified day.
        /// </summary>
        /// <param name="userID">User's ID.</param>
        /// <param name="referenceTime">The day to verify.</param>
        /// <returns>A list of borrows.</returns>
        IEnumerable<Borrow> GetAllBorrowsOfBorrowerInADay(int userID, DateTime referenceTime);

        /// <summary>
        /// Gets all borrows of a certain employee in a specified day.
        /// </summary>
        /// <param name="userID">Employee's ID.</param>
        /// <param name="referenceTime">The day to verify.</param>
        /// <returns>A list of borrows.</returns>
        IEnumerable<Borrow> GetAllBorrowsOfLenderInADay(int userID, DateTime referenceTime);

        /// <summary>
        /// Gets the item with the exact info in the borrow provided.
        /// </summary>
        /// <param name="borrow">Target borrow.</param>
        /// <returns>The borrow from the database that has the info. Null if not found.</returns>
        Borrow GetByInfo(Borrow borrow);

        /// <summary>
        /// Changes the active state of all entries if their DueDate is higher than the reference time.
        /// </summary>
        /// <param name="referenceTime">The reference time. Should be now, in most cases.</param>
        /// <returns>A intiger that counts how many entries have changed.</returns>
        int ChangeActiveWhenDueDateReached(DateTime referenceTime);
    }
}
