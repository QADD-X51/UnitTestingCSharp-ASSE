// <copyright file="AuthorTests.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMapper;
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;

namespace TestServiceLayer
{
    /// <summary>
    /// A testing class designated to testing authors.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AuthorTests
    {
        private AuthorService service;
        private MyContext databaseContext;

        /// <summary>
        /// A set up function that is run before every test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            this.databaseContext = new MyContext();
            this.service = new AuthorService(new AuthorRepository(this.databaseContext));
            this.service.DeleteAll();
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCorrectAuthor()
        {
            var newAuthor = new Author
            {
                Name = "El Gringo",
            };

            this.service.Insert(newAuthor);

            var result = this.service.GetByName(newAuthor.Name);

            Assert.IsTrue(result.Count() == 1);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestDeleteCorrectAuthor()
        {
            var newAuthor = new Author
            {
                Name = "El Gringo",
            };

            this.service.Insert(newAuthor);

            newAuthor = this.service.GetByName(newAuthor.Name).First();

            this.service.Delete(newAuthor);

            var result = this.service.GetById(newAuthor.Id);

            Assert.IsTrue(result == null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestDeleteAllAuthor()
        {
            var newAuthor = new Author
            {
                Name = "El Gringo",
            };

            this.service.Insert(newAuthor);
            newAuthor.Name = "Captain Claw";
            this.service.Insert(newAuthor);

            this.service.DeleteAll();

            var result = this.service.GetAll();

            Assert.IsTrue(result.Count() == 0);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetAllAuthor()
        {
            var newAuthor = new Author
            {
                Name = "El Gringo",
            };

            this.service.Insert(newAuthor);
            newAuthor.Name = "Captain Claw";
            this.service.Insert(newAuthor);

            var result = this.service.GetAll();

            Assert.IsTrue(result.Count() == 2);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetByIdAuthor()
        {
            var newAuthor = new Author
            {
                Name = "El Gringo",
            };

            this.service.Insert(newAuthor);

            newAuthor = this.service.GetByName(newAuthor.Name).First();
            var result = this.service.GetById(newAuthor.Id);

            Assert.IsTrue(result.Name == newAuthor.Name);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetByNameAuthor()
        {
            var newAuthor = new Author
            {
                Name = "El Gringo",
            };

            this.service.Insert(newAuthor);

            var result = this.service.GetByName(newAuthor.Name).First();

            Assert.IsTrue(result.Name == newAuthor.Name);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertNameTooShortAuthor()
        {
            var newAuthor = new Author
            {
                Name = "E",
            };

            var insert = this.service.Insert(newAuthor);
            var result = this.service.GetByName(newAuthor.Name);

            Assert.IsFalse(result.Count() == 1 || insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertNameEmptyAuthor()
        {
            var newAuthor = new Author
            {
                Name = string.Empty,
            };

            var insert = this.service.Insert(newAuthor);
            var result = this.service.GetByName(newAuthor.Name);

            Assert.IsFalse(result.Count() == 1 || insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertNameBlankAuthor()
        {
            var newAuthor = new Author
            {
                Name = "     ",
            };

            var insert = this.service.Insert(newAuthor);
            var result = this.service.GetByName(newAuthor.Name);

            Assert.IsFalse(result.Count() == 1 || insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertNameNullAuthor()
        {
            var newAuthor = new Author
            {
                Name = null,
            };

            var insert = this.service.Insert(newAuthor);
            var result = this.service.GetByName(newAuthor.Name);

            Assert.IsFalse(result.Count() == 1 || insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertNameTooLongAuthor()
        {
            var newAuthor = new Author
            {
                Name = new string('q', 151),
            };

            var insert = this.service.Insert(newAuthor);
            var result = this.service.GetByName(newAuthor.Name);

            Assert.IsFalse(result.Count() == 1 || insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertNameShortEnoughAuthor()
        {
            var newAuthor = new Author
            {
                Name = "El",
            };

            var insert = this.service.Insert(newAuthor);
            var result = this.service.GetByName(newAuthor.Name);

            Assert.IsTrue(result.Count() == 1 || insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertNameLongEnoughAuthor()
        {
            var newAuthor = new Author
            {
                Name = new string('q', 150),
            };

            var insert = this.service.Insert(newAuthor);
            var result = this.service.GetByName(newAuthor.Name);

            Assert.IsTrue(result.Count() == 1 || insert == ValidationResult.Success);
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
