using System.ComponentModel.DataAnnotations;

namespace image_gallery.Models;

public class Image
{
    public int Id { get; set; }
    public User User { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool Private { get; set; }
}