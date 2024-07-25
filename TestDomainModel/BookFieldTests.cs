// <copyright file="BookFieldTests.cs" company="Transilvania University of Brasov">
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
    /// A test class focused around book field testing.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BookFieldTests
    {
        private BookField parentField;
        private BookField childField;

        /// <summary>
        /// The set-up function.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.parentField = new BookField
            {
                Name = "Science",
                ParentField = null,
            };

            this.childField = new BookField
            {
                Name = "Mathematics",
                ParentField = this.parentField,
            };
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestCorrectParentBookField()
        {
            var context = new ValidationContext(this.parentField, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.parentField, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestCorrectChildBookField()
        {
            var context = new ValidationContext(this.childField, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.childField, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestShortEnoughNameBookField()
        {
            this.parentField.Name = "515";

            var context = new ValidationContext(this.parentField, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.parentField, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestLongEnoughNameBookField()
        {
            this.parentField.Name = new string('1', 30);

            var context = new ValidationContext(this.parentField, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.parentField, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestShortNameBookField()
        {
            this.parentField.Name = "5";

            var context = new ValidationContext(this.parentField, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.parentField, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestLongNameBookField()
        {
            this.parentField.Name = new string('q', 31);

            var context = new ValidationContext(this.parentField, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.parentField, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNullNameBookField()
        {
            this.parentField.Name = null;

            var context = new ValidationContext(this.parentField, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.parentField, context, results, true));
        }
    }
}
