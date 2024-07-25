// <copyright file="BookFieldTests.cs" company="Transilvania University of Brasov">
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
    /// A testing class designated to book field testing in service layer.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BookFieldTests
    {
        private BookFieldService service;
        private MyContext databaseContext;

        /// <summary>
        /// The set-up function that is called before every test.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.databaseContext = new MyContext();
            this.service = new BookFieldService(new BookFieldRepository(this.databaseContext));
            this.service.DeleteAll();
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCorrectRootBookField()
        {
            var newBookField = new BookField
            {
                Name = "TestParentField",
                ParentField = null,
            };

            this.service.Insert(newBookField);

            var result = this.service.GetByName(newBookField.Name);

            Assert.IsTrue(result != null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestDeleteCorrectRootBookField()
        {
            var newBookField = new BookField
            {
                Name = "TestParentField",
                ParentField = null,
            };

            this.service.Insert(newBookField);

            newBookField = this.service.GetById(newBookField.Id);

            this.service.Delete(newBookField);

            var result = this.service.GetById(newBookField.Id);

            Assert.IsTrue(result == null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestDeleteAllBookField()
        {
            var newBookField = new BookField
            {
                Name = "BookField1",
                ParentField = null,
            };

            this.service.Insert(newBookField);
            newBookField.Name = "BookField2";
            this.service.Insert(newBookField);

            this.service.DeleteAll();

            var result = this.service.GetAll();

            Assert.IsTrue(result.Count() == 0);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetAllBookField()
        {
            var newBookField = new BookField
            {
                Name = "BookField1",
                ParentField = null,
            };

            this.service.Insert(newBookField);
            newBookField.Name = "BookField2";
            this.service.Insert(newBookField);

            var result = this.service.GetAll();

            Assert.IsTrue(result.Count() == 2);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetByIdBookField()
        {
            var newBookField = new BookField
            {
                Name = "A field",
                ParentField = null,
            };

            this.service.Insert(newBookField);

            newBookField = this.service.GetByName("A field");
            var result = this.service.GetById(newBookField.Id);

            Assert.IsTrue(result.Name == newBookField.Name && newBookField.ParentField == result.ParentField);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetByNameBookField()
        {
            var newBookField = new BookField
            {
                Name = "A field",
                ParentField = null,
            };

            this.service.Insert(newBookField);

            var result = this.service.GetByName("A field");

            Assert.IsTrue(result.Name == newBookField.Name && newBookField.ParentField == result.ParentField);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetAllDirectChildBookFields()
        {
            var parentBookField = new BookField
            {
                Name = "TestRootField",
                ParentField = null,
                ChildFields = new List<BookField>(),
            };

            var childBookField = new BookField
            {
                Name = "TestChildField1",
                ParentField = parentBookField,
            };
            parentBookField.ChildFields.Add(childBookField);
            childBookField = new BookField { Name = "TestChildField2", ParentField = parentBookField };
            parentBookField.ChildFields.Add(childBookField);
            childBookField = new BookField { Name = "TestChildField3", ParentField = parentBookField };
            parentBookField.ChildFields.Add(childBookField);

            childBookField.ChildFields = new List<BookField>();

            var deeperChildField = new BookField
            {
                Name = "TestDeepChildField1",
                ParentField = childBookField,
            };
            childBookField.ChildFields.Add(deeperChildField);
            deeperChildField = new BookField { Name = "TestDeepChildField2", ParentField = childBookField };
            childBookField.ChildFields.Add(deeperChildField);

            this.service.Insert(parentBookField);
            parentBookField = this.service.GetByName(parentBookField.Name);

            parentBookField.ChildFields = this.service.GetDirectChildren(parentBookField);

            Assert.IsTrue(parentBookField.ChildFields.Count() == 3);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestGetAllDeepChildBookFields()
        {
            var parentBookField = new BookField
            {
                Name = "TestRootField",
                ParentField = null,
                ChildFields = new List<BookField>(),
            };

            var childBookField = new BookField
            {
                Name = "TestChildField1",
                ParentField = parentBookField,
            };
            parentBookField.ChildFields.Add(childBookField);
            childBookField = new BookField { Name = "TestChildField2", ParentField = parentBookField };
            parentBookField.ChildFields.Add(childBookField);
            childBookField = new BookField { Name = "TestChildField3", ParentField = parentBookField };
            parentBookField.ChildFields.Add(childBookField);

            childBookField.ChildFields = new List<BookField>();

            var deeperChildField = new BookField
            {
                Name = "TestDeepChildField1",
                ParentField = childBookField,
            };
            childBookField.ChildFields.Add(deeperChildField);
            deeperChildField = new BookField { Name = "TestDeepChildField2", ParentField = childBookField };
            childBookField.ChildFields.Add(deeperChildField);

            this.service.Insert(parentBookField);
            parentBookField = this.service.GetByName(parentBookField.Name);

            parentBookField.ChildFields = this.service.GetAllChildrenDeep(parentBookField);

            Assert.IsTrue(parentBookField.ChildFields.ElementAt(2).ChildFields.Count() == 2);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestDeleteAllComplexTreeBookFields()
        {
            var parentBookField = new BookField
            {
                Name = "TestRootField",
                ParentField = null,
                ChildFields = new List<BookField>(),
            };

            var childBookField = new BookField
            {
                Name = "TestChildField1",
                ParentField = parentBookField,
            };
            parentBookField.ChildFields.Add(childBookField);
            childBookField = new BookField { Name = "TestChildField2", ParentField = parentBookField };
            parentBookField.ChildFields.Add(childBookField);
            childBookField = new BookField { Name = "TestChildField3", ParentField = parentBookField };
            parentBookField.ChildFields.Add(childBookField);

            childBookField.ChildFields = new List<BookField>();

            var deeperChildField = new BookField
            {
                Name = "TestDeepChildField1",
                ParentField = childBookField,
            };
            childBookField.ChildFields.Add(deeperChildField);
            deeperChildField = new BookField { Name = "TestDeepChildField2", ParentField = childBookField };
            childBookField.ChildFields.Add(deeperChildField);

            this.service.Insert(parentBookField);

            this.service.DeleteAll();

            var result = this.service.GetAll();

            Assert.IsTrue(result.Count() == 0);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestCascadeDeleteRootBookField()
        {
            var parentBookField = new BookField
            {
                Name = "TestRootField",
                ParentField = null,
                ChildFields = new List<BookField>(),
            };

            var childBookField = new BookField
            {
                Name = "TestChildField1",
                ParentField = parentBookField,
            };
            parentBookField.ChildFields.Add(childBookField);
            childBookField = new BookField { Name = "TestChildField2", ParentField = parentBookField };
            parentBookField.ChildFields.Add(childBookField);
            childBookField = new BookField { Name = "TestChildField3", ParentField = parentBookField };
            parentBookField.ChildFields.Add(childBookField);

            childBookField.ChildFields = new List<BookField>();

            var deeperChildField = new BookField
            {
                Name = "TestDeepChildField1",
                ParentField = childBookField,
            };
            childBookField.ChildFields.Add(deeperChildField);
            deeperChildField = new BookField { Name = "TestDeepChildField2", ParentField = childBookField };
            childBookField.ChildFields.Add(deeperChildField);

            this.service.Insert(parentBookField);
            parentBookField = this.service.GetByName(parentBookField.Name);

            this.service.DeleteWithChildren(parentBookField);

            var result = this.service.GetAll();

            Assert.IsTrue(result.Count() == 0);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestCascadeDeleteNotRootParentBookField()
        {
            var parentBookField = new BookField
            {
                Name = "TestRootField",
                ParentField = null,
                ChildFields = new List<BookField>(),
            };

            var childBookField = new BookField
            {
                Name = "TestChildField1",
                ParentField = parentBookField,
            };
            parentBookField.ChildFields.Add(childBookField);
            childBookField = new BookField { Name = "TestChildField2", ParentField = parentBookField };
            parentBookField.ChildFields.Add(childBookField);
            childBookField = new BookField { Name = "TestChildField3", ParentField = parentBookField };
            parentBookField.ChildFields.Add(childBookField);

            childBookField.ChildFields = new List<BookField>();

            var deeperChildField = new BookField
            {
                Name = "TestDeepChildField1",
                ParentField = childBookField,
            };
            childBookField.ChildFields.Add(deeperChildField);
            deeperChildField = new BookField { Name = "TestDeepChildField2", ParentField = childBookField };
            childBookField.ChildFields.Add(deeperChildField);

            this.service.Insert(parentBookField);
            parentBookField = this.service.GetByName(parentBookField.Name);
            parentBookField.ChildFields = this.service.GetDirectChildren(parentBookField);

            this.service.DeleteWithChildren(parentBookField.ChildFields.ElementAt(2));

            var result = this.service.GetAll();

            Assert.IsTrue(result.Count() == 3);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestUpdateCorrectRootBookField()
        {
            var newBookField = new BookField
            {
                Name = "TestParentField",
                ParentField = null,
            };

            this.service.Insert(newBookField);
            newBookField = this.service.GetByName(newBookField.Name);
            newBookField.Name = "NewName";
            this.service.Update(newBookField);

            var resultNewName = this.service.GetByName(newBookField.Name);
            var resultOldName = this.service.GetByName("TestParentField");

            Assert.IsTrue(resultNewName.Id == newBookField.Id && resultOldName == null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestUpdateCorrectChildBookField()
        {
            var parentBookField = new BookField
            {
                Name = "TestRootField",
                ParentField = null,
                ChildFields = new List<BookField>(),
            };

            var childBookField = new BookField
            {
                Name = "TestChildField1",
                ParentField = parentBookField,
            };
            parentBookField.ChildFields.Add(childBookField);
            childBookField = new BookField { Name = "TestChildField2", ParentField = parentBookField };
            parentBookField.ChildFields.Add(childBookField);
            childBookField = new BookField { Name = "TestChildField3", ParentField = parentBookField };
            parentBookField.ChildFields.Add(childBookField);

            this.service.Insert(parentBookField);
            var targetField = this.service.GetByName("TestChildField2");
            targetField.Name = "NewName";
            this.service.Update(targetField);

            var resultNewName = this.service.GetByName(targetField.Name);
            var resultOldName = this.service.GetByName("TestChildField2");

            Assert.IsTrue(resultNewName.Id == targetField.Id && resultOldName == null);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestUpdateNameTooShortBookField()
        {
            var newBookField = new BookField
            {
                Name = "TestParentField",
                ParentField = null,
            };

            this.service.Insert(newBookField);
            newBookField = this.service.GetByName(newBookField.Name);
            newBookField.Name = "aa";
            var update = this.service.Update(newBookField);

            var resultNewName = this.service.GetByName(newBookField.Name);

            Assert.IsFalse(update == ValidationResult.Success || resultNewName != null);
            newBookField.Name = "TestParentField";
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckCorrectRootBookField()
        {
            var newBookField = new BookField
            {
                Name = "TestParentField",
                ParentField = null,
            };

            var insert = this.service.InsertCheck(newBookField);

            var result = this.service.GetByName(newBookField.Name);

            Assert.IsTrue(result != null && insert == ValidationResult.Success);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckCorrectRootNameTakenBookField()
        {
            var newBookField = new BookField
            {
                Name = "TestParentField",
                ParentField = null,
            };

            this.service.InsertCheck(newBookField);
            var result = this.service.InsertCheck(newBookField);

            var allFields = this.service.GetAll();

            Assert.IsFalse(result == ValidationResult.Success || allFields.Count() == 2);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckNameTooShortBookField()
        {
            var newBookField = new BookField
            {
                Name = "aa",
                ParentField = null,
            };

            var result = this.service.InsertCheck(newBookField);

            var allFields = this.service.GetAll();

            Assert.IsFalse(result == ValidationResult.Success || allFields.Count() == 1);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckNameTooLongBookField()
        {
            var newBookField = new BookField
            {
                Name = new string('q', 31),
                ParentField = null,
            };

            var result = this.service.InsertCheck(newBookField);

            var allFields = this.service.GetAll();

            Assert.IsFalse(result == ValidationResult.Success || allFields.Count() == 1);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckNameShortEnoughBookField()
        {
            var newBookField = new BookField
            {
                Name = "aaa",
                ParentField = null,
            };

            var result = this.service.InsertCheck(newBookField);

            var allFields = this.service.GetAll();

            Assert.IsTrue(result == ValidationResult.Success && allFields.Count() == 1);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestInsertCheckNameLongEnoughBookField()
        {
            var newBookField = new BookField
            {
                Name = new string('q', 30),
                ParentField = null,
            };

            var result = this.service.InsertCheck(newBookField);

            var allFields = this.service.GetAll();

            Assert.IsTrue(result == ValidationResult.Success && allFields.Count() == 1);
        }

        /// <summary>
        /// A test.
        /// </summary>
        [TestMethod]
        public void TestDeleteTooShortNameBookField()
        {
            var newBookField = new BookField
            {
                Name = "aa",
                ParentField = null,
            };

            var result = this.service.Delete(newBookField);

            Assert.IsFalse(result == ValidationResult.Success);
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
