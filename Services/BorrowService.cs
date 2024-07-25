// <copyright file="BorrowService.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Interfaces;
using DomainModel;
using Services.Interfaces;

namespace Services
{
    /// <summary>
    /// A service class designated to working with borrows.
    /// </summary>
    public class BorrowService : BaseService<Borrow, IBorrowRepository>, IBorrowService
    {
        private static readonly int NMC;        // The book count limit defined at PER.
        private static readonly int PER;        // A period in days that has a maximum borrow limit.
        private static readonly int DELTA;      // How many days must pass before a user can borrow the same book.
        private static readonly int NCZ;        // The number of books a user can borrow per day.
        private static readonly int PERSIMP;    // The number of books an employee can lend per day.

        static BorrowService()
        {
            NMC = int.Parse(System.Configuration.ConfigurationManager.AppSettings["NMC"]);
            PER = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PER"]);
            DELTA = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DELTA"]);
            NCZ = int.Parse(System.Configuration.ConfigurationManager.AppSettings["NCZ"]);
            PERSIMP = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PERSIMP"]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BorrowService"/> class.
        /// </summary>
        /// <param name="repository">The repository object.</param>
        public BorrowService(IBorrowRepository repository)
            : base(repository)
        {
        }

        /// <inheritdoc/>
        public Borrow GetByInfo(Borrow borrow)
        {
            var context = new ValidationContext(borrow);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(borrow, context, results, true))
            {
                Log.Warn("Can't get borrow by information because: " + results.First().ErrorMessage);
                return null;
            }

            return this.repository.GetByInfo(borrow);
        }

        /// <inheritdoc/>
        public ValidationResult InsertCheck(Borrow borrow, DateTime? referenceTime = null)
        {
            var result = this.VerifyBorrow(borrow, referenceTime);

            if (result != ValidationResult.Success)
            {
                return result;
            }

            return this.Insert(borrow);
        }

