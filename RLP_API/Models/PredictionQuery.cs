using System;

namespace RLP_API.Models
{
    // TODO - Edit this to match the entity in the DB
    public class PredictionQuery
    {
        public int Id { get; set; }
        public DateTime LaunchDate { get; set; }
        public string RocketType { get; set; }
        public string LaunchSite { get; set; }
        public string MissionType { get; set; }
    }
}