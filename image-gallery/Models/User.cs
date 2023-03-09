using System.ComponentModel.DataAnnotations;

namespace image_gallery.Models;

public class User
{
    public int Id { get; set; }
    [Required, StringLength(16)]
    public string? Username { get; set; } = string.Empty;
    [Required, StringLength(20)]
    public string? Password { get; set; } = string.Empty;
}