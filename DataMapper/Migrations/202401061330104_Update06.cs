// <copyright file="202401061330104_Update06.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Data.Entity.Migrations;

namespace DataMapper.Migrations
{
    /// <summary>
    /// A migration class.
    /// </summary>
    public partial class Update06 : DbMigration
    {
        /// <summary>
        /// Up function.
        /// </summary>
        public override void Up()
        {
            this.RenameTable(name: "dbo.BookBookFields", newName: "BookFieldBooks");
            this.DropPrimaryKey("dbo.BookFieldBooks");
            this.CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Id);
            this.CreateTable(
                "dbo.BookAuthors",
                c => new
                    {
                        Book_Id = c.Int(nullable: false),
                        Author_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Book_Id, t.Author_Id })
                .ForeignKey("dbo.Books", t => t.Book_Id, cascadeDelete: true)
                .ForeignKey("dbo.Authors", t => t.Author_Id, cascadeDelete: true)
                .Index(t => t.Book_Id)
                .Index(t => t.Author_Id);
            this.AddPrimaryKey("dbo.BookFieldBooks", new[] { "BookField_Id", "Book_Id" });
        }

        /// <summary>
        /// Down function.
        /// </summary>
        public override void Down()
        {
            this.DropForeignKey("dbo.BookAuthors", "Author_Id", "dbo.Authors");
            this.DropForeignKey("dbo.BookAuthors", "Book_Id", "dbo.Books");
            this.DropIndex("dbo.BookAuthors", new[] { "Author_Id" });
            this.DropIndex("dbo.BookAuthors", new[] { "Book_Id" });
            this.DropPrimaryKey("dbo.BookFieldBooks");
            this.DropTable("dbo.BookAuthors");
            this.DropTable("dbo.Authors");
            this.AddPrimaryKey("dbo.BookFieldBooks", new[] { "Book_Id", "BookField_Id" });
            this.RenameTable(name: "dbo.BookFieldBooks", newName: "BookBookFields");
        }
    }
}
