namespace Domain.Entities
{
    /// <summary>
    /// the way a MC host an event
    /// </summary>
    public class HostManner : BaseEntity
    {
        public string Label { get; set; } = string.Empty; 
    }
}
