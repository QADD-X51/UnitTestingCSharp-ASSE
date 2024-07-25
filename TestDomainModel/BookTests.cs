// <copyright file="BookTests.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestDomainModel
{
    /// <summary>
    /// A test class focused around book testing.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BookTests
    {
        private Book book;
        private BookField parentField;
        private BookField childField;

        /// <summary>
        /// A set-up method.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
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
                Name = "A Cool Book",
                Authors = new List<Author>
                {
                    new Author { Id = 0, Name = "Eu Nutu" },
                    new Author { Id = 1, Name = "Relu Dorelu" },
                },
                PagesCount = 51,
                Fields = new List<BookField> { this.parentField },
                Borrowable = true,
                TotalCopies = 10,
            };
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestCorrectBook()
        {
            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNullNamedBook()
        {
            this.book.Name = null;

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestTooShortNameBook()
        {
            this.book.Name = "1";

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestShortEnoughNameBook()
        {
            this.book.Name = "12";

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestZeroPageCountNameBook()
        {
            this.book.PagesCount = 0;

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNegativePageCountNameBook()
        {
            this.book.PagesCount = -51;

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestAuthorNameTooLongBook()
        {
            this.book.Authors.Add(new Author() { Id = 100, Name = new string('q', 151) });

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestAuthorNameTooShortBook()
        {
            this.book.Authors.Add(new Author() { Id = 100, Name = new string('q', 1) });

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNoAuthorsBook()
        {
            this.book.Authors = new List<Author>();

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNullAuthorsBook()
        {
            this.book.Authors = null;

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNoFieldsBook()
        {
            this.book.Fields = new List<BookField>();

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNullFieldsBook()
        {
            this.book.Fields = null;

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestProhibitedFieldsBook()
        {
            this.book.Fields.Add(this.childField);

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestMultipleOkFieldsBook()
        {
            this.book.Fields.Add(new BookField() { Id = 3, Name = "New Field", ParentField = null });

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestTooManyOkFieldsBook()
        {
            int max_index = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DOMENII"]);
            this.book.Fields = new List<BookField>();

            for (int index = 0; index <= max_index; ++index)
            {
                this.book.Fields.Add(new BookField() { Id = index, Name = "Name", ParentField = null });
            }

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestJustEnoughOkFieldsBook()
        {
            int max_index = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DOMENII"]);
            this.book.Fields = new List<BookField>();

            for (int index = 0; index < max_index; ++index)
            {
                this.book.Fields.Add(new BookField() { Id = index, Name = "Name", ParentField = null });
            }

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestOkFieldsChildFieldFirstBook()
        {
            this.book.Fields = new List<BookField>();
            this.book.Fields.Add(this.childField);
            this.book.Fields.Add(new BookField() { Id = 51, Name = "Unrelated Field", ParentField = null });

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNoCopiesBook()
        {
            this.book.TotalCopies = 0;

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNegativeCopiesBook()
        {
            this.book.TotalCopies = -1;

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestOneInvallidFieldAndOneGoodFieldBook()
        {
            BookField newField = new BookField();
            newField = this.parentField;
            newField.Id = 99;
            newField.Name = "a";
            this.book.Fields.Add(newField);

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.book, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestOnlyOneInvallidFieldBook()
        {
            BookField newField = new BookField();
            newField = this.parentField;
            newField.Id = 99;
            newField.Name = "a";
            this.book.Fields = new List<BookField>();
            this.book.Fields.Add(newField);

            var context = new ValidationContext(this.book, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.book, context, results, true));
        }
    }
}
