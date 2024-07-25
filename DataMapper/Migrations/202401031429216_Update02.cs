// <copyright file="202401031429216_Update02.cs" company="Transilvania University of Brasov">
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
    public partial class Update02 : DbMigration
    {
        /// <summary>
        /// The update database option.
        /// </summary>
        public override void Up()
        {
            this.DropForeignKey("dbo.BookFields", "Book_Id", "dbo.Books");
            this.DropIndex("dbo.BookFields", new[] { "Book_Id" });
            this.CreateTable(
                "dbo.BookBookFields",
                c => new
                {
                    Book_Id = c.Int(nullable: false),
                    BookField_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Book_Id, t.BookField_Id })
                .ForeignKey("dbo.Books", t => t.Book_Id, cascadeDelete: true)
                .ForeignKey("dbo.BookFields", t => t.BookField_Id, cascadeDelete: true)
                .Index(t => t.Book_Id)
                .Index(t => t.BookField_Id);

            this.DropColumn("dbo.BookFields", "Book_Id");
        }

        /// <summary>
        /// The drop database function.
        /// </summary>
        public override void Down()
        {
            this.AddColumn("dbo.BookFields", "Book_Id", c => c.Int());
            this.DropForeignKey("dbo.BookBookFields", "BookField_Id", "dbo.BookFields");
            this.DropForeignKey("dbo.BookBookFields", "Book_Id", "dbo.Books");
            this.DropIndex("dbo.BookBookFields", new[] { "BookField_Id" });
            this.DropIndex("dbo.BookBookFields", new[] { "Book_Id" });
            this.DropTable("dbo.BookBookFields");
            this.CreateIndex("dbo.BookFields", "Book_Id");
            this.AddForeignKey("dbo.BookFields", "Book_Id", "dbo.Books", "Id");
        }
    }
}
