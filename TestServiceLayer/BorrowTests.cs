// <copyright file="BorrowTests.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataMapper;
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhino.Mocks;
using Services;

namespace TestServiceLayer
{
    /// <summary>
    /// A testing class for testing borrows in service layer.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BorrowTests
    {
        private BorrowService borrowService;
        private BookService bookService;
        private BookFieldService bookFieldService;
        private PersonService personService;

        private MyContext databaseContext;

        private Borrow defaultBorrow;
        private BookField defaultField;
        private Person defaultBorrower;
        private Person defaultLender;
        private Book defaultBook;

        /// <summary>
        /// A setup function that is called before every test.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.databaseContext = new MyContext();

            this.borrowService = new BorrowService(new BorrowRepository(this.databaseContext));
            this.bookService = new BookService(new BookRepository(this.databaseContext));
            this.bookFieldService = new BookFieldService(new BookFieldRepository(this.databaseContext));
            this.personService = new PersonService(new PersonRepository(this.databaseContext));

            this.borrowService.DeleteAll();
            this.bookService.DeleteAll();
            this.bookFieldService.DeleteAll();
            this.personService.DeleteAll();

            this.defaultBorrower = new Person
            {
                Name = "Relu",
                Surname = "Dorelu",
                Email = "borr@borr.borr",
                Phone = "1234567890",
                IsPersonnel = false,
            };
            this.defaultLender = new Person
            {
                Name = "Halebarda",
                Surname = "Ana",
                Email = "lend@lend.lend",
                Phone = "2234567890",
                IsPersonnel = true,
            };

            this.personService.Insert(this.defaultBorrower);
            this.defaultBorrower = this.personService.GetByEmail(this.defaultBorrower.Email);

            this.personService.Insert(this.defaultLender);
            this.defaultLender = this.personService.GetByEmail(this.defaultLender.Email);

            this.defaultField = new BookField
            {
                Name = "DeafultField",
                ParentField = null,
            };

            this.bookFieldService.Insert(this.defaultField);
            this.defaultField = this.bookFieldService.GetByName(this.defaultField.Name);

            this.defaultBook = new Book
            {
                Name = "A nice book",
                Authors = new List<Author> { },
                Fields = new List<BookField> { this.defaultField },
                Borrowable = true,
                TotalCopies = 15,
                PagesCount = 51,
            };

            this.bookService.Insert(this.defaultBook);
            this.defaultBook = this.bookService.GetByName(this.defaultBook.Name).First();

            this.defaultBorrow = new Borrow
            {
                Books = new List<Book> { this.defaultBook },
                Lender = this.defaultLender,
                Borrower = this.defaultBorrower,
                StartDate = new DateTime(2000, 4, 1),
                DueDate = new DateTime(2000, 5, 17),
                Active = true,
            };
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCorrectBorrow()
        {
            this.borrowService.Insert(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsTrue(result != null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestDeleteCorrectBorrow()
        {
            this.borrowService.Insert(this.defaultBorrow);

            this.defaultBorrow = this.borrowService.GetByInfo(this.defaultBorrow);

            this.borrowService.Delete(this.defaultBorrow);

            var result = this.borrowService.GetById(this.defaultBorrow.Id);

            Assert.IsTrue(result == null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestDeleteAllBorrow()
        {
            this.borrowService.Insert(this.defaultBorrow);
            this.defaultBorrow.StartDate = new DateTime(2001, 4, 1);
            this.defaultBorrow.DueDate = new DateTime(2001, 4, 1);

            this.borrowService.Insert(this.defaultBorrow);

            this.borrowService.DeleteAll();

            var result = this.borrowService.GetAll();

            Assert.IsTrue(result.Count() == 0);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetAllBorrow()
        {
            this.borrowService.Insert(this.defaultBorrow);
            this.defaultBorrow.StartDate = new DateTime(2001, 4, 1);
            this.defaultBorrow.DueDate = new DateTime(2001, 4, 1);

            this.borrowService.Insert(this.defaultBorrow);

            var result = this.borrowService.GetAll();

            Assert.IsTrue(result.Count() == 2);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetByIdBorrow()
        {
            this.borrowService.Insert(this.defaultBorrow);

            this.defaultBorrow = this.borrowService.GetByInfo(this.defaultBorrow);
            var result = this.borrowService.GetById(this.defaultBorrow.Id);

            Assert.IsTrue(this.defaultBorrow.Lender.Id == result.Lender.Id &&
                          this.defaultBorrow.Borrower.Id == result.Borrower.Id &&
                          this.defaultBorrow.StartDate == result.StartDate &&
                          this.defaultBorrow.DueDate == result.DueDate &&
                          this.defaultBorrow.Active == result.Active);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetByInfoBorrow()
        {
            this.borrowService.Insert(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsTrue(this.defaultBorrow.Lender.Id == result.Lender.Id &&
                          this.defaultBorrow.Borrower.Id == result.Borrower.Id &&
                          this.defaultBorrow.StartDate == result.StartDate &&
                          this.defaultBorrow.DueDate == result.DueDate &&
                          this.defaultBorrow.Active == result.Active);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckCorrectBorrow()
        {
            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsTrue(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckInvalidBorrow()
        {
            this.defaultBorrow.DueDate = this.defaultBorrow.StartDate.AddDays(-1);

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckBookCountLimitBorrow()
        {
            int maximumBookBorrowLimit = int.Parse(System.Configuration.ConfigurationManager.AppSettings["C"]);

            var newBookField = new BookField
            {
                Name = "AnotherField",
                ParentField = null,
            };
            this.bookFieldService.Insert(newBookField);
            newBookField = this.bookFieldService.GetByName(newBookField.Name);

            for (int index = 0; index < maximumBookBorrowLimit; ++index)
            {
                var newBook = new Book
                {
                    Name = "A nice book " + index.ToString(),
                    Authors = new List<Author> { },
                    Fields = new List<BookField> { newBookField },
                    Borrowable = true,
                    TotalCopies = 15,
                    PagesCount = index + 1,
                };
                this.bookService.Insert(newBook);
                newBook = this.bookService.GetByName(newBook.Name).First();

                this.defaultBorrow.Books.Add(newBook);
            }

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckBookFieldLimitCorrectBorrow()
        {
            var newBookField = new BookField
            {
                Name = "AnotherField",
                ParentField = null,
            };
            this.bookFieldService.Insert(newBookField);
            newBookField = this.bookFieldService.GetByName(newBookField.Name);

            for (int index = 0; index < 2; ++index)
            {
                var newBook = new Book
                {
                    Name = "A nice book " + index.ToString(),
                    Authors = new List<Author> { },
                    Fields = new List<BookField> { newBookField },
                    Borrowable = true,
                    TotalCopies = 15,
                    PagesCount = index + 1,
                };
                this.bookService.Insert(newBook);
                newBook = this.bookService.GetByName(newBook.Name).First();

                this.defaultBorrow.Books.Add(newBook);
            }

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsTrue(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckBookFieldLimitAllSameBorrow()
        {
            for (int index = 0; index < 2; ++index)
            {
                var newBook = new Book
                {
                    Name = "A nice book " + index.ToString(),
                    Authors = new List<Author> { },
                    Fields = new List<BookField> { this.defaultField },
                    Borrowable = true,
                    TotalCopies = 15,
                    PagesCount = index + 1,
                };
                this.bookService.Insert(newBook);
                newBook = this.bookService.GetByName(newBook.Name).First();

                this.defaultBorrow.Books.Add(newBook);
            }

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckBorrowerDailyLimitReachedBorrow()
        {
            int dailyLimit = int.Parse(System.Configuration.ConfigurationManager.AppSettings["NCZ"]);

            for (int index = 0; index < dailyLimit; ++index)
            {
                var newField = new BookField
                {
                    Name = "DeafultField " + index.ToString(),
                    ParentField = null,
                };

                this.bookFieldService.Insert(newField);
                newField = this.bookFieldService.GetByName(newField.Name);

                var newBook = new Book
                {
                    Name = "A nice book " + index.ToString(),
                    Authors = new List<Author> { },
                    Fields = new List<BookField> { newField },
                    Borrowable = true,
                    TotalCopies = 15,
                    PagesCount = index + 1,
                };

                this.bookService.Insert(newBook);
                newBook = this.bookService.GetByName(newBook.Name).First();

                var newBorrow = new Borrow
                {
                    Books = new List<Book> { newBook },
                    Lender = this.defaultLender,
                    Borrower = this.defaultBorrower,
                    StartDate = new DateTime(2000, 4, 1).AddMilliseconds(index + 1),
                    DueDate = new DateTime(2000, 5, 17).AddMilliseconds(index + 1),
                    Active = true,
                };

                this.borrowService.Insert(newBorrow);
            }

            var insert = this.borrowService.InsertCheck(this.defaultBorrow, new DateTime(2000, 4, 1, 1, 1, 1));

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckBorrowerPersonnelDailyLimitReachedBorrow()
        {
            int dailyLimit = int.Parse(System.Configuration.ConfigurationManager.AppSettings["NCZ"]);

            this.defaultBorrower.IsPersonnel = true;
            this.personService.Update(this.defaultBorrower);
            this.defaultBorrower = this.personService.GetByEmail(this.defaultBorrower.Email);

            for (int index = 0; index < dailyLimit; ++index)
            {
                var newField = new BookField
                {
                    Name = "DeafultField " + index.ToString(),
                    ParentField = null,
                };

                this.bookFieldService.Insert(newField);
                newField = this.bookFieldService.GetByName(newField.Name);

                var newBook = new Book
                {
                    Name = "A nice book " + index.ToString(),
                    Authors = new List<Author> { },
                    Fields = new List<BookField> { newField },
                    Borrowable = true,
                    TotalCopies = 15,
                    PagesCount = index + 1,
                };

                this.bookService.Insert(newBook);
                newBook = this.bookService.GetByName(newBook.Name).First();

                var newBorrow = new Borrow
                {
                    Books = new List<Book> { newBook },
                    Lender = this.defaultLender,
                    Borrower = this.defaultBorrower,
                    StartDate = new DateTime(2000, 4, 1).AddMilliseconds(index + 1),
                    DueDate = new DateTime(2000, 5, 17).AddMilliseconds(index + 1),
                    Active = true,
                };

                this.borrowService.Insert(newBorrow);
            }

            var insert = this.borrowService.InsertCheck(this.defaultBorrow, new DateTime(2000, 4, 1, 1, 1, 1));

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsTrue(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckBorrowerDailyLimitBarelyReachedBorrow()
        {
            int dailyLimit = int.Parse(System.Configuration.ConfigurationManager.AppSettings["NCZ"]);

            for (int index = 0; index < dailyLimit - 1; ++index)
            {
                var newField = new BookField
                {
                    Name = "DeafultField " + index.ToString(),
                    ParentField = null,
                };

                this.bookFieldService.Insert(newField);
                newField = this.bookFieldService.GetByName(newField.Name);

                var newBook = new Book
                {
                    Name = "A nice book " + index.ToString(),
                    Authors = new List<Author> { },
                    Fields = new List<BookField> { newField },
                    Borrowable = true,
                    TotalCopies = 15,
                    PagesCount = index + 1,
                };

                this.bookService.Insert(newBook);
                newBook = this.bookService.GetByName(newBook.Name).First();

                var newBorrow = new Borrow
                {
                    Books = new List<Book> { newBook },
                    Lender = this.defaultLender,
                    Borrower = this.defaultBorrower,
                    StartDate = new DateTime(2000, 4, 1).AddMilliseconds(index + 1),
                    DueDate = new DateTime(2000, 5, 17).AddMilliseconds(index + 1),
                    Active = true,
                };

                this.borrowService.Insert(newBorrow);
            }

            var insert = this.borrowService.InsertCheck(this.defaultBorrow, new DateTime(2000, 4, 1, 1, 1, 1));

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsTrue(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckLenderDailyLimitReachedBorrow()
        {
            int dailyLimit = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PERSIMP"]);

            for (int index = 0; index < dailyLimit; ++index)
            {
                var newField = new BookField
                {
                    Name = "DeafultField " + index.ToString(),
                    ParentField = null,
                };

                this.bookFieldService.Insert(newField);
                newField = this.bookFieldService.GetByName(newField.Name);

                var newBook = new Book
                {
                    Name = "A nice book " + index.ToString(),
                    Authors = new List<Author> { },
                    Fields = new List<BookField> { newField },
                    Borrowable = true,
                    TotalCopies = 10,
                    PagesCount = index + 1,
                };

                this.bookService.Insert(newBook);
                newBook = this.bookService.GetByName(newBook.Name).First();

                var newBorrower = new Person
                {
                    Name = "NPC",
                    Surname = "#" + index.ToString(),
                    Email = "npc" + index.ToString() + "@npc.npc",
                    Phone = "1234567890",
                    IsPersonnel = false,
                };

                this.personService.Insert(newBorrower);
                newBorrower = this.personService.GetByEmail(newBorrower.Email);

                var newBorrow = new Borrow
                {
                    Books = new List<Book> { newBook },
                    Lender = this.defaultLender,
                    Borrower = newBorrower,
                    StartDate = new DateTime(2000, 4, 1).AddMilliseconds(index + 1),
                    DueDate = new DateTime(2000, 5, 17).AddMilliseconds(index + 1),
                    Active = true,
                };

                this.borrowService.Insert(newBorrow);
            }

            var insert = this.borrowService.InsertCheck(this.defaultBorrow, new DateTime(2000, 4, 1, 1, 1, 1));

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckLenderDailyLimitBarelyReachedBorrow()
        {
            int dailyLimit = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PERSIMP"]);

            for (int index = 0; index < dailyLimit - 1; ++index)
            {
                var newField = new BookField
                {
                    Name = "DeafultField " + index.ToString(),
                    ParentField = null,
                };

                this.bookFieldService.Insert(newField);
                newField = this.bookFieldService.GetByName(newField.Name);

                var newBook = new Book
                {
                    Name = "A nice book " + index.ToString(),
                    Authors = new List<Author> { },
                    Fields = new List<BookField> { newField },
                    Borrowable = true,
                    TotalCopies = 10,
                    PagesCount = index + 1,
                };

                this.bookService.Insert(newBook);
                newBook = this.bookService.GetByName(newBook.Name).First();

                var newBorrower = new Person
                {
                    Name = "NPC",
                    Surname = "#" + index.ToString(),
                    Email = "npc" + index.ToString() + "@npc.npc",
                    Phone = "1234567890",
                    IsPersonnel = false,
                };

                this.personService.Insert(newBorrower);
                newBorrower = this.personService.GetByEmail(newBorrower.Email);

                var newBorrow = new Borrow
                {
                    Books = new List<Book> { newBook },
                    Lender = this.defaultLender,
                    Borrower = newBorrower,
                    StartDate = new DateTime(2000, 4, 1).AddMilliseconds(index + 1),
                    DueDate = new DateTime(2000, 5, 17).AddMilliseconds(index + 1),
                    Active = true,
                };

                this.borrowService.Insert(newBorrow);
            }

            var insert = this.borrowService.InsertCheck(this.defaultBorrow, new DateTime(2000, 4, 1, 1, 1, 1));

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsTrue(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckContainsUnborrowableBookBorrow()
        {
            var newBook = new Book
            {
                Name = "A nice book you can't borrow.",
                Authors = new List<Author> { },
                Fields = new List<BookField> { this.defaultField },
                Borrowable = false,
                TotalCopies = 15,
                PagesCount = 1,
            };

            this.defaultBorrow.Books.Add(newBook);

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckNoBooksBorrow()
        {
            this.defaultBorrow.Books = new List<Book>();

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckDupicateBooksBorrow()
        {
            this.defaultBorrow.Books.Add(this.defaultBook);

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckEqualBooksDeltaBorrow()
        {
            int delta = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DELTA"]);

            var newBorrow = new Borrow
            {
                Books = new List<Book> { this.defaultBook },
                Lender = this.defaultLender,
                Borrower = this.defaultBorrower,
                StartDate = new DateTime(2000, 5, 17).AddDays(delta),
                DueDate = new DateTime(2000, 6, 6).AddDays(delta),
                Active = true,
            };

            this.borrowService.InsertCheck(this.defaultBorrow);
            var insert = this.borrowService.InsertCheck(newBorrow, newBorrow.StartDate);

            var result = this.borrowService.GetByInfo(newBorrow);

            Assert.IsTrue(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckOverBooksDeltaBorrow()
        {
            int delta = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DELTA"]);

            var newBorrow = new Borrow
            {
                Books = new List<Book> { this.defaultBook },
                Lender = this.defaultLender,
                Borrower = this.defaultBorrower,
                StartDate = new DateTime(2000, 5, 17).AddDays(delta + 1),
                DueDate = new DateTime(2000, 6, 6).AddDays(delta + 1),
                Active = true,
            };

            this.borrowService.InsertCheck(this.defaultBorrow);
            var insert = this.borrowService.InsertCheck(newBorrow, newBorrow.StartDate);

            var result = this.borrowService.GetByInfo(newBorrow);

            Assert.IsTrue(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckUnderBooksDeltaBorrow()
        {
            int delta = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DELTA"]);

            var newBorrow = new Borrow
            {
                Books = new List<Book> { this.defaultBook },
                Lender = this.defaultLender,
                Borrower = this.defaultBorrower,
                StartDate = new DateTime(2000, 5, 17).AddDays(delta - 1),
                DueDate = new DateTime(2000, 6, 6).AddDays(delta - 1),
                Active = true,
            };

            this.borrowService.InsertCheck(this.defaultBorrow);
            var insert = this.borrowService.InsertCheck(newBorrow, newBorrow.StartDate);

            var result = this.borrowService.GetByInfo(newBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckEqualBooksDeltaPersonnelBorrow()
        {
            int delta = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DELTA"]);

            this.defaultBorrower.IsPersonnel = true;
            this.personService.Update(this.defaultBorrower);
            this.defaultBorrower = this.personService.GetByEmail(this.defaultBorrower.Email);

            var newBorrow = new Borrow
            {
                Books = new List<Book> { this.defaultBook },
                Lender = this.defaultLender,
                Borrower = this.defaultBorrower,
                StartDate = new DateTime(2000, 5, 17).AddDays(delta),
                DueDate = new DateTime(2000, 6, 6).AddDays(delta),
                Active = true,
            };

            this.borrowService.InsertCheck(this.defaultBorrow);
            var insert = this.borrowService.InsertCheck(newBorrow, newBorrow.StartDate);

            var result = this.borrowService.GetByInfo(newBorrow);

            Assert.IsTrue(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckOverBooksDeltaPersonnelBorrow()
        {
            int delta = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DELTA"]);

            this.defaultBorrower.IsPersonnel = true;
            this.personService.Update(this.defaultBorrower);
            this.defaultBorrower = this.personService.GetByEmail(this.defaultBorrower.Email);

            var newBorrow = new Borrow
            {
                Books = new List<Book> { this.defaultBook },
                Lender = this.defaultLender,
                Borrower = this.defaultBorrower,
                StartDate = new DateTime(2000, 5, 17).AddDays(delta + 1),
                DueDate = new DateTime(2000, 6, 6).AddDays(delta + 1),
                Active = true,
            };

            this.borrowService.InsertCheck(this.defaultBorrow);
            var insert = this.borrowService.InsertCheck(newBorrow, newBorrow.StartDate);

            var result = this.borrowService.GetByInfo(newBorrow);

            Assert.IsTrue(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckUnderBooksDeltaPersonnelBorrow()
        {
            int delta = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DELTA"]);

            this.defaultBorrower.IsPersonnel = true;
            this.personService.Update(this.defaultBorrower);
            this.defaultBorrower = this.personService.GetByEmail(this.defaultBorrower.Email);

            var newBorrow = new Borrow
            {
                Books = new List<Book> { this.defaultBook },
                Lender = this.defaultLender,
                Borrower = this.defaultBorrower,
                StartDate = new DateTime(2000, 5, 17).AddDays(delta - 1),
                DueDate = new DateTime(2000, 6, 6).AddDays(delta - 1),
                Active = true,
            };

            this.borrowService.InsertCheck(this.defaultBorrow);
            var insert = this.borrowService.InsertCheck(newBorrow, newBorrow.StartDate);

            var result = this.borrowService.GetByInfo(newBorrow);

            Assert.IsTrue(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckEqualBooksHalfDeltaPersonnelBorrow()
        {
            int delta = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DELTA"]) / 2;

            this.defaultBorrower.IsPersonnel = true;
            this.personService.Update(this.defaultBorrower);
            this.defaultBorrower = this.personService.GetByEmail(this.defaultBorrower.Email);

            var newBorrow = new Borrow
            {
                Books = new List<Book> { this.defaultBook },
                Lender = this.defaultLender,
                Borrower = this.defaultBorrower,
                StartDate = new DateTime(2000, 5, 17).AddDays(delta),
                DueDate = new DateTime(2000, 6, 6).AddDays(delta),
                Active = true,
            };

            this.borrowService.InsertCheck(this.defaultBorrow);
            var insert = this.borrowService.InsertCheck(newBorrow, newBorrow.StartDate);

            var result = this.borrowService.GetByInfo(newBorrow);

            Assert.IsTrue(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckOverBooksHalfDeltaPersonnelBorrow()
        {
            int delta = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DELTA"]) / 2;

            this.defaultBorrower.IsPersonnel = true;
            this.personService.Update(this.defaultBorrower);
            this.defaultBorrower = this.personService.GetByEmail(this.defaultBorrower.Email);

            var newBorrow = new Borrow
            {
                Books = new List<Book> { this.defaultBook },
                Lender = this.defaultLender,
                Borrower = this.defaultBorrower,
                StartDate = new DateTime(2000, 5, 17).AddDays(delta + 1),
                DueDate = new DateTime(2000, 6, 6).AddDays(delta + 1),
                Active = true,
            };

            this.borrowService.InsertCheck(this.defaultBorrow);
            var insert = this.borrowService.InsertCheck(newBorrow, newBorrow.StartDate);

            var result = this.borrowService.GetByInfo(newBorrow);

            Assert.IsTrue(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckUnderBooksHalfDeltaPersonnelBorrow()
        {
            int delta = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DELTA"]) / 2;

            this.defaultBorrower.IsPersonnel = true;
            this.personService.Update(this.defaultBorrower);
            this.defaultBorrower = this.personService.GetByEmail(this.defaultBorrower.Email);

            var newBorrow = new Borrow
            {
                Books = new List<Book> { this.defaultBook },
                Lender = this.defaultLender,
                Borrower = this.defaultBorrower,
                StartDate = new DateTime(2000, 5, 17).AddDays(delta - 1),
                DueDate = new DateTime(2000, 6, 6).AddDays(delta - 1),
                Active = true,
            };

            this.borrowService.InsertCheck(this.defaultBorrow);
            var insert = this.borrowService.InsertCheck(newBorrow, newBorrow.StartDate);

            var result = this.borrowService.GetByInfo(newBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckNullBorrowerBorrow()
        {
            this.defaultBorrow.Borrower = new Person();
            this.defaultBorrow.Borrower = null;

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckNullLenderBorrow()
        {
            this.defaultBorrow.Lender = new Person();
            this.defaultBorrow.Lender = null;

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckBorrowerWrongEmailBorrow()
        {
            var newPerson = new Person
            {
                Name = "NPC",
                Surname = "Number2",
                Email = "a@a",
                IsPersonnel = false,
                Phone = "0987655321",
            };

            this.defaultBorrow.Borrower = newPerson;

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckLenderWrongEmailBorrow()
        {
            var newPerson = new Person
            {
                Name = "NPC",
                Surname = "Number2",
                Email = "a.a",
                IsPersonnel = true,
                Phone = "0987655321",
            };

            this.defaultBorrow.Lender = newPerson;

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckLenderNotEmployeeBorrow()
        {
            var newPerson = new Person
            {
                Name = "NPC",
                Surname = "Number2",
                Email = "guy@a.a",
                IsPersonnel = false,
                Phone = "0987655321",
            };

            this.personService.Insert(newPerson);
            newPerson = this.personService.GetByEmail(newPerson.Email);

            this.defaultBorrow.Lender = newPerson;

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckThreeBooksSameFieldBorrow()
        {
            for (int index = 0; index < 2; ++index)
            {
                var newBook = new Book
                {
                    Name = "A nice boo " + index.ToString(),
                    Authors = new List<Author> { },
                    Fields = new List<BookField> { this.defaultField },
                    Borrowable = true,
                    TotalCopies = 15,
                    PagesCount = 5,
                };
                this.bookService.Insert(newBook);
                newBook = this.bookService.GetByName(newBook.Name).First();

                this.defaultBorrow.Books.Add(newBook);
            }

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckTwoBooksSameFieldBorrow()
        {
            for (int index = 0; index < 1; ++index)
            {
                var newBook = new Book
                {
                    Name = "A nice boo " + index.ToString(),
                    Authors = new List<Author> { },
                    Fields = new List<BookField> { this.defaultField },
                    Borrowable = true,
                    TotalCopies = 15,
                    PagesCount = 5,
                };
                this.bookService.Insert(newBook);
                newBook = this.bookService.GetByName(newBook.Name).First();

                this.defaultBorrow.Books.Add(newBook);
            }

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsTrue(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckNotActiveBorrow()
        {
            this.defaultBorrow.Active = false;

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckNotEnoughBooksLeftToBorrowBorrow()
        {
            this.defaultBook.TotalCopies = 10;
            this.bookService.Update(this.defaultBook);
            this.defaultBook = this.bookService.GetByName(this.defaultBook.Name).First();

            for (int index = 0; index < 9; ++index)
            {
                var newPerson = new Person
                {
                    Name = "NPC",
                    Surname = "Number" + index.ToString(),
                    Email = "npc" + index.ToString() + "@boot.ni",
                    Phone = "1234567890",
                    IsPersonnel = false,
                };
                this.personService.Insert(newPerson);
                newPerson = this.personService.GetByEmail(newPerson.Email);

                var newBorrow = new Borrow
                {
                    Borrower = newPerson,
                    Lender = this.defaultLender,
                    Books = new List<Book>() { this.defaultBook },
                    StartDate = this.defaultBorrow.StartDate,
                    DueDate = this.defaultBorrow.DueDate,
                    Active = true,
                };

                this.borrowService.Insert(newBorrow);
            }

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsFalse(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckJustEnoughBooksLeftToBorrowBorrow()
        {
            this.defaultBook.TotalCopies = 10;
            this.bookService.Update(this.defaultBook);
            this.defaultBook = this.bookService.GetByName(this.defaultBook.Name).First();

            for (int index = 0; index < 8; ++index)
            {
                var newPerson = new Person
                {
                    Name = "NPC",
                    Surname = "Number" + index.ToString(),
                    Email = "npc" + index.ToString() + "@boot.ni",
                    Phone = "1234567890",
                    IsPersonnel = false,
                };
                this.personService.Insert(newPerson);
                newPerson = this.personService.GetByEmail(newPerson.Email);

                var newBorrow = new Borrow
                {
                    Borrower = newPerson,
                    Lender = this.defaultLender,
                    Books = new List<Book>() { this.defaultBook },
                    StartDate = this.defaultBorrow.StartDate,
                    DueDate = this.defaultBorrow.DueDate,
                    Active = true,
                };

                this.borrowService.Insert(newBorrow);
            }

            var insert = this.borrowService.InsertCheck(this.defaultBorrow);

            var result = this.borrowService.GetByInfo(this.defaultBorrow);

            Assert.IsTrue(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A cleanup function that is called after every test.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            this.borrowService.DeleteAll();
            this.bookService.DeleteAll();
            this.bookFieldService.DeleteAll();
            this.personService.DeleteAll();

            this.databaseContext.SaveChanges();
            this.databaseContext.Dispose();
        }
    }
}
