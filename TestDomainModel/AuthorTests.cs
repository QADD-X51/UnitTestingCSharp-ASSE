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
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestDomainModel
{
    /// <summary>
    /// A testing class designated to testing Authors validators.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AuthorTests
    {
        private Author author;

        /// <summary>
        /// A function that is run before every test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            this.author = new Author()
            {
                Name = "Mita Emilul",
            };
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestCorrectAutor()
        {
            var context = new ValidationContext(this.author, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.author, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNullNameAutor()
        {
            this.author.Name = null;

            var context = new ValidationContext(this.author, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.author, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNameTooShortAutor()
        {
            this.author.Name = "a";

            var context = new ValidationContext(this.author, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.author, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNameTooLongAutor()
        {
            this.author.Name = new string('q', 151);

            var context = new ValidationContext(this.author, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.author, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNameShortEnoughAutor()
        {
            this.author.Name = "aa";

            var context = new ValidationContext(this.author, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.author, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNameLongEnoughAutor()
        {
            this.author.Name = new string('q', 150);

            var context = new ValidationContext(this.author, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.author, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNameEmptyAutor()
        {
            this.author.Name = string.Empty;

            var context = new ValidationContext(this.author, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.author, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNameBlankAutor()
        {
            this.author.Name = "     ";

            var context = new ValidationContext(this.author, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.author, context, results, true));
        }
    }
}
