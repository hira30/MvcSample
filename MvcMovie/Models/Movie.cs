using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [DisplayName("タイトル")]
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? Title { get; set; }

        [DisplayName("ジャンル")]
        [StringLength(30)]
        [Required]
        public string? Genre { get; set; }

        [DisplayName("興行収入（億円）")]
        [Range(1, 1000)]
        public decimal Revenue { get; set; }

        [DisplayName("公開日")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("レイティング")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [StringLength(5)]
        [Required]
        public string? Rating { get; set; }
    }
}