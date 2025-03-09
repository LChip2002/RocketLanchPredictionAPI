public class Image
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Credit { get; set; }
    public License? License { get; set; }
    public bool SingleUse { get; set; }
    public List<Variant>? Variants { get; set; }
}