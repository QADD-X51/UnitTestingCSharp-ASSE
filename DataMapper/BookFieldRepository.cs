// <copyright file="BookFieldRepository.cs" company="Transilvania University of Brasov">
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
    public class BookFieldRepository : BaseRepository<BookField>, IBookFieldRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookFieldRepository"/> class.
        /// </summary>
        /// <param name="context">A database context.</param>
        public BookFieldRepository(MyContext context)
            : base(context)
        {
        }

        /// <inheritdoc/>
        public ICollection<BookField> GetAllChildrenFields(int rootFieldId)
        {
            ICollection<BookField> toReturn = this.GetAllDirectChildrenFields(rootFieldId);

            foreach (BookField bookField in toReturn) 
            {
                bookField.ChildFields = this.GetAllChildrenFields(bookField.Id);
            }

            return toReturn;
        }

        /// <inheritdoc/>
        public ICollection<BookField> GetAllDirectChildrenFields(int parentFieldId)
        {
            return this.dbContext.Set<BookField>().Where(field => field.ParentField.Id == parentFieldId).ToList();
        }

        /// <inheritdoc/>
        public BookField GetByName(string name)
        {
            return this.dbContext.Set<BookField>().FirstOrDefault(field => field.Name == name);
        }
    }
}
