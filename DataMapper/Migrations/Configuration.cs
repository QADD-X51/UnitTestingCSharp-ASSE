// <copyright file="Configuration.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DataMapper.Migrations
{
    /// <summary>
    /// A configuration class for dbMigrations.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class Configuration : DbMigrationsConfiguration<DataMapper.MyContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// This method will be called after migrating to the latest version.
        /// You can use the DbSet.<T>.AddOrUpdate() helper extension method.
        /// to avoid creating duplicate seed data.
        /// </summary>
        /// <param name="context">Database context.</param>
        protected override void Seed(DataMapper.MyContext context)
        {
        }
    }
}
