using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Glossary.Models
{
    /// <summary>
    /// Individual terms and their corresponding definition within a Glossary.
    /// </summary>
    public class Entry
    {
        /// <summary>
        /// Unique id given to every new term when added to the Glossary.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Single word or short phrase.
        /// </summary>
        [Display(Name = "Term")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A Term is required")]
        [StringLength(255, ErrorMessage = "Term cannot be more that 255 characters long")]
        [Index(IsUnique = true)]
        public string Term { get; set; }

        /// <summary>
        /// Paragraph of text defining the corresponding term.
        /// </summary>
        [Display(Name = "Definition")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A Definition is required")]
        public string Definition { get; set; }

        /// <summary>
        /// Constant placeholder to indicate a new Entry that therefore doesn't have an Id.
        /// </summary>
        public static readonly int UNASSIGNED_ID = 0;
    }

    /// <summary>
    /// Context class for the Entity Framework representing the Glossary Entity.
    /// </summary>
    public class GlossaryContext : DbContext, IGlossaryContext
    {
        public DbSet<Entry> Entries { get; set; }
    }

    public interface IGlossaryContext
    {
        DbSet<Entry> Entries { get; set; }
    }
}