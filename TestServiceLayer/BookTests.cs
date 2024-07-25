// <copyright file="BookTests.cs" company="Transilvania University of Brasov">
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
using Services;

namespace TestServiceLayer
{
    /// <summary>
    /// A testing class for service layer book testing.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BookTests
    {
        private BookService service;
        private BookFieldService bookFieldService;
        private AuthorService authorService;
        private MyContext databaseContext;
        private BookField defaultBookField;

        /// <summary>
        /// The set-up function that is called before every test.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.databaseContext = new MyContext();

            this.service = new BookService(new BookRepository(this.databaseContext));
            this.service.DeleteAll();

            this.authorService = new AuthorService(new AuthorRepository(this.databaseContext));
            this.authorService.DeleteAll();

            this.bookFieldService = new BookFieldService(new BookFieldRepository(this.databaseContext));
            this.bookFieldService.DeleteAll();

            this.defaultBookField = new BookField()
            {
                Name = "DeafaultBookFoeld",
                ParentField = null,
            };

            this.bookFieldService.Insert(this.defaultBookField);
            this.defaultBookField = this.bookFieldService.GetByName(this.defaultBookField.Name);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCorrectSimpleBook()
        {
            var newBook = new Book
            {
                Name = "A very nice book",
                Authors = new List<Author> { },
                PagesCount = 51,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            this.service.Insert(newBook);

            var result = this.service.GetByName(newBook.Name);

            Assert.IsTrue(result.Count() == 1);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCorrectComplexBook()
        {
            var newBookField = new BookField()
            {
                Name = "Nice Books",
                ParentField = null,
            };

            var newAuthor = new Author()
            {
                Name = "Sir David Attenborough",
            };

            this.authorService.Insert(newAuthor);
            this.bookFieldService.Insert(newBookField);

            newAuthor = this.authorService.GetByName(newAuthor.Name).First();
            newBookField = this.bookFieldService.GetByName(newBookField.Name);

            var newBook = new Book
            {
                Name = "A very nice book",
                Authors = new List<Author>() { newAuthor },
                PagesCount = 51,
                Fields = new List<BookField>() { this.defaultBookField, newBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            this.service.Insert(newBook);

            var result = this.service.GetByName(newBook.Name);

            Assert.IsTrue(result.Count() == 1 &&
                          result.ElementAt(0).Authors.ElementAt(0) == newAuthor &&
                          result.ElementAt(0).Fields.ElementAt(1) == newBookField);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestDeleteCorrectBook()
        {
            var newBook = new Book
            {
                Name = "A very nice book",
                Authors = new List<Author> { },
                PagesCount = 51,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            this.service.Insert(newBook);

            newBook = this.service.GetByName(newBook.Name).First();

            this.service.Delete(newBook);

            var result = this.service.GetById(newBook.Id);

            Assert.IsTrue(result == null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestDeleteAllBook()
        {
            var newBook = new Book
            {
                Name = "A very nice book",
                Authors = new List<Author> { },
                PagesCount = 51,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            this.service.Insert(newBook);
            newBook.Name = "An even nicer book";
            this.service.Insert(newBook);

            this.service.DeleteAll();

            var result = this.service.GetAll();

            Assert.IsTrue(result.Count() == 0);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetAllBook()
        {
            var newBook = new Book
            {
                Name = "A very nice book",
                Authors = new List<Author> { },
                PagesCount = 51,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            this.service.Insert(newBook);
            newBook.Name = "An even nicer book";
            this.service.Insert(newBook);

            var result = this.service.GetAll();

            Assert.IsTrue(result.Count() == 2);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetByIdBook()
        {
            var newBook = new Book
            {
                Name = "A very nice book",
                Authors = new List<Author> { },
                PagesCount = 51,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            this.service.Insert(newBook);

            newBook = this.service.GetByName(newBook.Name).First();
            var result = this.service.GetById(newBook.Id);

            Assert.IsTrue(result.Name == newBook.Name);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetByNameBook()
        {
            var newBook = new Book
            {
                Name = "A very nice book",
                Authors = new List<Author> { },
                PagesCount = 51,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            this.service.Insert(newBook);

            var result = this.service.GetByName(newBook.Name).First();

            Assert.IsTrue(result.Name == newBook.Name);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestChangeBookBorrowableBook()
        {
            var newBook = new Book
            {
                Name = "A very nice book",
                Authors = new List<Author> { },
                PagesCount = 51,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            this.service.Insert(newBook);

            newBook = this.service.GetByName(newBook.Name).First();
            var result = this.service.ChangeBookBorrowable(newBook, false);

            Assert.IsTrue(result == ValidationResult.Success && newBook.Borrowable == false);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestChangeBookBorrowableByIdBook()
        {
            var newBook = new Book
            {
                Name = "A very nice book",
                Authors = new List<Author> { },
                PagesCount = 51,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            this.service.Insert(newBook);

            newBook = this.service.GetByName(newBook.Name).First();
            var result = this.service.ChangeBookBorrowableById(newBook.Id, false);

            Assert.IsTrue(result == ValidationResult.Success && newBook.Borrowable == false);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestChangeBookBorrowableByIdWithIdNotFoundBook()
        {
            var newBook = new Book
            {
                Name = "A very nice book",
                Authors = new List<Author> { },
                PagesCount = 51,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            this.service.Insert(newBook);

            newBook = this.service.GetByName(newBook.Name).First();
            var result = this.service.ChangeBookBorrowableById(newBook.Id + 1, false);

            Assert.IsFalse(result == ValidationResult.Success || newBook.Borrowable == false);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestChangeBookBorrowableInvaliDPageCountBook()
        {
            var newBook = new Book
            {
                Name = "A very nice book",
                Authors = new List<Author> { },
                PagesCount = 51,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            this.service.Insert(newBook);

            newBook = this.service.GetByName(newBook.Name).First();
            newBook.PagesCount = -10;
            var result = this.service.ChangeBookBorrowable(newBook, false);

            Assert.IsFalse(result == ValidationResult.Success && newBook.Borrowable == false);
            newBook.PagesCount = 10;
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestChangeBookBorrowableNullBook()
        {
            var result = this.service.ChangeBookBorrowable(null, false);

            Assert.IsFalse(result == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertTooShortNameBook()
        {
            var newBook = new Book
            {
                Name = "A",
                Authors = new List<Author> { },
                PagesCount = 51,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            var insert = this.service.Insert(newBook);

            var result = this.service.GetByName(newBook.Name);

            Assert.IsFalse(result.Count() == 1 && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertShortEnoughNameBook()
        {
            var newBook = new Book
            {
                Name = "Ab",
                Authors = new List<Author> { },
                PagesCount = 51,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            var insert = this.service.Insert(newBook);

            var result = this.service.GetByName(newBook.Name);

            Assert.IsTrue(result.Count() == 1 && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertNegativeCoppiesBook()
        {
            var newBook = new Book
            {
                Name = "A nice book",
                Authors = new List<Author> { },
                PagesCount = 51,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = -10,
            };

            var insert = this.service.Insert(newBook);

            var result = this.service.GetByName(newBook.Name);

            Assert.IsFalse(result.Count() == 1 && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertZeroCoppiesBook()
        {
            var newBook = new Book
            {
                Name = "A nice book",
                Authors = new List<Author> { },
                PagesCount = 51,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 0,
            };

            var insert = this.service.Insert(newBook);

            var result = this.service.GetByName(newBook.Name);

            Assert.IsTrue(result.Count() == 1 && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertShortEnougNegativePageCountBook()
        {
            var newBook = new Book
            {
                Name = "A nice book",
                Authors = new List<Author> { },
                PagesCount = -51,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            var insert = this.service.Insert(newBook);

            var result = this.service.GetByName(newBook.Name);

            Assert.IsFalse(result.Count() == 1 && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertZeroPageCountBook()
        {
            var newBook = new Book
            {
                Name = "A nice book",
                Authors = new List<Author> { },
                PagesCount = 0,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            var insert = this.service.Insert(newBook);

            var result = this.service.GetByName(newBook.Name);

            Assert.IsFalse(result.Count() == 1 && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertOnePageCountBook()
        {
            var newBook = new Book
            {
                Name = "A nice book",
                Authors = new List<Author> { },
                PagesCount = 1,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            var insert = this.service.Insert(newBook);

            var result = this.service.GetByName(newBook.Name);

            Assert.IsTrue(result.Count() == 1 && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertWithInvalidFieldBook()
        {
            var newField = new BookField
            {
                Name = "a",
                ParentField = null,
            };

            var newBook = new Book
            {
                Name = "A nice book",
                Authors = new List<Author> { },
                PagesCount = 1,
                Fields = new List<BookField> { this.defaultBookField, newField },
                Borrowable = true,
                TotalCopies = 10,
            };

            var insert = this.service.Insert(newBook);

            var result = this.service.GetByName(newBook.Name);

            Assert.IsFalse(result.Count() == 1 && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertTooManyOkFieldsBook()
        {
            int max_index = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DOMENII"]);

            var newBook = new Book
            {
                Name = "A nice book",
                Authors = new List<Author> { },
                PagesCount = 1,
                Fields = new List<BookField> { this.defaultBookField},
                Borrowable = true,
                TotalCopies = 10,
            };

            for (int index = 0; index < max_index; ++index)
            {
                var newField = new BookField
                {
                    Name = "newField" + index.ToString(),
                    ParentField = null,
                };
                this.bookFieldService.Insert(newField);
                newField = this.bookFieldService.GetByName(newField.Name);

                newBook.Fields.Add(newField);
            }

            var insert = this.service.Insert(newBook);

            var result = this.service.GetByName(newBook.Name);

            Assert.IsFalse(result.Count() == 1 && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertJustEnoughOkFieldsBook()
        {
            int max_index = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DOMENII"]);

            var newBook = new Book
            {
                Name = "A nice book",
                Authors = new List<Author> { },
                PagesCount = 1,
                Fields = new List<BookField> { this.defaultBookField },
                Borrowable = true,
                TotalCopies = 10,
            };

            for (int index = 0; index < max_index - 1; ++index)
            {
                var newField = new BookField
                {
                    Name = "newField" + index.ToString(),
                    ParentField = null,
                };
                this.bookFieldService.Insert(newField);
                newField = this.bookFieldService.GetByName(newField.Name);

                newBook.Fields.Add(newField);
            }

            var insert = this.service.Insert(newBook);

            var result = this.service.GetByName(newBook.Name);

            Assert.IsTrue(result.Count() == 1 && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertRelatedFieldsBook()
        {
            var newField = new BookField
            {
                Name = "newField",
                ParentField = this.defaultBookField,
            };
            this.bookFieldService.Insert(newField);
            newField = this.bookFieldService.GetByName(newField.Name);

            var newBook = new Book
            {
                Name = "A nice book",
                Authors = new List<Author> { },
                PagesCount = 1,
                Fields = new List<BookField> { this.defaultBookField, newField },
                Borrowable = true,
                TotalCopies = 10,
            };

            var insert = this.service.Insert(newBook);

            var result = this.service.GetByName(newBook.Name);

            Assert.IsFalse(result.Count() == 1 && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A cleanup function that is called after every test.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            this.databaseContext.SaveChanges();
            this.databaseContext.Dispose();
        }
    }
}
