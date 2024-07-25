// <copyright file="AuthorRepository.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Interfaces;
using DomainModel;

namespace DataMapper
{
    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorRepository"/> class.
        /// </summary>
        /// <param name="context">A database context.</param>
        public AuthorRepository(MyContext context)
            : base(context)
        {
        }

        /// <inheritdoc/>
        public ICollection<Author> GetByName(string name)
        {
            return this.dbContext.Set<Author>().Where(author => author.Name == name).ToList();
        }
    }
}
