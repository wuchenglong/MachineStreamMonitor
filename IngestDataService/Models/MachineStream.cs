namespace DataService.Models //IngestDataService.Models
{
    public class MachineStream
    {
        public string? Id { get; set; }
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
