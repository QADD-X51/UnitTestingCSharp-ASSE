// <copyright file="202401041150375_Update05.cs" company="Transilvania University of Brasov">
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
    public partial class Update05 : DbMigration
    {
        /// <summary>
        /// Up function.
        /// </summary>
        public override void Up()
        {
            this.CreateIndex("dbo.BookFields", "Name", unique: true);
        }

        /// <summary>
        /// Down function.
        /// </summary>
        public override void Down()
        {
            this.DropIndex("dbo.BookFields", new[] { "Name" });
        }
    }
}
