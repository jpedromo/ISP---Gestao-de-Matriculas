
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Everis.Core.Data
{
    /// <summary>
    /// Represents a single instance that handles the business logic for all rows in a datasource table or view.
    /// </summary>
    /// <typeparam name="TKEY">The type of the identifier field</typeparam>
    public abstract class Entity<TKEY> : IEntity
    {
        /// <summary>
        /// Gets or sets the datasource ID field in an object to maintain identity between an in-memory object and a datasource row.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKEY Id { get; set; }
    }
}
