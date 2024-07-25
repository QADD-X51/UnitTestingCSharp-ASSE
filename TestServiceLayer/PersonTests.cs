// <copyright file="PersonTests.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DataMapper;
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;

namespace TestServiceLayer
{
    /// <summary>
    /// A testing class designated to testing people.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PersonTests
    {
        private PersonService service;
        private MyContext databaseContext;

        /// <summary>
        /// A set up function that is called before running any test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            this.databaseContext = new MyContext();
            this.service = new PersonService(new PersonRepository(this.databaseContext));
            this.service.DeleteAll();
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCorrectPerson()
        {
            var newPerson = new Person
            {
                Name = "Eu",
                Surname = "Nutu",
                Email = "a@a.a",
                Phone = "0123456789",
                IsPersonnel = false,
            };

            this.service.Insert(newPerson);

            var result = this.service.GetByEmail(newPerson.Email);

            Assert.IsTrue(result != null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestDeleteCorrectPerson()
        {
            var newPerson = new Person
            {
                Name = "Eu",
                Surname = "Nutu",
                Email = "a@a.a",
                Phone = "0123456789",
                IsPersonnel = false,
            };

            this.service.Insert(newPerson);

            newPerson = this.service.GetByEmail(newPerson.Email);

            this.service.Delete(newPerson);

            var result = this.service.GetById(newPerson.Id);

            Assert.IsTrue(result == null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestDeleteAllPerson()
        {
            var newPerson = new Person
            {
                Name = "Eu",
                Surname = "Nutu",
                Email = "a@a.a",
                Phone = "0123456789",
                IsPersonnel = false,
            };

            this.service.Insert(newPerson);
            newPerson.Email = "b@b.b";
            this.service.Insert(newPerson);

            this.service.DeleteAll();

            var result = this.service.GetAll();

            Assert.IsTrue(result.Count() == 0);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetAllPerson()
        {
            var newPerson = new Person
            {
                Name = "Eu",
                Surname = "Nutu",
                Email = "a@a.a",
                Phone = "0123456789",
                IsPersonnel = false,
            };

            this.service.Insert(newPerson);
            newPerson.Email = "b@b.b";
            this.service.Insert(newPerson);

            var result = this.service.GetAll();

            Assert.IsTrue(result.Count() == 2);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetByIdPerson()
        {
            var newPerson = new Person
            {
                Name = "Eu",
                Surname = "Nutu",
                Email = "a@a.a",
                Phone = "0123456789",
                IsPersonnel = false,
            };

            this.service.Insert(newPerson);

            newPerson = this.service.GetByEmail(newPerson.Email);
            var result = this.service.GetById(newPerson.Id);

            Assert.IsTrue(result.Name == newPerson.Name &&
                          result.Surname == newPerson.Surname &&
                          result.Email == newPerson.Email &&
                          result.Phone == newPerson.Phone &&
                          result.IsPersonnel == newPerson.IsPersonnel);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetByEmailPerson()
        {
            var newPerson = new Person
            {
                Name = "Eu",
                Surname = "Nutu",
                Email = "a@a.a",
                Phone = "0123456789",
                IsPersonnel = false,
            };

            this.service.Insert(newPerson);

            var result = this.service.GetByEmail(newPerson.Email);

            Assert.IsTrue(result.Name == newPerson.Name &&
                          result.Surname == newPerson.Surname &&
                          result.Email == newPerson.Email &&
                          result.Phone == newPerson.Phone &&
                          result.IsPersonnel == newPerson.IsPersonnel);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckEmailTakenPerson()
        {
            var newPerson = new Person
            {
                Name = "Eu",
                Surname = "Nutu",
                Email = "a@a.a",
                Phone = "0123456789",
                IsPersonnel = false,
            };

            this.service.Insert(newPerson);
            var anotherPerson = new Person
            {
                Name = "Another",
                Surname = "Person",
                Email = "a@a.a",
                Phone = "3123456789",
                IsPersonnel = false,
            };

            var insert = this.service.InsertCheck(anotherPerson);
            var result = this.service.GetAll();

            Assert.IsFalse(result.Count() == 2 || insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckCorrectPerson()
        {
            var newPerson = new Person
            {
                Name = "Eu",
                Surname = "Nutu",
                Email = "a@a.a",
                Phone = "0123456789",
                IsPersonnel = false,
            };

            var result = this.service.InsertCheck(newPerson);
            newPerson = this.service.GetByEmail(newPerson.Email);

            Assert.IsTrue(result == ValidationResult.Success && newPerson != null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckWrongEmailFormatPerson()
        {
            var newPerson = new Person
            {
                Name = "Eu",
                Surname = "Nutu",
                Email = "a@a",
                Phone = "0123456789",
                IsPersonnel = false,
            };

            var result = this.service.InsertCheck(newPerson);
            newPerson = this.service.GetByEmail(newPerson.Email);

            Assert.IsFalse(result == ValidationResult.Success || newPerson != null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckNullEmailPerson()
        {
            var newPerson = new Person
            {
                Name = "Eu",
                Surname = "Nutu",
                Email = null,
                Phone = "0123456789",
                IsPersonnel = false,
            };

            var result = this.service.InsertCheck(newPerson);
            newPerson = this.service.GetByEmail(newPerson.Email);

            Assert.IsFalse(result == ValidationResult.Success || newPerson != null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckNullNamePerson()
        {
            var newPerson = new Person
            {
                Name = null,
                Surname = "Nutu",
                Email = "a@b.c",
                Phone = "0123456789",
                IsPersonnel = false,
            };

            var result = this.service.InsertCheck(newPerson);
            newPerson = this.service.GetByEmail(newPerson.Email);

            Assert.IsFalse(result == ValidationResult.Success || newPerson != null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckTooShortNamePerson()
        {
            var newPerson = new Person
            {
                Name = new string('q', 1),
                Surname = "Nutu",
                Email = "a@b.c",
                Phone = "0123456789",
                IsPersonnel = false,
            };

            var result = this.service.InsertCheck(newPerson);
            newPerson = this.service.GetByEmail(newPerson.Email);

            Assert.IsFalse(result == ValidationResult.Success || newPerson != null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckTooLongNamePerson()
        {
            var newPerson = new Person
            {
                Name = new string('q', 51),
                Surname = "Nutu",
                Email = "a@b.c",
                Phone = "0123456789",
                IsPersonnel = false,
            };

            var result = this.service.InsertCheck(newPerson);
            newPerson = this.service.GetByEmail(newPerson.Email);

            Assert.IsFalse(result == ValidationResult.Success || newPerson != null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckShortEnoughNamePerson()
        {
            var newPerson = new Person
            {
                Name = new string('q', 2),
                Surname = "Nutu",
                Email = "a@b.c",
                Phone = "0123456789",
                IsPersonnel = false,
            };

            var result = this.service.InsertCheck(newPerson);
            newPerson = this.service.GetByEmail(newPerson.Email);

            Assert.IsTrue(result == ValidationResult.Success || newPerson != null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckLongEnoughNamePerson()
        {
            var newPerson = new Person
            {
                Name = new string('q', 50),
                Surname = "Nutu",
                Email = "a@b.c",
                Phone = "0123456789",
                IsPersonnel = false,
            };

            var result = this.service.InsertCheck(newPerson);
            newPerson = this.service.GetByEmail(newPerson.Email);

            Assert.IsTrue(result == ValidationResult.Success || newPerson != null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckTooLongEmailPerson()
        {
            var newPerson = new Person
            {
                Name = "Eu",
                Surname = "Nutu",
                Email = new string('q', 97) + "@a.b",
                Phone = "0123456789",
                IsPersonnel = false,
            };

            var result = this.service.InsertCheck(newPerson);
            newPerson = this.service.GetByEmail(newPerson.Email);

            Assert.IsFalse(result == ValidationResult.Success || newPerson != null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckLongEnoughEmailPerson()
        {
            var newPerson = new Person
            {
                Name = "Eu",
                Surname = "Nutu",
                Email = new string('q', 96) + "@a.b",
                Phone = "0123456789",
                IsPersonnel = false,
            };

            var result = this.service.InsertCheck(newPerson);
            newPerson = this.service.GetByEmail(newPerson.Email);

            Assert.IsTrue(result == ValidationResult.Success || newPerson != null);
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
