// <copyright file="202401031227172_InitialCreate.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Data.Entity.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace DataMapper.Migrations
{
    /// <summary>
    /// A migration class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class InitialCreate : DbMigration
    {
        /// <summary>
        /// The update database option.
        /// </summary>
        public override void Up()
        {
            this.CreateTable(
                "dbo.BookFields",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 30),
                    ParentField_Id = c.Int(),
                    Book_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookFields", t => t.ParentField_Id)
                .ForeignKey("dbo.Books", t => t.Book_Id)
                .Index(t => t.ParentField_Id)
                .Index(t => t.Book_Id);

            this.CreateTable(
                "dbo.Books",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    PagesCount = c.Int(nullable: false),
                    Borrowable = c.Boolean(nullable: false),
                    TotalCopies = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            this.CreateTable(
                "dbo.Borrows",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    StartDate = c.DateTime(nullable: false),
                    DueDate = c.DateTime(nullable: false),
                    Active = c.Boolean(nullable: false),
                    Borrower_Id = c.Int(nullable: false),
                    Lender_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Borrower_Id, cascadeDelete: false)
                .ForeignKey("dbo.People", t => t.Lender_Id, cascadeDelete: false)
                .Index(t => t.Borrower_Id)
                .Index(t => t.Lender_Id);

            this.CreateTable(
                "dbo.People",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 50),
                    Surname = c.String(nullable: false, maxLength: 50),
                    Email = c.String(nullable: false, maxLength: 100),
                    Phone = c.String(nullable: false, maxLength: 10),
                    IsPersonnel = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            this.CreateTable(
                "dbo.BorrowBooks",
                c => new
                {
                    Borrow_Id = c.Int(nullable: false),
                    Book_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Borrow_Id, t.Book_Id })
                .ForeignKey("dbo.Borrows", t => t.Borrow_Id, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.Book_Id, cascadeDelete: true)
                .Index(t => t.Borrow_Id)
                .Index(t => t.Book_Id);
        }

        /// <summary>
        /// The drop database function.
        /// </summary>
        public override void Down()
        {
            this.DropForeignKey("dbo.BookFields", "Book_Id", "dbo.Books");
            this.DropForeignKey("dbo.Borrows", "Lender_Id", "dbo.People");
            this.DropForeignKey("dbo.Borrows", "Borrower_Id", "dbo.People");
            this.DropForeignKey("dbo.BorrowBooks", "Book_Id", "dbo.Books");
            this.DropForeignKey("dbo.BorrowBooks", "Borrow_Id", "dbo.Borrows");
            this.DropForeignKey("dbo.BookFields", "ParentField_Id", "dbo.BookFields");
            this.DropIndex("dbo.BorrowBooks", new[] { "Book_Id" });
            this.DropIndex("dbo.BorrowBooks", new[] { "Borrow_Id" });
            this.DropIndex("dbo.Borrows", new[] { "Lender_Id" });
            this.DropIndex("dbo.Borrows", new[] { "Borrower_Id" });
            this.DropIndex("dbo.BookFields", new[] { "Book_Id" });
            this.DropIndex("dbo.BookFields", new[] { "ParentField_Id" });
            this.DropTable("dbo.BorrowBooks");
            this.DropTable("dbo.People");
            this.DropTable("dbo.Borrows");
            this.DropTable("dbo.Books");
            this.DropTable("dbo.BookFields");
        }
    }
}
