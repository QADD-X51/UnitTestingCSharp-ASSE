// <copyright file="PersonTests.cs" company="Transilvania University of Brasov">
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
    /// A class designated for testing users.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PersonTests
    {
        private Person person;

        /// <summary>
        /// A set-up method.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.person = new Person()
            {
                Id = 0,
                Name = "Wider",
                Surname = "Toby",
                Email = "TobY299@gmail.com",
                Phone = "1234567890",
                IsPersonnel = false,
            };
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestCorrectPerson()
        {
            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNullNamePerson()
        {
            this.person.Name = null;

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNullSurnamePerson()
        {
            this.person.Surname = null;

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNullEmailPerson()
        {
            this.person.Email = null;

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNullPhonePerson()
        {
            this.person.Name = null;

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestTooShortNamePerson()
        {
            this.person.Name = new string('q', 1);

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestTooLongNamePerson()
        {
            this.person.Name = new string('q', 51);

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestShortEnoughNamePerson()
        {
            this.person.Name = new string('q', 2);

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestLongEnoughNamePerson()
        {
            this.person.Name = new string('q', 50);

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestTooShortSurnamePerson()
        {
            this.person.Surname = new string('q', 1);

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestTooLongSurnamePerson()
        {
            this.person.Surname = new string('q', 51);

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestShortEnoughSurnamePerson()
        {
            this.person.Surname = new string('q', 2);

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestLongEnoughSurnamePerson()
        {
            this.person.Surname = new string('q', 50);

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestTooLongEmailPerson()
        {
            this.person.Email = new string('q', 97) + "@e.e";

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestLongEnoughEmailPerson()
        {
            this.person.Email = new string('q', 96) + "@e.e";

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestTooLongPhonePerson()
        {
            this.person.Phone = "12345678901";

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestTooShortPhonePerson()
        {
            this.person.Phone = "123456789";

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestEmailNoAtSignPerson()
        {
            this.person.Email = "email.somestuff.gmail.com";

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestEmailNoDotComPerson()
        {
            this.person.Email = "email.somestuff@gmail";

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestPhoneContainsLetterPerson()
        {
            this.person.Phone = "123q567890";

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestNameContainsSpacePerson()
        {
            this.person.Name = "Wi der";

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.person, context, results, true));
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestSurnameContainsSpacePerson()
        {
            this.person.Surname = "Wi der";

            var context = new ValidationContext(this.person, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.person, context, results, true));
        }
    }
}
