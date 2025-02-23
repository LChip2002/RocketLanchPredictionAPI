namespace RLP_API.Models
{
    // TODO - Edit this to match the ML model parameters
    public class PredictionMaker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PredictionDate { get; set; }
        public string PredictionDetails { get; set; }
    }
}