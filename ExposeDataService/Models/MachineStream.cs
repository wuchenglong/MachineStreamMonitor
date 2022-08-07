using MongoDB.Bson.Serialization.Attributes;

namespace DataService.Models //ExposeDataService.Models
{
    public class MachineStream
    {
        [BsonId]
        public string Id { get; set; }
        public string? MachineId { get; set; }
        public DateTime Timestamp { get; set; }
        public MachineStreamStatus Status { get; set; }
        public string? EventName { get; set; }
    }
    public enum MachineStreamStatus
    {
        idle,
        running,
        finished,
        errored,
        repaired
    }
}
