using System.ComponentModel.DataAnnotations;

namespace image_gallery.Models;

public class Image
{
    public int Id { get; set; }
    public User User { get; set; }
    // public byte[] Content { get; set; }
    // public string ImageName { get; set; } = string.Empty;
    public int Rating { get; set; } = 0;
    public string Url { get; set; } = string.Empty;
}