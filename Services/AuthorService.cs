// <copyright file="AuthorService.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Interfaces;
using DomainModel;
using Services.Interfaces;

namespace Services
{
    /// <summary>
    /// A service class to manipulate the author tables.
    /// </summary>
    public class AuthorService : BaseService<Author, IAuthorRepository>, IAuthorService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorService"/> class.
        /// </summary>
        /// <param name="repository">The author repository.</param>
        public AuthorService(IAuthorRepository repository)
            : base(repository)
        {
        }

        /// <inheritdoc/>
        public ICollection<Author> GetByName(string name)
        {
            return this.repository.GetByName(name);
        }
    }
}
