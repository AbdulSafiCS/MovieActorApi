

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dataModel;

public partial class Actor
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!; 

    public int Age { get; set; } 

    [StringLength(100)]
    public string CharacterName { get; set; } = null!; 

    [Column("movieID")]
    public int MovieId { get; set; } 

    [ForeignKey("MovieId")]
    [JsonIgnore] // Prevent serialization to avoid circular references
    public virtual Movie? Movie { get; set; } // Make the navigation property optional
}
