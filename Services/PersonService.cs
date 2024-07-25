// <copyright file="PersonService.cs" company="Transilvania University of Brasov">
// Viju Tudor-Alexandru
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMapper.Interfaces;
using DomainModel;
using Services.Interfaces;

namespace Services
{
    /// <summary>
    /// A service class that allows manipulation of persons in the database.
    /// </summary>
    public class PersonService : BaseService<Person, IPersonRepository>, IPersonService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonService"/> class.
        /// </summary>
        /// <param name="repository">The repository object.</param>
        public PersonService(IPersonRepository repository)
            : base(repository)
        {
        }

        /// <inheritdoc/>
        public Person GetByEmail(string email)
        {
            Log.Info("Getting person with emial: " + email);
            return this.repository.GetByEmail(email);
        }

        /// <inheritdoc/>
        public ValidationResult InsertCheck(Person person)
        {
            var checkPersonExisting = this.GetByEmail(person.Email);

            if (checkPersonExisting != null)
            {
                Log.Warn("Person not added. The following email is already taken: " + person.Email);
                return new ValidationResult("Email is taken.");
            }

            var result = this.Insert(person);

            if (result != ValidationResult.Success)
            {
                Log.Warn("Person not added because it's inavlid: " + result.ToString());
                return result;
            }

            Log.Info("Person added:: " + person.Name + " " + person.Surname + " " + person.Email);
            return result;
        }
    }
}
