// <copyright file="PersonRepository.cs" company="Transilvania University of Brasov">
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
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonRepository"/> class.
        /// </summary>
        /// <param name="context">A database context.</param>
        public PersonRepository(MyContext context)
            : base(context)
        {
        }

        /// <inheritdoc/>
        public Person GetByEmail(string email)
        {
            var result = this.dbContext.Set<Person>().Where(u => u.Email == email).ToList();
            if (result.Count() == 0)
            {
                return null;
            }

            return result[0];
        }
    }
}
