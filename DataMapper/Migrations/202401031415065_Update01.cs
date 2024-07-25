// <copyright file="202401031415065_Update01.cs" company="Transilvania University of Brasov">
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
    public partial class Update01 : DbMigration
    {
        /// <summary>
        /// The update database option.
        /// </summary>
        public override void Up()
        {
            this.CreateIndex("dbo.People", "Email", unique: true);
        }

        /// <summary>
        /// The drop database function.
        /// </summary>
        public override void Down()
        {
            this.DropIndex("dbo.People", new[] { "Email" });
        }
    }
}
