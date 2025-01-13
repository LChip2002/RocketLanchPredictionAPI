public class Pad
{
    public int Id { get; set; }
    public string Url { get; set; }
    public bool Active { get; set; }
    public List<object> Agencies { get; set; }
    public string Name { get; set; }
    public Image Image { get; set; }
    public string Description { get; set; }
    public string WikiUrl { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}