// <copyright file="BorrowRepository.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Interfaces;
using DomainModel;

namespace DataMapper
{
    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public class BorrowRepository : BaseRepository<Borrow>, IBorrowRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BorrowRepository"/> class.
        /// </summary>
        /// <param name="context">A database context.</param>
        public BorrowRepository(MyContext context)
            : base(context)
        {
        }

        /// <inheritdoc/>
        public IEnumerable<Borrow> GetAllActive()
        {
            return this.dbContext.Set<Borrow>().Where(b => b.Active == true).ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<Borrow> GetAllActiveOfUserInLastDays(int userId, int days, DateTime targetDate)
        {
            DateTime targetDueDate = targetDate.AddDays(days);
            return this.dbContext.Set<Borrow>().Where(b => b.Active == true && b.Borrower.Id == userId &&
                                               b.StartDate <= targetDate && b.DueDate >= targetDueDate).ToList();
        }

        /// <inheritdoc/>
        public Borrow GetLastBorrowOfUserContainingBook(int userId, int bookId)
        {
            IEnumerable<Borrow> borrows = this.dbContext.Set<Borrow>().Where(borr => borr.Borrower.Id == userId &&
                                                                      borr.Books.Any(book => book.Id == bookId)).ToList();

            return borrows.Where(bor => bor.DueDate == borrows.Max(borr => borr.DueDate)).SingleOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<Borrow> GetAllActiveBorrowsContainingBook(int bookId)
        {
                return this.dbContext.Set<Borrow>().Where(borr => borr.Active == true &&
                                               borr.Books.Any(book => book.Id == bookId)).ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<Borrow> GetAllBorrowsOfBorrowerInADay(int userID, DateTime referenceTime)
        {
            DateTime referenceDueTime = referenceTime.AddDays(1);
            return this.dbContext.Set<Borrow>().Where(borrow => borrow.Borrower.Id == userID &&
                                           borrow.StartDate >= referenceTime &&
                                           borrow.StartDate < referenceDueTime).ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<Borrow> GetAllBorrowsOfLenderInADay(int userID, DateTime referenceTime)
        {
            DateTime referenceDueTime = referenceTime.AddDays(1);
            return this.dbContext.Set<Borrow>().Where(borrow => borrow.Lender.Id == userID &&
                                               borrow.StartDate >= referenceTime &&
                                               borrow.StartDate < referenceDueTime).ToList();
        }

        /// <inheritdoc/>
        public Borrow GetByInfo(Borrow borrow)
        {
            return this.dbContext.Set<Borrow>().Where(borr => borr.Lender.Id == borrow.Lender.Id &&
                                                      borr.Borrower.Id == borrow.Borrower.Id &&
                                                      borr.StartDate == borrow.StartDate &&
                                                      borr.DueDate == borrow.DueDate &&
                                                      borr.Active == borrow.Active).FirstOrDefault();
        }

        /// <inheritdoc/>
        public int ChangeActiveWhenDueDateReached(DateTime referenceTime)
        {
            var setToChange = this.dbContext.Set<Borrow>().Where(borrow => borrow.DueDate < referenceTime &&
                                                                 borrow.Active == true);

            foreach (var borrow in setToChange)
            {
                borrow.Active = false;
                this.Update(borrow);
            }

            return setToChange.Count();
        }
    }
}
