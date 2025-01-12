
namespace RLP_External_Data_Ingest;

public class Launch
{
    public string Id { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public string ResponseMode { get; set; }
    public string Slug { get; set; }
    public string LaunchDesignator { get; set; }
    public Status Status { get; set; }
    public string LastUpdated { get; set; }
    public string Net { get; set; }
    public string WindowEnd { get; set; }
    public string WindowStart { get; set; }
    public Image Image { get; set; }
    public Mission Mission { get; set; }
    public Pad Pad { get; set; }
}