        /// <inheritdoc/>
        public ValidationResult VerifyBorrow(Borrow borrow, DateTime? referenceTime = null)
        {
            if (borrow.Borrower == null || borrow.Lender == null)
            {
                return new ValidationResult("Borrower or lender can't be null.");
            }

            int booksLeft;
            int period = PER / 2;
            int booksPerPeriod = NMC * 2;

            if (!borrow.Borrower.IsPersonnel)
            {
                period = PER;
                booksPerPeriod = NMC;

                booksLeft = NCZ - this.VerifyBorrowerDailyLimit(borrow.Borrower, referenceTime) - borrow.Books.Count();
                if (booksLeft < 0)
                {
                    return new ValidationResult("User borrowed too many books today.\n" +
                                                (borrow.Books.Count() + booksLeft).ToString() + " book(s) can be borrowed today.");
                }
            }

            booksLeft = PERSIMP - this.VerifyLenderDailyLimit(borrow.Lender, referenceTime) - borrow.Books.Count();
            if (booksLeft < 0)
            {
                return new ValidationResult("Employe lended too many books today.\n" +
                                            (borrow.Books.Count() + booksLeft).ToString() + " book(s) can be lended today.");
            }

            booksLeft = booksPerPeriod - this.GetBooksBorrowedInLastDays(borrow.Borrower, period, referenceTime);
            if (borrow.Books.Count() > booksLeft)
            {
                return new ValidationResult("Too many books borrowed in the last " + PER.ToString() + " days.\n" +
                                            "The numbers of books left to borrow: " + booksLeft.ToString());
            }

            foreach (var book in borrow.Books)
            {
                if (!this.ValidateBookDelta(borrow.Borrower, book, referenceTime))
                {
                    return new ValidationResult("User borrowed this book too often: " + book.Name);
                }

                if (!this.BookCountBorrowable(book))
                {
                    return new ValidationResult("Not enough coppies left to borrow: " + book.Name);
                }
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Get total borrowed book count in the last days from current day.
        /// These borrows are for a certain user.
        /// </summary>
        /// <param name="user">A user.</param>
        /// <param name="days">The number of days to be subtracted from referenceTime.</param>
        /// <param name="referenceTime">The reference time. It is the current time, by default.</param>
        /// <returns>The numbers of books borrowed.</returns>
        private int GetBooksBorrowedInLastDays(Person user, int days, DateTime? referenceTime = null)
        {
            if (referenceTime == null)
            {
                referenceTime = DateTime.Now;
            }

            int count = 0;

            var borrows = this.repository.GetAllActiveOfUserInLastDays(user.Id, days, (DateTime)referenceTime);

            foreach (var borrow in borrows)
            {
                count += borrow.Books.Count();
            }

            return count;
        }

        /// <summary>
        /// Checks if a user wants to borrow a book that does not have delta expired.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="book">Target book.</param>
        /// <param name="referenceTime">The reference time. It is the current time, by default.</param>
        /// <returns>True if the user can borrow the book.</returns>
        private bool ValidateBookDelta(Person user, Book book, DateTime? referenceTime = null)
        {
            if (referenceTime == null)
            {
                referenceTime = DateTime.Now;
            }

            int deltaLimit = DELTA;
            if (user.IsPersonnel)
            {
                deltaLimit /= 2;
            }

            var lastBorrow = this.repository.GetLastBorrowOfUserContainingBook(user.Id, book.Id);

            if (lastBorrow == null)
            {
                return true;
            }

            TimeSpan delta = (DateTime)referenceTime - lastBorrow.DueDate;

            return delta.Days >= deltaLimit;
        }

        /// <summary>
        /// Checks if the current borrowable book has enough copies left to be borrowed.
        /// </summary>
        /// <param name="book">The book to be checked.</param>
        /// <param name="procentageLeftUnborrowable">
        /// This procentage is taken from the total copies of the book.
        /// The result determin how many books are left in the library to make them unborrowable.
        /// By default it's 10%.
        /// </param>
        /// <returns>True if book can be borrowed.</returns>
        private bool BookCountBorrowable(Book book, float procentageLeftUnborrowable = 0.1f)
        {
            var borrows = this.repository.GetAllActiveBorrowsContainingBook(book.Id);

            int copiesBorrowerdCount = borrows.Count();

            return book.TotalCopies - copiesBorrowerdCount > book.TotalCopies * procentageLeftUnborrowable;
        }

        /// <summary>
        /// Provides the number of books a user can borrow in a certain day.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="referenceTime">
        /// The day to be verified.
        /// Uses the current time by default.
        /// </param>
        /// <returns>The number of books that the user can borrow.</returns>
        private int VerifyBorrowerDailyLimit(Person user, DateTime? referenceTime = null)
        {
            if (referenceTime == null)
            {
                referenceTime = this.GetCurrentTime();
            }

            referenceTime = new DateTime(referenceTime.Value.Year, referenceTime.Value.Month, referenceTime.Value.Day);

            var borrows = this.repository.GetAllBorrowsOfBorrowerInADay(user.Id, (DateTime)referenceTime);

            int bookCount = 0;

            foreach (var borrow in borrows)
            {
                bookCount += borrow.Books.Count();
            }

            return bookCount;
        }

        /// <summary>
        /// Provides the number of books an employee can lend in a certain day.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="referenceTime">
        /// The day to be verified.
        /// Uses the current time by default.
        /// </param>
        /// <returns>The number of books that the employee can lend.</returns>
        private int VerifyLenderDailyLimit(Person user, DateTime? referenceTime = null)
        {
            if (referenceTime == null)
            {
                referenceTime = this.GetCurrentTime();
            }

            referenceTime = new DateTime(referenceTime.Value.Year, referenceTime.Value.Month, referenceTime.Value.Day);

            var borrows = this.repository.GetAllBorrowsOfLenderInADay(user.Id, (DateTime)referenceTime);

            int bookCount = 0;

            foreach (var borrow in borrows)
            {
                bookCount += borrow.Books.Count();
            }

            return bookCount;
        }
    }
}
