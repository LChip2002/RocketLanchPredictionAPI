public class Mission
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public object Image { get; set; }
    public Orbit Orbit { get; set; }
    public List<Agency> Agencies { get; set; }
    public List<object> InfoUrls { get; set; }
    public List<object> VidUrls { get; set; }
}