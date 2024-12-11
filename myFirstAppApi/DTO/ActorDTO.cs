//using System.ComponentModel.DataAnnotations;

//namespace myFirstAppApi.DTO
//{
//    public class ActorDTO
//    {
//        public int Id { get; set; } // Actor ID

//        [Required]
//        [StringLength(100)]
//        public string Name { get; set; } = null!; // Actor's Name

//        public int Age { get; set; } // Actor's Age

//        [Required]
//        [StringLength(100)]
//        public string CharacterName { get; set; } = null!; // Character Name played by the Actor

//        [Required]
//        public string MovieTitle { get; set; } = null!; // Title of the Movie the Actor is linked to
//    }
//}


using System.ComponentModel.DataAnnotations;

namespace myFirstAppApi.DTO
{
    public class ActorDTO
    {
        public int Id { get; set; } 

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!; 

        public int Age { get; set; } 

        [Required]
        [StringLength(100)]
        public string CharacterName { get; set; } = null!; 

        [Required]
        public int MovieId { get; set; } 

        public string MovieTitle { get; set; } = null!; 
    }
}
