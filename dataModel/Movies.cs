

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dataModel;

public partial class Movie
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Title { get; set; } = null!; 

    [Column(TypeName = "date")]
    public DateTime ReleaseDate { get; set; } 

    [StringLength(50)]
    [Unicode(false)]
    public string Genre { get; set; } = null!; 

    [Column(TypeName = "decimal(3,1)")]
    public decimal Rating { get; set; } 

    // One-to-Many Relationship: A movie can have many actors
    [InverseProperty("Movie")]
    public virtual ICollection<Actor> Actors { get; set; } = new List<Actor>();
}
