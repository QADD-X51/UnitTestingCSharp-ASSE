// <copyright file="BorrowTests.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestDomainModel
{
    /// <summary>
    /// A class to test borrowing a book.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BorrowTests
    {
        private Borrow borrow;
        private Book book;
        private BookField parentField;
        private BookField childField;

        /// <summary>
        /// A set-up method.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            Person lender = new Person()
            {
                Id = 0,
                Name = "Kodyack",
                Surname = "Ana",
                Email = "ultracrunch822@yahoo.com",
                Phone = "1234567890",
                IsPersonnel = true,
            };

            Person borrower = new Person()
            {
                Id = 1,
                Name = "Wider",
                Surname = "Toby",
                Email = "TobY299@gmail.com",
                Phone = "1234567890",
                IsPersonnel = false,
            };

            this.parentField = new BookField
            {
                Id = 1,
                Name = "Science",
                ParentField = null,
            };

            this.childField = new BookField
            {
                Id = 2,
                Name = "Mathematics",
                ParentField = this.parentField,
            };

            this.book = new Book
            {
                Id = 0,
                Name = "A Cool Book",
                Authors = new List<Author>
                {
                    new Author { Id = 0, Name = "Eu Nutu" },
                    new Author { Id = 1, Name = "Relu Dorelu" },
                },
                PagesCount = 51,
                Fields = new List<BookField> { this.childField },
                Borrowable = true,
                TotalCopies = 15,
            };

            this.borrow = new Borrow()
            {
                Id = 0,
                Lender = lender,
                Borrower = borrower,
                Books = new List<Book> { this.book },
                StartDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(15),
                Active = true,
            };
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestCorrectBorrow()
        {
            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNoBorrowerBorrow()
        {
            this.borrow.Borrower = null;

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNoLenderBorrow()
        {
            this.borrow.Lender = null;

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNotActiveBorrow()
        {
            this.borrow.Active = false;

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestLenderIsNotEmployeeBorrow()
        {
            this.borrow.Lender.IsPersonnel = false;

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestDuplicateBookBorrow()
        {
            this.borrow.Books.Add(this.book);

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestAtLeast2CorrectBooksBorrow()
        {
            Book newBook = new Book()
            {
                Id = 51,
                Name = "A Cool Book",
                Authors = new List<Author>
                {
                    new Author { Id = 0, Name = "Eu Nutu" },
                    new Author { Id = 1, Name = "Relu Dorelu" },
                },
                PagesCount = 51,
                Fields = new List<BookField> { this.childField },
                Borrowable = true,
                TotalCopies = 10,
            };
            this.borrow.Books.Add(newBook);

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestStartBiggerThanDueDateBorrow()
        {
            this.borrow.StartDate = this.borrow.DueDate.AddDays(5);

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestStartEqualWithDueDateBorrow()
        {
            this.borrow.StartDate = this.borrow.DueDate;

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestOneNonBorrowableBookBorrow()
        {
            this.borrow.Books = new List<Book>();
            var newBook = this.book;
            newBook.Borrowable = false;
            newBook.Id = 51;
            this.borrow.Books.Add(newBook);

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestOneNonBorrowableBookInMultipleBorrow()
        {
            var newBook = this.book;
            newBook.Borrowable = false;
            newBook.Id = 51;
            this.borrow.Books.Add(newBook);

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestOneZeroCopiesBookBorrow()
        {
            this.borrow.Books = new List<Book>();
            var newBook = this.book;
            newBook.Borrowable = false;
            newBook.Id = 51;
            newBook.TotalCopies = 0;
            this.borrow.Books.Add(newBook);

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestOneZeroCopiesBookInMultipleBorrow()
        {
            var newBook = this.book;
            newBook.Borrowable = false;
            newBook.Id = 51;
            newBook.TotalCopies = 0;
            this.borrow.Books.Add(newBook);

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestBadNameBorrowerBorrow()
        {
            this.borrow.Borrower.Name = "a";

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestBadNameBorrowerAndLenderBorrow()
        {
            this.borrow.Borrower.Name = "a";
            this.borrow.Lender.Name = "a";

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestBadNameLenderBorrow()
        {
            this.borrow.Lender.Name = "a";

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestBadEmailBorrowerBorrow()
        {
            this.borrow.Borrower.Email = "a";

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestBadEmailLenderBorrow()
        {
            this.borrow.Lender.Email = "a";

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestBadEmailBorrowerAndLenderBorrow()
        {
            this.borrow.Borrower.Email = "a";
            this.borrow.Lender.Email = "a";

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestOneBadBookNameAndOneGoodBookBorrow()
        {
            var newBook = this.book;
            newBook.Id = 51;
            newBook.Name = "a";
            this.borrow.Books.Add(newBook);

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestOnlyOneBadBookNameBorrow()
        {
            var newBook = this.book;
            newBook.Id = 51;
            newBook.Name = "a";
            this.borrow.Books = new List<Book>();
            this.borrow.Books.Add(newBook);

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNoBooksBorrow()
        {
            this.borrow.Books = new List<Book>();

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNullBooksBorrow()
        {
            this.borrow.Books = null;

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestOneNegativePageCountBookAndOneGoodBookBorrow()
        {
            var newBook = this.book;
            newBook.Id = 51;
            newBook.PagesCount = -1;
            this.borrow.Books.Add(newBook);

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestOnlyOneNegativePageCountBookBorrow()
        {
            var newBook = this.book;
            newBook.Id = 51;
            newBook.PagesCount = -1;
            this.borrow.Books = new List<Book>();
            this.borrow.Books.Add(newBook);

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestJustEnoughOkBooksBorrow()
        {
            int maxBookCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["C"]);

            for (int count = 1; count < maxBookCount; ++count)
            {
                BookField newField = new BookField()
                {
                    Id = 1000 + count,
                    Name = "Base Field",
                    ParentField = null,
                };
                Book newBook = new Book()
                {
                    Id = 1000 + count,
                    Name = "A Cool Book",
                    Authors = new List<Author>
                    {
                        new Author { Id = 0, Name = "Eu Nutu" },
                        new Author { Id = 1, Name = "Relu Dorelu" },
                    },
                    PagesCount = 51,
                    Fields = new List<BookField>() { newField },
                    Borrowable = true,
                    TotalCopies = 15,
                };
                this.borrow.Books.Add(newBook);
            }

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestOverLimitOkBooksBorrow()
        {
            int maxBookCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["C"]);

            for (int count = 1; count < maxBookCount + 1; ++count)
            {
                BookField newField = new BookField()
                {
                    Id = 1000 + count,
                    Name = "Base Field",
                    ParentField = null,
                };
                Book newBook = new Book()
                {
                    Id = 1000 + count,
                    Name = "A Cool Book",
                    Authors = new List<Author>
                    {
                        new Author { Id = 0, Name = "Eu Nutu" },
                        new Author { Id = 1, Name = "Relu Dorelu" },
                    },
                    PagesCount = 51,
                    Fields = new List<BookField>() { newField },
                    Borrowable = true,
                    TotalCopies = 15,
                };
                this.borrow.Books.Add(newBook);
            }

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestJustEnoughOkBooksEmployeeBorrow()
        {
            int maxBookCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["C"]);

            this.borrow.Borrower.IsPersonnel = true;
            maxBookCount *= 2;

            for (int count = 1; count < maxBookCount; ++count)
            {
                BookField newField = new BookField()
                {
                    Id = 1000 + count,
                    Name = "Base Field",
                    ParentField = null,
                };
                Book newBook = new Book()
                {
                    Id = 1000 + count,
                    Name = "A Cool Book",
                    Authors = new List<Author>
                    {
                        new Author { Id = 0, Name = "Eu Nutu" },
                        new Author { Id = 1, Name = "Relu Dorelu" },
                    },
                    PagesCount = 51,
                    Fields = new List<BookField>() { newField },
                    Borrowable = true,
                    TotalCopies = 15,
                };
                this.borrow.Books.Add(newBook);
            }

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestOverLimitOkBooksEmployeeBorrow()
        {
            int maxBookCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["C"]);

            this.borrow.Borrower.IsPersonnel = true;
            maxBookCount *= 2;

            for (int count = 1; count < maxBookCount + 1; ++count)
            {
                BookField newField = new BookField()
                {
                    Id = 1000 + count,
                    Name = "Base Field",
                    ParentField = null,
                };
                Book newBook = new Book()
                {
                    Id = 1000 + count,
                    Name = "A Cool Book",
                    Authors = new List<Author>
                    {
                        new Author { Id = 0, Name = "Eu Nutu" },
                        new Author { Id = 1, Name = "Relu Dorelu" },
                    },
                    PagesCount = 51,
                    Fields = new List<BookField>() { newField },
                    Borrowable = true,
                    TotalCopies = 15,
                };
                this.borrow.Books.Add(newBook);
            }

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestJustEnoughOkBooksOfSameFieldBorrow()
        {
            int maxBookCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["C"]);

            this.borrow.Books.ElementAt<Book>(0).Fields = new List<BookField>() { this.parentField };

            for (int count = 1; count < maxBookCount; ++count)
            {
                Book newBook = new Book()
                {
                    Id = 1000 + count,
                    Name = "A Cool Book",
                    Authors = new List<Author>
                    {
                        new Author { Id = 0, Name = "Eu Nutu" },
                        new Author { Id = 1, Name = "Relu Dorelu" },
                    },
                    PagesCount = 51,
                    Fields = new List<BookField>() { this.parentField },
                    Borrowable = true,
                    TotalCopies = 15,
                };
                this.borrow.Books.Add(newBook);
            }

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestJustEnoughOkBooksOfSameRootFieldBorrow()
        {
            int maxBookCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["C"]);

            this.borrow.Books.ElementAt<Book>(0).Fields = new List<BookField>() { this.childField };

            for (int count = 1; count < maxBookCount; ++count)
            {
                BookField newField = new BookField()
                {
                    Id = 1000 + count,
                    Name = "Base Field",
                    ParentField = this.childField,
                };
                Book newBook = new Book()
                {
                    Id = 1000 + count,
                    Name = "A Cool Book",
                    Authors = new List<Author>
                    {
                        new Author { Id = 0, Name = "Eu Nutu" },
                        new Author { Id = 1, Name = "Relu Dorelu" },
                    },
                    PagesCount = 51,
                    Fields = new List<BookField>() { newField },
                    Borrowable = true,
                    TotalCopies = 15,
                };
                this.borrow.Books.Add(newBook);
            }

            var context = new ValidationContext(this.borrow, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.borrow, context, results, true));
        }
    }
}
